using UnityEngine;
using UnityEngine.UIElements;

public class TurnWeapon : MonoBehaviour
{

    public enum Turn { PlayerTurn, EnemyTurn }
    public Turn currentTurn;

    public GameObject rifle; // Reference to the arena object
    public GameObject melee;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

            //// Start a coroutine to switch back to the player's turn after a delay
            //StartCoroutine(SwitchBackToPlayerTurnAfterDelay(enemyTurnDuration));
        }
    }


    // make it so that it change when pressing q or something !!!!!!!!
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Gun")
        {
            SwitchTurn();
        }
    }
}
