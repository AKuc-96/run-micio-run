using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    // private GameObject spawnedBoss; 
    public float bossSpeed = 2f; 
    private Vector3 stopAt = new(6, -3, 0);


    [Header("Retreat Settings")]
    [SerializeField] private float retreatSpeed = 6f;
    [SerializeField] private float retreatTargetX = 20f;

    private Rigidbody2D bossRB;
    private bool isRetreating = false;


    void Start()
    {
        bossRB = GetComponent<Rigidbody2D>();
        
        if (bossRB == null)
        {
            Debug.LogError($"На объекте {gameObject.name} отсутствует компонент RigidBody2D!");
        }
    } 

    void Update()
    {
        if (bossRB == null) return;

        if (isRetreating)
        {
            bossRB.linearVelocity = Vector3.right * retreatSpeed;

            if (transform.position.x >= retreatTargetX)
            {
                Debug.Log("Босс успешно удрал. Клон уничтожен.");
                Destroy(gameObject);
            }

            return;
        }

        bossRB.linearVelocity = Vector3.left * bossSpeed; 

        if (Vector3.Distance(transform.position, stopAt) < 0.1f)
        {
            bossSpeed = 0f;
        }
        
    }

    public void StartRetreat()
    {
        isRetreating = true;
        Debug.Log("У босса кончились патроны! Запускаем протокол отступления.");
    }
}
