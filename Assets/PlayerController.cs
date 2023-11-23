using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f; // Player movement speed
    public float rotationSpeed = 100f; // Player rotation speed
    private Rigidbody2D rb;
    private bool paused = false; // Indicates if the game is paused
    public float rotateAmount = 1f;

    private bool activeCollisions = true;

    private float disableCollisionsTime = 1.5f;
    void Start()
    {
        // Get the reference to the Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is not paused before processing input
        if (!paused)
        {
            if (Input.touchCount > 0)
            {
                // Iteramos a través de todos los toques
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    // Verificamos si el toque está en la mitad izquierda o derecha de la pantalla
                    if (touch.position.x < Screen.width / 2)
                    {
                        // Lado izquierdo
                        Rotate((rotateAmount/10));

                    }
                    else
                    {
                        Rotate(-(rotateAmount / 10));

                    }
                }
            }

            BoundariesLimit();

        }
    }
    private void FixedUpdate()
    {
        if (!paused)
        {
            // Move the player forward constantly using Rigidbody2D
            rb.velocity = transform.up * movementSpeed;
        }
        else
        {
            // Si el juego está pausado, detenemos el movimiento estableciendo la velocidad a cero
            rb.velocity = Vector2.zero;
        }
    }
    void BoundariesLimit()
    {
        // Limit the player's position within the screen boundaries
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Clamp the x and y coordinates separately to ensure the entire player stays within the screen
        float clampedX = Mathf.Clamp01(viewPos.x);
        float clampedY = Mathf.Clamp01(viewPos.y);

        // Update the player's position based on the clamped values
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(clampedX, clampedY, viewPos.z));
    }

    // Method to rotate the player in the specified direction
    void Rotate(float direction)
    {
        // Calculate the amount of rotation
        float rotation = direction * (rotationSpeed*10) * Time.deltaTime;

        // Rotate the player
        transform.Rotate(Vector3.forward, rotation);
    }

    // Method to indicate that the game is paused
    public void PauseGame()
    {
        paused = true;
        // You can perform other actions here, such as showing a victory message or triggering transitions.
    }
    public void UnPauseGame()
    {
        paused = false;

        //disable collision for first X seconds
        activeCollisions = false;
        Invoke("SetActiveCollisions", disableCollisionsTime);
    }
    public bool IsPaused()
    {
        return paused;
    }
    public void IncreaseDifficulty()
    {
        IncreaseSpeed();
        IncreaseRotation();

    }
    void IncreaseSpeed()
    {
        movementSpeed += 0.10f;

    }
    void IncreaseRotation()
    {
        rotationSpeed += 30f;
    }
    void SetActiveCollisions()
    {
        activeCollisions = true;
    }
    public bool IsActiveCollisions()
    {
        return activeCollisions;
    }
}
