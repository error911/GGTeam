// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level), true)]    //, true, isFallback = false)
[CanEditMultipleObjects]
public class LevelEditor : Editor
{
    SerializedProperty _Data;

#if UNITY_EDITOR
    void OnEnable()
    {
        _Data = serializedObject.FindProperty("_Data");
    }
#endif

#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(_Data, true);

        var d_num = _Data.FindPropertyRelative("num").intValue;
        var d_score = _Data.FindPropertyRelative("score").intValue;
        var d_stars = _Data.FindPropertyRelative("stars").floatValue;

        //EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("- Логика уровня --------------------------------------------------------------------");
        EditorGUILayout.LabelField("Уровень: " + d_num + " / " + "Очки: " + d_score + " / " + "Звезды: " + d_stars);
        EditorGUILayout.LabelField("------------------------------------------------------------------------------------");
        //EditorGUILayout.EndVertical();


        //EditorGUILayout.PropertyField(m);
        serializedObject.ApplyModifiedProperties();
        
        base.OnInspectorGUI();
    }
#endif
}
