using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunAmmoUI : MonoBehaviour
{
    public Text currentAmmoField;
    public ParticleSystem ammoEffect;

    private void Start()
    {
        var main = ammoEffect.main;
        main.playOnAwake = false;
    }


    public void UpdateAmmoType(int ammoType)
    {
        string name = "SCALE";

        var main = ammoEffect.main;

        switch (ammoType)
        {
            case 1:
                name = "SCALE";
                main.startColor = Color.red;
                break;
            case 2:
                name = "SPEED";
                main.startColor = Color.yellow;
                break;
            case 3:
                name = "HACK";
                main.startColor = Color.blue;
                break;
        }
        currentAmmoField.text = name;
        currentAmmoField.color = ammoType < 3 ? (ammoType == 1? Color.red: Color.yellow) : Color.blue;
        ammoEffect.Emit(1);
    }
}
