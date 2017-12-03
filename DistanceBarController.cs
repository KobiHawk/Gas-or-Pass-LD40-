using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceBarController : MonoBehaviour {
    public CarController car;
    public Transform destinationPointer;

    public float maxDistance = 10000; // distance at which the pointer stops updating
    public float minDistance = 0; // distance at which the pointer meets the target
    private float screenPointerDistance; // distance on screen from start to end for pointers

    //private bool isVeryFarAhead = false; // if player is over max distance from opponent, this is true

    private float distanceTraveled = -300;
    private int currDifficulty = 1;
    private float speed = 0.5f;
    private Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        screenPointerDistance = destinationPointer.transform.position.y - transform.position.y;
    }

    private void Update()
    {
        if (Time.timeScale != 0.0f)
        {
            distanceTraveled += speed;
            updateDistance();

            if (car.transform.position.z / (currDifficulty * 2500) >= 1)
            {
                increaseDifficulty();
                currDifficulty++;
                Debug.Log("Difficulty increased to " + speed);
            }
        }
    }

    private void updateDistance()
    {
        int distanceDelta = Mathf.RoundToInt(car.transform.position.z - distanceTraveled);

        if (distanceDelta <= minDistance)
        {
            text.text = distanceDelta.ToString();
            car.gameOver();
            //gameover
        }
        else if(distanceDelta >= (maxDistance / 5))
        {
            text.text = distanceDelta.ToString();
            //isVeryFarAhead = true;
            //maxDistance
        }
        else
        {
            text.text = distanceDelta.ToString();
            transform.position = new Vector3(transform.position.x,
                                             destinationPointer.transform.position.y - ((distanceDelta / maxDistance) * screenPointerDistance),//(distanceDelta/maxDistance) * screenPointerDistance, 
                                             transform.position.z);
            //isVeryFarAhead = false;
        }
    }

    public void increaseDifficulty()
    {
        speed += 0.1f;
    }
}
