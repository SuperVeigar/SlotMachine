using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReelState
{
    Idle = 0, SpinStart, SpinAccel, Spin, SpinEnd, Stop
}
public class Reel : MonoBehaviour
{
    public Symbol[] m_symbols = new Symbol[5];
    public Animator m_winEffectAnimator;
    public Animator m_winFrameAnimator;

    bool m_isWinable;
    ReelState m_reelState;
    int m_col;
    const float m_acceleration = 3000f;
    float m_currentMoveSpeed;
    const float m_maxMoveSpeed= 3000f;
    Animator m_damperAnimator;
       

    // Start is called before the first frame update
    void Start()
    {
        m_damperAnimator = GetComponent<Animator>();
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_reelState)
        {
            case ReelState.Idle:
                break;
            case ReelState.SpinStart:
                break;
            case ReelState.SpinAccel:
                UpdateOnSpinAccelState();
                break;
            case ReelState.Spin:
                UpdateOnSpinState();
                break;
            case ReelState.SpinEnd:
                break;
            case ReelState.Stop:
                break;
        }
    }

    #region Public Method
    public void SetRowCol(int col)
    {
        m_col = col;
        for(int i = 0; i < GameDataManager.Instance.GetSlotRow() + 2; i++)
        {
            m_symbols[i].SetRowCol(i, m_col);
        }
    }
    public void StartSpin()
    {
        if (m_reelState != ReelState.Idle) return;

        m_damperAnimator.enabled = true;
        m_damperAnimator.SetTrigger("Start");
        m_reelState = ReelState.SpinStart;
    }
    #endregion Public Method

    #region Private Method
    void ResetValues()
    {
        m_isWinable = false;             
        m_damperAnimator.enabled = false;
        m_currentMoveSpeed = 0f;
        m_reelState = ReelState.Idle;
    }
    void SetSpinState()
    {
        if (m_reelState != ReelState.SpinStart) return;

        m_damperAnimator.enabled = false;
        m_reelState = ReelState.SpinAccel;        
    }
    void UpdateOnSpinAccelState()
    {
        m_currentMoveSpeed += (m_acceleration * Time.deltaTime);

        if (m_currentMoveSpeed >= m_maxMoveSpeed)
        {
            m_currentMoveSpeed = m_maxMoveSpeed;
            m_reelState = ReelState.Spin;
        }
        MoveReel();
    }
    void UpdateOnSpinState()
    {
        MoveReel();
    }
    void MoveReel()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Time.deltaTime * m_currentMoveSpeed * -1f);

        if(pos.y <= -240)
        {
            pos.y += 240;
            //SwitchSymbols();
        }
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
    void SwitchSymbols()
    {
        SymbolSort[] currentSymbols = new SymbolSort[m_symbols.Length];

        for(int i = 0; i < m_symbols.Length; i++)
        {
            currentSymbols[i] = m_symbols[i].m_symboleSort;
            
        }
    }    
    #endregion Private Method
}
