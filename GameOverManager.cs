using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public CarController player;

    private Text text;
    private Button button;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        this.gameObject.SetActive(false);
	}

    public void gameOver()
    {
        this.gameObject.SetActive(true);
        button.onClick.AddListener(newGame);

        text.text = "Game over. You reached Wave: " + (player.nextRoadToBuild - 2) + "."; // game starts by needing third road built, but that's only wave 1
    }
    public void newGame()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

}
