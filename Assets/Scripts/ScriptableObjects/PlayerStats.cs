using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerPowerUpsObject")]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public int numOfHealthPotions;
    public int Health;
    
    [Header("Speed")]
    public float playerSpeed;
    public int numOfSpeedBoosts;
    
    [Header("Ammo")]
    public int Ammo;
    public int minAmmoPickup;
    public int maxAmmoPickup;
    
}
