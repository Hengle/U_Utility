using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(AreaSetter))]
public class AreaSetterEditor : Editor
{   
    public EAreaType mEObjType { get { return _type; } }
    EAreaType _type;

    void OnEnable()
    {   
        Apply();
    }
    public override void OnInspectorGUI()
    {   
        _type = (EAreaType)EditorGUILayout.EnumPopup(" EObjType : ", _type);
        GUILayout.Label("Update Type in  Editor : ");
        var btn = GUILayout.Button(" Update Type ");
        if (btn)
        {   
            Apply();
        }
    }

    void Apply()
    {
        AreaSetter _obj = (AreaSetter)target;
        //_type = _obj.ApplyTypeInEditorMode();
        EditorUtility.SetDirty(target);
        AssetDatabase.Refresh();
    }   
}