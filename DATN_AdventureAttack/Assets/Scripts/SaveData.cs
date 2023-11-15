using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    
    public DataBase data = new DataBase();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (File.Exists(Application.dataPath + "/DataFile.json"))
        {
            LoadFromJson();
        }
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/DataFile.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/DataFile.json");
        data = JsonUtility.FromJson<DataBase>(json);
    }
}
