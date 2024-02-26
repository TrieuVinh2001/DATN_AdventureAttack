using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
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

    [HideInInspector] public DataBase data ;

    private bool checkShop = false;

    // Start is called before the first frame update
    void Start()
    {
        data = SaveData.instance.data;

        if(data.posLoadDoor != Vector3.zero)
        {
            transform.position = data.posLoadDoor;
        }
        
        StartCoroutine(ManaRecovery());
    }

    // Update is called once per frame
    void Update()
    {
        timeCooldown4 -= Time.deltaTime;
        timeCooldown5 -= Time.deltaTime;
        timeCooldown6 -= Time.deltaTime;
        timeCooldownAttack -= Time.deltaTime;

        if (checkShop)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameManager.instance.OpenShop();
            }
        }
    }

    public void ManaConsumption(float mana)
    {
        data.currentMP -= mana;
    }

    private IEnumerator ManaRecovery()//Hồi mana mỗi 3s
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (data.currentMP + 1 <= data.maxMP)
            {
                data.currentMP += 1;
            }
        }
    }

    public void TakeDamage(float damage)//Nhận sát thương
    {
        GameObject point = Instantiate(floatingText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage;
        data.currentHP -= damage;
        if (data.currentHP <= 0)
        {
            SceneManager.LoadScene("Main");
            data.currentHP = data.maxHP;
            data.currentMP = data.maxMP;
            Destroy(gameObject);
        }
    }

   

    public void AttackCombo()
    {
        timeCooldownAttack = 0.5f;
        AudioManager.instance.AttackSFX();
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
        if(timeCooldown4 < 0 && data.currentMP - manaAttack4 > 0)
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
        if (timeCooldown5 < 0 && data.currentMP - manaAttack5 > 0)
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
        if (timeCooldown6 < 0 && data.currentMP - manaAttack6 > 0)
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
        if (data.countPotionHP <= 0 || data.currentHP == data.maxHP) return;
        data.countPotionHP -= 1;
        if (data.currentHP + hp < data.maxHP)
        {
            data.currentHP += hp;
        }
        else
        {
            data.currentHP = data.maxHP;
        }
        SaveDataPlayer();
    }

    public void UsePotionMP(float mp)
    {
        if (data.countPotionMP <= 0 || data.currentMP == data.maxMP) return; 
        data.countPotionMP -= 1;
        if (data.currentMP + mp < data.maxMP)
        {
            data.currentMP += mp;
        }
        else
        {
            data.currentMP = data.maxMP;
        }
        SaveDataPlayer();
    }

    public void GetExp(int exp)//Nhận kinh nghiệm
    {
        data.currentExp += exp;
        if (data.currentExp >= data.expToLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()//Tăng cấp
    {
        data.currentExp -= data.expToLevel;
        data.expToLevel = Mathf.RoundToInt(data.expToLevel * 1.2f);
        data.level++;

        //Tăng các chỉ số
        data.maxHP += 5;
        data.maxMP ++;
        data.damage++;

        data.currentHP = data.maxHP;
        data.currentMP = data.maxMP;

        //Cập nhật dữ liệu trong data
        SaveDataPlayer();
        GameManager.instance.UpdateUI();

    }

    private void OnApplicationQuit()
    {
        SaveDataPlayer();
    }

    public void SaveDataPlayer()
    {
        SaveData.instance.SaveToJson();
        GameManager.instance.UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.GetCoin(collision.GetComponent<Coin>().coin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Shop"))
        {
            checkShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop"))
        {
            checkShop = false;
        }
    }

}
