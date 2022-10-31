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
    public AudioClip m_buttonOverSound;
    public AudioClip m_buttonPushedSound;

    static CommonSoundManager m_instance;
    AudioSource m_soundSource;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_soundSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonOverSound()
    {
        m_soundSource.PlayOneShot(m_buttonOverSound);
    }

    public void PlayButtonPushedSound()
    {
        m_soundSource.PlayOneShot(m_buttonPushedSound);
    }
}
