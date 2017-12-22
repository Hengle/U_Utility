using UnityEngine;
using System.Collections;

namespace ICode.Actions
{   
    [Category(Category.GameState)]
    [Tooltip("Prints a message to the console.")]
    [HelpUrl("http://docs.unity3d.com/Documentation/ScriptReference/Debug.Log.html")]
    [System.Serializable]
    public class GoEvent : StateAction
    {
        [InspectorLabel("Event")]
        [Tooltip("Event that is received from StateMachine.SendEvent")]
        public FsmString _event;
        [Shared]
        [ParameterType]
        public FsmVariable parameter;

        public override void OnEnter()
        {
            //this.Root.Owner.onReceiveAction = OnTrigger;
            ICodeViewer.Get.OnChangeEvent += OnTrigger;
        }

        private void OnTrigger(string eventName, object parameter)
        {
            if (_event.Value == eventName)
            {
                if (this.parameter != null)
                {
                    this.parameter.SetValue(parameter);
                }
                else
                {
                    Debug.LogError("parameter null ??? ");
                }
            }
            //Debug.LogError("====" + eventName + " | " + _event.Value + "  " + parameter);
        }

        public override void OnUpdate()
        {
            //Debug.LogError(ICodeViewer.Get.State);

        }
    }
}
