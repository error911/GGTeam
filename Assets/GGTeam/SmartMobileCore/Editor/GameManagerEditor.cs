// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

//namespace GGTeam.SmartMobileCore
//{
    [CustomEditor(typeof(GameManager), true)]
    [CanEditMultipleObjects]
    public class GameManagerEditor : Editor
    {
        SerializedProperty gameConfig;

        Texture2D icon_ads_on;
        Texture2D icon_ads_off;

        Texture2D icon_stat_on;
        Texture2D icon_stat_off;
        
        Texture2D icon_iap_on;
        Texture2D icon_iap_off;

#if UNITY_EDITOR
        void OnEnable()
        {
            //gameConfig = serializedObject.FindProperty("gameConfig");

            if (icon_ads_on == null) icon_ads_on = Resources.Load<Texture2D>("SmartMobileCore/Icons/ads_1");
            if (icon_ads_off == null) icon_ads_off = Resources.Load<Texture2D>("SmartMobileCore/Icons/ads_0");

            if (icon_stat_on == null) icon_stat_on = Resources.Load<Texture2D>("SmartMobileCore/Icons/stat_1");
            if (icon_stat_off == null) icon_stat_off = Resources.Load<Texture2D>("SmartMobileCore/Icons/stat_0");

            if (icon_iap_on == null) icon_iap_on = Resources.Load<Texture2D>("SmartMobileCore/Icons/iap_1");
            if (icon_iap_off == null) icon_iap_off = Resources.Load<Texture2D>("SmartMobileCore/Icons/iap_0");
        }
#endif

#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (gameConfig == null) gameConfig = serializedObject.FindProperty("gameConfig");

            //GameManager gm = this.target as GameManager;
            //GameConfigSO cfg = gm.cfgEditor;
            GameConfigSO cfg = gameConfig.objectReferenceValue as GameConfigSO;

            bool b_ads = false;
            bool b_stat = false;
            bool b_iap = false;

            if (cfg != null)
            {
                if (!string.IsNullOrEmpty(cfg.ADS_APP_KEY))
                    if (cfg.ADS_ENABLE_BANNER || cfg.ADS_ENABLE_VIDEO) b_ads = true;

                if (!string.IsNullOrEmpty(cfg.ANALYTICS_APP_KEY)) b_stat = true;

#if UNITY_PURCHASING
                b_iap = true;
#endif
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (b_ads) { if (icon_ads_on) GUILayout.Label(icon_ads_on); } else if (icon_ads_off) GUILayout.Label(icon_ads_off);
            if (b_stat) { if (icon_stat_on) GUILayout.Label(icon_stat_on); } else if (icon_stat_off) GUILayout.Label(icon_stat_off);
            if (b_iap) { if (icon_iap_on) GUILayout.Label(icon_iap_on); } else if (icon_iap_off) GUILayout.Label(icon_iap_off);
            GUILayout.EndHorizontal();


            EditorGUILayout.Space();
            if (!b_iap) EditorGUILayout.HelpBox("Для активации внутриигровых покупок (IAP), включите In-App Purchasing в окне Services [Ctrl+0]", MessageType.Info);
            if (!b_stat) EditorGUILayout.HelpBox("Для активации аналитики (STAT), задайте ключь ANALYTICS_APP_KEY в GameConfig", MessageType.Info);
            if (!b_ads) EditorGUILayout.HelpBox("Для активации рекламы (ADS), задайте ключь ADS_APP_KEY в GameConfig и активируйте Видео и/или Баннер рекламу", MessageType.Info);


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
//}