        
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace ICode.Actions{
	[Category(Category.GameState)]
	[Tooltip("Prints a message to the console.")]
	[HelpUrl("http://docs.unity3d.com/Documentation/ScriptReference/Debug.Log.html")]
	[System.Serializable]
	public class GameState : StateAction {

        public EGameState mGameState;
        
        [Tooltip("Message to print.")]
		public FsmString message;

        List<System.Action> OnEnterActs = new List<System.Action>();
        List<System.Action> OnExitActs = new List<System.Action>();


        public void SetAct(System.Action enter , System.Action exit )
        {   

            if (enter != null && !OnEnterActs.Contains(enter))
                OnEnterActs.Add( enter );
            if (exit != null && !OnExitActs.Contains(exit))
                OnExitActs.Add(exit);
            _isInited = true;
        }   

        public bool isInited { get { return _isInited; } }
        bool _isInited = false;

        public override void OnEnter ()
        {   
            base.OnEnter();
            if ( ICodeViewer.Get.allowStateDebug)
			    Debug.Log ("GameState OnEnter " + message.Value);

            if (OnEnterActs != null && OnEnterActs.Count > 0)
            {   
                OnEnterActs.ForEach(a => { if (a != null) a(); });
            }   
        }

		public override void OnUpdate ()
		{   
            
		}

        public override void OnExit()
        {   
            if (ICodeViewer.Get.allowStateDebug)
                Debug.Log("GameState OnExit " + message.Value);
    
            if (OnExitActs != null && OnExitActs.Count > 0)
            {   
                OnExitActs.ForEach(a => { if (a != null) a(); });
            }
            base.OnExit();
        }
    }
}
