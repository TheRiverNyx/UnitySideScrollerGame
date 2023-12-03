using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerPowerUpsObject")]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public int numOfHealthPotions;
    public float Health;
    public float minHealthPickup;
    public float maxHealthPickup;
    
    [Header("Speed")]
    public float playerSpeed;
    public float boostAmount;
    public int numOfSpeedBoosts;
    public float JumpHeight;
    
    [Header("Ammo")]
    public int Ammo;
    public int minAmmoPickup;
    public int maxAmmoPickup;
    
}
