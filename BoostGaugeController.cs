using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoostGaugeController : MonoBehaviour {

    public CarController player;
    public int gauge;
    public const int MAX_GAUGE = 4;

    public Sprite[] emptyImages = new Sprite[3];
    public Sprite[] fullImages = new Sprite[3];

    private int[] locations = new int[5]; 

    private Image[] images;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
        locations[0] = 0;
        locations[1] = 0;
        locations[2] = 1;
        locations[3] = 1;
        locations[4] = 2;
    }

    public void updateGauge()
    {
        images[gauge].sprite = fullImages[locations[gauge]];
        gauge++;
        if(gauge > MAX_GAUGE)
        {
            gauge = MAX_GAUGE;
        }
    }

    public void clearGauge()
    {
        for(int i = 0; i < locations.Length; i++)
        {
            images[i].sprite = emptyImages[locations[i]];
        }
        gauge = 0;
    }
}
