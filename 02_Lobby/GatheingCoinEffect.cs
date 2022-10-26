using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheingCoinEffect : MonoBehaviour
{
    public RectTransform m_targetTransform;
    public RectTransform m_startTransform;

    const float m_timeToSetParticlesVelocity = 0.1f;
    const float m_timeToTurnOff = 4f;
    const float m_moveSpeed = 10f;
    ParticleSystem m_coinParicleSystem;
    ParticleSystem.Particle[] m_particles;
        
    // Start is called before the first frame update
    void Start()
    {
        m_coinParicleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //StartCoroutine(SetForceToPaticles());
        //StartCoroutine(TurnOffThis());
    }

    #region Public Method    
    
    #endregion Public Method

    #region Private Method
    IEnumerator SetForceToPaticles()
    {
        yield return new WaitForSeconds(m_timeToSetParticlesVelocity);
        m_particles = new ParticleSystem.Particle[m_coinParicleSystem.main.maxParticles];
        m_coinParicleSystem.GetParticles(m_particles);
        Vector3 dir;
        for (int i = 0; i < m_particles.Length; i++)
        {
            m_startTransform.position = m_coinParicleSystem.transform.position;
            m_startTransform.Translate(m_particles[i].position, Space.Self);
            dir = (m_targetTransform.position - m_startTransform.position).normalized;
            m_particles[i].velocity = dir * m_moveSpeed;
            Debug.Log(m_particles[i].velocity);
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
