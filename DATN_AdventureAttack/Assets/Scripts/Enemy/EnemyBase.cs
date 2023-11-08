using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    [SerializeField] protected GameObject floatingText;

    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;
    [SerializeField] private float hurtSpeed = 0.001f;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        hpImage.fillAmount = currentHealth / maxHealth;

        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }

    public void TakeDamage(float dame)
    {
        GameObject point = Instantiate(floatingText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + dame;
        currentHealth -= dame;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {

    }
}
