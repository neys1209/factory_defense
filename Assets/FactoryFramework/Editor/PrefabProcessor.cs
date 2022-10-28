using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace FactoryFramework
{
    // Whenever a prefab is imported or updated, check if it is a LogisticComponent and assign _prefabPath appropriately
    public class PrefabProcessor : AssetPostprocessor
    {
        public static Regex resourceReg = new Regex(@".*Resources/(.*)\.prefab");
        void OnPostprocessPrefab(GameObject g)
        {
            var lc = g.GetComponent<LogisticComponent>();
            if (lc)
            {
                MatchCollection matches = resourceReg.Matches(AssetDatabase.GetAssetPath(g));
                if (matches.Count > 0)
                {
                    lc._prefabPath = matches[0].Groups[1].Value;
                }
            }
        }
    }

    // Whenever a prefab is moved, check if it is a LogisticComponent and assign _prefabPath appropriately
    public class PrefabModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {

            GameObject g = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            var lc = g.GetComponent<LogisticComponent>();
            if (lc)
            {
                MatchCollection matches = PrefabProcessor.resourceReg.Matches(AssetDatabase.GetAssetPath(g));
                if (matches.Count > 0)
                {
                    lc._prefabPath = matches[0].Groups[1].Value;
                    EditorUtility.SetDirty(g);
                }
            }
            AssetDatabase.SaveAssetIfDirty(g);
            return AssetMoveResult.DidNotMove;
        }
    }

}