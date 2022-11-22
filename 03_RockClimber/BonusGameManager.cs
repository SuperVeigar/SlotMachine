using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BonusGameManager : MonoBehaviour
{
    public event Action onBonusGameEnd;
    enum BonusGameState { Intro = 0,Ready, Choice, Reward, CheckMore, TotalAward, End }
    enum Jackpot { Mini = 0, Minor, Major, Grand, X300, X200, X100 }
    static public BonusGameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<BonusGameManager>();
            }
            return m_instance;
        }
    }

    public Text m_bonusGameCount;
    public Text m_bonusWinNumber;
    public Text m_resultWinNumber;
    public Image m_choiceTimeBar;
    public Gem[] m_gems;
    public AudioClip m_breakingGemSound_1;
    public AudioClip m_breakingGemSound_2;
    public GameObject m_resultPanel;

    static BonusGameManager m_instance;
    const int m_jackpotCount = 10;
    bool m_isPlay;
    bool m_isAnimatingMoneyText;
    long m_increasingAmount;
    long m_targetMoneyToAnimate;
    long m_currentMoneyToAnimate;
    int m_amountIncreasingResult;
    int m_currentResultMoneyToAnimate;
    int[] m_choosedGem;
    const float m_timeIncreasingMoney = 1.25f;
    const float m_delayToReward = 0.8f;
    const float m_timeAnimatingResult = 8f;
    const float m_timeIncreasingResultMoney = 4f;
    const float m_timeToPick = 3f;
    float m_elapsedTimeToPick;
    Jackpot[] m_jackpots;
    BonusGameState m_bonusGameState;
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isPlay) return;

        switch(m_bonusGameState)
        {
            case BonusGameState.Intro:
                OnIntroState();
                break;
            case BonusGameState.Ready:
                OnReadyState();
                break;
            case BonusGameState.Choice:
                break;
            case BonusGameState.Reward:
                OnRewardState();
                break;
            case BonusGameState.CheckMore:
                OnCheckMoreState();
                break;
            case BonusGameState.TotalAward:
                OnTotalAward();
                break;
            case BonusGameState.End:
                break;
        }
    }

    private void FixedUpdate()
    {
        UpdateWinText();        
    }

    #region Public Method
    public void CalculateGame()
    {
        ResetValues();
        ShuffleJackpot(100);
    }
    public void ChoiceGem(int arrayNum)
    {
        if (arrayNum >= m_gems.Length ||
            m_bonusGameState != BonusGameState.Ready) return;

        m_choosedGem[GameDataManager.Instance.m_bonusGameCurrentCount] = arrayNum;
        int winValue = GetJackpot(m_jackpots[GameDataManager.Instance.m_bonusGameCurrentCount++]);
        int winNumber = GameDataManager.Instance.AddBonusWin(winValue);        
        m_bonusGameCount.text = (GameDataManager.Instance.m_bonusGameTotalCount - GameDataManager.Instance.m_bonusGameCurrentCount).ToString();        
        PlayBreakingGemSound();
        m_gems[arrayNum].BreakGem(winNumber);
        m_elapsedTimeToPick = m_timeToPick;

        m_bonusGameState = BonusGameState.Choice;

        StartCoroutine(MoveToReward());
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {        
        m_audioSource = GetComponent<AudioSource>();
        m_jackpots = new Jackpot[m_jackpotCount] { Jackpot.Mini, Jackpot.Minor, Jackpot.Major, Jackpot.Grand, Jackpot.X300, Jackpot.X200, Jackpot.X200, Jackpot.X100, Jackpot.X100, Jackpot.X100 };
        m_choosedGem = new int[5];
        
        ResetValues();

        m_isPlay = false;
    }
    void ResetValues()
    {
        foreach (Gem gem in m_gems) gem?.ResetValues();
        for (int i = 0; i < m_choosedGem.Length; i++) m_choosedGem[i] = -1;
        m_bonusGameCount.text = GameDataManager.Instance.m_bonusGameTotalCount.ToString();
        m_isPlay = true;
        m_isAnimatingMoneyText = false;
        m_increasingAmount = 0;
        m_targetMoneyToAnimate = 0;
        m_currentMoneyToAnimate = 0;
        m_currentResultMoneyToAnimate = 0;
        m_elapsedTimeToPick = m_timeToPick;
        m_bonusWinNumber.text = "0";
        m_resultPanel.SetActive(false);
        m_bonusGameState = BonusGameState.Intro;
    }
    void ShuffleJackpot(int count)
    {
        for(int i = 0; i < count; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, m_jackpotCount);
            int rand2 = UnityEngine.Random.Range(0, m_jackpotCount);
            Jackpot tempJackpot = m_jackpots[rand1];
            m_jackpots[rand1] = m_jackpots[rand2];
            m_jackpots[rand2] = tempJackpot;
        }
    }
    void OnIntroState()
    {
        if (GameEffectManager.Instance.IsEndBonusIntro()) m_bonusGameState = BonusGameState.Ready;
    }
    void OnReadyState()
    {
        UpdateTimeToPick();
        TestKey();
    }
    void UpdateTimeToPick()
    {
        m_elapsedTimeToPick -= Time.deltaTime;
        m_choiceTimeBar.fillAmount = m_elapsedTimeToPick / m_timeToPick;
        if (m_elapsedTimeToPick <= 0)
        {
            int rand;
            while(true)
            {
                rand = UnityEngine.Random.Range(0, m_gems.Length);

                bool isNew = true;

                foreach(int i in m_choosedGem) if(rand == i ) isNew = false;

                if (isNew) break;
            }
            ChoiceGem(rand);
        }
    }
    IEnumerator MoveToReward()
    {
        yield return new WaitForSeconds(m_delayToReward);

        AnimateIncreasingMyMoneyText(GameDataManager.Instance.m_bonusWin);
        GameSoundManager.Instance?.PlayWinCoinSound();
        m_bonusGameState = BonusGameState.Reward;

        StartCoroutine(CheckMoreGame());
    }
    void OnRewardState()
    {
        
    }
    IEnumerator CheckMoreGame()
    {
        m_bonusGameState = BonusGameState.CheckMore;

        if (GameDataManager.Instance.m_bonusGameCurrentCount >= GameDataManager.Instance.m_bonusGameTotalCount)
        {
            yield return new WaitForSeconds(m_timeIncreasingMoney + 1f);
            StartCoroutine(MoveToEndFromTotalAward());
        }
        else
        {
            yield return new WaitForSeconds(m_timeIncreasingMoney);
            m_bonusGameState = BonusGameState.Ready;
        }
    }
    void OnCheckMoreState()
    {
        
    }
    void OnTotalAward()
    {
            
    }
    IEnumerator MoveToEndFromTotalAward()
    {
        GameDataManager.Instance.CheckMegaWin();
        GameSoundManager.Instance.TurnBonusBGM(false);
        m_resultPanel.SetActive(true);        
        StartCoroutine(UpdateResultNum());
        m_bonusGameState = BonusGameState.TotalAward;

        yield return new WaitForSeconds(m_timeAnimatingResult);

        m_isPlay = false;
        onBonusGameEnd();
        m_bonusGameState = BonusGameState.End;
    }
    IEnumerator UpdateResultNum()
    {
        m_amountIncreasingResult = (int)(GameDataManager.Instance.m_bonusWin / (m_timeIncreasingResultMoney) * Time.deltaTime);

        while(m_currentResultMoneyToAnimate < GameDataManager.Instance.m_bonusWin)
        {
            yield return null;
            m_currentResultMoneyToAnimate += m_amountIncreasingResult;

            if(m_currentResultMoneyToAnimate > GameDataManager.Instance.m_bonusWin) m_currentResultMoneyToAnimate = GameDataManager.Instance.m_bonusWin;

            m_resultWinNumber.text = string.Format("{0:#,###}", m_currentResultMoneyToAnimate);
        }
    }
    int GetJackpot(Jackpot jackpot)
    {
        switch(jackpot)
        {
            case Jackpot.X100:
                return 100;
            case Jackpot.X200:
                return 200;
            case Jackpot.X300:
                return 300;
            case Jackpot.Mini:
                return 500;
            case Jackpot.Minor:
                return 1000;
            case Jackpot.Major:
                return 3000;
            case Jackpot.Grand:
                return 5000;
            default:
                return 0;
        }
    }
    void AnimateIncreasingMyMoneyText(long targetMoney)
    {
        m_targetMoneyToAnimate = targetMoney;
        m_isAnimatingMoneyText = true;
        m_increasingAmount = (int)((m_targetMoneyToAnimate - m_currentMoneyToAnimate) / m_timeIncreasingMoney * Time.fixedDeltaTime);
    }
    void UpdateWinText()
    {
        if (m_isAnimatingMoneyText)
        {
            m_currentMoneyToAnimate += m_increasingAmount;

            if (m_currentMoneyToAnimate >= m_targetMoneyToAnimate)
            {
                m_currentMoneyToAnimate = m_targetMoneyToAnimate;
                m_isAnimatingMoneyText = false;
            }
            m_bonusWinNumber.text = string.Format("{0:#,###}", m_currentMoneyToAnimate);
        }
    }
    void PlayBreakingGemSound()
    {
        if (GameDataManager.Instance.m_bonusGameCurrentCount % 2 == 0) m_audioSource.PlayOneShot(m_breakingGemSound_1);
        else m_audioSource.PlayOneShot(m_breakingGemSound_2);
    }
    #endregion Private Method

    #region Test Method
    void TestKey()
    {
        if (InputManager.Instance.CheckKeyDown(GameKey.BigBonus))
        {
            m_jackpots = new Jackpot[m_jackpotCount] { Jackpot.Grand, Jackpot.Minor, Jackpot.Major, Jackpot.Mini, Jackpot.X300, Jackpot.X200, Jackpot.X200, Jackpot.X100, Jackpot.X100, Jackpot.X100 };
        }
    }
    #endregion  Test Method
}
