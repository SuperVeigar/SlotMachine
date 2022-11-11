using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEffectManager : MonoBehaviour
{
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
    public GameObject m_winableFrame;
    public GameObject[] m_winFrame;

    static GameEffectManager m_instance;
    int m_winableFrameCol;

    // Start is called before the first frame update
    void Start()
    {
        m_winableFrame.SetActive(false);
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
            if(m_refBonus[i] ==1) m_winFrame[i].SetActive(true);
        }
    }
    public void TurnOffWinFrame()
    {
        foreach(GameObject obj in m_winFrame) obj.SetActive(false);
    }
    public void AnimateBonusInro()
    {
        m_bonusIntroBG.gameObject.SetActive(true);
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
    #endregion Public Method

    #region Private Method
    #endregion Private Method
}
