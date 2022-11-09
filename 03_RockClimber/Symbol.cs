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
    public SymbolSort m_symboleSort;
    public Image m_winFrame;
    public Image[] m_idleImageArray;
    public Animator m_bonusWinAnim;
    public Animator m_winEffect;
        
    int m_row;
    int m_col;
    
    SymbolState m_symbolState;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();        
    }

    // Update is called once per frame
    void Update()
    {

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
        m_winFrame.enabled = false;
        m_bonusWinAnim.GetComponent<Image>().enabled = false;
        m_winEffect.GetComponent<Image>().enabled = false;
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

        switch(m_symboleSort)
        {
            case SymbolSort.Wild:
                m_idleImageArray[(int)SymbolSort.Bigfoot + 1].enabled = true;
                break;
            case SymbolSort.Free:
                m_idleImageArray[(int)SymbolSort.Bigfoot + 2].enabled = true;
                break;
            case SymbolSort.Bonus:
                m_idleImageArray[(int)SymbolSort.Bigfoot + 3].enabled = true;
                break;
            default:
                m_idleImageArray[(int)m_symboleSort].enabled = true;
                break;
        }
    }
    #endregion Private Method
}
