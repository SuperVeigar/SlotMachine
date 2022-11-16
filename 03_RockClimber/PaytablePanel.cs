using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaytablePanel : MonoBehaviour
{
    public GameObject[] m_pages;
    int m_currentPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region Public Method
    public void ResetValues()
    {
        m_currentPage = 0;
        SetActivePages();
    }
    public void SetPageDown()
    {
        m_currentPage--;
        if (m_currentPage < 0) m_currentPage = 2;
        SetActivePages();
    }
    public void SetPageUp()
    {
        m_currentPage++;
        if (m_currentPage > 2) m_currentPage = 0;
        SetActivePages();
    }
    #endregion Public Method

    #region Private Method
    void SetActivePages()
    {
        foreach (GameObject page in m_pages) page?.SetActive(false);
        m_pages[m_currentPage].SetActive(true);
    }
    #endregion Public Method

}
