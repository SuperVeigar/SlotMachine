using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheingCoinEffect : MonoBehaviour
{
    public Transform m_targetTransform;

    const float m_timeToGetParticlesInfo = 0.1f;
    const float m_timeToSetParticlesVelocity = 1f;
    const float m_timeToTurnOff = 4f;
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
        StartCoroutine(SetForceToPaticles());
        //StartCoroutine(TurnOffThis());
    }

    #region Public Method    
    public void GOGOGOGOGOGOGOGO()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    #endregion Public Method

    #region Private Method
    IEnumerator SetForceToPaticles()
    {
        yield return new WaitForSeconds(m_timeToSetParticlesVelocity);
        m_particles = new ParticleSystem.Particle[m_coinParicleSystem.main.maxParticles];
        m_coinParicleSystem.GetParticles(m_particles);
        for (int i = 0; i < m_particles.Length; i++)
        {
            m_particles[i].velocity = new Vector3(10, 10, 0);
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
