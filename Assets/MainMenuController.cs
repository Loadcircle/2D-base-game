
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    // Start is called before the first frame update
    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int highLevel = PlayerPrefs.GetInt("HighLevel", 0);

        HighScoreText.text = "High Score: " + highScore.ToString() + "\nMax Level: " + highLevel.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
