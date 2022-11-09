using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MainGameState
{
    Ready = 0, Spin, Stop,
    CheckWin, ShowScatterWin,
    CheckBonus, BonusIntro, BonusGame, BonusEnd,
    CheckMain, MainReward, MainEnd,
    CheckFree, BigWheelIntro, BigWheelGame, BigWheelEnd,
    FreeIntro, FreeReady, FreeSpin, FreeStop, FreeCheckWin, FreeReward, FreeTotalReward, FreeEnd,
    TotalReward, TotalEnd
}

public class MainGameManager : MonoBehaviour
{
    static public MainGameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MainGameManager>();
            }
            return m_instance;
        }
    }

    public Reels m_reels;
    public bool m_isPaused { get; private set; }
    public SymbolSort[,] m_pulledSymbols { get; private set; }

    static MainGameManager m_instance;    
    bool m_isAutoSpin;
    MainGameState m_mainGameState;    
    const int m_probability_1Free = 10;
    const int m_probability_2Free = m_probability_1Free+ 7;
    const int m_probability_3Free = m_probability_2Free + 5;
    const int m_probability_1Bonus = m_probability_3Free + 15;
    const int m_probability_2Bonus = m_probability_1Bonus + 13;
    const int m_probability_3Bonus = m_probability_2Bonus + 10;
    const int m_probability_4Bonus = m_probability_3Bonus + 7;
    const int m_probability_5Bonus = m_probability_4Bonus + 5;
    const int m_probability_Main = m_probability_5Bonus + 28;
    const int m_probability_Wild = 5;
    const int m_probability_BigFoot = m_probability_Wild + 5;
    const int m_probability_Climber = m_probability_BigFoot + 8;
    const int m_probability_Logo = m_probability_Climber + 10;
    const int m_probability_Helmet = m_probability_Logo + 12;
    const int m_probability_Shoes = m_probability_Helmet + 14;
    const int m_probability_Hook = m_probability_Shoes + 14;
    const int m_probability_Tent = m_probability_Hook + 16;
    const int m_probability_Pick = m_probability_Tent + 16;
    const float m_timeToActAutoSpin = 2f;
    const float m_timeToSwitchStopButton = 0.5f;
    WinChecker m_winChecker;

    void Start()
    {
        CommonUIManager.Instance.ExitGameEvent += BackToLobby;
        CommonUIManager.Instance.SetActiveCommonUI(true);
        ResetValues();
        CommonUIManager.Instance.m_menuDropdown.onOpenHelpPanel += PauseGame;
        CommonUIManager.Instance.m_menuDropdown.onCloseHelpPanel += ResumeGame;
        m_winChecker = GetComponent<WinChecker>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (MainGameManager.Instance.m_isPaused) return;

        switch (m_mainGameState)
        {
            case MainGameState.Ready:
                CheckGameStartByTestKey();
                break;
            case MainGameState.Spin:
                break;
            case MainGameState.Stop:
                OnStopState();                
                break;
            case MainGameState.CheckWin:
                break;
            case MainGameState.ShowScatterWin:
                break;
            case MainGameState.CheckBonus:
                break;
            case MainGameState.BonusIntro:
                break;
            case MainGameState.BonusGame:
                break;
            case MainGameState.BonusEnd:
                break;
            case MainGameState.CheckMain:
                break;
            case MainGameState.MainReward:
                break;
            case MainGameState.MainEnd:
                break;
            case MainGameState.CheckFree:
                break;
            case MainGameState.BigWheelIntro:
                break;
            case MainGameState.BigWheelGame:
                break;
            case MainGameState.BigWheelEnd:
                break;
            case MainGameState.FreeIntro:
                break;
            case MainGameState.FreeReady:
                break;
            case MainGameState.FreeSpin:
                break;
            case MainGameState.FreeStop:
                break;
            case MainGameState.FreeCheckWin:
                break;
            case MainGameState.FreeReward:
                break;
            case MainGameState.FreeTotalReward:
                break;
            case MainGameState.FreeEnd:
                break;
            case MainGameState.TotalReward:
                break;
            case MainGameState.TotalEnd:
                OnTotalEndState();
                break;
        }
    }

    #region Public Method
    public void StartGame()
    {
        if (MainGameManager.Instance.m_isPaused) return;

        if (m_mainGameState == MainGameState.Ready &&
            PlayerDataManager.Instance.m_playerData.m_myCurrentMoney >= GameDataManager.Instance.m_totalBet)
        {
            PlayerDataManager.Instance.AddPlayerCurrentMoneyAndChangeText(-1 * GameDataManager.Instance.m_totalBet);

            GameDataManager.Instance.ResetValues();
            m_winChecker.ResetValues();
                       
            CalculateMainGame();
            CalculateTotalGame();
            StartCoroutine(SwitchSpinStopButton(false));

            m_reels.StartSpin();

            m_mainGameState = MainGameState.Spin;
#if UNITY_EDITOR
            ShowPulledSymbols();
#endif
        }        
    }
    public void StopReels()
    {
        if (MainGameManager.Instance.m_isPaused) return;

        if (m_mainGameState == MainGameState.Spin)
        {
            m_reels.StopSpin();
        }
    }
    public bool IsOnReadyState()
    {
        return m_mainGameState == MainGameState.Ready;
    }
    public void FinishMainSpin()
    {
        m_mainGameState = MainGameState.Stop;
    }
    #endregion Public Method

    #region Private Method
    void BackToLobby()
    {
        if (MainGameManager.Instance.m_isPaused) return;

        CommonUIManager.Instance.SetActiveCommonUI(false);
        CommonUIManager.Instance.ResetCommonUI();
        CommonUIManager.Instance.SetLobbyMode();
        SceneManager.LoadScene("02_0_LoadingScene_Lobby");
        CommonUIManager.Instance.ExitGameEvent -= BackToLobby;
        CommonUIManager.Instance.m_menuDropdown.onOpenHelpPanel -= PauseGame;
        CommonUIManager.Instance.m_menuDropdown.onCloseHelpPanel -= ResumeGame;
    }
    void ResetValues()
    {
        m_isPaused = false;
        m_isAutoSpin = false;
        m_mainGameState = MainGameState.Ready;
        m_pulledSymbols = new SymbolSort[3,5];
    }
    void PauseGame()
    {
        m_isPaused = true;
        m_reels.PauseGame();
    }
    void ResumeGame()
    {
        m_isPaused = false;
        m_reels.ResumeGame();
    }
    void CalculateMainGame()
    {
        for(int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
        {
            for (int k = 0; k < GameDataManager.Instance.GetSlotRow(); k++)
            {
                int thisWin = Random.Range(0, 100);

                switch(thisWin)
                {
                    case int a when a < m_probability_Wild:
                        m_pulledSymbols[k, i] = SymbolSort.Wild;
                        break;
                    case int a when a < m_probability_BigFoot:
                        m_pulledSymbols[k, i] = SymbolSort.Bigfoot;
                        break;
                    case int a when a < m_probability_Climber:
                        m_pulledSymbols[k, i] = SymbolSort.Climber;
                        break;
                    case int a when a < m_probability_Logo:
                        m_pulledSymbols[k, i] = SymbolSort.Logo;
                        break;
                    case int a when a < m_probability_Helmet:
                        m_pulledSymbols[k, i] = SymbolSort.Helmet;
                        break;
                    case int a when a < m_probability_Shoes:
                        m_pulledSymbols[k, i] = SymbolSort.Shoes;
                        break;
                    case int a when a < m_probability_Hook:
                        m_pulledSymbols[k, i] = SymbolSort.Hook;
                        break;
                    case int a when a < m_probability_Tent:
                        m_pulledSymbols[k, i] = SymbolSort.Tent;
                        break;
                    case int a when a < m_probability_Pick:
                        m_pulledSymbols[k, i] = SymbolSort.Pick;
                        break;
                }
            }
        }
    }
    void CalculateTotalGame()
    {
        int thisWin = Random.Range(0, 100);

        switch (thisWin)
        {
            case int a when a < m_probability_1Free:
                GameDataManager.Instance.m_freeSymbolCount = 1;
                break;
            case int a when a < m_probability_2Free:
                GameDataManager.Instance.m_freeSymbolCount = 2;
                break;
            case int a when a < m_probability_3Free:
                GameDataManager.Instance.m_freeSymbolCount = 3;
                break;
            case int a when a < m_probability_1Bonus:
                GameDataManager.Instance.m_bonusSymbolCount = 1;
                break;
            case int a when a < m_probability_2Bonus:
                GameDataManager.Instance.m_bonusSymbolCount = 2;
                break;
            case int a when a < m_probability_3Bonus:
                GameDataManager.Instance.m_bonusSymbolCount = 3;
                break;
            case int a when a < m_probability_4Bonus:
                GameDataManager.Instance.m_bonusSymbolCount = 4;
                break;
            case int a when a < m_probability_5Bonus:
                GameDataManager.Instance.m_bonusSymbolCount = 5;
                break;
            case int a when a < m_probability_Main:
                break;
        }

        SetFreeScatter(GameDataManager.Instance.m_freeSymbolCount);
        SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
    }    
    void SetFreeScatter(int freeCount)
    {
        if (freeCount == 0) return;

        if (freeCount == 1)
        {
            int row = Random.Range(0, 3);
            int col = Random.Range(1, 4);

            m_pulledSymbols[row, col] = SymbolSort.Free;
        }
        else if (freeCount == 2)
        {
            int row1 = Random.Range(0, 3);
            int col1 = Random.Range(1, 4);

            int row2 = Random.Range(0, 3);
            int col2;

            while(true)
            {
                col2 = Random.Range(1, 4);

                if (col1 != col2) break;
            }

            m_pulledSymbols[row1, col1] = SymbolSort.Free;
            m_pulledSymbols[row2, col2] = SymbolSort.Free;
        }
        else if (freeCount == 3)
        {
            int row1 = Random.Range(0, 3);
            int row2 = Random.Range(0, 3);
            int row3 = Random.Range(0, 3);

            m_pulledSymbols[row1, 1] = SymbolSort.Free;
            m_pulledSymbols[row2, 2] = SymbolSort.Free;
            m_pulledSymbols[row3, 3] = SymbolSort.Free;
        }
    }
    void SetBonusScatter(int bonusCount)
    {
        if (bonusCount == 0) return;

        if (bonusCount == 1)
        {
            int row = Random.Range(0, 3);
            int col = Random.Range(0, 5);

            m_pulledSymbols[row, col] = SymbolSort.Bonus;
        }
        else if (bonusCount == 2)
        {
            int row1 = Random.Range(0, 3);
            int col1 = Random.Range(0, 5);

            int row2 = Random.Range(0, 3);
            int col2;

            while (true)
            {
                col2 = Random.Range(1, 4);

                if (col1 != col2) break;
            }

            m_pulledSymbols[row1, col1] = SymbolSort.Bonus;
            m_pulledSymbols[row2, col2] = SymbolSort.Bonus;
        }
        else if (bonusCount == 3)
        {
            int col1_not = Random.Range(0, 5);
            int col2_not;

            while (true)
            {
                col2_not = Random.Range(0, 5);

                if (col1_not != col2_not) break;
            }

            for(int i = 0; i< GameDataManager.Instance.GetSlotCol(); i++)
            {
                if(i != col1_not &&
                    i != col2_not)
                {
                    m_pulledSymbols[Random.Range(0, 3), i] = SymbolSort.Bonus;
                }
            }
        }
        else if (bonusCount == 4)
        {
            int col1_not = Random.Range(0, 5);

            for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
            {
                if (i != col1_not)
                {
                    m_pulledSymbols[Random.Range(0, 3), i] = SymbolSort.Bonus;
                }
            }
        }
        else if (bonusCount == 5)
        {
            for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
            {
                m_pulledSymbols[Random.Range(0, 3), i] = SymbolSort.Bonus;
            }
        }
    }
    IEnumerator SwitchSpinStopButton(bool isSpinButonOn)
    {
        yield return new WaitForSeconds(m_timeToSwitchStopButton);

        GameUIManager.Instance.SetStartButtonOn(isSpinButonOn);
    }
    void OnStopState()
    {
        m_mainGameState = MainGameState.TotalEnd;
        GameUIManager.Instance.SetStartButtonOn(true);
    }
    void OnTotalEndState()
    {
        m_mainGameState = MainGameState.Ready;
    }
    #endregion Private Method

    #region Test
    void ShowPulledSymbols()
    {
        Debug.Log("Free : " + GameDataManager.Instance.m_freeSymbolCount + "  /  Bounus : " + GameDataManager.Instance.m_bonusSymbolCount);
        string logToPrint;
        Debug.Log("------------------------------------------------------------------");
        for (int k = 0; k < GameDataManager.Instance.GetSlotRow(); k++)
        {
            logToPrint = "";
            for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
            {
                logToPrint += (m_pulledSymbols[k, i] + "\t\t");
            }
            Debug.Log(logToPrint);
        }
        Debug.Log("------------------------------------------------------------------");
    }    
    void CheckGameStartByTestKey()
    {
        if (MainGameManager.Instance.m_isPaused) return;

        if (InputManager.Instance.CheckKeyDown(GameKey.AllWilds))
        {
            GameDataManager.Instance.ResetValues();
            for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
            {
                for (int k = 0; k < GameDataManager.Instance.GetSlotRow(); k++)
                {
                    m_pulledSymbols[k, i] = SymbolSort.Wild;
                }
            }
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.NoScatter))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Free1))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_freeSymbolCount = 1;
            SetFreeScatter(GameDataManager.Instance.m_freeSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Free2))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_freeSymbolCount = 2;
            SetFreeScatter(GameDataManager.Instance.m_freeSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Free3))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_freeSymbolCount = 3;
            SetFreeScatter(GameDataManager.Instance.m_freeSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Bonus1))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_bonusSymbolCount = 1;
            SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Bonus2))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_bonusSymbolCount = 2;
            SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Bonus3))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_bonusSymbolCount = 3;
            SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Bonus4))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_bonusSymbolCount = 4;
            SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
            StartGameByTestKey();
        }
        else if (InputManager.Instance.CheckKeyDown(GameKey.Bonus5))
        {
            GameDataManager.Instance.ResetValues();
            CalculateMainGame();
            GameDataManager.Instance.m_bonusSymbolCount = 5;
            SetBonusScatter(GameDataManager.Instance.m_bonusSymbolCount);
            StartGameByTestKey();
        }
    }
    void StartGameByTestKey()
    {
        m_mainGameState = MainGameState.Spin;
        ShowPulledSymbols();
        StartCoroutine(SwitchSpinStopButton(false));
        m_reels.StartSpin();
    }
    #endregion Test
}
