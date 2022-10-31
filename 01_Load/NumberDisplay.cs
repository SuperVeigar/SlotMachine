using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public float m_numGap;
    public float m_commaGap;
    public Text[] m_numArray;
    public Text[] m_commaArray;

    // Start is called before the first frame update
    void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private Method
    public void SetPosition()
    {
        // Set the number's position
        for (int i = 0; i < m_numArray.Length; i++)
        {
            float gap = i * m_numGap + i / 3 * m_commaGap;
            float x = m_numArray[i].transform.position.x - gap;
            float y = m_numArray[i].transform.position.y;
            float z = m_numArray[i].transform.position.z;
            m_numArray[i].transform.position = new Vector3(x, y, z);
        }
        // Set the comma's position
        for (int i = 0; i < m_commaArray.Length; i++)
        {
            float callibrattion = m_commaGap * 0.33f;
            float gap = (i + 1) * 3 * m_numGap + i * m_commaGap;
            float x = m_commaArray[i].transform.position.x - gap;
            float y = m_commaArray[i].transform.position.y;
            float z = m_commaArray[i].transform.position.z;
            m_commaArray[i].transform.position = new Vector3(x, y, z);
        }
    }
    #endregion Private Method

    #region Public Method
    public void SetNumber(long num)
    {
        long refNum = num; 
        long setNum = 0;
        int numCount = 0;
        
        for (int i = 0; i < m_numArray.Length; i++)
        {
            setNum = refNum % 10;
            refNum = refNum / 10;
            m_numArray[i].gameObject.SetActive(true);
            m_numArray[i].text = setNum.ToString();
        }
        for(int i = m_numArray.Length - 1; i >= 0; i--)
        {
            if (m_numArray[i].text == "0")
            {
                m_numArray[i].gameObject.SetActive(false);                
            }
            else
            {
                numCount = i + 1;
                break;
            }
        }

        int commaCount = (numCount - 1) / 3;
        for (int i = 0; i < m_commaArray.Length; i++)
        {
            if (i < commaCount) m_commaArray[i].gameObject.SetActive(true);
            else m_commaArray[i].gameObject.SetActive(false);
        }
    }
    #endregion Public Method
}
