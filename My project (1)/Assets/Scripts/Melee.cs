using UnityEngine;

public class Melee : MonoBehaviour
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
            isBursting = true;
            bulletsShot = 0;
            lastBurstTime = Time.time;
        }

        if (isBursting && bulletsShot < bulletsPerBurst && Time.time >= lastBulletTime + timeBetweenBullets)
        {
            Shoot();
            bulletsShot++;
            lastBulletTime = Time.time;

            if (bulletsShot >= bulletsPerBurst)
            {
                isBursting = false; // End burst
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        Destroy(bullet, 0.05f);
    }
}