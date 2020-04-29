// ================================
// Free license: CC BY Murnik Roman
// ================================

using UnityEngine;
using UnityEngine.EventSystems;

namespace GGTeam.SmartMobileCore
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] GameConfigSO gameConfig = null;

        public ConfigHeader Config;
        public LogHeader Log;
        public ScenesHeader Scenes;
        public LevelsHeader Levels;
        public UIHeader UI;
        public AdsHeader ADS;
        IAdsProvider adwareProvider = new IronSourceAdsProvider();

        internal bool inited = false;
        public static GameManager api;


        void Awake()
        {
            api = this;

            Prepare();
            Init();
        }

        void Prepare()
        {
            Config = new ConfigHeader(this, gameConfig);
            Log = new LogHeader(this);
            Scenes = new ScenesHeader(this);
            Levels = new LevelsHeader(this);
            UI = new UIHeader(this);
            ADS = new AdsHeader(this, adwareProvider);
        }

        void Init()
        {
            if (Config.main.POLICY_ENABLED)
            {
                var gdrpPref = Resources.Load<GameObject>("Prefabs/[GDRPPolicy]");
                if (gdrpPref == null)
                {
                    Log.Error("Prefab 'Resources/Prefabs/[GDRPPolicy]' not found.");
                    On_GDRPP_Complete();
                }
                else
                {
                    var go = Instantiate(gdrpPref);
                    GDRPPolicy created_GDRPPolicy = go.GetComponent<GDRPPolicy>();
                    if (created_GDRPPolicy != null)
                    {
                        created_GDRPPolicy.Init(Config.main.POLICY_GDRP_TEXT, Config.main.POLICY_CCOPA_TEXT, On_GDRPP_Complete);
                    }
                }

                // Проверяем есть ли EventSystem
                var es = GetComponent<EventSystem>();
                if (es == null)
                {
                    GameObject go = new GameObject("EventSystem");
                    go.AddComponent<EventSystem>();
                    go.AddComponent<StandaloneInputModule>();
                }

            }
            else
            {
                On_GDRPP_Complete();
            }

            void On_GDRPP_Complete()
            {
                UI.Init();
            }
        }

        void Start()
        {
            if (Config.main.ADS_APP_KEY.Length > 0) ADS.Init(Config.main.ADS_APP_KEY, Config.main.ADS_ENABLE_VIDEO, Config.main.ADS_ENABLE_BANNER);
            else ADS.Init("", false, false);
            
            if (Config.main.HARD_TARGET_FRAMERATE >= 0) Application.targetFrameRate = Config.main.HARD_TARGET_FRAMERATE;

            inited = true;
            Log.Info("GameManager", "Started");
        }
    }
}


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