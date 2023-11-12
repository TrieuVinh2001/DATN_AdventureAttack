using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMana;
    public float damage;
    private float currentHealth;
    private float currentMana;

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
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;


    private float hurtSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        StartCoroutine(ManaRecovery());
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar();
        ManaBar();
        timeCooldown4 -= Time.deltaTime;
        timeCooldown5 -= Time.deltaTime;
        timeCooldown6 -= Time.deltaTime;
        timeCooldownAttack -= Time.deltaTime;
    }

    public void ManaConsumption(float mana)
    {
        currentMana -= mana;
    }

    private IEnumerator ManaRecovery()//Hồi mana mỗi 3s
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (currentMana + 1 <= maxMana)
            {
                currentMana += 1;
            }
        }
        
    }

    public void UsePotionHealth()
    {
        if (currentHealth + 10 > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += 10;
        }
    }

    public void TakeDamage(float damage)
    {
        GameObject point = Instantiate(floatingText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Death();
            Destroy(gameObject);
        }
    }

    private void HealthBar()
    {
        hpImage.fillAmount = currentHealth / maxHealth;
        hpText.text = "HP: " + currentHealth + "/" + maxHealth;

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
        mpImage.fillAmount = currentMana / maxMana;
        mpText.text = "MP: " + currentMana + "/" + maxMana;

        if (mpEffectImage.fillAmount > mpImage.fillAmount)
        {
            mpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            mpEffectImage.fillAmount = mpImage.fillAmount;
        }
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
        if(timeCooldown4 < 0 && currentMana - manaAttack4 > 0)
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
        if (timeCooldown5 < 0 && currentMana - manaAttack5 > 0)
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
        if (timeCooldown6 < 0 && currentMana - manaAttack6 > 0)
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
}
