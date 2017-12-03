using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public CarController player;

    private Text text;
    private Button button;

	// Use this for initialization
	void Awake () {
        text = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        Debug.Log(button == null);
        this.gameObject.SetActive(false);
	}

    public void gameOver()
    {
        Time.timeScale = 0.0f;
        button.onClick.AddListener(newGame);

        text.text = "Game over. You reached Wave: " + (player.nextRoadToBuild - 2) + "."; // game starts by needing third road built, but that's only wave 1
    }
    void newGame()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        
    }

}
