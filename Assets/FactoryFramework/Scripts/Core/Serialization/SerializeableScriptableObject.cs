using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FactoryFramework
{
    public class SerializeableScriptableObject : ScriptableObject
    {
		[SerializeField, HideInInspector] private string _guid;
		public string Guid => _guid;

#if UNITY_EDITOR
		void OnValidate()
		{
			var path = AssetDatabase.GetAssetPath(this);
			_guid = AssetDatabase.AssetPathToGUID(path);
		}
#endif
	}
}