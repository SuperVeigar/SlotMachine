using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUIManager : MonoBehaviour
{
    public static CommonUIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CommonUIManager>();
            }
            return m_instance;
        }
    }
    public Canvas m_CommonUICanvas;

    static CommonUIManager m_instance;

    public void OnMouseUpAsButton()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_CommonUICanvas.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
