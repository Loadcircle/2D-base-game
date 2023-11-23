using TMPro;
using UnityEngine.SceneManagement;
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

    //endgame
    public GameObject endGameCanvas;
    public TextMeshProUGUI newRecordText;
    public TextMeshProUGUI endGameInfo;
    void Start()
    {
        currentFoodAmount = initialFoodAmount;
        currentDangerAmount = initialDangerAmount;
        foodCount = currentFoodAmount;

        GenerateGrid();
        GenerateFoodInGrid();
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

        FillAllGridPositions();

        GenerateFoodInGrid();

        // Increment the level
        LevelIncrease();

        levelCompletedCanvas.SetActive(false);
    }


    public void ScoreIncrease(int ammount = 1)
    {
        score += ammount;
        foodCount--;

        UpdateUI();

        if (foodCount == 0)
        {
            LevelCompleted();
        }
    }
    public void LevelIncrease()
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
        SetPuasedGame();
    }
    public void SetPuasedGame()
    {
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



    //End game methods
    public void EndGame()
    {
        PlayDangerSound();
        SetPuasedGame();
        endGameCanvas.SetActive(true);

        endGameInfo.text = "Total Score: " + score.ToString() + "\nLevel: " + level.ToString();

        //if new record

        // Recuperar el récord actual
        int currentRecord = PlayerPrefs.GetInt("HighScore", 0);

        if (score > currentRecord)
        {
            UpdateRecord();
        }
        else
        {
            newRecordText.enabled = false;
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }
    public void UpdateRecord()
    {
        // Update record
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.SetInt("HighLevel", level);
        PlayerPrefs.Save(); // Guardar los cambios

    }





    //grid genertor
    private Vector2[,] gridLocations;
    private Vector2[] allGridPositions;
    private int rows;
    private int columns;

    void GenerateGrid()
    {
        // Get the size of the foodPrefab
        Renderer foodRenderer = foodPrefab.GetComponent<Renderer>();
        float foodWidth = foodRenderer.bounds.size.x;
        float foodHeight = foodRenderer.bounds.size.y;

        // Get the size of the screen in world units
        float screenWidth = Camera.main.orthographicSize * 2.0f * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2.0f;

        // Calculate the number of rows and columns based on the foodPrefab size
        columns = Mathf.FloorToInt(screenWidth / (foodWidth * 1.5f)); // Aumentamos el tamaño de la columna
        rows = Mathf.FloorToInt(screenHeight / (foodHeight * 1.5f)); // Aumentamos el tamaño de la fila

        // Initialize the gridLocations array
        gridLocations = new Vector2[rows, columns];

        // Calculate the spacing between positions in the grid
        float spacingX = screenWidth / columns;
        float spacingY = screenHeight / rows;

        // Fill the gridLocations array with positions
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Añadimos un pequeño desplazamiento aleatorio a la posición
                float randomOffsetX = Random.Range(-spacingX * 0.25f, spacingX * 0.25f);
                float randomOffsetY = Random.Range(-spacingY * 0.25f, spacingY * 0.25f);

                float posX = col * spacingX - screenWidth / 2.0f + spacingX / 2.0f + randomOffsetX;
                float posY = screenHeight / 2.0f - row * spacingY - spacingY / 2.0f + randomOffsetY;

                gridLocations[row, col] = new Vector2(posX, posY);
            }
        }

        FillAllGridPositions();
    }

    void FillAllGridPositions()
    {
        int totalGridPositions = rows * columns;
        allGridPositions = new Vector2[totalGridPositions];

        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                allGridPositions[index] = gridLocations[row, col];
                index++;
            }
        }
    }

    void GenerateFoodInGrid()
    {
        //if ((currentFoodAmount + currentDangerAmount) > rows * columns)
        //{
        //    Debug.LogError("Number of food and danger items exceeds available grid positions!");
        //    return;
        //}

        GenerateItemsOfType(foodPrefab, currentFoodAmount);
        GenerateItemsOfType(dangerPrefab, currentDangerAmount);
    }

    void GenerateItemsOfType(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Ensure there are still available positions in the grid
            if (allGridPositions.Length == 0)
            {
                Debug.LogWarning("No more available positions in the grid.");
                break;
            }

            // Randomly select an index from the available positions
            int randomIndex = Random.Range(0, allGridPositions.Length);
            Vector2 position = allGridPositions[randomIndex];

            // Remove the selected position from the available positions
            RemoveGridPositionFromArray(randomIndex);

            // Instantiate the prefab at the selected position
            Instantiate(prefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }
    void RemoveGridPositionFromArray(int index)
    {
        Vector2[] newArray = new Vector2[allGridPositions.Length - 1];

        for (int i = 0, newIndex = 0; i < allGridPositions.Length; i++)
        {
            if (i != index)
            {
                newArray[newIndex] = allGridPositions[i];
                newIndex++;
            }
        }

        allGridPositions = newArray;
    }
    void DestroyBallsWithTag(string tag)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }
}
