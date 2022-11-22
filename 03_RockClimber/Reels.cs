using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Reels : MonoBehaviour
{    
    public Reel[] m_reels = new Reel[5];
    public AudioClip m_slotStartSound;

    const float m_reelSpinTime = 2f;
    const float m_reelSpinTimeInFree = 1.2f;
    const float m_reelStopInterval = 0.3f;
    const float m_winableScatterTime = 1.6f;
    AudioSource m_audiosource;
    WinChecker m_winChecker;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        m_audiosource = GetComponent<AudioSource>();
        m_winChecker = GetComponent<WinChecker>();
        StartCoroutine(InitReels());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    // 이 함수는 Free Game에서 사용합니다. Main Game에서는 StartSpin(int[] refBonus, int[] refFree)를 사용하세요.
    public void StartSpin()
    {
        m_winChecker.ResetValues();
        TurnWinSymbolAnim(false);
        m_audiosource.PlayOneShot(m_slotStartSound);

        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].StartSpin(m_reelSpinTimeInFree + m_reelStopInterval * i, false);
        }
    }
    // 이 함수는 Main Game에서 사용합니다. Free Game에서는 StartSpin()를 사용하세요.
    public void StartSpin(int[] refBonus, int[] refFree)
    {
        Debug.Log("refBonus : " + refBonus[0] + refBonus[1] + refBonus[2] + refBonus[3] + refBonus[4] + "  refFree  : " + refFree[0] + refFree[1] + refFree[2] + refFree[3] + refFree[4]);

        m_winChecker.ResetValues();
        TurnWinSymbolAnim(false);
        m_audiosource.PlayOneShot(m_slotStartSound);

        int bonusCount = 0;
        int freeCount = 0;
        int scatterTimeCount = 1;
        bool isFreeWinable = false;
        for (int i = 0; i < m_reels.Length; i++)
        {
            if (bonusCount >= 2 &&
                i >= 2)
            {
                m_reels[i].StartSpin(m_reelSpinTime + m_reelStopInterval * i + m_winableScatterTime * scatterTimeCount++, true);
            }
            else if (freeCount == 2 &&
                i == 3)                
            {
                m_reels[i].StartSpin(m_reelSpinTime + m_reelStopInterval * i + m_winableScatterTime * scatterTimeCount, true);
                isFreeWinable = true;          
            }
            else if (isFreeWinable)
            {
                m_reels[i].StartSpin(m_reelSpinTime + m_reelStopInterval * i + m_winableScatterTime * scatterTimeCount, false);
            }
            else
            {
                m_reels[i].StartSpin(m_reelSpinTime + m_reelStopInterval * i, false);
            }
            bonusCount += refBonus[i];
            freeCount += refFree[i];
        }
    }
    public void PauseGame()
    {
        foreach (Reel reel in m_reels) reel.PauseGame();
    }
    public void ResumeGame()
    {
        foreach (Reel reel in m_reels) reel.ResumeGame();
    }
    public void StopSpin()
    {
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].StopSpin();
        }
    }
    public void CheckWin(bool isMain)
    {
        m_winChecker?.CheckWin(isMain);
    }
    public void TurnWinSymbolAnim(bool isOn)
    {
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].TurnWinSymbolAnim(isOn);
        }
    }
    public void TurnBonusWinSymbolAnim(bool isOn)
    {
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].TurnBonusWinSymbolAnim(isOn);
        }
    }
    public void TurnFreeWinSymbolAnim(bool isOn)
    {
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].TurnFreeWinSymbolAnim(isOn);
        }
    }
    #endregion Public Method

    #region Private Method
    IEnumerator InitReels()
    {
        while (!m_reels[0].m_isReady ||
            !m_reels[1].m_isReady ||
            !m_reels[2].m_isReady ||
            !m_reels[3].m_isReady ||
            !m_reels[4].m_isReady )
        {
            yield return null;
        }

        SetRowCol();
        LoadRandomReels();
        ConnectSymbols();
        m_reels[1].onSpinEnd += SetWinableReelAnim;
        m_reels[2].onSpinEnd += SetWinableReelAnim;
        m_reels[3].onSpinEnd += SetWinableReelAnim;
        m_reels[4].onSpinEnd += InformFinToMain;
    }
    void SetRowCol()
    {
        for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
        {
            m_reels[i].SetRowCol(i);
        }
    }
    void LoadRandomReels()
    {
        CSVReaderForRandomReel.LoadRandomReels("randomreel", out m_reels[0].m_randomReel, out m_reels[1].m_randomReel, out m_reels[2].m_randomReel, out m_reels[3].m_randomReel, out m_reels[4].m_randomReel);
    }
    void InformFinToMain(int dummy)
    {
        MainGameManager.Instance.FinishReelSpin();
    }
    void SetWinableReelAnim(int currentCol)
    {
        m_reels[currentCol + 1].SetWinableReelAnim();
    }
    void ConnectSymbols()
    {
        Symbol[,] symbols = new Symbol[GameDataManager.Instance.GetSlotRow(), GameDataManager.Instance.GetSlotCol()];

        for(int row = 0; row < GameDataManager.Instance.GetSlotRow(); row++)
        {
            for(int col = 0; col < GameDataManager.Instance.GetSlotCol(); col++)
            {                
                symbols[row, col] = m_reels[col].GetSymbol(row + 1);     
            }
        }
        m_winChecker.SetSymbols(ref symbols);
    }
    #endregion Private Method

    #region Test
    #endregion Test
}
