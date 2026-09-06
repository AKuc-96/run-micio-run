using UnityEngine;

public class ExtraLife : MovableObject
{
    [SerializeField] private int livesToGive = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollision player = other.GetComponent<PlayerCollision>();
            if (player != null)
            {
                player.AddLife(livesToGive);
            }

            Destroy(gameObject);
        }
    }
}
