using UnityEngine;

public class ExtraLifeCollision : MonoBehaviour
{

    private bool isCollected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollected) return; 

        if (collision.gameObject.CompareTag("Player"))
        {

            isCollected = true; 

            if (TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = false;
            }

            Destroy(gameObject); 
            Debug.Log("Объект допжизни разрушен!");
        }
    }
}
