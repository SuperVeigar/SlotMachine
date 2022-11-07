using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    
    static public GameUIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameUIManager>();
            }
            return m_instance;
        }
    }

    public Text m_totalBetNum;
    public Button m_startButton;
    public Button m_stopButton;

    static GameUIManager m_instance;

    private void Awake()
    {
        GameDataManager.Instance.GameDataInitEvent += UpdateTotalBetWindow;
        GameDataManager.Instance.TotalBetChangeEvent += UpdateTotalBetWindow;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void UpdateTotalBetWindow()
    {
        int totalBet = GameDataManager.Instance.m_totalBet;

        switch (totalBet)
        {
            case int i when i >= 1000000:
                m_totalBetNum.text = totalBet.ToString().Substring(0, 1) + "M";
                break;
            case int i when i >= 100000:
                m_totalBetNum.text = totalBet.ToString().Substring(0, 3) + "K";
                break;
            case int i when i >= 10000:
                m_totalBetNum.text = totalBet.ToString().Substring(0, 2) + "K";
                break;
            default:
                m_totalBetNum.text = string.Format("{0:#,###}", totalBet);
                break;
        }
    }
    public void SetStartButtonOn(bool isOn)
    {
        m_startButton.gameObject.SetActive(isOn);
        m_stopButton.gameObject.SetActive(!isOn);
    }
    #endregion Public Method

    #region Private Method
    void InitGame()
    {
        SetStartButtonOn(true);
    }
    #endregion Private Method
}
