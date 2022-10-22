using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<LobbyManager>();
            }
            return m_instance;
        }
    }
        
    public Text m_specialBonusInText;
    public Text m_specialBonusTimeText;
    public Text m_collectSpecialBonusText;
    public Button m_specialBonusButton;
    public ParticleSystem m_specialBonusParticle;
    public AudioClip m_collectingCashSound;

    static LobbyManager m_instance;
    bool m_isReadySpecialBonus;
    static int m_specialBonusMoneyAmount = 100000;
    PlayerData m_playerData;
    AudioSource m_audioSource;


    // Start is called before the first frame update
    void Start()
    {
        m_playerData = PlayerDataManager.Instance.m_playerData;
        SetActiveCollectSpecialBonus(false);
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TestGame();
    }

    private void FixedUpdate()
    {
        UpdateSpecialBonus();
    }

    #region Public Method
    public void CollectSpecialBonus()
    {
        SetActiveCollectSpecialBonus(false);
        Debug.Log("1 : " + m_playerData.m_myCurrentMoney);
        CommonUIManager.Instance.AnimateIncreasingMyMoneyText(m_playerData.m_myCurrentMoney, m_playerData.m_myCurrentMoney += m_specialBonusMoneyAmount);
        Debug.Log("2 : " + m_playerData.m_myCurrentMoney);
        PlayerDataManager.Instance.AddPlayerCurrentMoney(m_specialBonusMoneyAmount);        
        PlayerDataManager.Instance.ResetSpecialBonusTime();
        m_audioSource.PlayOneShot(m_collectingCashSound);
    }
    public void EnterRockClimberGame()
    {
        SceneManager.LoadScene("02_GamcScene_RockClimber");
    }   
    #endregion Public Method

    #region Private Method
    void UpdateSpecialBonus()
    {
        if(m_playerData.m_timeToCollectSpecialBonus > DateTime.Now)
        {
            TimeSpan remainedTime = m_playerData.m_timeToCollectSpecialBonus - DateTime.Now;
            m_specialBonusTimeText.text = string.Format("{0:D2} : {1:D2}", remainedTime.Minutes, remainedTime.Seconds);
        }
        else
        {
            if(!m_isReadySpecialBonus)
            {
                SetActiveCollectSpecialBonus(true);
            }
        }
    }

    void SetActiveCollectSpecialBonus(bool setOn)
    {
        m_isReadySpecialBonus = setOn;
        m_collectSpecialBonusText.gameObject.SetActive(setOn);
        m_specialBonusParticle.gameObject.SetActive(setOn);
        m_specialBonusButton.GetComponent<Animator>().enabled = setOn;

        m_specialBonusInText.gameObject.SetActive(!setOn);
        m_specialBonusTimeText.gameObject.SetActive(!setOn);

        m_specialBonusButton.interactable = setOn;
    }
    #endregion Private Method

    #region Test Method
    void TestGame()
    {
        if(InputManager.Instance.CheckKeyDown(GameKey.TimeForSpecialBonus))
        {
            PlayerDataManager.Instance.SetSpecialBonusTime(5);
        }
    }
    #endregion Test Method
}
