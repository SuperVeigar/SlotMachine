using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SymbolState
{
    Idle = 0, Winable, Win
}
public enum SymbolSort
{
    Pick = 0, Tent, Hook, Shoes, Helmet, Logo, Climber, Bigfoot,
    Wild = 10,
    Free = 20,
    Bonus = 30
}

public class Symbol : MonoBehaviour
{
    public bool m_isWon;
    public bool m_isDisplayingWin;
    public SymbolSort m_symboleSort;
    public Image m_winFrame;
    public Image[] m_idleImageArray;
    public GameObject m_bonusWinAnim;
    public GameObject m_freeWinAnim;
    public Animator m_winEffect;

    int m_row;
    int m_col;
    SymbolState m_symbolState;
    GameObject m_currentSymbol;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_symbolState)
        {
            case SymbolState.Idle:
                break;
            case SymbolState.Winable:
                OnWinableState();
                break;
            case SymbolState.Win:
                break;
        }
    }


    #region Public Method
    public void SetRowCol(int row, int col)
    {
        m_row = row;
        m_col = col;

        InitSort();
    }
    public void SwitchSymbol(SymbolSort upperSymbolSort)
    {
        m_symboleSort = upperSymbolSort;
        SetIdleImage(m_symboleSort);
    }
    public void ResetValues()
    {
        m_isWon = false;
        m_isDisplayingWin = false;
        m_winFrame.enabled = false;
        m_bonusWinAnim.SetActive(false);
        m_winEffect.GetComponent<Image>().enabled = false;
    }
    public void SetIdleState()
    {
        m_winFrame.enabled = false;
        m_winEffect.GetComponent<Image>().enabled = false;
    }
    public void SetWinState()
    {
        if (!m_isWon) return;

        m_winFrame.enabled = true;
        m_winEffect.GetComponent<Image>().enabled = true;
    }
    public void SetBonusWinAnim(bool isOn)
    {
       if(m_symboleSort == SymbolSort.Bonus)
        {
            m_bonusWinAnim.SetActive(isOn);
        }
    }
    public void SetFreeWinAnim(bool isOn)
    {
        if (m_symboleSort == SymbolSort.Free)
        {
            m_freeWinAnim.SetActive(isOn);
        }
    }
    public void CheckWinable(bool isForcedToStop)
    {
        if (isForcedToStop ||
            m_row == 0 ||
            m_row == 4 ||
            m_symbolState != SymbolState.Idle) return;

        if(m_symboleSort == SymbolSort.Bonus)
        {
            if (m_col < 4)
            {
                m_currentSymbol.GetComponent<Animator>()?.SetTrigger("Winable");
                StartCoroutine(PlayWinableSound());
                m_symbolState = SymbolState.Winable;
            }
        }        
    }    
    #endregion Public Method


    #region Private Method
    void InitValues()
    {
        m_symbolState = SymbolState.Idle;
        ResetValues();
    }    
    void InitSort()
    {
        if(m_row == 0 ||
            m_row == GameDataManager.Instance.GetSlotRow() + 1)
        {
            m_symboleSort = (SymbolSort)Random.Range((int)SymbolSort.Pick, (int)SymbolSort.Bigfoot); 
        }
        else
        {
            m_symboleSort = SymbolSort.Bigfoot;
        }
        SetIdleImage(m_symboleSort);
    }
    void SetIdleImage(SymbolSort symbolSort)
    {
        foreach (Image img in m_idleImageArray) img.enabled = false;
        Image currentImage;
        switch(m_symboleSort)
        {
            case SymbolSort.Wild:
                currentImage = m_idleImageArray[(int)SymbolSort.Bigfoot + 1];
                break;
            case SymbolSort.Free:
                currentImage = m_idleImageArray[(int)SymbolSort.Bigfoot + 2];
                break;
            case SymbolSort.Bonus:
                currentImage = m_idleImageArray[(int)SymbolSort.Bigfoot + 3];
                break;
            default:
                currentImage = m_idleImageArray[(int)m_symboleSort];                
                break;
        }
        currentImage.enabled = true;
        m_currentSymbol = currentImage.gameObject;
    }
    void OnWinableState()
    {
        m_symbolState = SymbolState.Idle;
    }
    IEnumerator PlayWinableSound()
    {
        yield return new WaitForSeconds(0.15f);
        m_currentSymbol.GetComponent<AudioSource>()?.Play();
    }
    #endregion Private Method
}
