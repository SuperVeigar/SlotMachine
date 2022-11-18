using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    static public GameSoundManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameSoundManager>();
            }
            return m_instance;
        }
    }    

    public AudioClip m_winCoinSound;
    public AudioClip m_bonusStartSiren;
    public AudioClip m_bigWheelintroSound;
    public GameObject m_mainBGM;
    public GameObject m_bonusBGM;
    public GameObject m_bigWheelBGM;

    static GameSoundManager m_instance;
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void PlayWinCoinSound()
    {
        m_audioSource.PlayOneShot(m_winCoinSound);
    }
    public void PlayBonusStartSiren()
    {
        m_audioSource.PlayOneShot(m_bonusStartSiren);
    }
    public void TurnMainBGM(bool on)
    {
        if(on) m_mainBGM.GetComponent<AudioSource>().Play();
        else m_mainBGM.GetComponent<AudioSource>().Pause();
    }
    public void TurnBonusBGM(bool on)
    {
        if (on) m_bonusBGM.GetComponent<AudioSource>().Play();
        else m_bonusBGM.GetComponent<AudioSource>().Stop();
    }
    public void TurnBigWheelBGM(bool on)
    {
        if (on) m_bigWheelBGM.GetComponent<AudioSource>().Play();
        else m_bigWheelBGM.GetComponent<AudioSource>().Stop();
    }
    public void PlayBigWheelIntro()
    {
        m_audioSource.PlayOneShot(m_bigWheelintroSound);
    }
    #endregion Public Method

}
