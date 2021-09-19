using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUiScript : MonoBehaviour
{
    // timer for how long to show each UI pop up
    private WaitForSeconds timer = new WaitForSeconds(2.5f);

    private TextMeshProUGUI text;
    private bool isReady = false;

    // variables to hold player info
    private Camera cam;
    private Vector3 pos;
    int whichSide;
    int p1Layer;
    private float screenW;

    // UI font info
    private const float FRAME_LENGTH = 1f / 24f;
    private Color defaultColor = new Color(1, 0.98f, 0.65f);
    private float initialFontSize;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        // assign itself to damagepopupUI class and set default values
        DamagePopUpUi.AddUI(this);
        text.enabled = false;
        p1Layer = LayerMask.NameToLayer("P1Cam");
        screenW = Screen.width/2;
        initialFontSize = text.fontSize;
    }

    // give a basic animation to text over time (fade out and get smaller)
    private void UpdateText()
    {
        text.alpha *= 0.7f;
        text.fontSize *= 0.8f;
    }

    private void Update()
    {
        if (isReady)
        {
            // place UI on screen based on where hit contact point was
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(cam, pos);

            // only show up on screen of p1, not p2
            if ((whichSide == 1 && uiPos.x < screenW) || (whichSide == 2 && uiPos.x > screenW))
            {
                text.enabled = true;
                transform.position = uiPos;
            }
            // if out of view, then disable
            else
            {
                text.enabled = false;
            }

        }
    }

    // assign values such as colour and location based on info given to by player
    public void ShowDamage(int damageValue, Vector3 pos, Camera cam, bool isElement)
    {
        if (damageValue == 0)
        {
            return;
        }

        // only been seen on the specified player's camera
        text.enabled = true;
        this.cam = cam;
        gameObject.layer = cam.gameObject.layer;
        whichSide = gameObject.layer == p1Layer ? 1 : 2;
        this.pos = pos;

        // elemental attacks are in colour
        if (isElement)
        {
            text.color = (whichSide == 1) ? Color.red : Color.blue;
        }
        // regular melee attacks are in white
        else
        {
            text.color = defaultColor;

        }
        // place UI at right spot
        Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(cam, pos);
        transform.position = uiPos;
        text.SetText(damageValue+"");
        isReady = true;
        // play text animation
        InvokeRepeating("UpdateText", 1f, FRAME_LENGTH);
        StartCoroutine(DisableUI());
    }

    // stop text animation and disable it
    IEnumerator DisableUI()
    {
        yield return timer;
        text.SetText("");
        CancelInvoke();
        text.fontSize = initialFontSize;
        text.alpha = 1;
        text.color = defaultColor;
        isReady = false;
        text.enabled = false;
    }
}
