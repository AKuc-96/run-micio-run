using UnityEngine;

public class Obstacle : MovableObject
{
    [SerializeField] private int damageAmount = 1; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerCollision player = collision.gameObject.GetComponent<PlayerCollision>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }

        Destroy(gameObject);
    }
}
