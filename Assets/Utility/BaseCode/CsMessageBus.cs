using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgHandleObj
{
    public string msgType;
    public bool removed = false;
    public CsMessageBus.MessageHandle handle;
    public ulong uid = UInt64.MinValue;
    public int msgId;

    public void Reset()
    {   
        msgType = string.Empty;
        removed = true;
        handle = null;
        uid = UInt64.MinValue;
        msgId = 0;
    }   

    public static Stack<MsgHandleObj> recircleObj = new Stack<MsgHandleObj>(_intSize);
    const int _intSize = 800; 
    const int  _stackSize = 1000;

    public static bool isMaxSize { get { return recircleObj.Count >= _stackSize; }  }

    public static void Release(MsgHandleObj b )
    {   
        if (isMaxSize)
            return;
        recircleObj.Push(  b );
    }   

    public static MsgHandleObj Allocate(int mId, ulong uId, CsMessageBus.MessageHandle h, string type)
    {
        MsgHandleObj tmp;
        if (recircleObj.Count > 0)
        {
            tmp = recircleObj.Pop();
            tmp.msgId = mId;
            tmp.uid = uId;
            tmp.handle = h;
            tmp.msgType = type;
            ReCounter++;
        }
        else
        {
            tmp = new MsgHandleObj() { msgId = mId, uid = uId, handle = h, msgType = type };
            newCounter++;
        }
        tmp.removed = false;
        //Debug.LogError("newCounter " + newCounter + "  ReCounter " + ReCounter);
        return tmp;
    }

    public static void PreInit()
    {   
        for (int i = 0; i < _intSize; i++)
        {   
            var o = new MsgHandleObj();
            recircleObj.Push( o );
        }   
    }   

    static int ReCounter = 0;
    static int newCounter = 0;
}

public  class MsgUtil
{   
    public static string[] battleMsgList = new string[10]
    {
         "SkillMessage.OnUseSkillFail",
        "SkillMessage.OnSkillCasting",
        "SkillMessage.OnUseSkill",
        "DamageMessage.OnDamageNotify",
        "CsPlatoMoveStop.Stop",
        "CsPlatoColliderTrigger.In",
        "ActorMessage.OnActorNotify",
        "SkillMessage.OnSkillEndNotify",
        "SkillMessage.OnSkillEnd",
        "InterruptMessage.OnInterrupt"
    };

    public static void ClearBattleMsg()
    {   
        for (int i = 0; i < battleMsgList.Length; i++)
        {   
            var type = battleMsgList[i];
            CsMessageBus.Instance.ClearDicByType(type);
        }
    }

}

public class CsMessageBus
{
    bool _debug = false;
    bool m_useNewMsgDis = true;

    public static CsMessageBus Instance
    {   
        get { return instance ?? (instance = new CsMessageBus()); }
    }

    public void Init()
    {   
        MsgHandleObj.PreInit();
    }   

    // MsgDis 
    //================================
    Dictionary<string, List<MsgHandleObj>> _msgDic = new Dictionary<string, List<MsgHandleObj>>();
    Dictionary<int, string> _msgRef = new Dictionary<int, string>();

    public void ClearDicByType(string type )
    {   
        if (_msgDic.ContainsKey(type))
        {   
             _msgDic[type].ForEach(  x=> { MsgHandleObj.Release( x );  } );
            _msgDic[type].Clear();
            _msgDic.Remove(type);
        }
        //else
        //    Debug.LogError( type +" not exit ? " );
    }


    int RegMsgWithId(string messageType, ulong id, MessageHandle handle)
    {   
        return RegMsg(messageType, handle, id);
    }   
    int RegMsg(string messageType, MessageHandle handle, ulong id = UInt64.MinValue)
    {   
        int _hid = ++handleCounter;
        List<MsgHandleObj> list = null;
        if (!_msgDic.TryGetValue(messageType, out list))
        {   
            list = new List<MsgHandleObj>();
            _msgDic.Add(messageType, list);
        }   
        _msgRef.Add(_hid, messageType);

        if (handle == null)
        {   
            // ?  null ?
            Debug.LogError("RegMsg " + messageType);
        }   
        else
        {   
            MsgHandleObj obj = MsgHandleObj.Allocate(_hid, id, handle, messageType);
            if (list.Contains(obj) == false)
            {   
                if (list.Count > 0)
                {   
                    var o = list[list.Count - 1];
                    // todo, 可以针对 plato trigger In 高频 事件 优化 插入
                    if (o.removed)
                    {   
                        list[list.Count - 1] = obj;
                    }   
                    else
                    {
                        list.Add(obj);
                    }
                }
                else
                {   
                    list.Add( obj );
                }
            }
        }
        return _hid;
    }   

