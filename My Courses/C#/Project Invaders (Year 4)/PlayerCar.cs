using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCar : MonoBehaviour
{
    // car properties
    [HideInInspector] public bool AllowControls = false;
    private float turnValue = 0;
    private float accelerateValue = 0;
    private float brakeValue = 0;
    private float forwardValue = 0;
    private bool isBlocking;
    private bool isBoosting;

    // car controller
    public HoverCarController m_Car; 

    // references to other classes
    private PlayerScript pScript;
    private PlayerHealth pHealth;

    // thrusters
    public ParticleSystem boostL;
    public ParticleSystem boostR;
    public ParticleSystem brakeL;
    public ParticleSystem brakeR;

    private PlayerAudioSources pAudio;

    private bool sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        pScript = GetComponent<PlayerScript>();
        pHealth = GetComponent<PlayerHealth>();
        pAudio = GetComponent<PlayerAudioSources>();
    }

    private void FixedUpdate()
    {
        if (LevelWinCondition.Gameover)
        {
            // turn off car engine audio if level/game over
            EngineAudio(0);
            return;
        }

        if (AllowControls)
        {
            // do not let car rotate in the wrong directions
            if (transform.rotation.x != 0)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

            // player can press brake/accelerate at the same time, but brake has higher priority
            forwardValue = (accelerateValue  - (4 * brakeValue));
            float forwardThruster = accelerateValue - brakeValue;

            EngineAudio(forwardValue);

            if (forwardValue < 0) // going backwards
            {
                // reverse should be slower top speed
                forwardValue /= 3f;
            }

            bool m = false;

            // double speed with turbo if stamina available
            if (pScript.GetStamina() > 0 && isBoosting)
            {
                forwardValue *= 2f;
                m = true;
            }
            // otherwise turn off turbo SFX and particle system
            else if (boostR.isPlaying)
            {
                boostR.Stop();
                boostL.Stop();
            }

            // send inputs to car controller
            m_Car.Move(turnValue, forwardValue, m);
            m_Car.RotateThrusters(turnValue, forwardThruster);
        }
    }
    
    // get all inputs using unity's new input system
    private void OnTurn(InputValue value)
    {

        turnValue = value.Get<Vector2>().x;
    }

    private void OnAccelerate(InputValue value)
    {
        accelerateValue= value.Get<float>();
    }

    // turns on/off engine audio based on parameter == 0
    private void EngineAudio(float forwardVal)
    {
        if (forwardVal != 0)
        {
            pAudio.PlayCarEngine(true);
        }
        else
        {
            pAudio.PlayCarEngine(false);
        }
    }

    private void OnBrake(InputValue value)
    {
        brakeValue = value.Get<float>();
        // turn on brake lights if car is slowing down
        if (brakeValue > 0 && forwardValue >= 0 && !pHealth.isDrone)
        {
            brakeL.Play();
            brakeR.Play();
        }
        else
        {
            brakeL.Stop();
            brakeR.Stop();
        }
    }

    private void OnBlockCar(InputValue value)
    {
        isBlocking = value.isPressed;
    }

    private void OnBoost(InputValue value)
    {
        isBoosting = value.isPressed;
        // play turbo systems if have stamina
        if (isBoosting && pScript.GetStamina() > 0)
        {
            boostL.Play();
            boostR.Play();
        }
        else
        {
            boostL.Stop();
            boostR.Stop();
        }
    }

}
