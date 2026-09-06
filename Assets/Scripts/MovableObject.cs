using Codice.CM.Client.Differences;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Header ("Base Movement Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float destroyXPosition = -15f; 
    [SerializeField] protected float lifetime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        CheckBounds();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    protected virtual void CheckBounds()
    {
        if (transform.position.x <= destroyXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
