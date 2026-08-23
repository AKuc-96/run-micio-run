using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5.0f;
    [SerializeField] private float bulletLifetime = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy (gameObject, bulletLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime);
    }
}
