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
    public event Action GameDataInitEvent;
    public event Action TotalBetChangeEvent;

    static public GameDataManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameDataManager>();
            }
            return m_instance;
        }
    }    

    public int m_totalBet { get; private set; }

    static GameDataManager m_instance;
    const int m_slotRow = 3;
    const int m_slotCol = 5;
    const int m_maxBetFactor = 20;
    const int m_basicBetFactor = 100;
    const int m_lines = 50;
    int m_totalWin;
    int m_mainWin;
    int m_bonusWin;
    int m_freeWin;
    int m_myDisplayMoney;
    int m_freeSymbolCount;
    int m_freeGameTotalCount;
    int m_freeGameCurrentCount;
    int m_bonusSymbolCount;
    int m_bonusGameTotalCount;
    int m_bonusGameCurrentCount;
    BET m_maxBet;
    BET m_bet;
    Dictionary<BET, int> m_totalBetRefDic;

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
    public void BetMore()
    {
        ChangeBet(-1);
        TotalBetChangeEvent();
    }
    public void BetLess()
    {
        ChangeBet(1);
        TotalBetChangeEvent();
    }
    public BET GetBet()
    {
        return m_bet;
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
                Debug.Log(betFactor + "    " + refNum + "   " + bet);
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
