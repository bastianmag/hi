using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3 (0, 0, -10);

    public float mouseSensitivity = 0.1f; // Sensitivity for mouse movement
    public KeyCode panKey = KeyCode.LeftShift; // Key to hold for mouse panning

    private Vector3 playerPosition;

    private void Start()
    {
        if (player != null)
        {
            playerPosition = player.position + offset;
            transform.position = playerPosition;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Input.GetKey(panKey))
            {
                float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;
                playerPosition += new Vector3(moveX, moveY, 0);
            }
            else
            {
                // Shift not held: follow the player with the offset
                playerPosition = player.position + offset;
            }

            transform.position = Vector3.Lerp(transform.position, playerPosition, 0.1f);



            //transform.position = player.position + offset;

        }
    }
}
