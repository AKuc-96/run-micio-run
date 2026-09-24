using UnityEngine;

public class BossShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 3.0f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Bullet Counter")]
    [Tooltip("Сколько пуль босс может выпустить за фазу/атаку")]
    
    private float nextFireTime = 0.0f; 

    // ссылка на скрипт с передвижением босса
    private BossMovement bossMovement;

    private bool coordinatesSent = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (bossMovement == null) bossMovement = GetComponent<BossMovement>();
        GameManager.onGameOver += ClearRemainingBullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossMovement.hasStopped)
            {
                if (Time.time >= nextFireTime)
                    {
                        Shoot();
                        nextFireTime = Time.time + fireRate;
                    }   
            }
    }

    private void Shoot()
    {
        if (!GameManager.Instance.isPlaying)
        {
            return;
        }
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void ClearRemainingBullets()
    {
        MovableObject[] activeBullets = FindObjectsByType<MovableObject>(FindObjectsSortMode.None);

        foreach (MovableObject bullet in activeBullets)
        {
            Destroy(bullet.gameObject);
        }

        Debug.Log("Все пули зачищенны со сцены!");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.onGameOver -= ClearRemainingBullets;
        }
    }

}
