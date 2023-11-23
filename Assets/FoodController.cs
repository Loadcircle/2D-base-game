using UnityEngine;

public class FoodController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            GameController gameController = FindObjectOfType<GameController>();

            if (playerController != null && !playerController.IsPaused())
            {
                Destroy(gameObject);
                gameController.PlayFoodSound();
                gameController.ScoreIncrease();
            }
        }
    }
}
