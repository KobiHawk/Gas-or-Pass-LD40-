using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmergencyGasSpriteChanger : MonoBehaviour {

    private Image image;
    public Sprite red;
    public Sprite green;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}

    public void updateImage(string newImage)
    {
        if (newImage == "green")
        {
           image.overrideSprite = green;
        }
        else if(newImage == "red")
        {
            image.overrideSprite = red;
        }
    }
}
