using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningScript : MonoBehaviour
{
    // Character controller script and also controls the camera follow
    public float speed = 12;
    public float jumpSpeed = 3;
    Rigidbody rb;
    public float gravity = -14;

    Vector3 velocity;

    public bool isRobot;
    private bool stayStill;
    private bool recall;

    CharacterController controller;

    float rotSpeed = 80;
    float rotSpeedY = 40;
    float rotX = 0;
    float rotY = 0;
    int robotIndex = 0;

    public GameObject player;

    public List<GameObject> robots = new List<GameObject>();

    public bool isDiscovered = false;

    private float health;
    bool AllowControls = true;

    public GameObject healthbar;
    public GameObject hurtCanvas;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isDiscovered = false;
        health = 100f;
        Time.timeScale = 1f;
        AllowControls = true;
        GetComponent<Transform>().Rotate(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (AllowControls)
        {
            if ((isDiscovered && isRobot) || (!isRobot))    // same rules apply to a discovered drone/robot and the player
            {
                rotX += Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
                rotY -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeedY;

                if (!isRobot)
                {
                    transform.rotation = Quaternion.Euler(0, rotX, 0);  // set player rotation based on mouse input
                    Camera.main.transform.LookAt(transform);
                }
                if (isRobot)
                {
                    transform.rotation = Quaternion.Euler(0, rotX - 214.85101f, 0); //make robots follow in same direction by applying same rotation value minus offset from model
                }


                Movement();
                GetInput();

                if (isRobot && recall)
                {
                    Recall();
                }

            }
            else
            {
                WaitForPlayer();    // if this drone has not been discovered, then just wait where it is

            }
        }
        if (healthbar != null && !isRobot)  // set health bar size of player
        {
            healthbar.GetComponent<RectTransform>().localScale = new Vector3(health / 100, healthbar.GetComponent<RectTransform>().localScale.y, healthbar.GetComponent<RectTransform>().localScale.z);
        }
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(1) && !isRobot && robotIndex < robots.Count)   // checks if player wants to place a drone 
        {
            for (int i = 0; i < robots.Count; i++)
            {
                if (robots[i].GetComponent<RunningScript>().speed != 0) // find first drone that is still moving
                {
                    robotIndex = i;
                    break;
                }
            }
            RunningScript robotScript = robots[robotIndex].GetComponent<RunningScript>();
            robotScript.speed = 0;
            robotScript.stayStill = true;   // make it stay still
        }

        if (Input.GetKeyDown("space"))  // call all still drones back to player
        {
            for (int i = 0; i < robots.Count; i++)
            {
                RunningScript robotScript = robots[i].GetComponent<RunningScript>();
                if (robotScript.stayStill)
                {
                    robotScript.speed = speed * 2;
                    robotScript.recall = true;
                }
            }
            robotIndex = 0;
        }
    }

    void Recall()
    {
        Vector3 dir = player.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame + 10)    // keep going until drone reaches a specific range to player
        {
            stayStill = false;
            recall = false;
            speed = player.GetComponent<RunningScript>().speed;
            return;
        }
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void Movement()
    {
        Vector3 move;
        float x, z;
        x = -Input.GetAxis("Vertical");
        z = Input.GetAxis("Horizontal");
        move = transform.right * x + transform.forward * z;

        if ((!stayStill && isRobot) || (!isRobot))
        {
            bool cond1 = isRobot && Vector3.Distance(transform.position, player.transform.position) > 7f;   // 2 conditions to make sure drones dont stray away from player movement
            bool cond2 = isRobot && Vector3.Distance(transform.position, player.transform.position) < 10f;

            if ((cond1 && cond2) || (!isRobot)) // keep moving with player if in range
                controller.Move(move * speed * Time.deltaTime);

            if (isRobot && Vector3.Distance(transform.position, player.transform.position) > 10f)   // if too far then call them to player
            {
                recall = stayStill = true;
                Recall();
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }

    void WaitForPlayer()
    {
        
        Vector3 dir = player.transform.position - transform.position;   // make drone wait in position until player gets close to it
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame + 10)
        {
            stayStill = false;
            recall = false;
            speed = player.GetComponent<RunningScript>().speed;
            transform.rotation = Camera.main.transform.rotation;
            isDiscovered = true;
            player.GetComponent<RunningScript>().addRobot(gameObject);
            return;
            
        }
    }

    public void addRobot(GameObject robot) // add this drone to list of drones in player obj
    {
        robots.Add(robot);
    }

    public void TakeDamage()
    {
        if (PauseMenu.endConditionLose != true && PauseMenu.endConditionWin != true)    // if the game is still on/no lost or won
        {
            if (health > 0)
            {
                health -= 25;
                StartCoroutine("DisplayHurt");
            }
            if (health <= 0 && AllowControls)
            {
                StartCoroutine("DestroyPlayer");
                AllowControls = false;
                PauseMenu.endConditionLose = true;
            }
        }
        else
        {
            AllowControls = false;
        }
    }

    private IEnumerator DestroyPlayer()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        yield return new WaitForSeconds(3);

    }
    private IEnumerator DisplayHurt()
    {
        
        hurtCanvas.SetActive(true);
        yield return new WaitForSeconds(1f);
        hurtCanvas.SetActive(false);

    }

    public bool isControlsActive()
    {
        return AllowControls;
    }

    public bool getStayStill()
    {
        return (speed == 0);
    }

    public void DisableControls()
    {
        AllowControls = false;
    }
}
