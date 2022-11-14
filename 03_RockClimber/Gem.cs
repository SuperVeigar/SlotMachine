using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gem : MonoBehaviour
{
    public Text m_winNumber;
    public Sprite[] m_idleImageArray;
    public GameObject m_breakingImage;
    public GameObject m_GlowParticle;

    const int m_idleImageCount = 4;


    // Start is called before the first frame update
    void Start()
    {
        InitValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void ResetValues()
    {
        GetComponent<Image>().sprite = m_idleImageArray[Random.Range(0, m_idleImageArray.Length)];
        GetComponent<Image>().enabled = true;
        m_breakingImage?.SetActive(false);
        m_GlowParticle?.SetActive(true);
    }
    public void BreakGem(int winNumber)
    {
        GetComponent<Image>().enabled = false;
        m_breakingImage?.SetActive(true);
        m_GlowParticle?.SetActive(false);
        m_winNumber.text = "+ " + string.Format("{0:#,###}", winNumber);
    }
    #endregion Public Method

    #region Private Method
    void InitValues()
    {
        ResetValues();
    }
    
    #endregion Private Method

}
