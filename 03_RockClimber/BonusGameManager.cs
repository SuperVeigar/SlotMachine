using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusGameManager : MonoBehaviour
{
    public event Action onBonusGameEnd;
    enum BonusGameState { Ready = 0, Reward, CheckMore, TotalAward, End }
    enum Jackpot { Mini = 0, Minor, Major, Grand, X5, X3, X1 }
    static public BonusGameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<BonusGameManager>();
            }
            return m_instance;
        }
    }

    public Gem[] m_gems;

    static BonusGameManager m_instance;
    const int m_jackpotCount = 10;
    Jackpot[] m_jackpots;
    BonusGameState m_bonusGameState;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_bonusGameState)
        {
            case BonusGameState.Ready:
                break;
            case BonusGameState.Reward:
                OnRewardState();
                break;
            case BonusGameState.CheckMore:
                OnCheckMore();
                break;
            case BonusGameState.TotalAward:
                OnTotalAward();
                break;
            case BonusGameState.End:
                break;
        }
    }

    #region Public Method
    public void CalculateGame()
    {
        ResetValues();
        ShuffleJackpot(100);
    }
    public void ChoiceGem(int arrayNum)
    {
        if (arrayNum >= m_gems.Length ||
            m_bonusGameState != BonusGameState.Ready) return;

        GameDataManager.Instance.m_bonusGameCurrentCount--;

        m_gems[arrayNum].BreakGem();
        m_bonusGameState = BonusGameState.Reward;
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_jackpots = new Jackpot[m_jackpotCount] { Jackpot.Mini, Jackpot.Minor, Jackpot.Major, Jackpot.Grand, Jackpot.X5, Jackpot.X3, Jackpot.X3, Jackpot.X1, Jackpot.X1, Jackpot.X1 };
        ResetValues();
    }
    void ResetValues()
    {
        foreach (Gem gem in m_gems) gem?.ResetValues();
        m_bonusGameState = BonusGameState.Ready;
    }
    void ShuffleJackpot(int count)
    {
        for(int i = 0; i < count; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, m_jackpotCount);
            int rand2 = UnityEngine.Random.Range(0, m_jackpotCount);
            Jackpot tempJackpot = m_jackpots[rand1];
            m_jackpots[rand1] = m_jackpots[rand2];
            m_jackpots[rand2] = tempJackpot;
        }
    }
    void OnRewardState()
    {
        m_bonusGameState = BonusGameState.CheckMore;
    }
    void OnCheckMore()
    {
        if (GameDataManager.Instance.m_bonusGameCurrentCount <= 0) m_bonusGameState = BonusGameState.TotalAward;
        else m_bonusGameState = BonusGameState.Ready;
    }
    void OnTotalAward()
    {
        StartCoroutine(MoveToEndFromTotalAward());        
    }
    IEnumerator MoveToEndFromTotalAward()
    {
        yield return new WaitForSeconds(1.5f);
        onBonusGameEnd();
        m_bonusGameState = BonusGameState.End;
    }
    #endregion Private Method
}
