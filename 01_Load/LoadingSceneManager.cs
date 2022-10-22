using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{


    public static LoadingSceneManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<LoadingSceneManager>();
            }
            return m_instance;
        }
    }
    public Image m_loadingBar;

    static LoadingSceneManager m_instance;
    const float m_loadingTime = 2f;
    float m_lodingSpeed;
    float m_fillAmountOFLoadingProcess = 0f;
    float m_currentDummyFillAmount = 0f;
    const float m_totalDummyFillAmount = 0.98f;

    private void Awake()
    {
        PlayerDataManager.Instance.CompleteLoadData += AddLoadingSliderValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_lodingSpeed = Mathf.Round(1 / m_loadingTime * 10) * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_currentDummyFillAmount < m_totalDummyFillAmount)
        {
            m_currentDummyFillAmount += m_lodingSpeed * Time.deltaTime;
        }

        m_loadingBar.fillAmount = m_currentDummyFillAmount + m_fillAmountOFLoadingProcess;

        if(m_loadingBar.fillAmount >= 1)
        {
            SceneManager.LoadScene("02_LobbyScene");
        }
    }

    #region Private Method
    void AddLoadingSliderValue()
    {
        m_fillAmountOFLoadingProcess += 0.3f;
    }
    #endregion
}
