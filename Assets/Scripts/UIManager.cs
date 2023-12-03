using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI deathMessageUi;
    public Slider healthUI;
    private TextMeshProUGUI ammoUI;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        deathMessageUi.enabled = false;
        UpdateHealth();
        UpdateAmmo();
    }

    // Update is called once per frame
    public void UpdateHealth()
    {
        healthUI.value = playerStats.Health;
    }

    public void UpdateAmmo()
    {
        
    }

    public void IsDeadMessage()
    {
        deathMessageUi.enabled = true;
    }
}
