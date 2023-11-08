using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosion;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().TakeDamage(damage);
            rb.velocity = Vector2.zero;
            GameObject bulletExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(bulletExplosion, 0.5f);
            Destroy(gameObject);
        }
    }
}
