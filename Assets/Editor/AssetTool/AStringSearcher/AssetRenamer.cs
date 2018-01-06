using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetTool
{
    public class AssetRenamer : EditorWindow
    {   
        public static GUILayoutOption GL_HEIGHT_30 = GUILayout.Height(30);

        public static void Init()
        {   
            var o = GetWindow<AssetRenamer>("AssetRenamer");

            o.Show();
            o.Reset();
        }

        string[] objs;
        void Reset()
        {   
            objs = null;
        }

        Object assetToSearch = null;
        string text = string.Empty;
        string suffix = string.Empty;
        string ret = string.Empty;
        int cnt = 0;
        string curFolderPath;
        void OnGUI()
        {   
            GUILayout.BeginVertical();

            assetToSearch = EditorGUILayout.ObjectField(" Drag Folder Here : ", assetToSearch, typeof(Object), true);

            GUILayout.Space(12);
            GUILayout.Label("批量改名,只处理第一层文件，无法撤回请小心使用", GUILayout.Width(328));
            GUILayout.Space(12);
            if (assetToSearch != null)
            {   
                GUILayout.BeginHorizontal();
                GUILayout.Label("Change Asset Name :", GUILayout.Width(128));
                GUILayout.Space( 32 );
                text = GUILayout.TextField(text, GUILayout.Width(64));
                GUILayout.Label("[ origin name ]", GUILayout.ExpandWidth(false));
                suffix = GUILayout.TextField(suffix, GUILayout.Width(64));
                GUILayout.EndHorizontal();
                
                GUILayout.Space(12);
                posGroupEnabled = true;
                posGroupEnabled = EditorGUILayout.BeginToggleGroup("Filter by Type (单选)(必选)", posGroupEnabled);
                toggleA = EditorGUILayout.Toggle("Material", toggleA);
                toggleB = EditorGUILayout.Toggle("Shader", toggleB);
                toggleC = EditorGUILayout.Toggle("Texture", toggleC);
                toggleD = EditorGUILayout.Toggle("Sprite", toggleD);
                toggleE = EditorGUILayout.Toggle("Prefab", toggleE);
                EditorGUILayout.EndToggleGroup();

                string filter = GetFilter();
                GUILayout.Space(12);
                GUILayout.Label("[ Filter : ] "+ filter);
                bool IsValidFolder = AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(assetToSearch));
                if (IsValidFolder)
                {   
                    curFolderPath = AssetDatabase.GetAssetPath(assetToSearch);
                    objs = AssetDatabase.FindAssets(filter, new string[1] { AssetDatabase.GetAssetPath(assetToSearch) });
                    ret = string.Format(" [result : ] There are {0}  asset under folder {1}", objs.Length, assetToSearch);
                }   
                else
                {   
                    Debug.LogError("文件夹!!!");
                }   

                GUILayout.Label(" [!!!] 请确保 没输入 非法符号!!!");
                GUILayout.Space(12);
                GUILayout.Label(ret);
                GUILayout.Space(12);
                if (GUILayout.Button("Renaming To : " + text + "[ origin name ]" + suffix, GL_HEIGHT_30))
                {   
                    if (assetToSearch != null)
                    {   
                        if (EditorUtility.DisplayDialog(_dlg_t, _dlg_t, _dlg_ok, _dlg_ng))
                        {   
                            if (objs != null && objs.Length > 0)
                            {   
                                for (int i = 0; i < objs.Length; i++)
                                {   
                                    var obj = objs[i];
                                    var path = AssetDatabase.GUIDToAssetPath(obj);
                                    var o = AssetDatabase.LoadAssetAtPath<Object>( path );
                                    if (o)
                                    {   
                                        var tgt = path.Remove(path.IndexOf(o.name));
                                        // 只处理第一层级 文件
                                        if (tgt.Equals(curFolderPath+"/"))
                                        {   
                                            AssetDatabase.RenameAsset(AssetDatabase.GUIDToAssetPath(obj),  text+o.name+suffix );
                                         }
                                    }   
                                }   
                            }   
                        }
                    }
                    Reset();
                }
            }   

            GUILayout.EndVertical();
            GUILayout.Space(12);
        }
        private Vector2 scrollPosition = Vector2.zero;
        public static GUILayoutOption GL_EXPAND_WIDTH = GUILayout.ExpandWidth(true);
        public static GUILayoutOption GL_EXPAND_HEIGHT = GUILayout.ExpandHeight(true);
        string _dlg_t = "Renaming";
        string _dlg_c = "Sure to Renaming ? ";
        string _dlg_ok = "Ok";
        string _dlg_ng = "No";
        bool toggleA = false;
        bool toggleB = false;
        bool toggleC = false;
        bool toggleD = false;
        bool toggleE = true;

        bool posGroupEnabled = true;
        string tmp = "";
        string GetFilter()
        {   
            if (toggleA)
                tmp = "t:Material";
            if (toggleB)
                tmp = "t:Shader";
            if (toggleC)
                tmp = "t:Texture";
            if (toggleD)
                tmp = "t:Sprite";
            if (toggleE)
                tmp = "t:GameObject";
            return tmp;
        }
    }

}

