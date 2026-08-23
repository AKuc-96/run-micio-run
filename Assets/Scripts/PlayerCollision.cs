using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Invulnerability Settings")]
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    private bool isInvulnerable = false;

    private int playerLayer;
    private int obstacleLayer;

    private SpriteRenderer spriteRenderer;
    private HealthSystem healthSystem;

    public int CurrentLives => healthSystem != null ? healthSystem.CurrentLives : 0;
    public int BonusCoins => healthSystem != null ? healthSystem.BonusCoins : 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("Obstacle"); 
    }
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            healthSystem = GameManager.Instance.Health;
        }
        GameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer()
    {
        healthSystem?.ResetLives(healthSystem.CurrentLives);
        isInvulnerable = false;

        SetObstacleCollision(false);
        
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        gameObject.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        } 

        else if (other.gameObject.CompareTag("Extra Life"))
        {
            AddLife();
        }
    }

    public void AddLife(int amount = 1)
    {
        healthSystem.AddLife(amount);
        Debug.Log($"Жизнь/бонус обработаны! Тек. жизней: {healthSystem.CurrentLives} | Бонусов: {healthSystem.BonusCoins} ");
    } 

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        healthSystem.TakeDamage(damage);
        Debug.Log($"Кот получил урон! Осталось жизней: {healthSystem.CurrentLives}");

        if (healthSystem.CurrentLives <= 0)
        {
            SetObstacleCollision(false);

            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true; 

        SetObstacleCollision(true);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }

        yield return new WaitForSeconds(invulnerabilityDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        SetObstacleCollision(false);

        isInvulnerable = false;
    }

    private void SetObstacleCollision(bool ignore)
    {
        if (playerLayer != -1 && obstacleLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, ignore);
        }
    }
}
