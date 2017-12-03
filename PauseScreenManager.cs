using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseScreenManager : MonoBehaviour {

    public CarController player;
    public Text wavesTraveled;
    public Text distanceTraveled;

    public void updateText()
    {
        wavesTraveled.text = "WAVE: " + (player.nextRoadToBuild - 2);
        distanceTraveled.text = "Distance    Traveled: " + Mathf.RoundToInt(player.transform.position.z);
    }

}
