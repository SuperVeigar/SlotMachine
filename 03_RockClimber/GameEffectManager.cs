using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject m_winableFrame;

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
    #endregion Public Method

    #region Private Method
    #endregion Private Method
}
