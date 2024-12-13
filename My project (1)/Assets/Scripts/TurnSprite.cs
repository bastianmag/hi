using UnityEngine;
using UnityEngine.UI; // Needed to interact with UI elements
using System.Collections; // Needed for Coroutine

public class TurnSprite : MonoBehaviour
{
    [Header("Child GameObjects")]
    public GameObject turnChild1; // Reference to the first child GameObject (alive)
    public GameObject turnChild2; // Reference to the second child GameObject (dead)

    [Header("Sound Effects")]
    public AudioSource turnSound; // Sound effect to play during turn transitions

    private bool isDead = false; // Flag to track if the object is already dead

    void Start()
    {
        // Ensure that the first child is active and the second is inactive at the start
        if (turnChild1 != null && turnChild2 != null)
        {
            turnChild1.SetActive(true);  // Activate the alive sprite
            turnChild2.SetActive(false); // Deactivate the dead sprite
        }
        else
        {
            Debug.LogWarning("turnChild1 or turnChild2 is not assigned in the inspector.");
        }
    }

    // This method is called when the object collides with another object (bullet)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Log the collision details for debugging purposes
        Debug.Log("Collision with: " + collision.gameObject.name);

        if (!isDead && collision.gameObject.CompareTag("Bullet"))
        {
            // Trigger the "death" sequence if not already dead
            Die();
        }
    }

    // Method to handle the transition to the "dead" state
    public void Die()
    {
        if (isDead) return; // Exit if already dead

        // Set the isDead flag to true to prevent further changes
        isDead = true;

        Debug.Log("Enemy has died!");  // Debug log to confirm the method is triggered

        // Disable the alive sprite (turnChild1) and enable the dead sprite (turnChild2)
        if (turnChild1 != null)
        {
            transform.GetChild(1).gameObject.SetActive(false); // Deactivate the alive sprite
            Debug.Log("turnChild1 (alive) is now disabled.");
        }
        if (turnChild2 != null)
        {
            turnChild2.SetActive(true);  // Activate the dead sprite
            Debug.Log("turnChild2 (dead) is now enabled.");
        }

        // Play the death sound effect, if assigned and it's not already playing
        if (turnSound != null && !turnSound.isPlaying)
        {
            turnSound.Play();
        }
    }
}