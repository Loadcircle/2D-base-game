using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            GameController gameController = FindObjectOfType<GameController>();

            if (playerController != null && !playerController.IsPaused() && playerController.IsActiveCollisions())
            {
                gameController.EndGame();
            }
        }
    }
}
