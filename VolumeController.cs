using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeController : MonoBehaviour {

    public AudioSource music;

    private Slider slider;
    private Toggle toggle;

    private float musicStopTime = 0;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        toggle = GetComponentInParent<Toggle>();

        slider.onValueChanged.AddListener(delegate { updateVolume(); });
        toggle.onValueChanged.AddListener(delegate { updateToggle(); });

        slider.value = .2f; // initial volume
        musicStopTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void updateVolume()
    {
        music.volume = slider.value;
    }

    public void updateToggle()
    {
        if(toggle.isOn)
        {
            music.time = musicStopTime;
            music.gameObject.SetActive(true);
        }
        else
        {
            musicStopTime = music.time;
            music.gameObject.SetActive(false);
        }
    }

}
