using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject dangerPrefab;
    public int initialFoodAmount = 4;
    public int initialDangerAmount = 1;

    private int currentFoodAmount;
    private int currentDangerAmount;

    //Sounds Triggers
    public AudioClip foodSoundClip;
    public AudioClip dangerSoundClip;

    //score handlers
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    private int score = 0;
    private int level = 1;

    //level completed
    public GameObject levelCompletedCanvas;
    private int foodCount = 0;
    void Start()
    {
        currentFoodAmount = initialFoodAmount;
        currentDangerAmount = initialDangerAmount;
        foodCount = currentFoodAmount;

        // Generate initial balls
        GenerateBalls(foodPrefab, currentFoodAmount);
        GenerateBalls(dangerPrefab, currentDangerAmount);
    }

    void GenerateBalls(GameObject ballPrefab, int amount)
    {
        // Get screen dimensions
        float maxX = Camera.main.orthographicSize * Screen.width / Screen.height;
        float maxY = Camera.main.orthographicSize;

        // Generate random positions within screen dimensions
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0f);
            Instantiate(ballPrefab, position, Quaternion.identity);
        }
    }

    public void Continue()
    {
        // Increment the amount of balls
        currentFoodAmount += 1; // You can adjust the amount as you prefer
        currentDangerAmount += 1;
        foodCount = currentFoodAmount;

        // Destroy current balls before loading the new scene
        DestroyBallsWithTag("Food");
        DestroyBallsWithTag("Danger");

        // Generate new food balls
        GenerateBalls(foodPrefab, currentFoodAmount);

        // Generate new dangerous balls
        GenerateBalls(dangerPrefab, currentDangerAmount);


        // Increment the level
        levelIncrease();

        levelCompletedCanvas.SetActive(false);
    }

    void DestroyBallsWithTag(string tag)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }


    public void scoreIncrease(int ammount = 1)
    {
        score += ammount;
        foodCount--;

        UpdateUI();

        if (foodCount == 0)
        {
            LevelCompleted();
        }
    }
    public void levelIncrease()
    {
        level++;
        UpdateUI();
    }
    void UpdateUI()
    {
        // Actualiza el texto del puntaje y nivel en los objetos de TextMeshPro
        scoreText.text = "Score: " + score.ToString();
        levelText.text = "Level: " + level.ToString();
    }
    public void LevelCompleted()
    {
        // Mostrar el canvas de victoria
        levelCompletedCanvas.SetActive(true);
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.PauseGame();
    }
    public void PlayFoodSound()
    {
        // Play the sound for when food is collected
        if (foodSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(foodSoundClip, Camera.main.transform.position);
        }
    }

    public void PlayDangerSound()
    {
        // Play the sound for when danger is encountered
        if (dangerSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(dangerSoundClip, Camera.main.transform.position);
        }
    }
}
