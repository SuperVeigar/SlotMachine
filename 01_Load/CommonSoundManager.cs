using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSoundManager : MonoBehaviour
{    
    public static CommonSoundManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<CommonSoundManager>();
            }
            return m_instance;
        }
    }
    public bool m_isSoundable;
    public AudioClip m_buttonOverSound;
    public AudioClip m_buttonPushedSound;
    public AudioClip m_coinSound;

    static CommonSoundManager m_instance;
    AudioSource m_soundSource;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_soundSource = GetComponent<AudioSource>();
        m_isSoundable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonOverSound()
    {
        if (!m_isSoundable) return;

        m_soundSource.PlayOneShot(m_buttonOverSound);
    }

    public void PlayButtonPushedSound()
    {
        if (!m_isSoundable) return;

        m_soundSource.PlayOneShot(m_buttonPushedSound);
    }

    public void PlayCoinSound()
    {
        m_soundSource?.PlayOneShot(m_coinSound);
    }
}
