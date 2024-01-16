using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    DataBase data;

    private void Start()
    {
        data = SaveData.instance.data;
    }
    public void Buy1PotionHP()
    {
        if (data.coin >= 10)
        {
            data.coin -= 10;
            data.countPotionHP += 1;
            SaveDataPlayer();
        }
    }

    public void Buy10PotionHP()
    {
        if (data.coin >= 90)
        {
            data.coin -= 90;
            data.countPotionHP += 10;
            SaveDataPlayer();
        }
    }

    public void Buy1PotionMP()
    {
        if (data.coin >= 10)
        {
            data.coin -= 10;
            data.countPotionMP += 1;
            SaveDataPlayer();
        }
    }

    public void Buy10PotionMP()
    {
        if (data.coin >= 90)
        {
            data.coin -= 90;
            data.countPotionMP += 10;
            SaveDataPlayer();
        }
    }

    private void SaveDataPlayer()
    {
        SaveData.instance.SaveToJson();
        GameManager.instance.UpdateUI();
    }
}
