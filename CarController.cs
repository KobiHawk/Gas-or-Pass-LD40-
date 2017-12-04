using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * CarController
 * The majority of the game's mechanics are handled by this script.
 * This controls the player, including managing their fuel, speed, as well as updating various UI elements based on the player's status.
 * Most other scripts connect here.
 * Game overs are also handled here.
 */

public class CarController : MonoBehaviour {

    Rigidbody rb;

    //Local Variables
    public int gasRemaining = 2000;
    public int gasCap = 4000;
    public int gasPickupAward = 1000;
    public int hitObstaclePenalty = 350;
    public float speed = 10;
    public const float MAX_SPEED = 100; //the absolute maximum speed
    public float maxSpeed = 60; // the current maximum speed, after accounting for drag from fuel carry
    public float maxRotation = 15;
    public float rollImpact = 1.5f;
    public float drag = 0.1f;
    public int nextRoadToBuild = 2;
    public bool hasGas = false;
    private int boostDuration = 60; // frames
    private bool isBoosting = false;
    private int framesAboveTargetSpeed = 0;
    private float targetSpeed = 75;
    public int boostCost = 300;

    public float velocity; // used to track rb.velocity.z

    protected int timesChangedRoadColor = 1;
    protected Color currRoadColor;

    private const int TARGET_FRAME_RATE = 60;

    public Slider gasSlider;
    public Slider speedSlider;
    public Text distanceTraveled;
    public EmergencyGasSpriteChanger emergencyGas;
    public GameObject road;
    public GameOverManager gameOverManager;
    public PauseScreenManager pauseMenu;
    public BoostGaugeController boostGauge;

    protected void Awake()
    {
        Application.targetFrameRate = TARGET_FRAME_RATE;
    }

    void Start () {
        Time.timeScale = 1.0f;
        rb = GetComponent<Rigidbody>();

        gasSlider.minValue = 0;
        gasSlider.maxValue = gasCap;
        gasSlider.value = gasRemaining;

        speedSlider.minValue = 0;
        speedSlider.maxValue = MAX_SPEED;
        speedSlider.value = rb.velocity.z;
        
    }

