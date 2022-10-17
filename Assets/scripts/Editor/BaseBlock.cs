using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Conveyor))]
public class BaseBlock : Editor
{
    Block m_block = null;

    private void OnEnable()
    {
        m_block = target as Conveyor;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if(GUILayout.Button("È¸Àü"))
        {
            m_block.OneRotate();
        }
    }
}
