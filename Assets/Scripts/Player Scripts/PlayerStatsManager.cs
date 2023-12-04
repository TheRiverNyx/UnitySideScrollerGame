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

    private AudioSource[] playerSoundsSources;

    private AudioSource hurtSound;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize player health
        playerStats.Health = playerMaxHealth;
        uiManager= GetComponent<UIManager>();
        playerSoundsSources = GetComponents<AudioSource>();
        hurtSound = playerSoundsSources[0];
        

    }
    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        // Reduce player health by the specified damage amount
        playerStats.Health -= damage;
        hurtSound.Play();
        Debug.Log("Player took damage");
        uiManager.UpdateHealth();
        // Check if player health is below or equal to zero
        if (playerStats.Health <= 0)
        {
            EndGame();
            Debug.Log("Player defeated!");
            
        }
        
    }

    public UnityEvent<int> PickUpAmmo()
    {
        int amount = Random.Range(playerStats.minAmmoPickup, playerStats.maxAmmoPickup + 1);
        // Increase player ammo count by the specified amount
        playerStats.Ammo += amount;
        uiManager.UpdateAmmo();
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
            uiManager.UpdateSpeedBoostCount();
        }
        return null;
    }

    public UnityEvent<int> PickUpSpeedBoost()
    {
        playerStats.numOfSpeedBoosts += 1;
        uiManager.UpdateSpeedBoostCount();
        return null;
    }

    public UnityEvent<int> PickUpHealthPotion()
    {
        playerStats.numOfHealthPotions += 1;
        uiManager.UpdateHealthPotionCount();
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
            uiManager.UpdateHealthPotionCount();
            uiManager.UpdateHealth();
        }

        return null;
    }

    public void Shoot()
    {
        playerStats.Ammo -= 1;
        uiManager.UpdateAmmo();
    }
    private void ResetSpeed()
    {
        playerStats.playerSpeed -= playerStats.boostAmount;
    }

    public void EndGame()
    {
        uiManager.IsDeadMessage();
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        
    }
}
