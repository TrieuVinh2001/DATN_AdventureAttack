using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int coin;

    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        coin = SaveData.instance.data.coin;
        coinText.text = "" + coin;
    }

    public void GetCoin(int coinCount)
    {
        coin += coinCount;
        UpdateUI();

        SaveData.instance.data.coin = coin;
        SaveData.instance.SaveToJson();
    }

    private void UpdateUI()
    {
        coinText.text = "" + coin;
    }
}
