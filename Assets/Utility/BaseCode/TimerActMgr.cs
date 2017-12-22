using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerActObj
{   
    public int UID;
    public float Timer;
    public System.Action Act;
    public bool isEnd = false;
    bool start = false;
    public int repeatTime;
    private int m_initRepeatTime;
    bool isRepeat = false;
    public System.Action OnActFinished;
    public TimerActMgr mMgr;

    public TimerActObj( float t,System.Action a, TimerActMgr m,int repeatTime =-1 )
    {   
        mMgr = m;
        this.UID = mMgr.GetUID();
        Timer = t;
        Act = a;
        this.repeatTime = repeatTime;
    }   

    protected float m_startTime;
    public void StartTimer()
    {   
        m_startTime = Time.time;
        start = true;
        isEnd = false;
        m_initRepeatTime = repeatTime;
        isRepeat = this.m_initRepeatTime > 0;
    }   
   
    public void Update()
    {   
        if ( !start ) return;
        if (Time.time - m_startTime > Timer)
        {   
            if (isRepeat)
            {   
                if (Act != null)
                {   
                    if (repeatTime > 0)
                    {   
                        Act( );
                        repeatTime--;
                        m_startTime = Time.time;
                    }   
                    else
                    {
                        if (null != OnActFinished)
                            OnActFinished();
                        Clear();
                    }   
                }
            }
            else
            {
                if (Act != null)
                {   
                    Act();
                    if (null != OnActFinished)
                        OnActFinished();
                    Clear();
                }
            }

        }       
    }

    public void Clear()
    {
        this.UID = mMgr.GetUID();
        Timer = 0f;
        Act = null;
        isEnd = true;
        start = false;
        repeatTime = -1;
        OnActFinished = null;
    }   


}

public class TimerActMgr : MonoBehaviour {

    int counter = 0;

    List<TimerActObj> TimerActLib;
    List<TimerActObj> _recircleActlib;
    bool isInted = false;
    // Use this for initialization
    public void Init() {
        TimerActLib = new List<TimerActObj>(100);
        _recircleActlib = new List<TimerActObj>(100);
        counter = 0;
        DontDestroyOnLoad( gameObject );
        isInted = true;
    }   

    public int GetUID()
    {
        return counter++;
    }

    // Update is called once per frame
    public void Update() {
        if (!isInted) return;
        for (int i = 0; i < TimerActLib.Count; i++)
        {   
            if (TimerActLib[i] != null && !TimerActLib[i].isEnd)
            {
                TimerActLib[i].Update();
            }
        }

        for (int i = TimerActLib.Count - 1; i >= 0; i--)
        {
            if (TimerActLib[i] != null && TimerActLib[i].isEnd)
            {   
                _recircleActlib.Add(TimerActLib[i]);
                TimerActLib.Remove(TimerActLib[i]);
            }
        }
    }

    public TimerActObj StartTimer(float time, System.Action  act ,int repeatTime = -1 )
    {   
        TimerActObj obj = null;
        if (_recircleActlib.Count > 0)
        {   
            obj = _recircleActlib[_recircleActlib.Count - 1];
            _recircleActlib.RemoveAt(_recircleActlib.Count - 1);
            obj.Timer = time;
            obj.Act = act;
            obj.repeatTime = repeatTime;
        }
        else
            obj = new TimerActObj(time, act, this,  repeatTime);
        if (!TimerActLib.Contains(obj))
        {   
            this.TimerActLib.Add(obj);
            obj.StartTimer();
        }
        else { }
        return obj;
    }
    
    public void StopTimer(TimerActObj value)
    {   

        var obj = value;
        if (obj != null)
            obj.Clear();

    }



    private static TimerActMgr instance = null;
    public static TimerActMgr Instance
    {
        get
        {
            if (instance == null)
            {
                if (GameObject.Find("TimerActMgr") == null)
                {
                    GameObject go = new GameObject("TimerActMgr");
                    instance = go.AddComponent<TimerActMgr>();
                }
                else
                {
                    var mgr = GameObject.Find("TimerActMgr");
                    if (mgr.GetComponent<TimerActMgr>() == null)
                    {
                        instance = mgr.AddComponent<TimerActMgr>();
                    }
                    else
                    {
                        instance = mgr.GetComponent<TimerActMgr>();
                    }
                }
            }
            return instance;
        }
    }
}
