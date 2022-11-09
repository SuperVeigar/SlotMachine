using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WinChecker : MonoBehaviour
{

    bool[] m_winLines;
    int[,] m_payTable;
    int[] asdasd;


    // Start is called before the first frame update
    void Start()
    {
        LoadPayTable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void ResetValues()
    {
        for (int i = 0; i < m_winLines.Length; i++) m_winLines[i] = false;
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_winLines = new bool[GameDataManager.Instance.GetLine()];
        ResetValues();
        asdasd = new int[GameDataManager.Instance.GetLine()];
    }    
    void LoadPayTable()
    {
        m_payTable = new int[GameDataManager.Instance.GetLine(), GameDataManager.Instance.GetSlotCol() * GameDataManager.Instance.GetSlotRow()];
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/paytable.csv");
        
        string lineData;
        string[] arrayData; 

        for(int i = 0; i< GameDataManager.Instance.GetLine(); i++)
        {
            lineData = reader.ReadLine();
            arrayData = lineData.Split(',');
            for (int k = 0; k < arrayData.Length; k++)
            {
                m_payTable[i, k] = int.Parse(arrayData[k]);
            }            
        }
    }
    #endregion Private Method
}
