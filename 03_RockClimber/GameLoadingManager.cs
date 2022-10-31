using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoadingManager : MonoBehaviour
{

    public string m_nextSceneName;
    public Image m_loadingBar;
    
    const float m_loadingTimr = 1f;

    // Start is called before the first frame update
    void Start()
    {
        m_loadingBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoadingBar();
    }

    #region Public Method
    void UpdateLoadingBar()
    {
        m_loadingBar.fillAmount += m_loadingTimr * Time.deltaTime;

        if (m_loadingBar.fillAmount >= 1f)
        {
            SceneManager.LoadScene(m_nextSceneName);
        }
    }
    #endregion Public Method
}
