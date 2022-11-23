using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameKey
{
    BetUp = 0, BetDown, Spin, Escape,
    NoScatter, FiveOfKind, Free1, Free2, Free3, Bonus1, Bonus2, Bonus3, Bonus4, Bonus5,
    Free20, Free15, Free10,
    BigBonus, None_1, None_2, None_3,
    AddMoney, ResetMoney,
    TimeForSpecialBonus
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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetMyKeys();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Method
    public bool CheckKeyDown(GameKey gameKey)
    {
        KeyCode keyCode;
        if (m_myKeys.TryGetValue(gameKey, out keyCode))
        {
            return Input.GetKeyDown(keyCode);
        }
        return false;
    }
    #endregion Public Method

    #region Private Method
    void SetMyKeys()
    {
        m_myKeys = new Dictionary<GameKey, KeyCode>();
        m_myKeys.Add(GameKey.BetUp, KeyCode.UpArrow);
        m_myKeys.Add(GameKey.BetDown, KeyCode.DownArrow);
        m_myKeys.Add(GameKey.Spin, KeyCode.Space);
        m_myKeys.Add(GameKey.Escape, KeyCode.Escape);
        m_myKeys.Add(GameKey.NoScatter, KeyCode.Q);
        m_myKeys.Add(GameKey.FiveOfKind, KeyCode.W);
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
        m_myKeys.Add(GameKey.BigBonus, KeyCode.F);
        m_myKeys.Add(GameKey.None_1, KeyCode.G);
        m_myKeys.Add(GameKey.None_2, KeyCode.H);
        m_myKeys.Add(GameKey.None_3, KeyCode.J);
        m_myKeys.Add(GameKey.AddMoney, KeyCode.K);
        m_myKeys.Add(GameKey.ResetMoney, KeyCode.L);
        m_myKeys.Add(GameKey.TimeForSpecialBonus, KeyCode.Z);
    }
    #endregion Private Method
}
