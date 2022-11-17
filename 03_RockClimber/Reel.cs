using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public enum ReelState
{
    Idle = 0, SpinStart, SpinAccel, Spin, SpinBreakReady, SpinBreak, SpinEnd, DampingUp, DampingDown, Stop
}
public class Reel : MonoBehaviour
{
    public event Action<int> onSpinEnd;

    public bool m_isReady { get; private set; }
    public int[] m_randomReel;
    public Symbol[] m_symbols = new Symbol[5];
    public AudioClip m_reelStopSound;
    public AudioClip m_winableReelSound;
        
    bool m_isWinable;
    bool m_isForcedToStop;
    ReelState m_reelState;
    int m_col;
    int m_randomReelNum;
    int m_switchCount;
    const float m_acceleration = 5000f;
    const float m_deceleration = -7000f;
    float m_currentMoveSpeed;
    const float m_maxMoveSpeed= 5000f;
    const float m_dampingSpeed = 1500f;
    float m_spinStartTime;
    float m_spinTime;
    Animator m_damperAnimator;
    AudioSource m_audiosource;
    Coroutine m_spinStopCoroutine;
       

    // Start is called before the first frame update
    void Start()
    {
        m_isReady = false;
        m_damperAnimator = GetComponent<Animator>();
        m_audiosource = GetComponent<AudioSource>();
        InitValues();
        m_isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainGameManager.Instance.m_isPaused) return;
        
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
            case ReelState.SpinBreakReady:
                UpdateOnSpinBreakReadyState();
                break;
            case ReelState.SpinBreak:
                UpdateOnSpinBreakState();
                break;
            case ReelState.SpinEnd:
                UpdateOnSpinEndState();
                break;
            case ReelState.DampingUp:
                UpdateOnDampingUpState();
                break;
            case ReelState.DampingDown:
                UpdateOnDampingDownState();
                break;
            case ReelState.Stop:
                UpdateOnStopState();
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
    public Symbol GetSymbol(int row)
    {
        if(row < m_symbols.Length) return m_symbols[row];

        return null;
    }
    public void StartSpin(float spinTime, bool winable)
    {
        if (m_reelState != ReelState.Idle) return;

        ResetValues();

        m_damperAnimator.enabled = true;
        m_damperAnimator.SetTrigger("Start");
        m_reelState = ReelState.SpinStart;
        m_spinStartTime = Time.time;
        m_spinTime = spinTime;
        m_isWinable = winable;
        m_spinStopCoroutine = StartCoroutine(BreakReelAfter(spinTime));
    }
    public void PauseGame()
    {
        if(m_reelState == ReelState.SpinStart ||
            m_reelState == ReelState.SpinAccel ||
            m_reelState == ReelState.Spin)
        {
            StopCoroutine(m_spinStopCoroutine);
            m_spinTime -= (Time.time - m_spinStartTime);
        }
    }
    public void ResumeGame()
    {
        if (m_reelState == ReelState.SpinStart ||
            m_reelState == ReelState.SpinAccel ||
            m_reelState == ReelState.Spin)
        {
            m_spinStopCoroutine = StartCoroutine(BreakReelAfter(m_spinTime));
        }        
    }
    public void StopSpin()
    {
        if(m_reelState == ReelState.Spin)
        {
            m_isForcedToStop = true;
            StopCoroutine(m_spinStopCoroutine);
            m_audiosource.Stop();
            m_reelState = ReelState.SpinBreakReady;
        }        
    }
    public void TurnWinSymbolAnim(bool isOn)
    {
        foreach (Symbol symbol in m_symbols)
        {
            if(isOn) symbol.SetWinState();
            else symbol.SetIdleState();
        }
    }
    public void TurnBonusWinSymbolAnim(bool isOn)
    {
        foreach (Symbol symbol in m_symbols)
        {
            symbol.SetBonusWinAnim(isOn);
        }
    }
    public void TurnFreeWinSymbolAnim(bool isOn)
    {
        foreach (Symbol symbol in m_symbols)
        {
            symbol.SetFreeWinAnim(isOn);
        }
    }
    public void SetWinableReelAnim()
    {
        if (m_reelState == ReelState.SpinStart ||
            m_reelState == ReelState.SpinAccel ||
            m_reelState == ReelState.Spin)
        {
            if(m_isWinable)
            {
                GameEffectManager.Instance.TurnWinableFrame(m_col, true, GetComponent<RectTransform>().anchoredPosition.x);

                m_audiosource.PlayOneShot(m_winableReelSound);
            }
        }
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_damperAnimator.enabled = false;
        m_randomReelNum = UnityEngine.Random.Range(0, 80);
        m_reelState = ReelState.Idle;

        ResetValues();
    }
    void ResetValues()
    {
        m_isWinable = false;
        m_isForcedToStop = false;
        m_currentMoveSpeed = 0f;
        m_switchCount = 0;        

        foreach (Symbol symbol in m_symbols) symbol.ResetValues();
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
            SwitchSymbols();
        }
        GetComponent<RectTransform>().anchoredPosition = pos;
    }    
    void MoveToReadyBreaking()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Time.deltaTime * m_currentMoveSpeed * -1f);

        if (pos.y <= -240)
        {
            pos.y += 240;
            SwitchSymbols();
            m_reelState = ReelState.SpinBreak;
        }
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
    void MoveReelOnBreak()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Time.deltaTime * m_currentMoveSpeed * -1f);

        if (pos.y <= -240)
        {
            pos.y += 240;
            m_switchCount++;
            SwitchSymbolsOnBreak();
        }
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
    void SwitchSymbols()
    {
        SymbolSort[] currentSymbols = new SymbolSort[m_symbols.Length - 1];

        for(int i = 0; i < m_symbols.Length; i++)
        {
            if(i < m_symbols.Length - 1)
            {
                currentSymbols[i] = m_symbols[i].m_symboleSort;
            }
            
            if( i == 0)
            {
                m_symbols[i].SwitchSymbol(GetRandomSymbol());
            }
            else
            {
                m_symbols[i].SwitchSymbol(currentSymbols[i - 1]);
            }
        }
    }
    void SwitchSymbolsOnBreak()
    {
        SymbolSort[] currentSymbols = new SymbolSort[m_symbols.Length - 1];

        for (int i = 0; i < m_symbols.Length; i++)
        {
            if (i < m_symbols.Length - 1)
            {                
                currentSymbols[i] = m_symbols[i].m_symboleSort;
            }

            if (i == 0)
            {
                if(0 < m_switchCount &&
                    m_switchCount < 4)
                {
                    m_symbols[i].SwitchSymbol(MainGameManager.Instance.m_pulledSymbols[GameDataManager.Instance.GetSlotRow() - m_switchCount, m_col]);
                }
                else
                {
                    m_symbols[i].SwitchSymbol(GetRandomSymbol());
                }               
            }
            else
            {
                m_symbols[i].SwitchSymbol(currentSymbols[i - 1]);
            }
        }
    }
    SymbolSort GetRandomSymbol()
    {
        if(m_randomReelNum >= m_randomReel.Length)
        {
            m_randomReelNum = 0;
        }

        return (SymbolSort)m_randomReel[m_randomReelNum++];
    }

    IEnumerator BreakReelAfter(float spinTime)
    {
        yield return new WaitForSeconds(spinTime);

        m_reelState = ReelState.SpinBreakReady;
    }
    void UpdateOnSpinBreakReadyState()
    {
        MoveToReadyBreaking();
    }
    void UpdateOnSpinBreakState()
    {
        m_currentMoveSpeed += (m_deceleration * Time.deltaTime);

        if (m_currentMoveSpeed < 0f ||
            (m_switchCount == 4 && GetComponent<RectTransform>().anchoredPosition.y < -45f))
        {
            m_currentMoveSpeed = 0f;
            m_reelState = ReelState.SpinEnd;
        }
        MoveReelOnBreak();
    }
    void UpdateOnSpinEndState()
    {
        m_reelState = ReelState.DampingUp;
        if(!m_isForcedToStop ||
            m_col == 4) m_audiosource.PlayOneShot(m_reelStopSound);
    }
    void UpdateOnDampingUpState()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Time.deltaTime * m_dampingSpeed);
        GetComponent<RectTransform>().anchoredPosition = pos;

        if (pos.y >= 25f)
        {
            m_reelState = ReelState.DampingDown;
        }
        
    }
    void UpdateOnDampingDownState()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition + new Vector2(0, Time.deltaTime * m_dampingSpeed * -1f);
        
        if (pos.y <= 0f)
        {
            pos.y = 0f;
            m_reelState = ReelState.Stop;
        }

        GetComponent<RectTransform>().anchoredPosition = pos;
    }
    void UpdateOnStopState()
    {
        if(m_col != 0) onSpinEnd(m_col);
        foreach (Symbol symbol in m_symbols) symbol.CheckWinable(m_isForcedToStop);
        GameEffectManager.Instance.TurnWinableFrame(m_col, false);
        m_reelState = ReelState.Idle;        
    }
    #endregion Private Method

    #region Test
    #endregion Test

}
