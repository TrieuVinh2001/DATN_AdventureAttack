using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float damage;
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
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            enemy.TakeDamage(damage);
            //enemy.TranformUp(5f);
            if (enemy.currentHealth <= 0)
            {
                GetComponentInParent<PlayerStats>().GetExp(enemy.exp);
            }
            rb.velocity = Vector2.zero;
            GameObject bulletExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(bulletExplosion, 0.5f);
            Destroy(gameObject);
        }
    }
}