    protected void Update()
    {
        //first need to make sure game doesn't do anything when paused
        if(Time.timeScale == 0.0f)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 1.0f;
                pauseMenu.gameObject.SetActive(false);
            }
        }
        else
        {
            if(Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 0.0f;
                pauseMenu.updateText();
                pauseMenu.gameObject.SetActive(true);
            }
            else //we can proceed normally, game is unpaused
            {
                //manage boost gauge, if we're going fast then charge a boost
                if(rb.velocity.z >= targetSpeed)
                {
                    framesAboveTargetSpeed++;
                    if(framesAboveTargetSpeed / TARGET_FRAME_RATE >= 1) // todo: calculate an average of past few frames instead
                    {
                        boostGauge.updateGauge();
                        framesAboveTargetSpeed -= TARGET_FRAME_RATE;
                    }
                }
                //bookkeeping at the start of each frame
                calculateMaxSpeed();
                if (gasRemaining <= 0 && rb.velocity.z < 0.5f && !hasGas)
                {
                    gameOver();
                }

                //handle input
                if (Input.GetButtonDown("space"))
                {
                    usePowerup();
                }
                if(Input.GetButtonDown("Fire1") && boostGauge.gauge == BoostGaugeController.MAX_GAUGE && gasRemaining >= boostCost)
                {
                    gasRemaining -= boostCost;
                    StartCoroutine(boost());
                    boostGauge.clearGauge();
                }
                move();

                //update UI
                speedSlider.value = rb.velocity.z;
                distanceTraveled.text = Mathf.RoundToInt(transform.position.z).ToString();
            }
        }
    }

    protected void calculateMaxSpeed()
    {
        float percentageOfMaxSpeed = (-.75f * (gasRemaining * 100 / gasCap)) + 100;
        maxSpeed = MAX_SPEED * (percentageOfMaxSpeed / 100);
    }

    private IEnumerator boost()
    {
        if(!isBoosting)
        {
            isBoosting = true;

            for(int i = 0; i < boostDuration; i++)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed * 2); // double speed for duration
                yield return null;
            }
            isBoosting = false;
        }
    }

    private void usePowerup()
    {
        if(hasGas)
        {
            gasRemaining += gasPickupAward;
            if(gasRemaining > gasCap)
            {
                gasRemaining = gasCap;
            }

            hasGas = false;
            emergencyGas.updateImage("red");
        }
    }

    protected void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * 5;
        float moveVertical = Input.GetAxis("Vertical");

        //handle vertical (forward) movement
        if(moveVertical < 0.0f)
        {
            moveVertical = 0.0f; // can't reverse
        }

        if (gasRemaining <= 0 || moveVertical == 0.0f || (rb.velocity.z >= maxSpeed)) // we either aren't accelerating, or can't
        {
            moveVertical = 0.0f;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * (1 - drag));
            if(rb.velocity.z < 0.01f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else // we are accelerating, let's go
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, moveVertical);
            rb.AddForce(direction * speed);

            gasRemaining -= 3;
            gasSlider.value = gasRemaining;
        }

        //handle horizontal movement
        if (moveHorizontal != 0.0f)
        {
            int turnPower = 10;
            if(gasRemaining <= 0)
            {
                turnPower = 1; // can still turn when out of gas, but very limited
            }

            //to improve control, velocity adjusted directly for horizontal movement. Less realistic but more fun
            rb.velocity = new Vector3(moveHorizontal * turnPower, rb.velocity.y, rb.velocity.z);
            gasRemaining--;
            if(gasRemaining < 0)
            {
                gasRemaining = 0;
            }
            gasSlider.value = gasRemaining;
        }


        //handle rotation
        if (moveHorizontal == 0.0f) //player not turning, return car to normal
        {
            if(transform.rotation.z > 0.0f)
            {
                transform.Rotate(0.0f, 0.0f, -0.2f);
            }
            else if(transform.rotation.z < 0.0f)
            {
                transform.Rotate(0.0f, 0.0f, 0.2f);
            }
            rb.velocity = new Vector3(rb.velocity.x * 0.5f, rb.velocity.y, rb.velocity.z); // return trajectory back to normal too
        }
        else if((moveHorizontal > 0.0f) && ((transform.rotation.eulerAngles.z > (360 - maxRotation)) || (transform.rotation.eulerAngles.z < 180)))
        {
            transform.Rotate(0.0f, 0.0f, -0.2f);
        }
        else if((moveHorizontal < 0.0f) && ((transform.rotation.eulerAngles.z < maxRotation) || (transform.rotation.eulerAngles.z > 180)))
        {
            transform.Rotate(0.0f, 0.0f, 0.2f);
        }
        velocity = rb.velocity.z; // used to track velocity
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            Destroy(other.gameObject);
            gasRemaining += gasPickupAward;
            if (gasRemaining > gasCap)
            {
                gasRemaining = gasCap;
            }
            gasSlider.value = gasRemaining;
        }
        else if(other.CompareTag("EmergencyGas"))
        {
            Destroy(other.gameObject);
            if(!hasGas)
            {
                hasGas = true;
                emergencyGas.updateImage("green");
            }
        }
        else if(other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            gasRemaining -= hitObstaclePenalty;
            if(gasRemaining < 0)
            {
                gasRemaining = 0;
            }
            gasSlider.value = gasRemaining;
            rb.velocity = new Vector3(rb.velocity.x / 2, 0.0f, rb.velocity.z / 2);
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road"))
        {


            Destroy(other.gameObject); //we've passed the road, so we can free the space
            GameObject newRoad = Instantiate(road) as GameObject;
            //make the road at the player's X coordinate, in the distance at the correct Z coordinate, then update the new Z coordinate (nextRoadToBuild)
            //we round to the nearest 5 to preserve the road texture matching with the previous ones.
            newRoad.transform.position = new Vector3(Mathf.Round(transform.position.x / 5.0f) * 5, newRoad.transform.position.y, newRoad.transform.position.z + (nextRoadToBuild * newRoad.transform.localScale.z));
            nextRoadToBuild++;

            //if (nextRoadToBuild / (10 * timesChangedRoadColor) >= 1) //commented out as was initially a bug, but updating color every wave has proven nice
            //{
                currRoadColor = new Color(Random.value, Random.value, Random.value);
                timesChangedRoadColor++;
            //}
            newRoad.GetComponent<MeshRenderer>().material.color = currRoadColor;
        }
    }

    protected void capSpeed()
    {
        if(rb.velocity.z > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }
    }

    public void gameOver()
    {
        gameOverManager.gameObject.SetActive(true);
        gameOverManager.gameOver();
    }
}