    public void DisMsg(string messageType, CsMessage message)
    {
        DisMsg(messageType, message, UInt64.MinValue);
    }

    public void DisMsgWithId(string messageType, ulong id, CsMessage message)
    {   
        DisMsg(messageType, message, id);
    }
    protected void DisMsg(string messageType, CsMessage message, ulong id = UInt64.MinValue)
    {   
        List<MsgHandleObj> handles;
        if (_msgDic.TryGetValue(messageType, out handles))
        {

        }
        else
        {
            // ? 
        }

        if (handles != null && handles.Count > 0)
        {   
             int total = handles.Count;
            for (int i= 0; i < total; i++)
            {   
                var _hand = handles[i];
                //if (message != null && message.IsSent == false)
                {   
                    var obj = _hand;
                    
                    if (obj == null || (obj != null && obj.removed))
                        continue;
                    // no  uid
                    if (obj.uid == UInt64.MinValue)
                    {   
                        if (obj.handle != null)
                            obj.handle(message);
                    }
                    else
                    {
                        // uid 
                        if (obj.uid == id)
                        {   
                            if( obj.handle != null )
                                obj.handle(message);
                        }
                    }
                }
                if (message == null)
                {
                    Debug.LogWarning("[msg Dis] " + messageType + " Msg is Null ??? ");
                }
            }
        }
    }

    void RemoveMsg( int id )
    {
        string type;
        if (this._msgRef.TryGetValue(id, out type))
        {   
            List<MsgHandleObj> obj;
            if (this._msgDic.TryGetValue(type, out obj))
            {   
                for (int i = obj.Count - 1; i >= 0; i--)
                {   
                    var o = obj[i];
                    if (o.msgId == id)
                    {   
                        o.Reset();
                    }   
                }
            }      
        }   
        if (string.IsNullOrEmpty(type) == false)
                _msgRef.Remove(id);
    }   

    // ===========================================================

    public int RegisterMessageHandleWithId(string messageType, ulong id, MessageHandle handle)
    {   
        if (m_useNewMsgDis)
            return this.RegMsgWithId(messageType,id,handle);
        return 0;
    }

    public int RegisterMessageHandle(string messageType, MessageHandle handle)
    {
        if (m_useNewMsgDis)
            return this.RegMsg(messageType,handle);
        return 0;
    }

    public void RemoveMessageHandle(int handleId)
    {
        if (m_useNewMsgDis)
        {
            this.RemoveMsg( handleId );
            return;
        }
    }

    public void SendMessageWithId(string messageType, ulong id, CsMessage message)
    {
        if (m_useNewMsgDis)
        {
            this.DisMsgWithId( messageType, id,message);
            return;
        }
    }

    public void SendMessage(string messageType, CsMessage message)
    {   
        if(_debug)
            Debug.LogError("[ msg ]    SendMessage   " + messageType + " | " + message);

        if (m_useNewMsgDis)
        {   
            this.DisMsg(messageType, message);
            return;
        }
    }

    private int handleCounter = 0;

    private static CsMessageBus instance;

    public delegate void MessageHandle(CsMessage message);
}


public class CsNetMsg : CsMessage
{       
    public string DoSmth = "CsNetMsg.DoSmth";
}

public class CsMessage
{   
    protected bool mIsSent = false;
    public bool IsSent
    {
        get { return mIsSent; }
        set { mIsSent = value; }
    }

    public object this[string key]
    {
        get
        {
            object val = null;
            if (dataTable != null && dataTable.ContainsKey(key))
            {
                val = dataTable[key];
            }
            return val;
        }
        set
        {
            if (dataTable == null)
            {
                dataTable = new Hashtable();
            }
            dataTable[key] = value;
        }
    }

    private Hashtable dataTable = null;

}

