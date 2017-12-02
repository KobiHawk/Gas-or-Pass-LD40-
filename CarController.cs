using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

    Rigidbody rb;

    //Local Variables
    public int gasRemaining = 1000;
    public int gasCap = 2000;
    public int gasPickupAward = 500;
    public int hitObstaclePenalty = 100;
    public float speed = 10;
    public const float MAX_SPEED = 100; //the absolute maximum speed
    public float maxSpeed = 60; // the current maximum speed, after accounting for drag from fuel carry
    public float maxRotation = 15;
    public float boostPower = 1500;
    public float rollImpact = 1.5f;
    public float drag = 0.1f;
    public int nextRoadToBuild = 3;
    public bool hasGas = false;

    public float velocity; // used to track rb.velocity.z

    public Slider gasSlider;
    public EmergencyGasSpriteChanger emergencyGas;
    public GameObject road;

    void Start () {
        rb = GetComponent<Rigidbody>();

        gasSlider.minValue = 0;
        gasSlider.maxValue = gasCap;
        gasSlider.value = gasRemaining;
    }

    private void Update()
    {
        calculateMaxSpeed();
    }

    void FixedUpdate () {

        move();

        if(Input.GetButtonUp("space"))
        {
            boost();
        }
	}

    private void calculateMaxSpeed()
    {
        float percentageOfMaxSpeed = (-.75f * (gasRemaining * 100 / gasCap)) + 100;
        maxSpeed = MAX_SPEED * (percentageOfMaxSpeed / 100);
    }

    private void boost()
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

    private void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * 5;
        float moveVertical = Input.GetAxis("Vertical");

        //handle vertical (forward) movement
        if(moveVertical < 0.0f)
        {
            moveVertical = 0.0f; // can't reverse
        }

        if (gasRemaining == 0 || moveVertical == 0.0f || (rb.velocity.z >= maxSpeed)) // we either aren't accelerating, or can't
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

            gasRemaining--;
            gasSlider.value = gasRemaining;
        }

        //handle horizontal movement
        rb.velocity = new Vector3(moveHorizontal * 10, rb.velocity.y, rb.velocity.z);

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
            Debug.Log(transform.rotation.eulerAngles.z);
            transform.Rotate(0.0f, 0.0f, -0.2f);
        }
        else if((moveHorizontal < 0.0f) && ((transform.rotation.eulerAngles.z < maxRotation) || (transform.rotation.eulerAngles.z > 180)))
        {
            Debug.Log(transform.rotation.eulerAngles.z);
            transform.Rotate(0.0f, 0.0f, 0.2f);
        }
        velocity = rb.velocity.z; // used to track velocity
    }

    private void OnTriggerEnter(Collider other)
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
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            Destroy(other.gameObject); //we've passed the road, so we can free the space
            GameObject newRoad = Instantiate(road) as GameObject;
            newRoad.transform.position = new Vector3(newRoad.transform.position.x, newRoad.transform.position.y, newRoad.transform.position.z + (nextRoadToBuild * newRoad.transform.localScale.z));
            nextRoadToBuild++;
        }
    }

    private void capSpeed()
    {
        if(rb.velocity.z > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }
    }
}
