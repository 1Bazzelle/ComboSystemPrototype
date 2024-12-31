using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Input))]
public class InputEditor : Editor
{
    private SerializedProperty type;

    private SerializedProperty secondary;
    private void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        secondary = serializedObject.FindProperty("secondary");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(secondary, new GUIContent("Secondary Input"));

        EditorGUILayout.PropertyField(type, new GUIContent("Input Type"));
       
        serializedObject.ApplyModifiedProperties();
    }
}
