using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmergencyGasSpriteChanger : MonoBehaviour {

    private Button button;
    public Sprite red;
    public Sprite green;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
	}

    public void updateImage(string newImage)
    {
        if (newImage == "green")
        {
            button.image.overrideSprite = green;
        }
        else if(newImage == "red")
        {
            button.image.overrideSprite = red;
        }
    }
}
