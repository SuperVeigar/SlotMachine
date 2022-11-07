using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reels : MonoBehaviour
{
    public Reel[] m_reels = new Reel[5];


    // Start is called before the first frame update
    void Start()
    {
        SetRowCol();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Method
    public void StartSpin()
    {
        foreach (Reel reel in m_reels) reel.StartSpin();
    }
    #endregion Public Method

    #region Private Method
    void SetRowCol()
    {
        for (int i = 0; i < GameDataManager.Instance.GetSlotCol(); i++)
        {
            m_reels[i].SetRowCol(i);
        }
    }
    #endregion Private Method
}
