using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetTool
{

    public class AssetToolUtil
    {   
        [MenuItem( "Helper/AssetTool",false,300 )]
        static void Go()
        {
            EditorWindow.GetWindow<AssetToolwnd>("AssetTool");
        }
    }


    public class AssetToolwnd : EditorWindow
    {   
        private Rect windowOneRect = new Rect(10, 30, 360, 100);
        private Rect windowTwoRect = new Rect(10,140, 360, 100);
        private Rect windowThreeRect = new Rect(10,260, 360, 100);
        private Rect windowFourRect = new Rect(10, 380, 360, 100);
        private Rect windowFiveRect = new Rect(10, 500, 360, 100);
        private void OnEnable()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void OnGUI()
        {   
            GUIWindow();
            Repaint();
        }

        void GUIWindow()
        {   
            BeginWindows();
            windowOneRect = GUILayout.Window(2, windowOneRect, AssetUsageFinder, "AssetUsageFinder");
            windowTwoRect = GUILayout.Window(3, windowTwoRect, SearchAssetInTableByArtPath, "SearchAssetInTableByArtPath");
            windowThreeRect = GUILayout.Window(4, windowThreeRect, SearchAssetInCodeByArtPath, "SearchAssetInCodeByArtPath");
            windowFourRect = GUILayout.Window(5, windowFourRect, Renamer, "Renamer");
            windowFiveRect = GUILayout.Window(6, windowFiveRect, SeachUnusedAsset, "SearchUnusedAsset");
            EndWindows();
        }   

        void AssetUsageFinder(int windowId)
        {   
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("AssetUsageFinder：");
            if (GUILayout.Button("Open Finder", GUILayout.Height(20), GUILayout.Width(240)))
            {
                AssetTool.AssetUsageDetector.Init();
            }   
            GUILayout.EndVertical();
        }

        void Renamer(int wId)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("Renamer：");
            if (GUILayout.Button("Open Renamer", GUILayout.Height(20), GUILayout.Width(240)))
            {
                AssetRenamer.Init();
            }
            GUILayout.EndVertical();
        }

        void SearchAssetInTableByArtPath(int wId)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("SearchAssetInTableByArtPath：");
            if (GUILayout.Button("Open Table Searcher", GUILayout.Height(20), GUILayout.Width(240)))
            {   
                AStringSearcher.InitTable();
            }
            GUILayout.EndVertical();
        }

        void SearchAssetInCodeByArtPath(int wId)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("SearchAssetInCodeByArtPath：");
            if (GUILayout.Button("Open Code Searcher", GUILayout.Height(20), GUILayout.Width(240)))
            {   
                AStringSearcher.InitCS();
            }
            GUILayout.EndVertical();
        }

        void SeachUnusedAsset(int wId)
        {   
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("SearchUnusedAsset：");
            if (GUILayout.Button("Open Tool", GUILayout.Height(20), GUILayout.Width(240)))
            {   
                AsssetHelper.AHMainWindow.OpenAssetHelper();
            }   
            GUILayout.EndVertical();
        }
    }
}


