using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BigWheel : MonoBehaviour
{
    public event Action onFinishIntoAnim;
    public event Action onStartFreeGame;
    enum BIGWHEELSTATE { Intro = 0, Ready, Accel, Spin, Break, Damp, Stop, Reward, End }

    public GameObject m_spinButton;
    public GameObject m_clock;
    public GameObject m_arrow;
    public GameObject m_bigWheel;
    public GameObject m_targetPoint;
    public GameObject m_result;
    public GameObject[] m_freeCountInResult;
    public Image m_clockBar;
    public AudioClip m_spinSound;
    public AudioClip m_stopSound;
    public AudioClip m_celebrationSound;

    const float m_timeToStartSpin = 5f;
    float m_currentTimeToStartSpin;
    float m_currentAngle;
    float m_breakAngle;
    float m_targetAngle;
    float m_targetAngleWithDamp;
    const float m_defualtSpinCount = 5f;
    const float m_maxRotSpeed = 500f;
    float m_currentRotSpeed;
    float m_dampingAngle;
    const float m_acceleration = 1000f;
    const float m_deceleration = 175f;
    const float m_maxDampingAngle = 10f;
    const float m_dampingSpeed = 3f;
    const float m_anglePerPiece = 45f;
    const float m_marginAngle = 10f;
    const float m_angleOnBreakState = 720f;
    const float m_angleFor20Frees = 135f;
    readonly float[] m_anglgeFor15Frees = new float[3] { 45f, 225f, 315f };
    readonly float[] m_anglgeFor10Frees = new float[4] { 0f, 90f, 180f, 270f };
    const float m_timeForIntro = 2f;
    const float m_rewardTime = 3f;
    BIGWHEELSTATE m_bigwheelState;
    AudioSource m_audiosource;

    // Start is called before the first frame update
    void Start()
    {
        m_bigwheelState = BIGWHEELSTATE.Intro;
        m_audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TestLog();
        
        switch (m_bigwheelState)
        {
            case BIGWHEELSTATE.Intro:
                break;
            case BIGWHEELSTATE.Ready:
                OnReadyState();
                break;
            case BIGWHEELSTATE.Accel:
                OnAccelState();
                break;
            case BIGWHEELSTATE.Spin:
                OnSpinState();
                break;
            case BIGWHEELSTATE.Break:
                OnBreakState();
                break;
            case BIGWHEELSTATE.Damp:
                OnDampState();
                break;
            case BIGWHEELSTATE.Stop:
                OnStopState();
                break;
            case BIGWHEELSTATE.Reward:
                OnRewardState();
                break;
            case BIGWHEELSTATE.End:
                OnEndState();
                break;
        }
    }

    #region Public Method
    public void CalculateGame(int freeCount)
    {
        ResetBigWheel();

        float randAngle = UnityEngine.Random.Range(0f, m_anglePerPiece - m_marginAngle);
        m_dampingAngle = UnityEngine.Random.Range(0f, m_maxDampingAngle);
        int rand;
        switch (freeCount)
        {
            case 20:
                m_targetAngle = -m_angleFor20Frees;
                break;
            case 15:
                rand = UnityEngine.Random.Range(0, m_anglgeFor15Frees.Length);
                m_targetAngle = -m_anglgeFor15Frees[rand];
                break;
            case 10:
                rand = UnityEngine.Random.Range(0, m_anglgeFor10Frees.Length);
                m_targetAngle = -m_anglgeFor10Frees[rand];
                break;
        }
        m_targetAngle += (-360f * m_defualtSpinCount - randAngle - m_marginAngle * 0.5f);
        m_targetAngleWithDamp = m_targetAngle - m_dampingAngle;
        m_breakAngle = m_targetAngleWithDamp + m_angleOnBreakState;

        m_targetPoint.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, (m_targetAngle * -1f)));
        Debug.Log("-----------------------------------------");
        Debug.Log("Free Count  :  " + freeCount);
        Debug.Log("-----------------------------------------");
        Debug.Log("m_targetAngle : " + m_targetAngle + " / " + (m_targetAngle % 360 + 360) + " /// m_breakAngle : " + m_breakAngle + " / " + (m_breakAngle % 360 + 360));

        foreach (GameObject obj in m_freeCountInResult) obj.SetActive(false);
        if(freeCount ==20) m_freeCountInResult[0].SetActive(true);
        else if(freeCount == 15) m_freeCountInResult[1].SetActive(true);
        else m_freeCountInResult[2].SetActive(true);
    }
    public void StartIntro()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Intro");
        StartCoroutine(MoveToReadyState());
        m_bigwheelState = BIGWHEELSTATE.Intro;
    }
    public void SpinBigWheel()
    {
        if (m_bigwheelState != BIGWHEELSTATE.Ready) return;

        GetComponent<Animator>().enabled = false;
        StartCoroutine(TurnOffSpinBtn());
        m_bigwheelState = BIGWHEELSTATE.Accel;
    }
    public void PlayCollisionSound()
    {
        if (m_bigwheelState == BIGWHEELSTATE.Accel ||
            m_bigwheelState == BIGWHEELSTATE.Spin ||
            m_bigwheelState == BIGWHEELSTATE.Break) 
        m_audiosource.PlayOneShot(m_spinSound);
    }
    #endregion Public Method

    #region Private Method
    void ResetBigWheel()
    {
        GetComponent<Animator>().enabled = true;
        m_spinButton.SetActive(false);
        m_clock.SetActive(false);
        m_arrow.SetActive(false);
        m_result.SetActive(false);
        m_currentTimeToStartSpin = m_timeToStartSpin;
        m_currentAngle = 0f;
        m_currentRotSpeed = 0f;
    }
    IEnumerator MoveToReadyState()
    {
        yield return new WaitForSeconds(m_timeForIntro);
        onFinishIntoAnim();
        m_bigWheel.GetComponent<RectTransform>().rotation = Quaternion.identity;
        m_spinButton.SetActive(true);
        m_clock.SetActive(true);
        m_arrow.SetActive(true);
        m_bigwheelState = BIGWHEELSTATE.Ready; 
    }
    void OnReadyState()
    {
        m_currentTimeToStartSpin -= Time.deltaTime;
        m_clockBar.fillAmount = m_currentTimeToStartSpin / m_timeToStartSpin;
        if (m_currentTimeToStartSpin <= 0 ) SpinBigWheel();
    }
    void OnAccelState()
    {
        m_currentRotSpeed += m_acceleration * Time.deltaTime;
        if (m_currentRotSpeed >= m_maxRotSpeed)
        {
            m_currentRotSpeed = m_maxRotSpeed;
            m_bigwheelState = BIGWHEELSTATE.Spin;
        }
        SetRotation();
    }
    void SetRotation()
    {
        m_currentAngle += m_currentRotSpeed * -1f * Time.deltaTime;
        m_bigWheel.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, m_currentAngle);
    }
    IEnumerator TurnOffSpinBtn()
    {
        yield return new WaitForSeconds(1f);

        m_spinButton.SetActive(false);
        m_clock.SetActive(false);
    }
    void OnSpinState()
    {
        if (m_currentAngle <= m_breakAngle)
        {
            m_bigwheelState = BIGWHEELSTATE.Break;
        }

        SetRotation();
    }
    void OnBreakState()
    {
        m_currentRotSpeed -= m_deceleration * Time.deltaTime;
        if (m_currentRotSpeed <= m_dampingSpeed) m_currentRotSpeed = m_dampingSpeed;

        if (m_currentAngle <= m_targetAngleWithDamp)
        {            
            m_currentRotSpeed = m_dampingSpeed * -1f;
            m_bigwheelState = BIGWHEELSTATE.Damp;
        }
        SetRotation();
    }
    void OnDampState()
    {
        if (m_currentAngle >= m_targetAngle)
        {
            m_currentRotSpeed = 0;
            m_currentAngle = m_targetAngle;
            m_audiosource.PlayOneShot(m_stopSound);
            m_bigwheelState = BIGWHEELSTATE.Stop;
        }
        SetRotation();
    }
    void OnStopState()
    {
        m_result.SetActive(true);
        m_result.GetComponent<Animator>().SetTrigger("Intro");
        StartCoroutine(MoveToReward());
    }
    IEnumerator MoveToReward()
    {
        m_bigwheelState = BIGWHEELSTATE.Reward;
        yield return new WaitForSeconds(1f);
        m_audiosource.PlayOneShot(m_celebrationSound);
        StartCoroutine(MoveToEnd());
    }
    void OnRewardState()
    {
        
    }
    IEnumerator MoveToEnd()
    {
        yield return new WaitForSeconds(m_rewardTime);
        GetComponent<Animator>().SetTrigger("Outro");
        GetComponent<Animator>().enabled = true;
        m_result.GetComponent<Animator>().SetTrigger("Outro");
        m_bigwheelState = BIGWHEELSTATE.End;

        onStartFreeGame();
    }
    void OnEndState()
    {

    }
    #endregion Private Method

    #region Test Method
    void TestLog()
    {
        if (InputManager.Instance.CheckKeyDown(GameKey.None_1)) Debug.Log(m_bigwheelState + "  current angle : " + m_currentAngle + " / " + (m_currentAngle % 360 + 360f));
    }
    #endregion Test Method
}
