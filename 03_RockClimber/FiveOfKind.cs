using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FiveOfKind : MonoBehaviour
{
    public event Action onEndFiveOfKindWithMain;
    public event Action onEndFiveOfKindWithFree;
    public Image m_blackBG;
    public Image m_fiveImage;
    public Image m_ofImage;
    public Image m_kindImage;

    bool m_isMain;
    const float m_animTime = 1.5f;
    AudioSource m_audioSource;


    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        SetAnim(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void StartAnim(bool isMain)
    {
        m_isMain = isMain;
        SetAnim(true);
        m_audioSource.Play();
        StartCoroutine(FinishAnim());
    }
    #endregion Public Method

    #region Private Method
    void SetAnim(bool on)
    {
        m_blackBG.gameObject.SetActive(on);
        m_fiveImage.gameObject.SetActive(on);
        m_ofImage.gameObject.SetActive(on);
        m_kindImage.gameObject.SetActive(on);
    }

    IEnumerator FinishAnim()
    {
        yield return new WaitForSeconds(m_animTime);

        SetAnim(false);
        if(m_isMain) onEndFiveOfKindWithMain();
        else onEndFiveOfKindWithFree();
    }
    #endregion Private Method
}
