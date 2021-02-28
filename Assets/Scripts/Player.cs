using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")] [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 1f;
    [SerializeField] private int health = 200;
    [SerializeField] private AudioClip playerKilledSfx;
    [Range(0,1)][SerializeField] private float playerKilledVolume = 1f;
    [SerializeField] private AudioClip playerLaserSfx;
    [Range(0,1)][SerializeField] private float playerLaserVolume = 0.25f;

    [Header("Projectile")] [SerializeField]
    private GameObject laserPrefab;

    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;

    private bool isFiring = false;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(nameof(FireContinuously));
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(nameof(FireContinuously));
            isFiring = false;
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerLaserSfx, Camera.main.transform.position, playerLaserVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetUpMoveBoundaries()
    {
        var gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        var position = transform.position;
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(position.y + deltaY, yMin, yMax);
        position = new Vector2(newXPos, newYPos);
        transform.position = position;
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
            FindObjectOfType<Level>().LoadGameOver();
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(playerKilledSfx, Camera.main.transform.position, playerKilledVolume);
        }
    }
}