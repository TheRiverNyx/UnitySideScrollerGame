using UnityEngine;
using UnityEngine.Events;

public class PickupEventHandler : MonoBehaviour
{
    private string type;

    public UnityEvent pickUpSpeedPotion;
    public UnityEvent pickUpHealthPotion;
    public UnityEvent pickUpAmmo;

    void Start()
    {
        type = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatsManager playerStatsManager = other.GetComponent<PlayerStatsManager>();
            if (playerStatsManager != null)
            {
                switch (type)
                {
                    case "SpeedPotion":
                        playerStatsManager.Invoke("PickUpSpeedBoost",0);
                        break;
                    case "HealthPotion":
                        playerStatsManager.Invoke("PickUpHealthPotion",0);
                        break;
                    case "AmmoPickup":
                        playerStatsManager.Invoke("PickUpAmmo",0);
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}