using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CommonUIManager : MonoBehaviour
{
    public static CommonUIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CommonUIManager>();
            }
            return m_instance;
        }
    }

    public Button m_homeBtn;
    public Image m_fadeImage;
    public Canvas m_CommonUICanvas;
    public NumberDisplay m_myMoneyDisplay;

    static CommonUIManager m_instance;
    bool m_isAnimatingMonetText;
    long m_startMoneyToAnimate;
    long m_targetMoneyToAnimate;
    long m_currentMoneyToAnimate;
    const float m_timeIncreasingMoney = 1.25f;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_CommonUICanvas.gameObject);
        ApplyMyMoneyText();
        m_homeBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(m_isAnimatingMonetText)
        {
            m_currentMoneyToAnimate += (int)((m_targetMoneyToAnimate - m_startMoneyToAnimate) / m_timeIncreasingMoney * Time.deltaTime);

            if(m_currentMoneyToAnimate >= m_targetMoneyToAnimate)
            {
                ApplyMyMoneyText();
                m_isAnimatingMonetText = false;
            }
            else
            {
                m_myMoneyDisplay.SetNumber(m_currentMoneyToAnimate);
            }
            
        }
    }

    #region Public Method
    public void ApplyMyMoneyText()
    {
        m_myMoneyDisplay.SetNumber(PlayerDataManager.Instance.m_playerData.m_myCurrentMoney);
    }
    public void AnimateIncreasingMyMoneyText(long startMoney, long targetMoney)
    {
        m_startMoneyToAnimate = startMoney;
        m_targetMoneyToAnimate = targetMoney;
        m_currentMoneyToAnimate = m_startMoneyToAnimate;
        m_isAnimatingMonetText = true;
    }
    public void SetActiveHomeButton(bool active)
    {
        m_homeBtn.gameObject.SetActive(active);
    }
    public void SetActiveCommonUI(bool active)
    {
        m_CommonUICanvas.enabled = active;
    }
    #endregion

    #region Private Method

    #endregion
}
