using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheingCoinEffect : MonoBehaviour
{
    public RectTransform m_targetTransform;

    bool m_isOnEmit;
    const float m_timeToStartConrol = 0.1f;
    const float m_timeToSetParticlesVelocity = 0.3f;
    const float m_timeToTurnOff = 3f;
    const float m_moveSpeed = 50f;    
    float m_timeStoppingEmit = 1f;
    float m_startTime;
    ParticleSystem m_coinParicleSystem;
    ParticleSystem.Particle[] m_particles;
        
    // Start is called before the first frame update
    void Start()
    {
        m_coinParicleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ApplyDrag();
    }

    private void OnEnable()
    {
        //StartCoroutine(StartParticle());
        //StartCoroutine(SetForceToPaticles());
        StartCoroutine(TurnOffThis());
    }

    #region Public Method    

    #endregion Public Method

    #region Private Method
    IEnumerator StartParticle()
    {
        yield return new WaitForSeconds(m_timeToStartConrol);         
        m_particles = new ParticleSystem.Particle[m_coinParicleSystem.main.maxParticles];
        m_coinParicleSystem.GetParticles(m_particles);
        m_isOnEmit = true;
        m_startTime = Time.time;
    }
    void ApplyDrag()
    {
        if (!m_isOnEmit) return;

        for (int i = 0; i < m_particles.Length; i++)
        {
            m_particles[i].velocity *= (Time.time - m_startTime - m_timeStoppingEmit) * (Time.time - m_startTime - m_timeStoppingEmit);
        }
        m_coinParicleSystem.SetParticles(m_particles, m_particles.Length);
    }
    IEnumerator SetForceToPaticles()
    {       
        yield return new WaitForSeconds(m_timeToSetParticlesVelocity);
        m_isOnEmit = false;
        Vector3 dir;
        for (int i = 0; i < m_particles.Length; i++)
        {
            dir = (m_targetTransform.anchoredPosition3D - m_particles[i].position).normalized;
            m_particles[i].velocity = dir * m_moveSpeed;
        }
        m_coinParicleSystem.SetParticles(m_particles, m_particles.Length);
    }
    IEnumerator TurnOffThis()
    {
        yield return new WaitForSeconds(m_timeToTurnOff);
        gameObject.SetActive(false);        
    }
    #endregion Private Method
}
