using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

    public GameObject obstacle;
    public GameObject gasPickup;
    public GameObject emergencyGasPickup;

    public int obstacleFrequency = 20;
    public int gasFrequency = 50;
    public int emergencyGasFrequency = 10;

	void Start ()
    {
        generateChunk();
	}

    private void generateChunk()
    {
        //temporary
        Vector3 newPosition;

        //prevent looking this up over and over
        float yPosition; 
        yPosition = obstacle.transform.position.y;

        /* Calculate newPosition:
         * x: Get a random value based on the scale of this road, then adjust to include negative numbers (Random.value is only [0, 1])
         * y: Use the yPosition, calculated based on the prefab's y. Some objects look better at different y coordinates.
         * z: Same as x, but we also shift the object up by the z axis, since we'll be making these roads back to back, we need to adjust from origin
         */

        for (int i = 0; i < obstacleFrequency; i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2, yPosition, ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(obstacle, newPosition, obstacle.transform.rotation) as GameObject;
            newObject.transform.parent = transform;
        }

        yPosition = gasPickup.transform.position.y;
        for (int i = 0; i < gasFrequency; i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2, yPosition, ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(gasPickup, newPosition, gasPickup.transform.rotation) as GameObject;
            newObject.transform.parent = transform;
        }
        yPosition = emergencyGasPickup.transform.position.y;
        for (int i = 0; i < emergencyGasFrequency; i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2, yPosition, ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(emergencyGasPickup, newPosition, emergencyGasPickup.transform.rotation) as GameObject;
            newObject.transform.parent = transform;
        }
    }
}
