using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemDrop
{
    [Range(0f, 1f)]
    public float rate;
    public GameObject itemPrefab;
}

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth;
    public float currentHealth;
    public int exp;

    [Header("Drop Coin")]
    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    [SerializeField] private int countCoin;
    [SerializeField] private GameObject coinPrefab;

    [Header("Drop Item")]
    [SerializeField] private ItemDrop[] itemDrops;

    [Header("UI")]
    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;
    [SerializeField] private float hurtSpeed = 0.1f;

    [SerializeField] protected GameObject floatingText;
    [SerializeField] protected GameObject hitPrefab;
    private SpriteRenderer sp;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        sp = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        Instantiate(hitPrefab, transform.position, Quaternion.identity);
        StartCoroutine(DamageColor());
        currentHealth -= dame;
        if (currentHealth <= 0)
        {
            
            //Death();
            DropCoin();
            DropItem();
            Destroy(gameObject);
        }
    }

    public void TranformUp(float addForce)
    {
        rb.velocity = new Vector2(addForce, rb.velocity.y);
    }

    public void DropCoin()
    {
        int count = Random.Range(countCoin - 1, countCoin + 2);
        for (int i = 0; i < count; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Coin>().minCoin = minCoin;
            coin.GetComponent<Coin>().maxCoin = maxCoin;
        }
    }

    private List<GameObject> itemRandomDrop;
    public void DropItem()
    {
        if (itemDrops == null || itemDrops.Length <= 0) return;//Nếu k có item thì thôi
        itemRandomDrop = new List<GameObject>();//Tạo list mới
        float rateRandom = Random.Range(0f, 1f);//Tỉ lệ ngẫu nhiên
        for (int i = 0; i < itemDrops.Length; i++)
        {
            if (itemDrops[i].rate <= rateRandom)//Nếu tỉ lệ của item <= tỉ lệ ngẫu nhiên
            {
                itemRandomDrop.Add(itemDrops[i].itemPrefab);//Thêm prefab item vào list
            }
        }

        if (itemRandomDrop.Count <= 0) return;//Nếu list trống
        //Tạo item
        var randomItem = itemRandomDrop[Random.Range(0, itemRandomDrop.Count)];
        Instantiate(randomItem, transform.position, Quaternion.identity);
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
