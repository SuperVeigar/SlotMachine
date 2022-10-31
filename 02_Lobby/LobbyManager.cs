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
    public Image m_emptySpecialBonusImage;
    public Image m_specialBonusImage;
    public ParticleSystem m_specialBonusParticle;
    public ParticleSystem m_gatheringCoinsParticle;
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
        CommonUIManager.Instance.SetActiveCommonUI(true);
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
        CommonUIManager.Instance.AnimateIncreasingMyMoneyText(m_playerData.m_myCurrentMoney, m_playerData.m_myCurrentMoney + m_specialBonusMoneyAmount);
        PlayerDataManager.Instance.AddPlayerCurrentMoney(m_specialBonusMoneyAmount);        
        PlayerDataManager.Instance.ResetSpecialBonusTime();
        m_audioSource.PlayOneShot(m_collectingCashSound);
        m_gatheringCoinsParticle.gameObject.SetActive(false);
        m_gatheringCoinsParticle.gameObject.SetActive(true);
    }
    public void StartRockClimberGame()
    {
        CommonUIManager.Instance.ResetCommonUI();
        CommonUIManager.Instance.SetGameMode();
        CommonUIManager.Instance.SetActiveCommonUI(false);
        SceneManager.LoadScene("03_0_LoadingScene_RockClimber");
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
        m_specialBonusImage.gameObject.SetActive(setOn);
        m_specialBonusImage.GetComponent<Animator>().enabled = setOn;

        m_emptySpecialBonusImage.gameObject.SetActive(!setOn);
        m_specialBonusInText.gameObject.SetActive(!setOn);
        m_specialBonusTimeText.gameObject.SetActive(!setOn);
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
