using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{
    //Local Variables
    public int gasRemaining;
    public int gasCap;
    public int gasPickupAward;

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    //External Unity Components
    public Slider gasSlider;

    private void Start()
    {
        gasSlider.minValue = 0;
        gasSlider.maxValue = gasCap;
        gasSlider.value = gasRemaining;
    }

    public void FixedUpdate()
    {
        move();
    }

    private void move()
    {  
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (gasRemaining > 0 && motor != 0.0f)
            {
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                } // only drive if we have gas, can still steer without it though
                gasRemaining--;
                gasSlider.value = gasRemaining;
            }
        } 
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
    }
}



[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}