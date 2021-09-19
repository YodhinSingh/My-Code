using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    // UI of suit energy
    public Image energyBarUI;
    public Gradient gradient;


    private float energyValue;

    // Start is called before the first frame update
    void Start()
    {
        energyValue = 100;
        CalculateValue();
    }

    // take energy away from UI bar
    public float RemoveEnergy(float drainValue)
    {
        energyValue = Mathf.Max(energyValue - drainValue, 0);

        CalculateValue();

        return energyValue;
    }

    // add energy to UI bar
    public float AddEnergy(float value)
    {
        energyValue = Mathf.Min(energyValue + value, 100);

        CalculateValue();

        return energyValue;
    }

    // handles applying change to UI
    private void CalculateValue()
    {
        energyBarUI.fillAmount = energyValue / 100f;
        energyBarUI.color = ColourFromGradient(energyValue / 100f);

        if (energyValue <= 0)
        {
            // Game Over
            LevelWinCondition.GameOver();
        }
    }

    private Color ColourFromGradient(float value)  // float between 0-1
    {
        return gradient.Evaluate(value);
    }


    public float GetEnergyLevel()
    {
        return energyValue;
    }
}
