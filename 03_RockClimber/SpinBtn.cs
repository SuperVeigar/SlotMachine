using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpinBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onSpin;
    public UnityEvent onAutoSpin;

    public GameObject m_particle;

    bool m_isOnClick;
    bool m_isEffectOn;
    float m_clickedTime;
    const float m_autoSpinTime = 1.5f;
    const float m_autoSpinStartTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        ResetBtn();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isOnClick)
        {
            m_clickedTime += Time.deltaTime;

            if (!m_isEffectOn &&
                m_clickedTime >= m_autoSpinStartTime) m_particle.SetActive(true);

            if (m_clickedTime >= m_autoSpinTime)
            {
                onAutoSpin?.Invoke();
                ResetBtn();
            }
            }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if(MainGameManager.Instance.IsOnReadyForMainOrFree())
        {
            m_isOnClick = true;
            m_isEffectOn = false;
            m_clickedTime = 0f;
        }        
    }
    public void OnPointerUp(PointerEventData data)
    {
        if(m_isOnClick)
        {
            if (m_clickedTime >= m_autoSpinTime) onAutoSpin?.Invoke();
            else onSpin?.Invoke();
            Debug.Log("m_clickedTime : " + m_clickedTime);
            ResetBtn();
        }        
    }

    void ResetBtn()
    {
        m_isOnClick = false;
        m_isEffectOn = false;
        m_particle.SetActive(false);
    }
}
