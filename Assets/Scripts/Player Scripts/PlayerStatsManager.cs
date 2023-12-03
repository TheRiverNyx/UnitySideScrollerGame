using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    private int playerMaxHealth = 100;

    private UIManager uiManager;
    
    private float fixedDeltaTime;
    
    private 
    // Start is called before the first frame update
    void Start()
    {
        // Initialize player health
        playerStats.Health = playerMaxHealth;
        uiManager= GetComponent<UIManager>();

    }
    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    public UnityEvent<float> TakeDamage(float damage)
    {
        // Reduce player health by the specified damage amount
        playerStats.Health -= damage;
        uiManager.UpdateHealth();
        // Check if player health is below or equal to zero
        if (playerStats.Health <= 0)
        {
            EndGame();
            Debug.Log("Player defeated!");
            
        }
        
        return null;
    }

    public UnityEvent<int> PickUpAmmo()
    {
        int amount = Random.Range(playerStats.minAmmoPickup, playerStats.maxAmmoPickup + 1);
        // Increase player ammo count by the specified amount
        playerStats.Ammo += amount;
        return null;
    }

    public UnityEvent<int> UseSpeedBoost()
    {
        // Apply speed boost to the player
        if (playerStats.numOfSpeedBoosts > 0)
        {
            playerStats.numOfSpeedBoosts -= 1;
            playerStats.playerSpeed += playerStats.boostAmount;
            Invoke("ResetSpeed",10);
        }
        return null;
    }

    public UnityEvent<int> PickUpSpeedBoost()
    {
        playerStats.numOfSpeedBoosts += 1;
        return null;
    }

    public UnityEvent<int> PickUpHealthPotion()
    {
        playerStats.numOfHealthPotions += 1;
        return null;
    }
    public UnityEvent<int> UseHealthPotion()
    {
        if (playerStats.numOfHealthPotions > 0)
        {
            playerStats.numOfHealthPotions -= 1;
            // Use health potion to restore player health
            playerStats.Health += Random.Range(playerStats.minHealthPickup,playerStats.maxHealthPickup);

            // Ensure health potions don't go below zero
            playerStats.numOfHealthPotions = Mathf.Max(playerStats.numOfHealthPotions, 0);
            
            uiManager.UpdateHealth();
        }

        return null;
    }

    public void Shoot()
    {
        playerStats.Ammo -= 1;
    }
    private void ResetSpeed()
    {
        playerStats.playerSpeed -= playerStats.boostAmount;
    }

    public void EndGame()
    {
        uiManager.IsDeadMessage();
        Time.timeScale = 0.7f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        
    }
}
