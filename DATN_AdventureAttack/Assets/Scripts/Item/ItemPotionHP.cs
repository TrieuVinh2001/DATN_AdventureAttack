using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPotionHP : MonoBehaviour
{
    [SerializeField] private float hp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().UsePotionHP(hp);
            Destroy(gameObject);
        }
    }
}
