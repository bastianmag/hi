using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public int bulletsPerBurst = 3;       // Number of bullets per burst
    public float timeBetweenBullets = 0.1f; // Delay between bullets in a burst
    public float timeBetweenBursts = 0.5f;  // Delay between bursts

    private int bulletsShot = 0;
    private float lastBurstTime = 0f;
    private float lastBulletTime = 0f;
    private bool isBursting = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastBurstTime + timeBetweenBursts)
        {
            Debug.Log("Burst started!");
            isBursting = true;
            bulletsShot = 0;
            lastBurstTime = Time.time;
        }

        if (isBursting && bulletsShot < bulletsPerBurst && Time.time >= lastBulletTime + timeBetweenBullets)
        {
            Debug.Log($"Shooting bullet {bulletsShot + 1}.");
            Shoot();
            bulletsShot++;
            lastBulletTime = Time.time;

            if (bulletsShot >= bulletsPerBurst)
            {
                Debug.Log("Burst completed.");
                isBursting = false; // End burst
            }
        }
    }

    void Shoot()
    {
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component from the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply force to the bullet
        if (rb != null)
        {
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Bullet does not have a Rigidbody2D component!");
        }
    }
}