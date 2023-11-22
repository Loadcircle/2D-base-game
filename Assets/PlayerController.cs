using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 2f; // Player movement speed
    public float rotationSpeed = 100f; // Player rotation speed
    private Rigidbody2D rb;
    private bool paused = false; // Indicates if the game is paused

    // Called when the script is initialized
    void Start()
    {
        // Get the reference to the Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is not paused before processing input
        if (!paused)
        {
            // Move the player forward constantly using Rigidbody2D
            rb.velocity = transform.up * movementSpeed;

            // Handle touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Check if the screen is touched
                if (touch.phase == TouchPhase.Began)
                {
                    // Get the touch position relative to the screen
                    float touchX = touch.position.x / Screen.width;

                    // Change the player's direction based on touch position
                    if (touchX < 0.5f)
                    {
                        // Rotate left
                        Rotate(1);
                    }
                    else
                    {
                        // Rotate right
                        Rotate(-1);
                    }
                }
            }

            // Handle mouse input
            //if (Input.GetMouseButton(0))
            //{
            //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    if (mousePos.x < 0)
            //    {
            //        Rotate(1);
            //    }
            //    else
            //    {
            //        Rotate(-1);
            //    }
            //}

            // Limit the player's position within the screen boundaries
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            viewPos.x = Mathf.Clamp01(viewPos.x);
            viewPos.y = Mathf.Clamp01(viewPos.y);
            transform.position = Camera.main.ViewportToWorldPoint(viewPos);
        }
    }

    // Method to rotate the player in the specified direction
    void Rotate(int direction)
    {
        // Calculate the amount of rotation
        float rotation = direction * rotationSpeed * Time.deltaTime;

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
        // You can perform other actions here, such as showing a victory message or triggering transitions.
    }
    public bool IsPaused()
    {
        return paused;
    }
}
