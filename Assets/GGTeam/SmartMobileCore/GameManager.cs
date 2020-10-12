// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.SmartMobileCore;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

//namespace GGTeam.SmartMobileCore
//{
    public class GameManager : MonoBehaviour
    {

        //#if UNITY_EDITOR
        //        protected GameConfigSO cfgEditor => gameConfig;
        //#endif

        [SerializeField] GameConfigSO gameConfig = null;

        /// <summary>
        /// Конфигурация
        /// </summary>
        public ConfigHeader Config;

        /// <summary>
        /// Логирование
        /// </summary>
        public LogHeader Log;

        /// <summary>
        /// Сцены
        /// </summary>
        [Obsolete("Будет исключено в будующем")]
        public ScenesHeader Scenes;

        /// <summary>
        /// Уровни
        /// </summary>
        [SerializeField] public LevelsHeader Levels;

        /// <summary>
        /// Пользовательский интерфейс
        /// </summary>
        public UIHeader UI;

        /// <summary>
        /// Рекламная площадка
        /// </summary>
        public AdsHeader ADS;

        readonly IAdsProvider adsProvider = new IronSourceAdsProvider();

        /// <summary>
        /// Аналитика
        /// </summary>
        [HideInInspector] public Analytics Metrica;

        internal bool IsInited { get; private set; } = false;

        /// <summary>
        /// Синглтон
        /// </summary>
        [Obsolete("Будет исключено в будующем (Используйте Level->Game)")]
        public static GameManager API;

        void Awake()
        {
#pragma warning disable CS0618 // Тип или член устарел
            API = this;
#pragma warning restore CS0618 // Тип или член устарел
            Prepare();

            //Invoke("Init", 8);
            Init();
        }

        void Prepare()
        {
            Config = new ConfigHeader(this, gameConfig);
            Log = new LogHeader(this);
            //#pragma warning disable CS0618 // Тип или член устарел
            //            Scenes = new ScenesHeader(this);
            //#pragma warning restore CS0618 // Тип или член устарел
            //            Levels = new LevelsHeader(this);
            UI = new UIHeader(this);
            ADS = new AdsHeader(this, adsProvider);

            #region === Инициализация Аналитики ===
            var metrPref = Resources.Load<GameObject>("SmartMobileCore/Prefabs/[Analytics]");
            if (metrPref == null) Debug.LogError("Не найден префаб SmartMobileCore/Prefabs/[Analytics]");
            var metrGo = Instantiate(metrPref);
            metrGo.name = "[Analytics]";
            //Metrica = Analytics.Instance;
            //Metrica.OnActivation += dfsdfsd;
            //Metrica.ResumeSession();
            Metrica = metrGo.GetComponent<Analytics>();

            #endregion
        }

        void Init()
        {
            // Проверяем есть ли EventSystem
            //var es = GetComponent<EventSystem>();
            var es = FindObjectOfType<EventSystem>();
            if (es == null)
            {
                GameObject go = new GameObject("EventSystem");
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }

            if (Config.GameConfig.POLICY_ENABLED)
            {
                var gdrpPref = Resources.Load<GameObject>("SmartMobileCore/Prefabs/[GDRPPolicy]");
                if (gdrpPref == null)
                {
                    Log.Error("Prefab 'Resources/SmartMobileCore/Prefabs/[GDRPPolicy]' not found.");
                    On_GDRPP_Complete();
                }
                else
                {
                    var go = Instantiate(gdrpPref);
                    GDRPPolicy created_GDRPPolicy = go.GetComponent<GDRPPolicy>();
                    if (created_GDRPPolicy != null)
                    {
                        created_GDRPPolicy.Init(Config.GameConfig.POLICY_GDRP_TEXT_EN, Config.GameConfig.POLICY_CCOPA_TEXT_EN, On_GDRPP_Complete);
                    }
                }
            }
            else
            {
                On_GDRPP_Complete();
            }

            void On_GDRPP_Complete()
            {
                //UI.Init();
                Loading();
            }
        }

        //void Start()
        void InitAds()
        {
            if (Config.GameConfig.ADS_APP_KEY.Length > 0)
            {
                ADS.Init(Config.GameConfig.ADS_APP_KEY, Config.GameConfig.ADS_ENABLE_VIDEO, Config.GameConfig.ADS_ENABLE_BANNER, Config.GameConfig.ADS_START_VIDEO_FROM_LEVEL, Config.GameConfig.ADS_SHOW_TIME_MIN_SEC);
            }
            else
            {
                ADS.Init("", false, false, 0, 0);
            }

            if (Config.GameConfig.HARD_TARGET_FRAMERATE >= 0) Application.targetFrameRate = Config.GameConfig.HARD_TARGET_FRAMERATE;

            IsInited = true;
            Log.Info("GameManager", "Started");
        }

        Loading ld;
        void Loading()
        {
            var loadingPref = Resources.Load<GameObject>("SmartMobileCore/Prefabs/[Loading]");
            if (loadingPref == null) Debug.LogError("Не найден префаб SmartMobileCore/Prefabs/[Loading]");
            var loadingGo = Instantiate(loadingPref);
            ld = loadingGo.GetComponent<Loading>();
            ld.StartProcess(this);

            Invoke("Loading2", 0.25f);
        }

        void Loading2()
        {
#pragma warning disable CS0618 // Тип или член устарел
            Scenes = new ScenesHeader(this);
#pragma warning restore CS0618 // Тип или член устарел
            Levels = new LevelsHeader(this);

            InitAds();
            UI.Init();
            if (ld != null) ld.Complete();
            Metrica.Report_Loading();
        }

    }
//}


#region === Хлам (удалить в релизе) ===

/*
[SerializeField] private UIConfigHeader InterfaceConfig;

[Serializable]
public struct UIConfigHeader
{
    [Header("Настройки интерфейса")]
    [Tooltip("Интерфейс главного экрана игры. Чаще всего там есть кнопка - [PLAY] и [QUIT]")]
    [SerializeField] UIScreen ScreenMainMenu;
    [Tooltip("Интерфейс в самой игре. Чаще всего отображает кол-во жизней, набранных очков и т.п.")]
    [SerializeField] UIScreen ScreenInGameUI;
    [Tooltip("Интерфейс, отображающийся после успешного прохождения уровня. Может отображать например - кол-во набранных очков и кнопку [NEXT]")]
    [SerializeField] UIScreen ScreenLevelComplete;
    [Tooltip("Интерфейс, отображающийся после провала уровня. Может отображать например - кнопку [RETRY]")]
    [SerializeField] UIScreen ScreenLevelFailed;
}
*/

/*
public class ExampleWindow : EditorWindow
{
    // Add menu item named "Example Window" to the Window menu
    [MenuItem("Window/Example Window1")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ExampleWindow));
    }

    List<SceneAsset> m_SceneAssets = new List<SceneAsset>();
    [ExecuteInEditMode]
    void OnGUI()
    {
        GUILayout.Label("Scenes to include in build:", EditorStyles.boldLabel);
        for (int i = 0; i < m_SceneAssets.Count; ++i)
        {
            m_SceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(m_SceneAssets[i], typeof(SceneAsset), false);
        }
        if (GUILayout.Button("Add"))
        {
            m_SceneAssets.Add(null);
        }

        GUILayout.Space(8);

        if (GUILayout.Button("Apply To Build Settings"))
        {
            SetEditorBuildSettingsScenes();
        }

        void SetEditorBuildSettingsScenes()
        {
            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            foreach (var sceneAsset in m_SceneAssets)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                if (!string.IsNullOrEmpty(scenePath))
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }
}
*/
#endregion