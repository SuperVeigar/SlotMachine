using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{
    public event Action onOpenHelpPanel;
    public event Action onCloseHelpPanel;

    public enum MenuDropdownState { Open = 0, Close }
    public Button m_paytableBtn;
    public Button m_myInfoBtn; 
    public Button m_settingBtn;
    public Button m_aboutBtn; 
    public Button m_supportBtn;

    bool m_isOpen;
    const int m_lobbyMenuCount = 4;
    const int m_gameMenuCount = 5;
    const float m_menuWidth = 400f;
    float m_buttonHeight;
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        InitValues();
        SetLobbyState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void CallMenuDropdown()
    {
        if(m_isOpen)
        {
            m_animator.SetTrigger("Close");            
        }
        else
        {
            m_animator.SetTrigger("Open");
        }
        m_isOpen = !m_isOpen;
    }
    public void CallMenuDropdown(MenuDropdownState state)
    {
        if (state == MenuDropdownState.Close)
        {
            m_animator.SetTrigger("Close");
            m_isOpen = false;
        }
    }
    public void SetLobbyState()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_menuWidth, m_buttonHeight * m_lobbyMenuCount);
        m_paytableBtn.gameObject.SetActive(false);
        m_myInfoBtn.gameObject.SetActive(true);
        m_settingBtn.gameObject.SetActive(true);
        m_aboutBtn.gameObject.SetActive(true);
        m_supportBtn.gameObject.SetActive(true);

        m_myInfoBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120f);
        m_settingBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40f);
        m_aboutBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40f);
        m_supportBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120f);
    }
    public void SetGameState()
    {
        float menuHeight = m_buttonHeight * m_gameMenuCount;

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_menuWidth, menuHeight);
        m_paytableBtn.gameObject.SetActive(true);
        m_myInfoBtn.gameObject.SetActive(true);
        m_settingBtn.gameObject.SetActive(true);
        m_aboutBtn.gameObject.SetActive(true);
        m_supportBtn.gameObject.SetActive(true);

        m_paytableBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 160f);
        m_myInfoBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 80f);
        m_settingBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0f);
        m_aboutBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -80f);
        m_supportBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -160f);
    }
    public void ResetMenu()
    {
        CallMenuDropdown(MenuDropdownState.Close);
    }
    public void OpenHelpPanel()
    {
        CallMenuDropdown(MenuDropdownState.Close);
        onOpenHelpPanel();
    }
    public void CloseHelpPanel()
    {
        onCloseHelpPanel();
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_buttonHeight = m_paytableBtn.GetComponent<RectTransform>().sizeDelta.y;
        m_animator = GetComponent<Animator>();

        ResetMenu();
    }    
    #endregion
}
