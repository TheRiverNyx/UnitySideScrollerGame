using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    private int playerMaxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize player health
        playerStats.Health = playerMaxHealth;
    }

    // Update is called once per frame
    public UnityEvent<int> TakeDamage(int damage)
    {
        // Reduce player health by the specified damage amount
        playerStats.Health -= damage;
        
        // Check if player health is below or equal to zero
        if (playerStats.Health <= 0)
        {
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

    public UnityEvent<int> UseSpeedBoost(float boostAmount)
    {
        // Apply speed boost to the player
        playerStats.numOfSpeedBoosts -= 1;
        playerStats.playerSpeed += boostAmount;
        
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
    public UnityEvent<int> UseHealthPotion(int healAmount)
    {
        if (playerStats.numOfHealthPotions > 0)
        {
            playerStats.numOfHealthPotions -= 1;
            // Use health potion to restore player health
            playerStats.Health += healAmount;

            // Ensure health potions don't go below zero
            playerStats.numOfHealthPotions = Mathf.Max(playerStats.numOfHealthPotions, 0);
        }

        return null;
    }
}
