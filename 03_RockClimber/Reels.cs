using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reels : MonoBehaviour
{
     
    public Reel[] m_reels = new Reel[5];

    const float m_reelSpinTime = 2f;
    const float m_reelStopInterval = 0.3f;
    WinChecker m_winChecker;

    // Start is called before the first frame update
    void Start()
    {
        m_winChecker = GetComponent<WinChecker>();
        SetRowCol();
        LoadRandomReels();
        ConnectSymbols();
        m_reels[4].onSpinEnd += InformFinToMain;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void StartSpin()
    {
        m_winChecker.ResetValues();
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].StartSpin(m_reelSpinTime + m_reelStopInterval * i);
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
    public void CheckWin()
    {
        m_winChecker?.CheckWin();
    }
    public void TurnWinSymbolAnim(bool isOn)
    {
        for (int i = 0; i < m_reels.Length; i++)
        {
            m_reels[i].TurnWinSymbolAnim(isOn);
        }
    }
    #endregion Public Method

    #region Private Method
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
    void InformFinToMain()
    {
        MainGameManager.Instance.FinishMainSpin();
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

        m_winChecker.SetSymbols(symbols);
    }
    #endregion Private Method
}
