using UnityEngine;

public class BossShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 3.0f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Bullet Counter")]
    [Tooltip("Сколько пуль босс может выпустить за фазу/атаку")]
    [SerializeField] private int maxBullets = 10;

    private int currentBulletsShot = 0;
    private float nextFireTime = 0.0f; 

    // ссылка на скрипт с передвижением босса
    private BossMovement bossMovement;

    private bool coordinatesSent = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearRemainingBullets);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBulletsShot >= maxBullets)
        {
            if (bossMovement == null) bossMovement = GetComponent<BossMovement>();

            if (bossMovement != null && !coordinatesSent)
            {
                bossMovement.StartRetreat(); // вызываем метод отступления
                coordinatesSent = true;
            }
            return;
        }

        if (bossMovement == null) bossMovement = GetComponent<BossMovement>();
            if (bossMovement == null) return;
            
        if (bossMovement.bossSpeed == 0f)
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
            
            // увеличиваем счётчик выпущенных пуль
            currentBulletsShot++;

            Debug.Log($"Босс выстрелил. Пуль осталось: {maxBullets - currentBulletsShot}");
        }
    }

    // публичный метод "на потом", чтоб уйти на новую фазу/уход
    public void ResetBulletCounter()
    {
        currentBulletsShot = 0;
    }

    private void ClearRemainingBullets()
    {
        Bullet[] activeBullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);

        foreach (Bullet bullet in activeBullets)
        {
            Destroy(bullet.gameObject);
        }

        Debug.Log("Все пули зачищенны со сцены!");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameOver.RemoveListener(ClearRemainingBullets);
        }
    }

}
