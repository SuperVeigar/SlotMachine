using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WinChecker : MonoBehaviour
{
    struct WinInfo
    {
        public int m_payLine;
        public SymbolSort m_symbolSort;
        public int m_winCount;
        public int m_winValue;

        public void InitValues(int payLine)
        {
            m_payLine = payLine;
            ResetValues();
        }

        public void ResetValues()
        {
            m_symbolSort = SymbolSort.Pick;
            m_winCount = 0;
            m_winValue = 0;
        }
    }


    WinInfo[] m_winLines;
    int[,] m_payTable;
    Symbol[,] m_symbols;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        InitValues();
        LoadPayTable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Method
    public void ResetValues()
    {
        for (int i = 0; i < m_winLines.Length; i++) m_winLines[i].ResetValues();
    }
    public void SetSymbols(ref Symbol[,] symbols)
    {
        for (int row = 0; row < GameDataManager.Instance.GetSlotRow(); row++)
        {
            for (int col = 0; col < GameDataManager.Instance.GetSlotCol(); col++)
            {
                m_symbols[row, col] = symbols[row, col];
            }
        }
    }
    public void CheckWin(bool isMain)
    {
        SymbolSort firstSymbol;
        int winCount;
        for(int payline = 0; payline < m_winLines.Length; payline++)
        {
            // Payline을 통한 Win 확인
            firstSymbol = m_symbols[FindWinRow(payline, 0), 0].m_symboleSort;
            m_winLines[payline].m_symbolSort = firstSymbol;
            winCount = 1;
            for (int col = 1; col < GameDataManager.Instance.GetSlotCol(); col++)
            {
                if(m_symbols[FindWinRow(payline, col), col].m_symboleSort == firstSymbol ||
                    m_symbols[FindWinRow(payline, col), col].m_symboleSort == SymbolSort.Wild)
                {
                    winCount++;
                }
                else
                {
                    break;
                }
            }
            m_winLines[payline].m_winCount = winCount;
            m_winLines[payline].m_winValue = GameDataManager.Instance.GetSymbolWinValue(firstSymbol, winCount);
            if (winCount == 5) GameDataManager.Instance.m_isFiveOfKinds = true;

            // Win line의 경우 symbol에 win 표시
            if(m_winLines[payline].m_winValue > 0)
            {
                Debug.Log((payline+1) + " : " + m_winLines[payline].m_winCount);
                if(isMain) GameDataManager.Instance.AddMainWin(m_winLines[payline].m_winValue);
                else GameDataManager.Instance.AddFreeWin(m_winLines[payline].m_winValue);

                for (int col = 0; col < m_winLines[payline].m_winCount; col++)
                {
                    m_symbols[FindWinRow(payline, col), col].m_isWon = true;
                }
            }
        }
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_winLines = new WinInfo[GameDataManager.Instance.GetLine()];
        for(int i = 0; i< m_winLines.Length; i++)
        {
            m_winLines[i].InitValues(i);
        }
        m_symbols = new Symbol[GameDataManager.Instance.GetSlotRow(), GameDataManager.Instance.GetSlotCol()];
        ResetValues();
    }    
    void LoadPayTable()
    {
        m_payTable = new int[GameDataManager.Instance.GetLine(), GameDataManager.Instance.GetSlotCol()];
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/paytable.csv");
        string lineData;
        string[] arrayData;
        for (int row = 0; row < GameDataManager.Instance.GetLine(); row++)
        {
            lineData = reader.ReadLine();
            arrayData = lineData.Split(',');
            for (int col = 0; col < arrayData.Length; col++)
            {
                m_payTable[row, col] = int.Parse(arrayData[col]);
            }            
        }
        reader.Close();
    }
    int FindWinRow(int payline, int col)
    {
        return m_payTable[payline, col];
    }
    #endregion Private Method
}
