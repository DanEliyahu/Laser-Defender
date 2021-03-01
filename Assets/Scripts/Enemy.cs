using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")] [SerializeField] private float health = 500;
    [SerializeField] private int scoreValue = 150;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private float durationOfExplosion = 1f;
    [SerializeField] private AudioClip enemyKilledSfx;
    [Range(0, 1)] [SerializeField] private float enemyKilledVolume = 0.4f;

    [Header("Enemy Laser")] [SerializeField]
    private float shotCounter; // for Debug

    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private AudioClip enemyLaserSfx;
    [Range(0, 1)] [SerializeField] private float enemyLaserVolume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(enemyLaserSfx, Camera.main.transform.position, enemyLaserVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.Damage;
        damageDealer.Hit();
        if (health <= 0)
        {
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            AudioSource.PlayClipAtPoint(enemyKilledSfx, Camera.main.transform.position, enemyKilledVolume);
            var explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(explosion, durationOfExplosion);
        }
    }
}