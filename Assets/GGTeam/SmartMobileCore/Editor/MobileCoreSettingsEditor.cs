using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobileCoreSettingsEditor : EditorWindow
{
    
    [SerializeField] GameConfigSO configSO
    {
        get
        {
            if (_configSO == null) _configSO = Resources.Load<GameConfigSO>("SmartMobileCore/MainConfig");
            return _configSO;
        }
    }
    GameConfigSO _configSO;
    

    UnityEditor.Editor editor;

    [MenuItem("GGTeam/SmartMobileCore/Settings")]
    static void Init()
    {
        var window = GetWindow(typeof(MobileCoreSettingsEditor));
        window.Show();
        window.titleContent = new GUIContent("SmartMobileCore");
        window.maxSize = new Vector2(512, 9999);    //469
        window.minSize = new Vector2(512, 480);
    }

    private void OnEnable()
    {
        GetConfig();
        editor = UnityEditor.Editor.CreateEditor(configSO);
        //editor = UnityEditor.Editor.CreateEditor(GameManager.API.Config.GameConfig);
    }

    private void GetConfig()
    {
        if (_configSO == null) _configSO = Resources.Load<GameConfigSO>("SmartMobileCore/MainConfig");
//        configSO = Resources.Load<GameConfigSO>("SmartMobileCore/MainConfig");
    }

    private void OnGUI()
    {
        ((WndMobileCoreSettingsEditor)editor).Draw();
    }


    [CustomEditor(typeof(GameConfigSO))]
    public class WndMobileCoreSettingsEditor : UnityEditor.Editor
    {
        #region Styles

        private GUIStyle style = new GUIStyle();

        private GUIStyle titleStyle;
        private GUIStyle titleTextStyle;

        private Texture2D logo;

        #endregion

        #region Properties
        private Vector2 scrollPosition;

        private List<SerializedProperty> properties_Gameplay;
        private List<SerializedProperty> properties_Debugging;
        private List<SerializedProperty> properties_Policy;
        private List<SerializedProperty> properties_Perfomance;
        private List<SerializedProperty> properties_Analytics;
        private List<SerializedProperty> properties_ADS;
        private List<SerializedProperty> properties_UserData;
        private List<SerializedProperty> properties_Publish;
        

        #endregion

        #region Enable

        void OnEnable()
        {
            GetProperties();

            logo = Resources.Load("SmartMobileCore/Textures/Logo") as Texture2D;

            titleStyle = new GUIStyle();
            titleTextStyle = new GUIStyle();

            var colorTexture = new Texture2D(1, 1);
            colorTexture.SetPixel(0, 0, new Color(0.17f, 0.17f, 0.17f, 1f));
            colorTexture.Apply();

            titleStyle.normal.background = colorTexture;
            titleTextStyle.alignment = TextAnchor.MiddleCenter;
            titleTextStyle.fontStyle = FontStyle.Bold;
            titleTextStyle.fontSize = 13;
            titleTextStyle.normal.textColor = Color.gray;
            titleTextStyle.margin = new RectOffset(0, 0, 3, 0);


            colorTexture = new Texture2D(1, 1);
            colorTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.5f));
            colorTexture.Apply();

            style.normal.background = colorTexture;
            style.fixedHeight = 0;
        }

        #endregion

        private void GetProperties()
        {
            properties_Gameplay = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL"),
                serializedObject.FindProperty("_LEVEL_USE_ONE_SCENE_FOR_ALL"),
                serializedObject.FindProperty("_LEVEL_ONE_SCENE_LEVELS_COUNT"),
            };

            properties_Policy = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_POLICY_ENABLED"),
                serializedObject.FindProperty("_POLICY_AGE_MIN"),
                serializedObject.FindProperty("_POLICY_GDRP_TEXT_EN"),
                serializedObject.FindProperty("_POLICY_CCOPA_TEXT_EN"),
            };


            properties_Debugging = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_LOG_SHOW_INFO"),
                serializedObject.FindProperty("_LOG_SHOW_DEBUG"),
                serializedObject.FindProperty("_LOG_SHOW_WARNING"),
                serializedObject.FindProperty("_LOG_SHOW_ERROR"),
            };

            properties_Perfomance = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_LEVEL_WAIT_AFTER_LOADING"),
                serializedObject.FindProperty("_HARD_TARGET_FRAMERATE"),
            };

            properties_Analytics = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_ANALYTICS_APP_KEY"),
                serializedObject.FindProperty("_ANALYTICS_LOCATION_TRACKING"),
                serializedObject.FindProperty("_ANALYTICS_EXCEPTIONS_REPORTING"),
            };

            properties_ADS = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_ADS_APP_KEY"),
                serializedObject.FindProperty("_ADS_ENABLE_VIDEO"),
                serializedObject.FindProperty("_ADS_ENABLE_BANNER"),
                serializedObject.FindProperty("_ADS_START_VIDEO_FROM_LEVEL"),
                serializedObject.FindProperty("_ADS_SHOW_TIME_MIN_SEC"),

            };

            properties_UserData = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_DATA_SAVE_PREFIX"),
                serializedObject.FindProperty("_DATA_SAVE_SUFFIX"),
            };

            properties_Publish = new List<SerializedProperty>()
            {
                serializedObject.FindProperty("_MARKET_URL_ANDROID"),
                serializedObject.FindProperty("_MARKET_URL_IOS"),
            };
        }

        private void DrawLabel(string content, string type)
        {
            float offset = 0;

            switch (type)
            {
                case "bool": offset = 40; break;
                case "Enum": offset = 100; break;
                case "float": offset = 100; break;
                case "uint": offset = 100; break;
            }


            GUILayout.Label(content, GUILayout.Width(Screen.width - offset));
        }


        public void Draw()
        {
            GUI.DrawTexture(new Rect((Screen.width / 2) - logo.width / 2, 0, logo.width, logo.height), logo);
            GUILayout.Space(logo.height);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

            var x = 0f;
            x += DrawWindow(x, properties_Gameplay, "GAMEPLAY");
            x += DrawWindow(x, properties_Policy, "POLICY");
            x += DrawWindow(x, properties_Debugging, "DEBUGGING");
            x += DrawWindow(x, properties_Perfomance, "PERFOMANCE");
            x += DrawWindow(x, properties_Analytics, "YANDEX ANALYTICS");
            x += DrawWindow(x, properties_ADS, "ADS");
            x += DrawWindow(x, properties_UserData, "USER DATA");
            x += DrawWindow(x, properties_Publish, "PUBLISH");

            GUILayout.EndScrollView();
        }

        public float DrawWindow(float offset, List<SerializedProperty> properties, string title)
        {
            var spacing = 25;
            var padding = 1 * spacing;
            var elementHeight = 24;
            var startPoint = offset * spacing + padding;
            var endPoint = properties.Count + padding / spacing;


            GUILayout.Space((elementHeight * endPoint) + endPoint);

            GUILayout.BeginArea(new Rect(0, startPoint - padding, Screen.width, elementHeight), titleStyle);

            GUILayout.Label(title, titleTextStyle, GUILayout.Width(Screen.width));

            GUILayout.EndArea();


            serializedObject.Update();

            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                if (property == null) continue;
                var tooltip = FieldData.GetValue(property.name, typeof(GameConfigSO), FieldData.ValueType.Tooltip);

                GUILayout.BeginArea(new Rect(0, startPoint + i * spacing, Screen.width, elementHeight), new GUIContent("", tooltip), EditorStyles.helpBox);
                GUILayout.BeginHorizontal();


                var name = FieldData.GetValue(property.name, typeof(GameConfigSO), FieldData.ValueType.Description);

                if (name == null)
                {
                    name = property.displayName;
                }
                //DrawLabel(name, property.type);
                if (property.type == "bool")
                {
                    //GUILayout.BeginArea(new Rect(0, startPoint + i * spacing, Screen.width, elementHeight), new GUIContent("", tooltip), EditorStyles.helpBox);
                    //    GUILayout.BeginHorizontal();
                    DrawLabel(name, property.type);
                    EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(74));
                    //    GUILayout.EndHorizontal();
                    //GUILayout.EndArea();
                }
                else if (property.type == "int")
                {
                    //GUILayout.BeginArea(new Rect(0, startPoint + i * spacing, Screen.width, elementHeight), new GUIContent("", tooltip), EditorStyles.helpBox);
                    //    GUILayout.BeginHorizontal();
                    //GUILayout.FlexibleSpace();
                    //      DrawLabel(name, property.type);
                        EditorGUILayout.LabelField(name);
                        EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(74));
                    //GUILayout.EndHorizontal();
                    //GUILayout.EndArea();
                }
                else if (property.type == "string")
                {
                    EditorGUILayout.LabelField(name, GUILayout.Width(230));
                    EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(Screen.width-250));

                    //EditorGUILayout.TextField("Hello",property.stringValue);
                    //Debug.Log(property.stringValue);
                    //EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(74));
                }



                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            serializedObject.ApplyModifiedProperties();

            return endPoint;
        }
    }

}
