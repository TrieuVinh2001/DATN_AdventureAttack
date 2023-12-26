using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP;
    [SerializeField] private float maxMP;
    public float damage;
    private float currentHP;
    private float currentMP;

    [SerializeField] private int level;
    [SerializeField] private int currentExp;
    [SerializeField] private int expToLevel;

    private float timeCooldownAttack;

    [Header("ManaSkill")]
    [SerializeField] private float manaAttack4;
    [SerializeField] private float manaAttack5;
    [SerializeField] private float manaAttack6;

    [Header("TimeSkill")]
    [SerializeField] private float timeAttack4;
    [SerializeField] private float timeAttack5;
    [SerializeField] private float timeAttack6;

    [Header("DamageSkill")]
    [SerializeField] private float damageAttack4;
    [SerializeField] private float damageAttack5;
    [SerializeField] private float damageAttack6;

    [Header("TimeCooldown")]
    [HideInInspector] public float timeCooldown4;
    [HideInInspector] public float timeCooldown5;
    [HideInInspector] public float timeCooldown6;

    [SerializeField] private GameObject floatingText;

    [Header("UI")]
    [SerializeField] private Image hpImage;
    [SerializeField] private Image hpEffectImage;
    [SerializeField] private Image mpImage;
    [SerializeField] private Image mpEffectImage;
    [SerializeField] private Image expImage;
    [SerializeField] private float hurtSpeed = 0.005f;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI expText;


    // Start is called before the first frame update
    void Start()
    {

        DataBase data = SaveData.instance.data;
        maxHP = data.maxHP;
        maxMP = data.maxMP;
        damage = data.damage;
        level = data.level;
        currentExp = data.currentExp;
        expToLevel = data.expToLevel;
        currentHP = maxHP;
        currentMP = maxMP;
        StartCoroutine(ManaRecovery());
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar();
        ManaBar();
        ExpBar();
        timeCooldown4 -= Time.deltaTime;
        timeCooldown5 -= Time.deltaTime;
        timeCooldown6 -= Time.deltaTime;
        timeCooldownAttack -= Time.deltaTime;
    }

    public void ManaConsumption(float mana)
    {
        currentMP -= mana;
    }

    private IEnumerator ManaRecovery()//Hồi mana mỗi 3s
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (currentMP + 1 <= maxMP)
            {
                currentMP += 1;
            }
        }
        
    }

    public void UsePotionHealth()
    {
        if (currentHP + 20 > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += 20;
        }
    }

    public void UsePotionMana()
    {
        if (currentMP + 10 > maxMP)
        {
            currentMP = maxMP;
        }
        else
        {
            currentMP += 10;
        }
    }

    public void TakeDamage(float damage)//Nhận sát thương
    {
        GameObject point = Instantiate(floatingText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            //Death();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
    }

    private void HealthBar()
    {
        hpImage.fillAmount = currentHP / maxHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;

        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }

    private void ManaBar()
    {
        mpImage.fillAmount = currentMP / maxMP;
        mpText.text = "MP: " + currentMP + "/" + maxMP;

        if (mpEffectImage.fillAmount > mpImage.fillAmount)
        {
            mpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            mpEffectImage.fillAmount = mpImage.fillAmount;
        }
    }

    private void ExpBar()
    {
        expImage.fillAmount = (float)currentExp / expToLevel;
        expText.text = "LV:" + level + "+" + Mathf.RoundToInt(((float)currentExp /expToLevel) * 100) + "%";
    }

    public void AttackCombo()
    {
        timeCooldownAttack = 0.5f;
    }

    public void Attack4()
    {
        ManaConsumption(manaAttack4);
        timeCooldown4 = timeAttack4;
    }
    public void Attack5()
    {
        ManaConsumption(manaAttack5);
        timeCooldown5 = timeAttack5;
    }
    public void Attack6()
    {
        ManaConsumption(manaAttack6);
        timeCooldown6 = timeAttack5;
    }

    public bool CanAttack4()
    {
        if(timeCooldown4 < 0 && currentMP - manaAttack4 > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanAttack5()
    {
        if (timeCooldown5 < 0 && currentMP - manaAttack5 > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanAttack6()
    {
        if (timeCooldown6 < 0 && currentMP - manaAttack6 > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public  bool CanAttack()
    {
        if (timeCooldownAttack < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void UsePotionHP(float hp)
    {

        if (currentHP + hp < maxHP)
        {
            currentHP += hp;
        }
        else
        {
            currentHP = maxHP;
        }
        
    }

    public void UsePotionMP(float mp)
    {
        if (currentMP + mp < maxMP)
        {
            currentMP += mp;
        }
        else
        {
            currentMP = maxMP;
        }
    }

    public void GetExp(int exp)//Nhận kinh nghiệm
    {
        currentExp += exp;
        if (currentExp >= expToLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()//Tăng cấp
    {
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * 1.2f);
        level++;

        //Tăng các chỉ số
        maxHP += 5;
        maxMP ++;
        damage++;

        currentHP = maxHP;
        currentMP = maxMP;

        //Cập nhật dữ liệu trong data
        SaveDataPlayer();

    }

    private void OnApplicationQuit()
    {
        SaveDataPlayer();
    }

    private void SaveDataPlayer()
    {
        DataBase data = SaveData.instance.data;
        data.maxHP = maxHP;
        data.maxMP = maxMP;
        data.currentHP = currentHP;
        data.currentMP = currentMP;
        data.damage = damage;
        data.level = level;
        data.currentExp = currentExp;
        data.expToLevel = expToLevel;

        SaveData.instance.SaveToJson();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.GetCoin(collision.GetComponent<Coin>().coin);
            Destroy(collision.gameObject);
        }
    }
}
