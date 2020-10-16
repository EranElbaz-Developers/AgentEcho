using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    private SpriteRenderer renderer;
    private float health;

    public delegate void Death(GameObject go);

    public event Death OnDeath;

    private void Awake()
    {
        health = 100;
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            health -= other.GetComponent<PlayerShooting>().damage;
        }

        if (health <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath(gameObject);
            }
            Destroy(gameObject);
        }
    }
}