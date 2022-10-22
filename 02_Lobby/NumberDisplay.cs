using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public float m_numGap;
    public float m_commaGap;
    public Text[] m_textArray;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< m_textArray.Length; i++)
        {
            if(i%3 != 0)
            {
                float gap = (i - i / 3) * m_numGap + i / 3 * m_commaGap;
                float x = m_textArray[i].transform.position.x - gap;
                float y = m_textArray[i].transform.position.y;
                float z = m_textArray[i].transform.position.z;
                m_textArray[i].transform.position = new Vector3(x, y, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
