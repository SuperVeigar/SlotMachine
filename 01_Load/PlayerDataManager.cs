using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[Serializable]
public class PlayerData
{
    public long m_myCurrentMoney;
    public string m_stringToCollectSpecialBonus;

    [NonSerialized]
    public DateTime m_timeToCollectSpecialBonus;
}

public class PlayerDataManager : MonoBehaviour
{
    public event Action CompleteLoadData;

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

    public PlayerData m_playerData { get; private set; }
        
    const int m_secondsForNextSpecialBonus = 300;
    const long m_startMoney = 1000000;
    string m_filePath;    
    static PlayerDataManager m_instance;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_filePath = Path.Combine(Application.dataPath, "Resources/", "PlayerData.json");
        m_playerData = new PlayerData();    
        LoadPlayerData();
        SavePlayerData();
        CompleteLoadData();
    }
    void Update()
    {
        TestGame();
    }
    private void OnDestroy()
    {
        SavePlayerData();
    }

    #region Public Method
    public void AddPlayerCurrentMoney(int money)
    {
        if (m_playerData != null)
        {
            m_playerData.m_myCurrentMoney += money;
            SavePlayerData();
        }
    }
    public void AddPlayerCurrentMoneyAndChangeText(int money)
    {
        AddPlayerCurrentMoney(money);
        CommonUIManager.Instance.ApplyMyMoneyText();
    }
    public void ResetPlayerCurrentMoneyAndChangeText()
    {
        if (m_playerData != null)
        {
            m_playerData.m_myCurrentMoney = m_startMoney;
            SavePlayerData();
        }
        CommonUIManager.Instance.ApplyMyMoneyText();
    }
    public void ResetSpecialBonusTime()
    {
        m_playerData.m_timeToCollectSpecialBonus = DateTime.Now.AddSeconds(m_secondsForNextSpecialBonus);
        SavePlayerData();
    }
    #endregion Public Method

    #region Private Method
    void LoadPlayerData()
    {
        if (File.Exists(m_filePath))
        {
            string loadJason = File.ReadAllText(m_filePath);

            m_playerData = JsonUtility.FromJson<PlayerData>(loadJason);

            m_playerData.m_timeToCollectSpecialBonus = DateTime.Parse(m_playerData.m_stringToCollectSpecialBonus);
        }
        else
        {
            m_playerData.m_myCurrentMoney = m_startMoney;
            ResetSpecialBonusTime();
        }
    }

    void SavePlayerData()
    {
        m_playerData.m_stringToCollectSpecialBonus = m_playerData.m_timeToCollectSpecialBonus.ToString();

        string json = JsonUtility.ToJson(m_playerData, true);

        File.WriteAllText(m_filePath, json);
    }
    #endregion Private Method

    #region Test Method
    void TestGame()
    {
        if (InputManager.Instance.CheckKeyDown(GameKey.AddMoney))
        {
            AddPlayerCurrentMoneyAndChangeText(100000);
            GameDataManager.Instance.UpdateMaxBet();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.ResetMoney))
        {            
            ResetPlayerCurrentMoneyAndChangeText();
            GameDataManager.Instance.UpdateMaxBet();
        }
    }
    public void SetSpecialBonusTime(int seconds)
    {
        m_playerData.m_timeToCollectSpecialBonus = DateTime.Now.AddSeconds(seconds);
        SavePlayerData();
    }    
    #endregion Test Method
}
