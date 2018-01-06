using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetTool
{
    public class AStringSearcher : EditorWindow
    {   
        public static GUILayoutOption GL_HEIGHT_30 = GUILayout.Height(30);
        public enum ESuffixType
        {   
            cs,
            lua,
            txt,
            md,
            csv
        }
        public bool isCsSearcher = false;
        public static void InitCS()
        {   
            var o = GetWindow<AStringSearcher>("code Searcher");
            o.isCsSearcher = true;
            o.Show();
            o.Reset();
        }   

        public static void InitTable()
        {   
            var o = GetWindow<AStringSearcher>("table Searcher");
            o.isCsSearcher = false;
            o.Show();
           o.Reset();
        }

        public int SearchKey(AStringSearcher.ESuffixType type, string KeyWord)
        {   
            
            var fs = GetFiles(type, KeyWord);

            if (fs != null && fs.Length > 0)
            {   
                for (int i = 0; i < fs.Length; i++)
                {   
                    GetTargetString(fs[i], KeyWord);
                }
            }
            return _ret_cnt;
        }   

        string _ret_string;
        List<Object> _ret;
        int _ret_cnt;
        void Reset()
        {   
            _ret_cnt = 0;
            _ret_string = string.Empty;
            if (_ret != null && _ret.Count > 0)
                _ret.Clear();
            _ret = new List<Object>();
            cnt = 0;
        }

        void OnSearchEnd()
        {   
            _ret_string += "\nMatch file found : " + _ret_cnt;
        }

        string[] GetFiles(ESuffixType suffix, string KeyWord)
        {
            return GetFiles(suffix.ToString(), KeyWord);
        }

        string[] GetFiles(string suffix, string KeyWord)
        {
            var files = Directory.GetFiles(@Application.dataPath, "*." + suffix, SearchOption.AllDirectories);
            _ret_string += "\nSearch [" + KeyWord + "] ." + suffix + " files total : " + files.Length;
            return files;
        }

        void GetTargetString(string file, string keyWord)
        {   
            var st = File.ReadAllText(@file);

            if (st.Contains(keyWord))
            {   
                ++_ret_cnt;
                var unityPath = file.Substring(file.IndexOf("Assets"));
                var go = AssetDatabase.LoadAssetAtPath(unityPath, typeof(Object));
                if (go != null)
                {   
                    _ret.Add(go);
                }
            }
            //else
            //    Debug.LogError("=====NO");
        }
        Object assetToSearch = null;
        string text = string.Empty;
        string ret = string.Empty;
        int cnt = 0;
        void OnGUI()
        {   
            GUILayout.BeginVertical();

            assetToSearch = EditorGUILayout.ObjectField("Asset: ", assetToSearch, typeof(Object), true);

            text = GUILayout.TextField(text, GL_HEIGHT_30);

            if (assetToSearch != null)
                text = assetToSearch.name;

            if (GUILayout.Button("Search KeyWord: " + text, GL_HEIGHT_30))
            {  
                Reset();
                if (string.IsNullOrEmpty(text) || (text != null && text.Length < 3))
                {   
                    ret = "[ warnning ] search keyword  is null or length < 3 ?? ";
                }
                else
                {   
             
                    if (isCsSearcher)
                    {   
                        cnt = SearchKey(ESuffixType.cs, text);
                    }   
                    else
                    {   
                        cnt = SearchKey(ESuffixType.md, text);
                        cnt += SearchKey(ESuffixType.lua, text);
                        cnt += SearchKey(ESuffixType.txt, text);
                    }   
                    OnSearchEnd();
                    if (cnt > 0)
                        ret = "search success ! " + _ret_string;
                    else
                        ret = " searched none ";

                }
            }

            GUILayout.Label(ret);
            GUILayout.EndVertical();
            GUILayout.Space(12);
            if (cnt > 0 && _ret != null && _ret.Count > 0)
            {   
                GUILayout.Label("Result : ");
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GL_EXPAND_WIDTH, GL_EXPAND_HEIGHT);
                for (int i = 0; i < _ret.Count; i++)
                {   
                    var obj = _ret[i];
                    if (GUILayout.Button(obj.name + " @ " + AssetDatabase.GetAssetPath(obj), GL_HEIGHT_30))
                    {
                        obj.SelectInEditor();
                    }
                }
                EditorGUILayout.EndScrollView();
            }   
        }
        private Vector2 scrollPosition = Vector2.zero;
        public static GUILayoutOption GL_EXPAND_WIDTH = GUILayout.ExpandWidth(true);
        public static GUILayoutOption GL_EXPAND_HEIGHT = GUILayout.ExpandHeight(true);
    }

}

