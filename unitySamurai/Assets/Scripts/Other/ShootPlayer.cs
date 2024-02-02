using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform playerTransform;
    public float projectileSpeed = 5f;
    public float shootInterval = 2f; // Time between shots

    private float lastShootTime = 0f;

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time - lastShootTime > shootInterval)
        {
            // Instantiate the projectile at the current position of this GameObject
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Calculate the direction towards the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Set the projectile's initial velocity
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.velocity = directionToPlayer * projectileSpeed;
            }

            // Update the last shoot time
            lastShootTime = Time.time;
        }
    }
}