using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BET
{
    _20000 = 0, _10000, _4000, _2000, _1000, _400, _200, _100, _60, _40, _20, _10, _2, _1
}

public class GameDataManager : MonoBehaviour
{
    struct SymbolWinValue
    {
        public SymbolSort m_symbolSort;
        public int[] m_values;

        public SymbolWinValue(SymbolSort sort, int values_5, int values_4, int values_3, int values_2 = 0, int values_1 = 0)
        {
            m_symbolSort = sort;
            m_values = new int[5] { values_1, values_2, values_3, values_4, values_5 };
        }
        public int GetValue(int winCount)
        {
            return m_values[winCount - 1];
        }
    }
    public event Action GameDataInitEvent;
    public event Action TotalBetChangeEvent;

    static public GameDataManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameDataManager>();
            }
            return m_instance;
        }
    }

    public bool m_isFiveOfKinds;
    public int m_totalBet { get; private set; }
    public int m_totalWin;
    public int m_mainWin;
    public int m_bonusWin;
    public int m_freeWin;    
    public int m_freeSymbolCount;
    public int m_freeGameTotalCount;
    public int m_freeGameCurrentCount;
    public int m_bonusSymbolCount;
    public int m_bonusGameTotalCount;
    public int m_bonusGameCurrentCount;
    public long m_myDisplayMoney;

    static GameDataManager m_instance;
    const int m_slotRow = 3;
    const int m_slotCol = 5;
    const int m_maxBetFactor = 20;
    const int m_basicBetFactor = 100;
    const int m_lines = 50;
    const int m_mainGameSymbolCount = 9;
    BET m_maxBet;
    BET m_bet;
    Dictionary<BET, int> m_totalBetRefDic;
    SymbolWinValue[] m_symbolWinValues;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
        GameDataInitEvent();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Method
    public void ResetValues()
    {
        m_isFiveOfKinds = false;
        m_totalWin = 0;
        m_mainWin = 0;
        m_bonusWin = 0;
        m_freeWin = 0;
        m_myDisplayMoney = PlayerDataManager.Instance.m_playerData.m_myCurrentMoney;
        m_freeSymbolCount = 0;
        m_freeGameTotalCount = 0;
        m_freeGameCurrentCount = 0;
        m_bonusSymbolCount = 0;
        m_bonusGameTotalCount = 0;
        m_bonusGameCurrentCount = 0;
    }
    public void BetMore()
    {
        if (!MainGameManager.Instance.IsOnReadyState() ||
            MainGameManager.Instance.m_isPaused) return;

        ChangeBet(-1);
        TotalBetChangeEvent();
    }
    public void BetLess()
    {
        if (!MainGameManager.Instance.IsOnReadyState() ||
            MainGameManager.Instance.m_isPaused) return;

        ChangeBet(1);
        TotalBetChangeEvent();
    }
    public int GetLine()
    {
        return m_lines;
    }
    public BET GetBet()
    {
        return m_bet;
    }
    public int GetSlotRow()
    {
        return m_slotRow;
    }
    public int GetSlotCol()
    {
        return m_slotCol;
    }
    public int GetSymbolWinValue(SymbolSort sort, int winCount)
    {
        for(int i = 0; i < m_symbolWinValues.Length; i++)
        {
            if (m_symbolWinValues[i].m_symbolSort == sort) return m_symbolWinValues[i].GetValue(winCount);
        }

        return 0;
    }
    public void SetDisplayMoney()
    {

    }
    public void AddMainWin(int winValue)
    {
        m_mainWin += (winValue * m_totalBet);
        m_totalWin += (winValue * m_totalBet);
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_totalBetRefDic = new Dictionary<BET, int>();
        m_totalBetRefDic.Add(BET._20000, 20000);
        m_totalBetRefDic.Add(BET._10000, 10000);
        m_totalBetRefDic.Add(BET._4000, 4000);
        m_totalBetRefDic.Add(BET._2000, 2000);
        m_totalBetRefDic.Add(BET._1000, 1000);
        m_totalBetRefDic.Add(BET._400, 400);
        m_totalBetRefDic.Add(BET._200, 200);        
        m_totalBetRefDic.Add(BET._100, 100);
        m_totalBetRefDic.Add(BET._60, 60);
        m_totalBetRefDic.Add(BET._40, 40);
        m_totalBetRefDic.Add(BET._20, 20);
        m_totalBetRefDic.Add(BET._10, 10);
        m_totalBetRefDic.Add(BET._2, 2);
        m_totalBetRefDic.Add(BET._1, 1);

        m_maxBet = CalcBetLimit(m_maxBetFactor);
        m_bet = CalcBetLimit(m_basicBetFactor);
        m_totalBet = m_totalBetRefDic[m_bet] * m_lines;

        m_symbolWinValues = new SymbolWinValue[m_mainGameSymbolCount];
        m_symbolWinValues[0] = new SymbolWinValue(SymbolSort.Pick, 7, 3, 1);
        m_symbolWinValues[1] = new SymbolWinValue(SymbolSort.Tent, 7, 3, 1);
        m_symbolWinValues[2] = new SymbolWinValue(SymbolSort.Hook, 10, 4, 2);
        m_symbolWinValues[3] = new SymbolWinValue(SymbolSort.Shoes, 10, 4, 2);
        m_symbolWinValues[4] = new SymbolWinValue(SymbolSort.Helmet, 15, 8, 4);
        m_symbolWinValues[5] = new SymbolWinValue(SymbolSort.Logo, 40, 15, 8);
        m_symbolWinValues[6] = new SymbolWinValue(SymbolSort.Climber, 60, 20, 10);
        m_symbolWinValues[7] = new SymbolWinValue(SymbolSort.Bigfoot, 60, 20, 10);
        m_symbolWinValues[8] = new SymbolWinValue(SymbolSort.Wild, 75, 25, 12);
    }
    BET CalcBetLimit(int betFactor)
    {
        BET bet = BET._1;
        int refNum = (int)(PlayerDataManager.Instance.m_playerData.m_myCurrentMoney / m_lines / betFactor);
        
        foreach(KeyValuePair<BET, int> dic in m_totalBetRefDic)
        {
            if(refNum > dic.Value)
            {
                bet = dic.Key;
                break;
            }
        }

        return bet;
    }
    void ChangeBet(int add)
    {
        int bet = (int)m_bet + add;

        if (bet < 0) bet = 0;
        else if(bet < (int)m_maxBet) bet = (int)m_maxBet;
        else if(bet > (int)BET._1) bet = (int)BET._1;

        m_bet = (BET)bet;
        m_totalBet = m_totalBetRefDic[m_bet] * m_lines;
    }
    #endregion Private Method
}
