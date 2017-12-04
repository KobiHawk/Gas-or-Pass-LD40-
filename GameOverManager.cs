using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public CarController player;
    public DistanceBarController distanceTracker;

    private Text text;
    private Button retryButton;
    private Button continueButton;

	// Use this for initialization
	void Awake () {
        text = GetComponentInChildren<Text>();
        retryButton = GetComponentsInChildren<Button>()[0];
        continueButton = GetComponentsInChildren<Button>()[1];

        this.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
	}

    public void gameOver()
    {
        Time.timeScale = 0.0f;
        retryButton.onClick.AddListener(newGame);
        if (player.gasRemaining > 0)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.AddListener(retry);

        }

        text.text = "Game over. You reached Wave: " + (player.nextRoadToBuild - 2) + "."; // game starts by needing third road built, but that's only wave 1
    }
    void newGame()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    void retry()
    {
        Time.timeScale = 1.0f;
        continueButton.gameObject.SetActive(false); // this option is not always available
        gameObject.SetActive(false);
        distanceTracker.gameObject.SetActive(false);
    }

}
