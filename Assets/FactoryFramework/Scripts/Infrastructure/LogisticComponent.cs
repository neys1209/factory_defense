using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FactoryFramework
{
    public abstract class LogisticComponent : MonoBehaviour
    {
        protected GlobalLogisticsSettings settings { get { return ConveyorLogisticsUtils.settings; } }

        public virtual void ProcessLoop() { }

        private Guid _guid = Guid.Empty;
        
        [HideInInspector]
        public Guid GUID
        {
            get
            {
                if (_guid.Equals(Guid.Empty))
                    _guid = Guid.NewGuid();
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        [SerializeField, HideInInspector]
        public string _prefabPath = "";

        static Regex rx = new Regex(@".*Resources/(.*)\.prefab");

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(PrefabUtility.IsPartOfAnyPrefab(this))
            {
                MatchCollection matches = rx.Matches(AssetDatabase.GetAssetPath(this));
                if (matches.Count > 0)
                {
                    _prefabPath = matches[0].Groups[1].Value;
                }
            }   
        }
#endif

    }
}