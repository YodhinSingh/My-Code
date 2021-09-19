using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyScript : MonoBehaviour
{
    // Choose the element type that this enemy is
    public elementType elementWeakness;
    // For faster processing, the string element is converted to a number at start. Fire = 0, Ice = 1, Both = 2
    private int elementAsInt;
    // Choose the enemy type that this enemy is
    public enemyType enemy;
    // For faster processing, the string enemy is converted to a number at start. normal = 0, brute = 1, fly = 2, boss = 3, etc
    private int enemyAsInt;

    // main properties
    private bool isInRange;
    private Transform playerToAttack;
    private EnemySightRange sight;
    private bool canShoot;
    private float initialHealth;
    private float health;
    private float damageMuliplier = 1;
    private float bossHitAmount = 0;
    private float speed;
    private float maxCloseDist;
    private float shootTimer;
    public GameObject bulletPrefab;
    private float bulletForce;
    private float rechargeTimeToShoot;

    // other components
    private NavMeshAgent nav;
    private WaitForSeconds vulnerableTime;
    private Rigidbody rb;
    public Animator anim;

    // other properties
    private bool isSpawned = false;
    private EnemySpawner spawner;
    public Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    public LayerMask playerLayer;
    private float attackPower = 5;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private bool isAttackAnim = false;
    int maxColliders = 3;
    Collider[] hitColliders;

    // UI related
    public Image healthUI;
    public Image healthDamageUI;
    Coroutine damageHealthAnim;
    private bool isHealthAnimOn = false;
    Coroutine attackWait;
    private bool isBeingAttacked = false;
    Coroutine vulnerableWait;
    private bool isVulnerable = false;

    // level 3 (car chase) related
    private bool isLvlThree;
    public bool isLeader;
    private bool isReady;
    private Transform carTarget;
    [HideInInspector] public Transform[] thrusters;
    private CarEnemyTravelPath instance;

    private int sceneNum;
    public Transform carShootCanon;

    public GameObject DeathExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // get references
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        sight = GetComponentInChildren<EnemySightRange>();

        sceneNum = LevelWinCondition.GetSceneNum();
        isLvlThree = sceneNum == 3;

        // set properties based on level and this specific enemy stats
        ConvertElementToInt();
        if (!isLvlThree)
        {
            CancelInvoke();
        }

        ApplyMaterialChange();
        ConvertEnemyToInt();
        EnemyTypeSpecificSetUp();
        isInRange = false;
        
        healthDamageUI.fillAmount = healthUI.fillAmount = health / initialHealth;

        // turn on car properties for level 3
        if (isLvlThree)
        {
            instance = CarEnemyTravelPath.instance;
            Invoke("GetCarTargets", 0.1f);
            InvokeRepeating("AdjustThrusters", 0.5f, 0.5f);
        }

        // if not a boss enemy, then it can be temporarily disabled if its too far from players
        if (enemyAsInt < 3)
        {
            EnemyDistCheck.instance.AddEnemy(gameObject);
        }
        
        vulnerableTime = new WaitForSeconds(5);
        hitColliders = new Collider[maxColliders];
        maxCloseDist = nav.stoppingDistance + 0.1f;
    }

    // level 3 car chase has the enemy cars move through city
    private void GetCarTargets()
    {
        // leader uses special navigation system
        if (isLeader)
        {
            carTarget = instance.GetTargetPoint();
        }
        // all other enemies follow leader always
        else
        {
            carTarget = instance.GetLeaderPos();
        }
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelWinCondition.IsGamePaused())
        {
            return;
        }

        // car chase level
        if (isLvlThree && isReady)
        {
            // update path travel for leader
            if (isLeader && nav.remainingDistance < 30)
            {
                carTarget = instance.GetNextPoint();
            }
            // if player is close enough, point gun at them
            if (playerToAttack != null)
            {
                carShootCanon.LookAt(playerToAttack.position);
            }
            // if cooldown over, shoot
            if (isInRange && Time.time > nextAttackTime)
            {
                ChooseAttackType();
            }
            
            nav.SetDestination(carTarget.position);
            return;
        }
        // for regular level enemies
        if (isInRange)
        {
            if (!nav.isActiveAndEnabled)
            {
                return;
            }
            // if near player, head towards them and if close enough, fire
            if (nav.remainingDistance < maxCloseDist && Time.time > nextAttackTime)
            {
                ChooseAttackType();
            }

            nav.SetDestination(playerToAttack.position);
        }
        else
        {
            // not in range of player, so wait
            shootTimer = Time.time + rechargeTimeToShoot;
        }
        // play enemy animations
        AnimationHandler();
    }

    private void AnimationHandler()
    {
        // enemy type normal, brute and boss have specific animation stats
        if (enemyAsInt <= 1 || enemyAsInt == 3)
        {
            // if not moving/so little it is close to zero
            if ((nav.isActiveAndEnabled && nav.velocity.magnitude <= 0.10f && !isInRange) || !nav.isActiveAndEnabled)
            {
                nav.velocity = Vector3.zero;
                anim.SetFloat("MoveSpeed", 0);
            }
            // otherwise animate moving
            else
            {
                anim.SetFloat("MoveSpeed", Mathf.FloorToInt(nav.velocity.magnitude));
            }

        }
    }
    // modify car thrusters based on direction of travel
    private void AdjustThrusters()
    {
        Vector3 dir = transform.InverseTransformDirection(nav.velocity).normalized;
        for (int i = 0; i < thrusters.Length; i++)
        {
            Vector3 rot = thrusters[i].localRotation.eulerAngles;
            if (enemyAsInt == 5) // car boss
            {
                rot.x = 30 * dir.x;
                rot.z = 30 * dir.z;
            }
            else
            {
                rot.x = 30 * dir.z;
                rot.z = 30 * dir.x;
            }

            thrusters[i].localRotation = Quaternion.Euler(rot.x, 0, rot.z);
        }
    }

    // set stats of enemy based on which type they are
    private void EnemyTypeSpecificSetUp()
    {
        int lvl = sceneNum;
        switch (enemyAsInt)
        {
            case 0: // normal
                initialHealth = health = 100 * lvl;
                nav.stoppingDistance = 3;
                canShoot = false;
                attackPower = 5 + (lvl / 2);
                break;
            case 1: // brute
                initialHealth = health = 200 * lvl;
                nav.stoppingDistance = 4;
                canShoot = false;
                attackPower = 10 + (lvl);
                break;
            case 2: // fly
                initialHealth = health = 100 * lvl;
                nav.stoppingDistance = 10;
                shootTimer = 5f;
                bulletForce = 5f;
                canShoot = true;
                rechargeTimeToShoot = 5f;
                anim.SetTrigger("Attack");
                attackPower = 5 + (lvl / 2);
                break;
            case 3: // boss
                initialHealth = health = 1000 * lvl;
                nav.stoppingDistance = 20;
                shootTimer = 10f;
                bulletForce = 5f;
                canShoot = true;
                rechargeTimeToShoot = 5f;
                attackRange = 7f;
                attackPower = 15 + (lvl * 3);
                attackRate = 4;
                InputManager.instance.SetBossLocation(transform);
                break;
            case 4: // car normal
                initialHealth = health = 100 * lvl;
                shootTimer = 5f;
                bulletForce = 3f;
                canShoot = true;
                attackPower = 15;
                rechargeTimeToShoot = 3f;
                break;
            case 5: // car boss
                initialHealth = health = 1000 * lvl;
                shootTimer = 10f;
                bulletForce = 3f;
                canShoot = true;
                attackPower = 20;
                rechargeTimeToShoot = 5f;
                break;
        }
    }

    // each enemy type has a different attack type and cooldown
    private void ChooseAttackType()
    {
        switch (enemyAsInt)
        {
            case 2: // fly
                AttackDist();
                nextAttackTime = Time.time + 1.2f * attackRate;
                break;
            case 3: // boss
                AttackBoss();
                nextAttackTime = Time.time + 1.5f * attackRate;
                break;
            case 4: // car normal
                AttackDist();
                nextAttackTime = Time.time + 1.2f * attackRate;
                break;
            case 5: // car boss
                AttackDist();
                nextAttackTime = Time.time + 1.5f * attackRate;
                break;
            default: // normal or brute
                AttackMelee();
                nextAttackTime = Time.time + 1f * attackRate;
                break;
        }
    }

    // bosses can attack either melee or ranged
    private void AttackBoss()
    {
        // randomly choose which attack to do
        int random = Random.Range(0, 10);
        // melee is more likely
        if (random <= 6)
        {
            AttackMelee();
        }
        else
        {
            AttackDist();
        }
    }

    // ranged attack has enemy shoot bullet
    private void AttackDist()
    {
        bool initialCondition = !canShoot || playerToAttack == null;
        bool initialConditionTwo = isBeingAttacked && (enemyAsInt < 3);

        if (initialCondition || initialConditionTwo)
        {
            return;
        }

        // play animation before actually shooting
        if (Time.time > shootTimer - 0.9f && !isAttackAnim)
        {
            // play attack animation
            if (anim != null)
            {
                anim.SetTrigger("Attack");
                isAttackAnim = true;
            }
        }
        // shoot bullet if no cooldown
        if (Time.time > shootTimer)
        {
            GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
            bullet.GetComponent<EnemyBullet>().SetUp(attackPower);
            Rigidbody rigBod = bullet.GetComponent<Rigidbody>();

            Vector3 modPos = playerToAttack.position;
            // car models require slight change in position of fire
            if (sceneNum == 3)
            {
                modPos = new Vector3(playerToAttack.position.x, playerToAttack.position.y - 1, playerToAttack.position.z);
            }

            // push bullet forward
            Vector3 targetPos = modPos - attackPoint.position;
            rigBod.AddForce(targetPos * bulletForce, ForceMode.Impulse);

            Destroy(bullet, 3f); //destroy bullet after 3 seconds

            shootTimer = Time.time + rechargeTimeToShoot;
            isAttackAnim = false;

        }

    }

    private void AttackMelee()
    {

        bool initialCondition =  playerToAttack == null || (isBeingAttacked && enemyAsInt != 3);

        if (initialCondition)
        {
            return;
        }

        // play attack animation
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        // detect enemies in range
        int numColliders = Physics.OverlapSphereNonAlloc(attackPoint.position, attackRange, hitColliders, playerLayer, QueryTriggerInteraction.Ignore);

        // damage them
        for (int i = 0; i < numColliders; i++)
        {
            Collider player = hitColliders[i];
            if (player != null)
            {
                player.GetComponent<PlayerHealth>().RecieveDamage(attackPower, false);
            }
        }
    }

    public float ReceiveElementalDamage(int elementType, float damageValue )
    {
        if (sight != null)
        {
            sight.IncreaseRangeTemporarily(3, 5); // increase range of sight if player attacks from a distance
        }

        float returnDamageValue;
        // if right element used or this enemy is weak to all elements, max damage
        if (elementAsInt == 2 || elementAsInt == elementType) 
        {
            returnDamageValue = TakeDamage(damageValue, 5, elementType);
            damageMuliplier = 1.5f; // melee attacks againsted this enemy that follow will be stronger

            // how long enemy takes increased damage
            if (isVulnerable)
            {
                StopCoroutine(vulnerableWait); 
            }
            isVulnerable = true;
            vulnerableWait = StartCoroutine(VulnerableTime());
        }
        // else only fourth of damage
        else
        {
            returnDamageValue = TakeDamage(damageValue/4f, 1, elementType);
        }

        return returnDamageValue;
    }

    public float ReceiveMeleeDamage(float damageValue, int playerNum, bool isHeavy)
    {
        // melee damage affects enemies different than elemental
        float damageTotal = damageValue * damageMuliplier;
        isBeingAttacked = true;
        if (enemyAsInt <= 1 && !isLvlThree && isHeavy && rb.isKinematic && playerToAttack != null)
        {
            // if enemy is not a brute/boss/car then there should be a knockback effect after attack
            Vector3 pushBackVel = (transform.position - playerToAttack.position).normalized * (damageTotal * 0.2f);

            rb.isKinematic = false;
            nav.enabled = false;
            rb.AddForce(pushBackVel.normalized * 7, ForceMode.Impulse);
            anim.SetFloat("MoveSpeed", 0);

        }

        float returnDamageValue = TakeDamage(damageTotal, 1, playerNum);
        return returnDamageValue;
    }

    private float TakeDamage(float damageValue, float stunTime, int playerNum)
    {
        // round damage and take health from this enemy
        float damageAsInt = Mathf.Round(damageValue);
        health -= damageAsInt;
        healthUI.fillAmount = health / initialHealth;
        
        // have health decrease UI animation
        if (isHealthAnimOn)
        {
            StopCoroutine(damageHealthAnim);
        }
        isHealthAnimOn = true;
        damageHealthAnim = StartCoroutine(HealthBarAnim());

        // bosses may switch elements if damaged too much
        BossSwitchElement(damageValue);

        // cannot attack while being attacked
        if (isBeingAttacked && attackWait!= null)
        {
            StopCoroutine(attackWait);
        }
        isBeingAttacked = true;
        attackWait = StartCoroutine(WaitAfterBeingAttacked(stunTime));

        if (health <= 0)
        {
            Die();
        }

        return damageAsInt;
    }

    private void Die()
    {
        GameObject explode = Instantiate(DeathExplosionPrefab, GetBodyLocation(), Quaternion.identity); // create explosion
        Destroy(explode, explode.GetComponent<ParticleSystem>().main.duration);

        if (enemyAsInt == 3 || enemyAsInt == 5)
        {
            // if this is a boss, then level won
            LevelWinCondition.BossIsDead();
        }
        else
        {
            // add score to players based on level
            if (LevelWinCondition.GetSceneNum() == 3)
            {
                // level 3 (car) has higher valued enemies
                LevelWinCondition.enemyKillCount += 5;
                InputManager.instance.AddEnergy(3);
            }
            else
            {
                // regular enemies give basic score
                if (isSpawned)
                {
                    spawner.RemoveEnemy();
                }
                LevelWinCondition.enemyKillCount++;
                InputManager.instance.AddEnergy(1);
                EnemyDistCheck.instance.RemoveEnemy(gameObject);
            }

        }

        Destroy(gameObject, 0.1f);
    }

    private void BossSwitchElement(float damageValue)
    {
        // if not boss, then return
        if (enemyAsInt != 3 && enemyAsInt != 5)
        {
            return;
        }
        // find out how much damage taken
        bossHitAmount += damageValue;
        if (bossHitAmount > initialHealth / 5)
        {
            // if enough damage taken, randomly switch to another element
            int rand = Random.Range(0, 2);
            bossHitAmount = 0;

            // if neutral element, take the new one and return
            if (elementAsInt == 3)
            {
                elementAsInt = rand;
                ApplyMaterialChange();
                return;
            }

            // otherwise switch to the next element in order
            elementAsInt = (elementAsInt + 1) % 2;
            ApplyMaterialChange();
        }


    }

    // change visual material colour based on element
    private void ApplyMaterialChange()
    {
        bool isBoss = enemyAsInt == 3 || enemyAsInt == 5;

        if (!isLvlThree)
        {
            GetComponent<EnemyTypeScript>().SetUp(elementAsInt, isBoss);
        }
        else
        {
            GetComponentInChildren<CarColourRandomizer>().EnemyCarColourSetUp(elementAsInt, isBoss);
        }
    }

    // have health bar UI decrease with animation
    private IEnumerator HealthBarAnim()
    {
        while (true)
        {
            yield return null;

            if (healthDamageUI.fillAmount > healthUI.fillAmount)
            {
                healthDamageUI.fillAmount -= 0.25f * Time.deltaTime;
            }
            else
            {
                isHealthAnimOn = false;
                StopCoroutine(damageHealthAnim);
            }
        }
    }

    private IEnumerator WaitAfterBeingAttacked(float time) // how long is enemy has to wait from attack
    {
        yield return new WaitForSeconds(time);
        if (sceneNum != 3 && rb != null)
        {
            isBeingAttacked = false;
            rb.isKinematic = true;
            nav.enabled = true;
        }

    }

    private IEnumerator VulnerableTime() // how long is enemy vulnerable from element attack
    {
        yield return vulnerableTime;
        damageMuliplier = 1f;
        isVulnerable = false;

    }

    public void IsPlayerInRange(bool isFound, Transform player)
    {
        isInRange = isFound;
        playerToAttack = player;
    }

    private void ConvertElementToInt()
    {
        if (elementWeakness.ToString().Equals("Fire"))
        {
            elementAsInt = 0;
        }
        else if (elementWeakness.ToString().Equals("Ice"))
        {
            elementAsInt = 1;
        }
        else if (elementWeakness.ToString().Equals("Both"))
        {
            elementAsInt = 2;
        }
        else
        {
            elementAsInt = Random.Range(0,3);
        }
    }

    private void ConvertEnemyToInt()
    {
        if (enemy.ToString().Equals("Normal"))
        {
            enemyAsInt = 0;
        }
        else if (enemy.ToString().Equals("Brute"))
        {
            enemyAsInt = 1;
        }
        else if (enemy.ToString().Equals("Fly"))
        {
            enemyAsInt = 2;
        }
        else if (enemy.ToString().Equals("Boss"))
        {
            enemyAsInt = 3;
        }
        else if (enemy.ToString().Equals("Car"))
        {
            enemyAsInt = 4;
        }
        else if (enemy.ToString().Equals("CarBoss"))
        {
            enemyAsInt = 5;
        }
        else
        {
            enemyAsInt = Random.Range(0, 3);
        }
    }

    // method to allow for choosing element types
    public enum elementType
    {
        Fire, Ice, Both, Random
    }

    // method to allow for choosing enemy types
    public enum enemyType
    {
        Normal, Brute, Fly, Random, Boss, Car, CarBoss
    }

    private void OnDrawGizmosSelected() // see area of attack range
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    public void RecieveSpawnInfo(EnemySpawner e)
    {
        isSpawned = true;
        spawner = e;
    }

    // get body position for animation
    public Vector3 GetBodyLocation()
    {
        if (anim == null)
        {
            return transform.position;
        }
        return anim.transform.position;
    }

}
