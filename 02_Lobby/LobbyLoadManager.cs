using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyLoadManager : MonoBehaviour
{
    public Image m_loadingBar;

    const float m_loadingTime = 1f;
    float m_lodingSpeed;


    // Start is called before the first frame update
    void Start()
    {
        m_lodingSpeed = Mathf.Round(1 / m_loadingTime * 10) * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        m_loadingBar.fillAmount += m_lodingSpeed * Time.deltaTime;

        if (m_loadingBar.fillAmount >= 1)
        {
            MoveToLobbyScene();
        }
    }

    #region Private Method
    void MoveToLobbyScene()
    {
        SceneManager.LoadScene("02_1_LobbyScene");
    }
    #endregion
}
