using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenButton : MonoBehaviour
{
    public Button[] m_buttons = new Button[m_buttonCount];
    public ScrollRect m_gamescroll;

    const int m_buttonCount = 3;
    const int m_displayGameButtonCount = 4;    
    int m_currentPage;
    float[] m_gamebtnPosX = new float[m_buttonCount];
    float m_denominatorForHorizontalNormalizedPosition;
    const float m_gamebtnWidth = 350f;
    const float m_gamebtnGap = 100f;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
        CheckContentPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void CheckContentPosition()
    {        
        float currentCenterPosX = m_gamescroll.content.gameObject.transform.position.x - ((m_gamebtnWidth + m_gamebtnGap) * m_displayGameButtonCount * 0.5f);
        for (int i = 0; i < m_buttonCount; i++)
        {
            if (i != m_buttonCount - 1)
            {
                if (m_gamebtnPosX[i + 1] < currentCenterPosX &&
                    currentCenterPosX <= m_gamebtnPosX[i])
                {
                    SetButtonImageScale(i);
                    break;
                }
            }
            else
            {
                if (currentCenterPosX <= m_gamebtnPosX[i])
                {
                    SetButtonImageScale(i);
                }
            }
        }
    }
    public void SetContentPos(int page)
    {
        if (m_gamebtnPosX.Length < page) return;

        float value = m_gamebtnPosX[page] * -1f / m_denominatorForHorizontalNormalizedPosition;
        
        m_gamescroll.horizontalNormalizedPosition = value;
    }
    #endregion

    #region Private Method
    void InitValues()
    {
        m_currentPage = -1;
        m_denominatorForHorizontalNormalizedPosition = m_gamescroll.content.rect.width - m_gamebtnGap - ((m_gamebtnWidth + m_gamebtnGap) * m_displayGameButtonCount);
        for (int i = 0; i < m_buttonCount; i++)
        {
            m_gamebtnPosX[i] = (m_gamebtnWidth + m_gamebtnGap) * m_displayGameButtonCount * i * -1;
        }
    }
    void ResetButtonImageScale()
    {        
        foreach (Button button in m_buttons) button.GetComponentInChildren<Image>().transform.localScale = Vector3.one;
    }
    void SetButtonImageScale(int page)
    {
        if(page != m_currentPage)
        {
            ResetButtonImageScale();
            m_buttons[page].GetComponentInChildren<Image>().transform.localScale = Vector3.one * 1.5f;
        }
    }
    #endregion
}
