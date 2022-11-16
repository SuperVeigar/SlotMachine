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
    public Text m_goodLuckText;
    public Text m_totalWinText;
    public Text m_totalWinNum;
    public Text m_winText;
    public Text m_winNum;
    public Button m_startButton;
    public Button m_stopButton;
    public Button m_betUpButton;
    public Button m_betDownButton;
    public Image m_winBoxBG;
    public GameObject m_paytablePanel;

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
    public void CloseHelpPanel()
    {
        CommonUIManager.Instance.m_menuDropdown.CloseHelpPanel();
        m_paytablePanel.SetActive(false);
        CommonUIManager.Instance.SetActiveAllButtons(true);
        SetActiveAllButtons(true);
        CommonSoundManager.Instance.m_isSoundable = true;        
    }
    public void SetGoodLuckText()
    {
        m_goodLuckText.enabled = true;
        m_totalWinText.enabled = false;
        m_totalWinNum.enabled = false;
        m_winText.enabled = false;
        m_winNum.enabled = false;

        m_winBoxBG.GetComponent<Animator>().SetTrigger("Idle");
    }
    public void SetWinTextAndNum(int winNum)
    {
        m_goodLuckText.enabled = false;
        m_totalWinText.enabled = true;
        m_totalWinNum.enabled = true;
        m_winText.enabled = true;
        m_winNum.enabled = true;

        m_winBoxBG.GetComponent<Animator>().SetTrigger("Win");
        m_winBoxBG.GetComponent<AudioSource>().Play();

        m_totalWinNum.text = string.Format("{0:#,###}", GameDataManager.Instance.m_totalWin);
        m_winNum.text = string.Format("{0:#,###}", winNum);
    }
    #endregion Public Method

    #region Private Method
    void InitGame()
    {
        SetStartButtonOn(true);
        CommonUIManager.Instance.m_menuDropdown.onOpenHelpPanel += OpenHelpPanel;
        SetGoodLuckText();
    }
    void OpenHelpPanel()
    {
        m_paytablePanel.GetComponent<PaytablePanel>().ResetValues();
        m_paytablePanel.SetActive(true);
        CommonUIManager.Instance.SetActiveAllButtons(false);
        SetActiveAllButtons(false);
        CommonSoundManager.Instance.m_isSoundable = false;
    }
    void SetActiveAllButtons(bool active)
    {
        m_startButton.interactable = active;
        m_stopButton.interactable = active;
        m_betUpButton.interactable = active;
        m_betDownButton.interactable = active;
    }
    #endregion Private Method
}
