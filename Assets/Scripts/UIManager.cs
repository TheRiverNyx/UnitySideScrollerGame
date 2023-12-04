using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI deathMessageUi;
    public Slider healthUI;
    public TextMeshProUGUI ammoUI;
    public GameObject SpeedBoostUI;
    public GameObject HealthBarUI;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        deathMessageUi.enabled = false;
        UpdateHealth();
        UpdateAmmo();
        UpdateHealthPotionCount();
        UpdateSpeedBoostCount();
    }

    // Update is called once per frame
    public void UpdateHealth()
    {
        healthUI.value = playerStats.Health;
    }

    public void UpdateAmmo()
    {
        ammoUI.text = "AMMO: " + playerStats.Ammo;
    }

    public void IsDeadMessage()
    {
        deathMessageUi.enabled = true;
    }

    public void UpdateSpeedBoostCount()
    {
        TextMeshProUGUI countText = SpeedBoostUI.transform.GetChild(0).GameObject().GetComponent<TextMeshProUGUI>();
        GameObject greyOutOverlay = SpeedBoostUI.transform.GetChild(2).GameObject();
        if (playerStats.numOfSpeedBoosts == 0)
        {
            greyOutOverlay.SetActive(true);
        }
        else
        {
            greyOutOverlay.SetActive(false);
        }

        countText.text = playerStats.numOfSpeedBoosts.ToString();
    }

    public void UpdateHealthPotionCount()
    {
        TextMeshProUGUI countText = HealthBarUI.transform.GetChild(0).GameObject().GetComponent<TextMeshProUGUI>();
        GameObject greyOutOverlay = HealthBarUI.transform.GetChild(2).GameObject();
        if (playerStats.numOfHealthPotions == 0)
        {
            greyOutOverlay.SetActive(true);
        }
        else
        {
            greyOutOverlay.SetActive(false);
        }

        countText.text = playerStats.numOfHealthPotions.ToString();
    }
}
