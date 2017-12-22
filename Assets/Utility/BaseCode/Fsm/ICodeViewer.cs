using ICode;
using ICode.Actions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateEventObj
{   
    public StateEventObj() { }
    public StateEventObj(EGameState s, UnityAction e, UnityAction x  )
    {   
        Name = s;
        OnEnterAct = e;
        OnExitAct = x;
    }
    public EGameState Name;
    public UnityAction OnEnterAct;
    public UnityAction OnExitAct;
}


public class ICodeViewer : MonoBehaviour {
    
    public bool allowStateDebug = false;
    const string stateChangeEvent = "stateChange";
    public int State;
    public static ICodeViewer Get;
    
    public System.Action<string, object> OnChangeEvent;

    Dictionary<EGameState, GameState> _StateEventDic = new Dictionary<EGameState, GameState>();

    void Awake()
    {   
        Get = this;
    }
        // Use this for initialization
    void Start()
    {   
        InitStateEvent(EGameState.PVE, () => { Debug.Log("[PVE State][=============game state] enter PVE"); },
        () => { Debug.Log("[PVE State][=============game state]  exit PVE"); });
    }

    public void Init(StateMachine sm)
    {   
        var states = sm.States;
        int len = states.Length;
        for (int i = 0; i < len; i++)
        {   
            // GameState only
            if (states[i].Actions != null)
            {   
                var act = states[i].Actions[0] as GameState;
                {   
                    if (act != null)
                    {   
                        if(_StateEventDic.ContainsKey( act.mGameState ) ==false)
                            _StateEventDic.Add(act.mGameState,act);
                    }   
                }   
            }
            else
            {   
                // states[i].Actions 不能 为 null, node 需 加上 gamestate action
            }
        }
    }

    public void InitStateEvent(EGameState s, System.Action enter, System.Action exit)
    {   
        if (_StateEventDic.ContainsKey(s))
        {   
            var gs = _StateEventDic[s];
            if(gs != null)
            {   
                    gs.SetAct(enter, exit); ;
            }   
        }
        else
        {   
            // 需要 先 init dic
        }   
    }

    // Update is called once per frame
    void Update () {
            
    }

    public void StateChange( int state )
    {   
        changeEvent(stateChangeEvent, state);
    }

    void changeEvent(string eventName, object parameter)
    {   
        //Debug.LogError("====" + eventName + " | " + parameter);
        if (OnChangeEvent != null)
        {   
            OnChangeEvent(eventName, parameter);
        }
    }
}
