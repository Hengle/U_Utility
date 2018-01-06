using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace AsssetHelper
{
    public class AHSettingsCreator : MonoBehaviour
    {
        public const string NAME = "AHSettingsData";

        internal static string GetAssetPath()
        {
            //SettingsData
            string[] scriptableAssetsFound = AssetDatabase.FindAssets("AHSettingsData t:ScriptableObject", null);
            if (scriptableAssetsFound.Length >= 1)
            {
                return AssetDatabase.GUIDToAssetPath(scriptableAssetsFound[0]);
            }
            //If the scriptableObject does not exist
            else
            {
                string[] allPaths = Directory.GetDirectories(Application.dataPath, "Settings", SearchOption.AllDirectories);
                foreach (string path in allPaths)
                {
                    if (path.Contains(string.Format("{0}AH{1}Editor{2}Settings", Path.DirectorySeparatorChar, Path.DirectorySeparatorChar, Path.DirectorySeparatorChar)))
                    {
                        string relativepath = "";

                        if (path.StartsWith(Application.dataPath))
                        {
                            relativepath = "Assets" + path.Substring(Application.dataPath.Length) + Path.DirectorySeparatorChar;
                        }

                        AHSettings asset = ScriptableObject.CreateInstance<AHSettings>();
                        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(relativepath + NAME + ".asset");

                        AssetDatabase.CreateAsset(asset, assetPathAndName);

                        AssetDatabase.SaveAssets();

                        return assetPathAndName;
                    }
                }
            }
            return null;
        }
    }
}