using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene

public class TurnDead : MonoBehaviour
{
    public enum Turn { PlayerTurn, EnemyTurn }
    public Turn currentTurn;

    public GameObject rifle; // Reference to the arena object
    public GameObject melee;

    private int hitCount = 0; // Counter to track how many times the player has been hit
    private const int maxHits = 1; // Max hits before restarting the scene

    void Start()
    {
        currentTurn = Turn.PlayerTurn;

        if (rifle != null)
        {
            rifle.SetActive(false);
        }

        if (melee != null)
        {
            melee.SetActive(true);
        }
    }

    void SwitchTurn()
    {
        if (currentTurn == Turn.PlayerTurn)
        {
            currentTurn = Turn.EnemyTurn;

            // Enable the arena when it's the enemy's turn
            if (rifle != null)
            {
                rifle.SetActive(true);
            }

            if (melee != null)
            {
                melee.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Bullet")
        {
            // Increment hit count
            hitCount++;

            // Check if the player has been hit twice
            if (hitCount >= maxHits)
            {
                RestartScene(); // Restart the scene if hit twice
            }
            else
            {
                SwitchTurn(); // Switch turns if hit once
            }
        }
    }

    // Method to restart the scene
    void RestartScene()
    {
        // Log the scene restart for debugging
        Debug.Log("Player has been hit twice. Restarting the scene.");

        // Reload the current scene (you can also specify the scene name if needed)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}