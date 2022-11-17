using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameEffectManager : MonoBehaviour
{
    public event Action onCollectMegaWin;

    static public GameEffectManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameEffectManager>();
            }
            return m_instance;
        }
    }

    public Image m_bonusIntroBG;
    public Image m_bonusCount_5;
    public Image m_bonusCount_4;
    public Image m_bonusCount_3;
    public Text m_currentMegaWinText;
    public GameObject m_winableFrame;
    public GameObject[] m_winFrame;
    public GameObject[] m_blackFrame;
    public GameObject m_megaWin;
    public GameObject m_collectButton;
    public MegaWinCoin[] m_megaWinCoins;

    static GameEffectManager m_instance;
    int m_winableFrameCol;
    int m_amountIncreasingMegawin;
    int m_currentMegaWin;
    int m_targetMegaWin;
    const float m_deltaTimeIncreasingMegaWin = 0.02f;
    const float m_timeIncreasingMegaWin = 8f;
    const float m_delayMegaWinUpdate = 1.3f;
    const float m_timeToActivateCollectButton = 5f;
    Coroutine m_megaWinUpdateCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        m_winableFrame.SetActive(false);
        m_megaWin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void TurnWinableFrame(int col, bool active, float posX = 0f)
    {
        if(active)
        {
            m_winableFrameCol = col;
            Vector2 pos = m_winableFrame.GetComponent<RectTransform>().anchoredPosition;
            pos.x = posX;
            m_winableFrame.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        else
        {
            if (m_winableFrameCol != col) return;
        }
        m_winableFrame.SetActive(active);
    }
    public void TurnOnWinFrame(int[] m_refBonus)
    {
        if (m_refBonus.Length != m_winFrame.Length) return;

        for(int i = 0; i < m_refBonus.Length; i++)
        {
            if (m_refBonus[i] == 1) m_winFrame[i].SetActive(true);
            else m_blackFrame[i].SetActive(true);
        }
    }
    public void TurnOffWinFrame()
    {
        foreach(GameObject obj in m_winFrame) obj.SetActive(false);
        foreach (GameObject obj in m_blackFrame) obj.SetActive(false);
    }
    public void AnimateBonusInro(int bonusCount)
    {
        m_bonusIntroBG.gameObject.SetActive(true);
        switch(bonusCount)
        {
            case 5:
                m_bonusCount_5.gameObject.SetActive(true);
                m_bonusCount_4.gameObject.SetActive(false);
                m_bonusCount_3.gameObject.SetActive(false);
                break;
            case 4:
                m_bonusCount_5.gameObject.SetActive(false);
                m_bonusCount_4.gameObject.SetActive(true);
                m_bonusCount_3.gameObject.SetActive(false);
                break;
            case 3:
                m_bonusCount_5.gameObject.SetActive(false);
                m_bonusCount_4.gameObject.SetActive(false);
                m_bonusCount_3.gameObject.SetActive(true);
                break;
        }
    }
    public bool isBonusChange()
    {
        if (m_bonusIntroBG.color.a >= 0.95f) return true;
        return false;
    }
    public bool IsEndBonusIntro()
    {
        if (m_bonusIntroBG.color.a <= 0f)
        {
            m_bonusIntroBG.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    public void TurnOnMegaWin(int win)
    {
        m_megaWin.SetActive(true);
        ResetCoins();        
        m_collectButton.SetActive(false);
        m_targetMegaWin = win;
        StartCoroutine(TurnOnCollectBtn());
        StartCoroutine(SetDelayMegaWinUpdate(win));
    }
    public void TurnOffMegaWin()
    {
        m_megaWin.SetActive(false);
        m_megaWinUpdateCoroutine = null;
    }
    public void CollectMegaWin()
    {
        if(m_megaWinUpdateCoroutine != null) StopCoroutine(m_megaWinUpdateCoroutine);
        m_megaWinUpdateCoroutine = null;
        m_collectButton.GetComponent<Button>().interactable = false;
        m_currentMegaWinText.text = string.Format("{0:#,###}", m_targetMegaWin);
        StartCoroutine(DelayForTurnOffMegaWin());        
    }
    #endregion Public Method

    #region Private Method
    IEnumerator TurnOnCollectBtn()
    {
        yield return new WaitForSeconds(m_timeToActivateCollectButton);

        m_collectButton.SetActive(true);
        m_collectButton.GetComponent<Button>().interactable = true;
    }
    IEnumerator SetDelayMegaWinUpdate(int win)
    {
        m_currentMegaWinText.text = "0";
        m_currentMegaWin = 0;
        m_amountIncreasingMegawin = (int)(win / m_timeIncreasingMegaWin * m_deltaTimeIncreasingMegaWin);

        yield return new WaitForSeconds(m_delayMegaWinUpdate);
        m_megaWinUpdateCoroutine = StartCoroutine(UpdateMegaWinText(win));
    }
    IEnumerator UpdateMegaWinText(int win)
    {
        while(m_currentMegaWin < win)
        {
            m_currentMegaWin += m_amountIncreasingMegawin;

            if (m_currentMegaWin > win) m_currentMegaWin = win;

            m_currentMegaWinText.text = string.Format("{0:#,###}", m_currentMegaWin);

            yield return new WaitForSeconds(m_deltaTimeIncreasingMegaWin);
        }
    }
    IEnumerator DelayForTurnOffMegaWin()
    {
        yield return new WaitForSeconds(1.5f);

        TurnOffMegaWin();
        onCollectMegaWin();
    }
    void ResetCoins()
    {
        if (m_megaWinCoins.Length == 0)
        {
            //m_megaWinCoins = new MegaWinCoin[31];
            m_megaWinCoins = FindObjectsOfType<MegaWinCoin>();
        }        

        foreach (MegaWinCoin coin in m_megaWinCoins) coin.ResetCoin();
    }
    #endregion Private Method
}
