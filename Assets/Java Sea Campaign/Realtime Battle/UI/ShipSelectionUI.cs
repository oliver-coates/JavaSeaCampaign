using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionUI : MonoBehaviour
{
    [SerializeField] private ShipInstance _selectedShip;

    [Header("References:")]
    [SerializeField] private TextMeshProUGUI _rudderText;
    [SerializeField] private Slider _rudderSlider;

    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private Slider _speedSlider;

    private void Awake()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        _rudderText.text = "";
        _speedText.text = "";

    }

    public void UpdateRudderValue()
    {
        float rudder = _rudderSlider.value;

        Color colorTint;
        string message;

        if (rudder < 0)
        {
            message = $"Rudder: Port ({(int)rudder})";
            colorTint = Color.red;
        }
        else if (rudder > 0)
        {
            message = $"Rudder: Starboard ({(int)rudder})";
            colorTint = Color.blue;
        }
        else
        {
            message = $"Rudder: Ahead";
            colorTint = Color.white;
        }

        _rudderText.text = message;
        _rudderText.color = Color.Lerp(Color.white, colorTint, (Mathf.Abs(rudder) / 10f) / 5f);

        _selectedShip.rudder = rudder / 10f;
    }

    public void UpdateSpeedValue()
    {
        int targetSpeedWhole = (int) _speedSlider.value;
        float targetSpeedActual = _speedSlider.value / 4f;

        string message;

        if (targetSpeedWhole == 0)
        {
            message = "Engine: Stop";
        }
        else if (targetSpeedWhole == 1)
        {
            message = "Engine: Slow Ahead";
        }
        else if (targetSpeedWhole == 2)
        {
            message = "Engine: Half Ahead";
        }
        else if (targetSpeedWhole == 3)
        {
            message = "Engine: Cruise Ahead";
        }
        else
        {
            message = "Engine: Full Ahead";
        }

        _speedText.text = message;
        _speedText.color = Color.Lerp(Color.white, Color.yellow, targetSpeedActual / 3f);
 
        _selectedShip.targetSpeed = targetSpeedActual;
    }

}
