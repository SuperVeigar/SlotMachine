using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BonusGameManager : MonoBehaviour
{
    public event Action onBonusGameEnd;
    enum BonusGameState { Ready = 0, Choice, Reward, CheckMore, TotalAward, End }
    enum Jackpot { Mini = 0, Minor, Major, Grand, X70, X40, X20 }
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
    public Gem[] m_gems;
    public AudioClip m_breakingGemSound_1;
    public AudioClip m_breakingGemSound_2;

    static BonusGameManager m_instance;
    int m_choicCount;
    const int m_jackpotCount = 10;
    bool m_isAnimatingMoneyText;
    long m_increasingAmount;
    long m_targetMoneyToAnimate;
    long m_currentMoneyToAnimate;
    const float m_timeIncreasingMoney = 1.25f;
    const float m_delayToReward = 0.8f;
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
        switch(m_bonusGameState)
        {
            case BonusGameState.Ready:
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
                
        int winValue = GetJackpot(m_jackpots[GameDataManager.Instance.m_bonusGameCurrentCount++]);
        int winNumber = GameDataManager.Instance.AddBonusWin(winValue);        
        m_bonusGameCount.text = (GameDataManager.Instance.m_bonusGameTotalCount - GameDataManager.Instance.m_bonusGameCurrentCount).ToString();        
        PlayBreakingGemSound();
        m_gems[arrayNum].BreakGem(winNumber);

        m_bonusGameState = BonusGameState.Choice;

        StartCoroutine(MoveToReward());
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_jackpots = new Jackpot[m_jackpotCount] { Jackpot.Mini, Jackpot.Minor, Jackpot.Major, Jackpot.Grand, Jackpot.X70, Jackpot.X40, Jackpot.X40, Jackpot.X20, Jackpot.X20, Jackpot.X20 };
        ResetValues();
    }
    void ResetValues()
    {
        foreach (Gem gem in m_gems) gem?.ResetValues();
        m_bonusGameCount.text = GameDataManager.Instance.m_bonusGameTotalCount.ToString();
        m_choicCount = 0;
        m_isAnimatingMoneyText = false;
        m_increasingAmount = 0;
        m_targetMoneyToAnimate = 0;
        m_currentMoneyToAnimate = 0;
        m_bonusWinNumber.text = "0";
        m_bonusGameState = BonusGameState.Ready;
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

        yield return new WaitForSeconds(m_timeIncreasingMoney);

        if (GameDataManager.Instance.m_bonusGameCurrentCount >= GameDataManager.Instance.m_bonusGameTotalCount) m_bonusGameState = BonusGameState.TotalAward;
        else m_bonusGameState = BonusGameState.Ready;
    }
    void OnCheckMoreState()
    {
        
    }
    void OnTotalAward()
    {
        StartCoroutine(MoveToEndFromTotalAward());        
    }
    IEnumerator MoveToEndFromTotalAward()
    {
        yield return new WaitForSeconds(4f);
        onBonusGameEnd();
        m_bonusGameState = BonusGameState.End;
    }
    int GetJackpot(Jackpot jackpot)
    {
        switch(jackpot)
        {
            case Jackpot.X20:
                return 20;
            case Jackpot.X40:
                return 40;
            case Jackpot.X70:
                return 70;
            case Jackpot.Mini:
                return 100;
            case Jackpot.Minor:
                return 200;
            case Jackpot.Major:
                return 500;
            case Jackpot.Grand:
                return 1000;
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
}
