using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CommonUIManager.Instance.ExitGameEvent += BackToLobby;
        //CommonUIManager.Instance.SetActiveCommonUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Public Method
    #endregion Public Method

    #region Private Method
    void BackToLobby()
    {
        CommonUIManager.Instance.SetActiveCommonUI(false);
        CommonUIManager.Instance.ResetCommonUI();
        CommonUIManager.Instance.SetLobbyMode();
        SceneManager.LoadScene("02_0_LoadingScene_Lobby");
        CommonUIManager.Instance.ExitGameEvent -= BackToLobby;
    }
    #endregion Private Method
}
