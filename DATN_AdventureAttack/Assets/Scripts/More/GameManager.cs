using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private GameObject shopPanel;

    [SerializeField] private TextMeshProUGUI countPotionHP;
    [SerializeField] private TextMeshProUGUI countPotionMP;

    DataBase data;

    private void Awake()
    {
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1;
    }

    private void Start()
    {
        data = SaveData.instance.data;
        UpdateUI();
    }

    private void Update()
    {
        HealthBar();
        ManaBar();
        ExpBar();
    }

    public void GetCoin(int coinCount)
    {
        data.coin += coinCount;
        UpdateUI();

        SaveData.instance.SaveToJson();
    }

    public void UpdateUI()
    {
        coinText.text = "" + data.coin;
        countPotionHP.text = "" + data.countPotionHP;
        countPotionMP.text = "" + data.countPotionMP;
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void HealthBar()
    {
        hpImage.fillAmount = data.currentHP / data.maxHP;
        hpText.text = "HP: " + data.currentHP + "/" + data.maxHP;

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
        mpImage.fillAmount = data.currentMP / data.maxMP;
        mpText.text = "MP: " + data.currentMP + "/" + data.maxMP;

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
        expImage.fillAmount = (float)data.currentExp / data.expToLevel;
        expText.text = "LV:" + data.level + "+" + Mathf.RoundToInt(((float)data.currentExp / data.expToLevel) * 100) + "%";
    }
}
