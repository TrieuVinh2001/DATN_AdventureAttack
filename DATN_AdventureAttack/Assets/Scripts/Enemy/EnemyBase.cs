using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    public float currentHealth;
    public int exp;
    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    [SerializeField] private int countCoin;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField] protected GameObject floatingText;

    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;
    [SerializeField] private float hurtSpeed = 0.1f;

    private SpriteRenderer sp;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        sp = GetComponentInChildren<SpriteRenderer>();
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
        StartCoroutine(DamageColor());
        currentHealth -= dame;
        if (currentHealth <= 0)
        {
            //Death();
            int count = Random.Range(countCoin - 1, countCoin + 2);
            for(int i = 0; i < count; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                coin.GetComponent<Coin>().minCoin = minCoin;
                coin.GetComponent<Coin>().maxCoin = maxCoin;
            }

            Destroy(gameObject);
        }
    }

    public virtual void Death()
    {

    }

    IEnumerator DamageColor()
    {
        sp.color = Color.red;//Đổi sang màu đỏ
        yield return new WaitForSeconds(0.2f);
        sp.color = Color.white;//Trở về màu ban đầu
    }
}
