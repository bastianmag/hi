using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Field of View Settings")]
    [SerializeField] Transform Target;
    NavMeshAgent agent;
    public float radius = 5f;
    public float speed = 5f;
    public bool DebugCuz = false;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask playerLayer;
    public LayerMask wallsAnyThingThatsSupposedToBlockView;
    public GameObject player;
    private float distance;
    private Vector3 lastSeePlayer;
    private Quaternion LastSeePlayer;
    public bool CanSeePlayer { get; private set; }
    public GameObject yea;

    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D

    // Shooting-related variables
    public GameObject projectilePrefab; // The projectile to shoot
    public float projectileSpeed = 5f;  // Speed of the projectile
    public float shootInterval = 1f;    // Time between each shot
    private float shootTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindWithTag("Player");
        boxCollider = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component
        StartCoroutine(FOVCheck());
        lastSeePlayer = transform.position;
        transform.rotation = transform.rotation;
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        // Proper check if yea is active
        if (yea.activeSelf)  // Check if 'yea' is active in the scene
        {
            // Check if the player is within the radius
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;

                // Check if the player is within the field of view (cone)
                if (Vector2.Angle(transform.right, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, target.position);

                    // Check if there are obstacles in the way
                    if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallsAnyThingThatsSupposedToBlockView))
                    {
                        CanSeePlayer = true;
                    }
                    else
                    {
                        CanSeePlayer = false;
                    }
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else if (CanSeePlayer)
            {
                CanSeePlayer = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (DebugCuz)
        {
            Gizmos.color = Color.white;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

            // Draw the cone (field of view) with a 90-degree rotation
            Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z + 90, angle / 2);  // 90-degree shift
            Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z + 90, -angle / 2); // 90-degree shift

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
            Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);
            if (CanSeePlayer)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private bool isFirstFrame = true;
    void Update()
    {
        // Check if 'yea' is active in the scene
        if (yea.activeSelf)
        {
            // Enable the BoxCollider2D when 'yea' is active
            if (boxCollider != null && !boxCollider.enabled)
                boxCollider.enabled = true;

            if (CanSeePlayer)
            {
                lastSeePlayer = player.transform.position;
            }

            distance = Vector2.Distance(transform.position, lastSeePlayer);
            Vector2 direction = lastSeePlayer - transform.position;
            direction.Normalize();
            float anglef = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // If the agent is close to the target
            if (distance < 0.1f)
            {
                Vector2 srt = transform.GetChild(0).position - transform.position;
                float anglefs = Mathf.Atan2(srt.y, srt.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(Vector3.forward * anglefs);
                isFirstFrame = false;
            }
            else
            {
                if (distance > 0.1f)
                {
                    // If there's no obstruction between the agent and the target
                    if (!Physics2D.Raycast(transform.position, direction, distance, wallsAnyThingThatsSupposedToBlockView))
                    {
                        agent.SetDestination(lastSeePlayer);
                        transform.rotation = Quaternion.Euler(Vector3.forward * anglef);
                    }
                }
                else
                {
                    // Ensure rotation is smooth even when distance is close
                    if (!Mathf.Approximately(transform.eulerAngles.z, anglef))
                    {
                        transform.rotation = Quaternion.Euler(Vector3.forward * anglef);
                    }
                }
            }

            // Shooting at the player if in sight
            if (CanSeePlayer)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootInterval)
                {
                    ShootAtPlayer();
                    shootTimer = 0f; // Reset the timer
                }
            }
        }
        else
        {
            // Stop the agent and disable the BoxCollider2D when 'yea' is not active
            agent.isStopped = true;  // Stop the movement
            agent.ResetPath();       // Clear any current path

            // Disable the BoxCollider2D when 'yea' is inactive
            if (boxCollider != null && boxCollider.enabled)
                boxCollider.enabled = false;
        }
    }

    void ShootAtPlayer()
    {
        // Calculate the direction toward the player
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Add an offset to spawn the projectile a little further out
        float offsetDistance = 0.5f; // Adjust this value to control how far the projectile spawns from the enemy
        Vector2 spawnPosition = (Vector2)transform.position + direction * offsetDistance;

        // Instantiate the projectile at the adjusted position
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Get the Rigidbody2D component and apply velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Optional: Rotate the projectile to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}