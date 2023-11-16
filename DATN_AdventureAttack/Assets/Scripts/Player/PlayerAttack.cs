using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            enemy.TakeDamage(damage);
            if (enemy.currentHealth <= 0)
            {
                GetComponentInParent<PlayerStats>().GetExp(enemy.exp);
            }
        }
    }
}
