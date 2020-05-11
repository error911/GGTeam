// ================================
// Free license: CC BY Murnik Roman
// ================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    [CustomEditor(typeof(GameManager), true)]
    [CanEditMultipleObjects]
    public class GameManagerEditor : Editor
    {
        //SerializedProperty Levels;


#if UNITY_EDITOR
//        void OnEnable()
//        {
//            Levels = serializedObject.FindProperty("Levels");
//        }
#endif

#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //EditorGUILayout.PropertyField(_Data, true);

            //if (Levels == null) return;
            //var d_num = Levels.FindPropertyRelative("CurrentNumber").intValue;

            //var d_score = _Data.FindPropertyRelative("score").intValue;
            //var d_stars = _Data.FindPropertyRelative("stars").floatValue;

            //EditorGUILayout.BeginVertical();
//            EditorGUILayout.LabelField("- Ядро --------------------------------------------------------------------");
            //EditorGUILayout.LabelField("Уровень: " + d_num + " / " + "Очки: " + d_score + " / " + "Звезды: " + d_stars);
//            EditorGUILayout.LabelField("Уровней: " + d_num);
//            EditorGUILayout.LabelField("------------------------------------------------------------------------------------");
            //EditorGUILayout.EndVertical();


            //EditorGUILayout.PropertyField(m);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
#endif

    }
}