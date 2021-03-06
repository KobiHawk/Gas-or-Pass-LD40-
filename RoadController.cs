﻿using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

    public GameObject obstacle;
    public GameObject gasPickup;
    public GameObject emergencyGasPickup;
    public CarController player;

    public int obstacleFrequency = 20;
    public int gasFrequency = 50;
    public int emergencyGasFrequency = 10;

	void Start ()
    {
        player = FindObjectOfType<CarController>();
        generateChunk();
	}

    private void generateChunk()
    {
        //temporary
        Vector3 newPosition;
        float playerX = player.gameObject.transform.position.x;

        //prevent looking this up over and over
        float yPosition; 
        yPosition = obstacle.transform.position.y;

        /* Calculate newPosition:
         * x: Get a random value based on the scale of this road, then adjust to include negative numbers (Random.value is only [0, 1]).
         *    Also adjust based on the players X axis to spawn objects no matter where the player goes.
         * y: Use the yPosition, calculated based on the prefab's y. Some objects look better at different y coordinates.
         * z: Same as x, but we also shift the object up by the z axis, since we'll be making these roads back to back, we need to adjust from origin
         */

        for (int i = 0; i < obstacleFrequency + (player.nextRoadToBuild * 3); i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2 + playerX,
                                       yPosition,
                                       ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(obstacle, newPosition, obstacle.transform.rotation) as GameObject;
            newObject.transform.parent = transform;
        }

        yPosition = gasPickup.transform.position.y;
        for (int i = 0; i < gasFrequency + (player.nextRoadToBuild * 5); i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2 + playerX,
                                       yPosition,
                                       ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(gasPickup, newPosition, gasPickup.transform.rotation) as GameObject;
            newObject.transform.parent = transform;
        }
        yPosition = emergencyGasPickup.transform.position.y;

        /*
         * So, this was really annoying.
         * The star being used as the mesh for the powerup pickup broke when the transform was parented to the road.
         * It would look sort of right, but blocky and fat, not ideal.
         * The pivot was also wrong slightly.
         * This does need to be parented to the road so it can be destroyed with the rest of memory cleanup.
         * 
         * So, as a workaround, the star has a parent, which contains the correct pivot transform.
         * That object also has a parent, created here, which is empty and only serves to invalidate the scale transformation
         * imposed by the road.
         * It works.
         */
        for (int i = 0; i < emergencyGasFrequency; i++)
        {
            newPosition = new Vector3((Random.value * transform.lossyScale.x) - transform.lossyScale.x / 2 + playerX,
                                       yPosition,
                                       ((Random.value * transform.lossyScale.z) - transform.lossyScale.z / 2) + transform.position.z);
            GameObject newObject = Instantiate(emergencyGasPickup, newPosition, emergencyGasPickup.transform.rotation) as GameObject;
            GameObject empty = new GameObject();
            newObject.transform.parent = empty.transform;
            empty.transform.parent = transform;
        }
    }
}
