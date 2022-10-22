using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameKey
{
    BetUp = 0, BetDown, Spin, Escape,
    AllWilds, NoScatter, Free1, Free2, Free3, Bonus1, Bonus2, Bonus3, Bonus4, Bonus5,
    Free20, Free15, Free10,
    Grandjackpot, Majorjackpot, Minorjacpot, Minijacpot,
    AddMoney, SubtractMoney
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InputManager>();  
            }
            return m_instance;
        }
    }

    static InputManager m_instance;
    Dictionary<GameKey, KeyCode> m_myKeys;

    // Start is called before the first frame update
    void Start()
    {
        SetMyKeys();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Private Method
    void SetMyKeys()
    {
        m_myKeys = new Dictionary<GameKey, KeyCode>();
        m_myKeys.Add(GameKey.BetUp, KeyCode.UpArrow);
        m_myKeys.Add(GameKey.BetDown, KeyCode.DownArrow);
        m_myKeys.Add(GameKey.Spin, KeyCode.Space);
        m_myKeys.Add(GameKey.Escape, KeyCode.Escape);
        m_myKeys.Add(GameKey.AllWilds, KeyCode.Q);
        m_myKeys.Add(GameKey.NoScatter, KeyCode.W);
        m_myKeys.Add(GameKey.Free1, KeyCode.E);
        m_myKeys.Add(GameKey.Free2, KeyCode.R);
        m_myKeys.Add(GameKey.Free3, KeyCode.T);
        m_myKeys.Add(GameKey.Bonus1, KeyCode.Y);
        m_myKeys.Add(GameKey.Bonus2, KeyCode.U);
        m_myKeys.Add(GameKey.Bonus3, KeyCode.I);
        m_myKeys.Add(GameKey.Bonus4, KeyCode.O);
        m_myKeys.Add(GameKey.Bonus5, KeyCode.P);
        m_myKeys.Add(GameKey.Free20, KeyCode.A);
        m_myKeys.Add(GameKey.Free15, KeyCode.S);
        m_myKeys.Add(GameKey.Free10, KeyCode.D);
        m_myKeys.Add(GameKey.Grandjackpot, KeyCode.F);
        m_myKeys.Add(GameKey.Majorjackpot, KeyCode.G);
        m_myKeys.Add(GameKey.Minorjacpot, KeyCode.H);
        m_myKeys.Add(GameKey.Minijacpot, KeyCode.J);
        m_myKeys.Add(GameKey.AddMoney, KeyCode.K);
        m_myKeys.Add(GameKey.SubtractMoney, KeyCode.L);
    }
    #endregion Private Method
}
