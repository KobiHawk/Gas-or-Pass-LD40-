using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceBarController : MonoBehaviour {
    public CarController car;
    public Transform destinationPointer;
    private Transform maxDistancePointer;

    public float maxDistance = 1000; // distance at which the pointer stops updating
    public float minDistance = 0; // distance at which the pointer meets the target
    private float screenPointerDistance; // distance on screen from start to end for pointers

    private float distanceTraveled = -300;
    private Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        screenPointerDistance = destinationPointer.transform.position.y - transform.position.y;
        maxDistancePointer = transform;
        Debug.Log("destinationPointer: " + destinationPointer.transform.position.y);
        Debug.Log("maxDistancePointer: " + maxDistancePointer.transform.position.y);
    }

    private void Update()
    {
        distanceTraveled += 0.5f;
        updateDistance();
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
        else if(distanceDelta >= maxDistance)
        {
            text.text = distanceDelta.ToString();
            //maxDistance
        }
        else
        {
            text.text = distanceDelta.ToString();
            transform.position = new Vector3(transform.position.x,
                                             destinationPointer.transform.position.y - ((distanceDelta / maxDistance) * screenPointerDistance),//(distanceDelta/maxDistance) * screenPointerDistance, 
                                             transform.position.z);
            //also update position
        }
    }
}
