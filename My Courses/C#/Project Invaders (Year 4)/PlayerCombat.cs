using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCombat : MonoBehaviour
{
    // player properties
    public Transform attackPoint;
    private float attackRange = 1.5f;

    private float quickAttackPower;
    private float heavyAttackPower;
    private float quickAttackPowerBase = 30;
    private float heavyAttackPowerBase = 50;
    private float damageMultiplier = 1;
    private float HeavyAttackCost = 0.2f;
    private float stamina = 1;

    private int attackIndex;
    int maxColliders = 10;

    [HideInInspector] public bool isKnockedOut = false;
    private bool isBlocking;
    [HideInInspector] public bool isReady = false;
    [HideInInspector] public bool isAttacking = false;

    private Coroutine attackIndexReset;
    [HideInInspector] public int playerIndex;

    // other components
    public Camera cam;
    public Animator anim;
    private PlayerScript playerMovement;

    // layermasks
    public LayerMask enemyLayers;
    private int playerLayerMask;
    private int EnemyLayerMask;

    public int sceneNum;

    // references to puzzle classes
    [HideInInspector] public int NearSwitchNum = 0;
    [HideInInspector] public bool isNearSwitch = false;
    [HideInInspector] public PuzzleThree puz3;
    [HideInInspector] public PuzzleTwo puz2;
    [HideInInspector] public PuzzleOne puz1;

    // particle systems
    private PlayerAudioSources pAudio;
    public GameObject hitParticlesPrefab;
    private GameObject hitParticles;
    public GameObject[] swordParticles;

    private void Start()
    {
        attackIndex = 0;
        playerMovement = GetComponent<PlayerScript>();
        playerLayerMask = gameObject.layer;
        EnemyLayerMask = LayerMask.NameToLayer("Enemy");

        // initialize damage values based on base damage number and current multiplier based on level
        quickAttackPower = quickAttackPowerBase * damageMultiplier;
        heavyAttackPower = heavyAttackPowerBase * damageMultiplier;

        pAudio = GetComponent<PlayerAudioSources>();
    }

    public void SetUpSwordColours(int index)
    {
        if (index == 0) // p1
        {
            swordParticles[0].SetActive(true);
            swordParticles[1].SetActive(false);
        }
        else // p2
        {
            swordParticles[0].SetActive(false);
            swordParticles[1].SetActive(true);
        }
    }


    public void SetUpDamageValues(float newValue)
    {
        // get damage multiplier from save system and round to clean number
        damageMultiplier += newValue;
        damageMultiplier = (float) Math.Round(damageMultiplier, 2, MidpointRounding.AwayFromZero);

        quickAttackPower = quickAttackPowerBase * damageMultiplier;
        heavyAttackPower = heavyAttackPowerBase * damageMultiplier;

        // base set up for level 4 puzzles
        isNearSwitch = false;
        NearSwitchNum = 0;
    }

    // function to handle player interacting with puzzle switches/etc
    void OnInteract()
    {
        if (sceneNum == 4 && isNearSwitch)
        {
            InteractSwitch();
            return;
        }
    }

    // get input for light attack
    void OnQuickAttack()
    {

        if (isBlocking || !isReady)
        {
            return;
        }

        // replace standard attack button's function with interaction if near a puzzle switch
        if (sceneNum == 4 && isNearSwitch)
        {
            InteractSwitch();
            return;
        }

        // if it is a successful attack, let main attack method handle it
        Attack(true, quickAttackPower);

    }

    // get input for heavy attack
    void OnHeavyAttack()
    {
        if (isBlocking || !isReady)
        {
            return;
        }

        stamina = playerMovement.GetStamina();

        // heavy attacks cost stamina
        if (stamina > HeavyAttackCost)
        {
            Attack(false, heavyAttackPower);
        }
    }

    private void OnBlock(InputValue value)
    {
        isBlocking = value.isPressed;
    }

    private void InteractSwitch()
    {
        if (NearSwitchNum == 2) // puzzle 2
        {
            puz2.TurnOnPuzzle();
        }
        else // puzzle 3
        {
            puz3.ActivateSwitch(NearSwitchNum, playerIndex);
        }
        
        // reset puzzle vars after successful interaction
        NearSwitchNum = 0;
        isNearSwitch = false;

    }

    // choose an animation for attack based on attack type
    private void AttackAnim(bool isQuickAttack)
    {
        string attackNum;

        // choose animation based on if quick or heavy attack
        if (isQuickAttack)
        {
            // attack index is a counter that keeps track of what number in combo it is (0 -> 1 -> 2 -> 0 -> 1 -> etc)
            switch (attackIndex)
            {
                case 0:
                    attackNum = "AttackLight1";
                    break;
                case 1:
                    attackNum = "AttackLight2";
                    break;
                default:
                    attackNum = "AttackLight3";
                    break;
            }
        }
        else
        {
            switch (attackIndex)
            {
                case 0:
                    attackNum = "AttackHeavy1";
                    break;
                case 1:
                    attackNum = "AttackHeavy2";
                    break;
                default:
                    attackNum = "AttackHeavy3";
                    break;
            }
        }

        // update attack counter index and make it loop back to start at the end
        attackIndex = (attackIndex + 1) % 3;

        // play the animation
        anim.SetTrigger(attackNum);
    }

    // starts the process for the attack
    private void Attack(bool isQuickAttack, float damage)
    {
        // cannot do an attack if already attacking
        if (!isAttacking)
        {
            // play attack animation
            AttackAnim(isQuickAttack);
            if (!isQuickAttack)
            {
                // drain stamina for heavy attack
                playerMovement.SetStamina(HeavyAttackCost);
            }
            
            // arial combat lowers gravity for combos
            playerMovement.LowerGravityForAttack();

            // start the next part of the process for the attack
            StartCoroutine(InitiateAttack(isQuickAttack, damage));
            isAttacking = true;

            // combo has a small timer before being reset, so every attack starts/delays the cooldown
            if (attackIndexReset != null)
            {
                // if a cooldown exists, stop the old time, it is no longer needed
                StopCoroutine(attackIndexReset);
            }
            // start a new cooldown timer as of this attack
            attackIndexReset = StartCoroutine(ResetCombo());
        }

    }

    //
    private void DealDamage(bool isQuickAttack, float damage)
    {

        Collider[] hitColliders = new Collider[maxColliders];

        // detect enemies in range
        int numColliders = Physics.OverlapSphereNonAlloc(attackPoint.position, attackRange, hitColliders, enemyLayers, QueryTriggerInteraction.Ignore);
        try
        {
            // damage them
            for (int i = 0; i < numColliders; i++)
            {
                Collider enemy = hitColliders[i];
                // if its an enemy/player
                if (enemy != null)
                {
                    // ignore enemy triggers
                    if (enemy.gameObject == gameObject)
                    {
                        continue;
                    }

                    // play vfx for hit
                    Vector3 ParticlePos = enemy.ClosestPoint(attackPoint.position);
                    hitParticles = Instantiate(hitParticlesPrefab, ParticlePos, Quaternion.identity) as GameObject;
                    Destroy(hitParticles, 2f);

                    // sfx for hit
                    pAudio.PlaySwordAttack();

                    float value = damage;
                    Vector3 e = enemy.transform.position;

                    // players take damage different than enemies
                    if (enemy.gameObject.layer == playerLayerMask)
                    {
                        value = enemy.GetComponent<PlayerHealth>().RecieveDamage(damage / 2, true);
                    }
                    else if (enemy.gameObject.layer == EnemyLayerMask)
                    {
                        value = enemy.GetComponent<EnemyScript>().ReceiveMeleeDamage(damage, playerIndex, !isQuickAttack);
                        e = enemy.GetComponent<EnemyScript>().GetBodyLocation();
                    }

                    // place the UI for the damage pop up number in a random spot in the vicinity of the attack point and show it
                    Vector3 pos = new Vector3(e.x + Random.Range(-0.5f, 0.5f), e.y + Random.Range(-0.5f, 0.5f), e.z + Random.Range(-0.5f, 0.5f));

                    DisplayDamageUI(value, pos, false);
                }

            }
        } catch (Exception e)
        {

        }

        // process done, no longer attacking
        isAttacking = false;
    }

    // have a small delay before the programming of damage is handled so it lines up with the attack animation
    private IEnumerator InitiateAttack(bool isQuickAttack, float damage)
    {
        float waitTime;
        // quick attacks are faster so smaller delay
        if (isQuickAttack)
        {
            waitTime = 0.45f;
        }
        else
        {
            waitTime = 0.55f;
        }

        yield return new WaitForSeconds(waitTime);

        // final step of process to deal damage
        DealDamage(isQuickAttack, damage);
    }

    // reset combo if too long gap between attacks
    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(0.9f);
        attackIndex = 0;
    }

    // show a visual UI popup of the damage numbers
    public void DisplayDamageUI(float damageValue, Vector3 pos, bool isElement)
    {
        DamagePopUpUi.ShowRandomUi(playerIndex, (int)damageValue, pos, isElement);
    }

    // reset multiplier if game is over/restarted
    public void ResetDamageValues()
    {
        damageMultiplier = 1;
    }

    public float GetMultiplier()
    {
        return damageMultiplier;
    }

    private void OnDrawGizmosSelected() // see area of attack range
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
