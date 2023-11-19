using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPotionMP : MonoBehaviour
{
    [SerializeField] private float mp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().UsePotionMP(mp);
            Destroy(gameObject);
        }
    }
}
