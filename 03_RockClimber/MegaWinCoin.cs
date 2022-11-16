using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWinCoin : MonoBehaviour
{
    int m_posX;
    float m_posY;
    float m_speed;
    // Start is called before the first frame update
    void Start()
    {
        ResetCoin();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCoin();
    }

    #region Public Method
    public void ResetCoin()
    {
        float scale = Random.Range(0.8f, 1.5f);
        GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);

        int rotation = Random.Range(0, 360);
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, rotation);

        m_posX = Random.Range(-850, 850);
        m_posY = Random.Range(650, 1300);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(m_posX, m_posY);

        m_speed = Random.Range(1f, 5f);

        float animSpeed = Random.Range(1f, 2f);
        GetComponent<Animator>().speed = animSpeed;
    }
    #endregion Public Method

    #region Private Method
    void MoveCoin()
    {
        m_posY += m_speed * -1f;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(m_posX, m_posY);

        if (m_posY < -650) ResetCoin();
    }
    #endregion Private Method
}
