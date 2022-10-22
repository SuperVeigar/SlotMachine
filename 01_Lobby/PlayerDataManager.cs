using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class PlayerData
{
    public int m_myCurrentMoney;
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerDataManager>();
            }
            return m_instance;
        }
    }

    const int m_startMoney = 1000000;
    string m_filePath;
    PlayerData m_playerData;
    static PlayerDataManager m_instance;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_filePath = Path.Combine(Application.dataPath, "PlayerData.json");
        m_playerData = new PlayerData();    
        LoadPlayerData();
        SavePlayerData();
    }

    #region Public Method
    public void AddPlayerCurrentMoney(int changedMoney)
    {
        if (m_playerData != null)
        {
            m_playerData.m_myCurrentMoney += changedMoney;
            SavePlayerData();

            Debug.Log("Add and save : " + changedMoney);
        }


    }
    #endregion Public Method

    #region Private Method
    void LoadPlayerData()
    {
        if (File.Exists(m_filePath))
        {
            string loadJason = File.ReadAllText(m_filePath);

            m_playerData = JsonUtility.FromJson<PlayerData>(loadJason);
        }
        else
        {
            m_playerData.m_myCurrentMoney = m_startMoney;
        }
    }

    void SavePlayerData()
    {
        string json = JsonUtility.ToJson(m_playerData, true);

        File.WriteAllText(m_filePath, json);
    }
    #endregion Private Method
}
