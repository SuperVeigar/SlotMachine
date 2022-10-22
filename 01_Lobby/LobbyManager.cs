using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<LobbyManager>();
            }
            return m_instance;
        }
    }

    public Text m_specialBonusInText;
    public Text m_collectSpecialBonusText;
    public Button m_specialBonusButton;
    public ParticleSystem m_specialBonusParticle;

    static LobbyManager m_instance;
    bool m_isReadySpecialBonus;
    const float m_specialBonusTime = 10f;
    float m_specialBonusElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        m_specialBonusElapsedTime = m_specialBonusTime;
        SetActiveCollectSpecialBonus(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpecialBonus();
    }

    #region Public Method
    public void EnterRockClimberGame()
    {
        SceneManager.LoadScene("02_GamcScene_RockClimber");
    }
    #endregion Public Method

    #region Private Method
    void UpdateSpecialBonus()
    {
        if(m_specialBonusElapsedTime > 0)
        {
            m_specialBonusElapsedTime -= Time.deltaTime;
            int mins = (int)m_specialBonusElapsedTime / 60;
            int secs = (int)m_specialBonusElapsedTime % 60;
            string minsString = string.Format("{0:##}", mins);
            string secsString = string.Format("{0:##}", secs);
            m_specialBonusInText.text = "<color = #ff0000>" + "SPECIAL BONUS IN " + "</color>" + "<color = #ffffff>" + minsString + " : " + secsString + "</color>";
        }
        else
        {
            if(!m_isReadySpecialBonus)
            {
                SetActiveCollectSpecialBonus(true);
            }
        }
    }

    void SetActiveCollectSpecialBonus(bool setOn)
    {
        m_isReadySpecialBonus = setOn;
        m_collectSpecialBonusText.gameObject.SetActive(setOn);
        m_specialBonusParticle.gameObject.SetActive(setOn);
        m_specialBonusButton.GetComponent<Animator>().enabled = setOn;
        m_specialBonusInText.gameObject.SetActive(!setOn);
    }
    #endregion Private Method
}
