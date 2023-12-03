using UnityEngine;
using UnityEngine.Events;

public class PickupEventHandler : MonoBehaviour
{
    private string type;

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
                    case "SpeedBoost":
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