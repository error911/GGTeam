// ================================
// Free license: CC BY Murnik Roman
// ================================

using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "GGTeam/SmartMobileCore/GameConfig", order = 1)]
public class GameConfigSO : ScriptableObject
{

    //[FieldData("Allow Core logs", "Allow FiberCore to show logs in console")]
    //[SerializeField] private bool _allowLogs;
    //public bool AllowLogs => _allowLogs;

    

    // ========== GAMEPLAY ==========
    //[Space(8)]
    //[Header("- GAMEPLAY -----------------------------------------------------------------")]
    //[Tooltip("Обнулять очки текущего уровня")]
    [FieldData("Обнулять очки текущего уровня", "Следующий уровень начнется со Score=0")]
    [SerializeField] private bool _GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL = true;
    public bool GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL => _GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL;
    /*
    [Header("- GAMEPLAY -----------------------------------------------------------------")]
    [Tooltip("Обнулять очки текущего уровня")]
    public bool GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL = true;
    */

    // ========== GAMEPLAY ==========
    //[Space(8)]
    //[Header("- LEVEL -----------------------------------------------------------------")]
    //[Tooltip("Использовать одну сцену для всех уровней")]
    [FieldData("Использовать одну сцену для всех уровней", "Каждый новый уровень будет начинаться со сцены с индексом=1")]
    [SerializeField] private bool _LEVEL_USE_ONE_SCENE_FOR_ALL = false;
    public bool LEVEL_USE_ONE_SCENE_FOR_ALL => _LEVEL_USE_ONE_SCENE_FOR_ALL;


    //[Tooltip("Кол-во уровней. Внимание! Имеет значение только если параметр выше=true")]
    [FieldData("Кол-во уровней", "Внимание! Имеет значение, только если используется только одна сцена для всех уровней")]
    [SerializeField] private int _LEVEL_ONE_SCENE_LEVELS_COUNT = 0;
    public int LEVEL_ONE_SCENE_LEVELS_COUNT => _LEVEL_ONE_SCENE_LEVELS_COUNT;

    // ========== POLICY ==========
    //[Space(8)]
    //[Header("- POLICY (Supported tags:{company},{age}) -----------------------------------------------------------------")]
    [FieldData("Отображать Политику", "В тексте политики работают теги tags:{company},{age}")]
    [SerializeField] private bool _POLICY_ENABLED = true;
    public bool POLICY_ENABLED => _POLICY_ENABLED;

    [FieldData("Минимальный возраст пользователей", "Минимальный возраст пользователей")]
    [SerializeField] private int _POLICY_AGE_MIN = 16;
    public int POLICY_AGE_MIN => _POLICY_AGE_MIN;


    //[Tooltip("Supported tags {company}, {age}")]
    //[TextArea(1,6)]
    [FieldData("Политика GDRP", "Supported tags {company}, {age}")]
    [SerializeField] private string _POLICY_GDRP_TEXT_EN = "I hereby consent to {company} processing of my personal data to personalize and improve the game and serving targeted advertisements in the game through advertising networks and their partners based on my personal preferences.";
    public string POLICY_GDRP_TEXT_EN => _POLICY_GDRP_TEXT_EN;
    //public string POLICY_GDRP_TEXT_EN = "I hereby consent to {company} processing of my personal data to personalize and improve the game and serving targeted advertisements in the game through advertising networks and their partners based on my personal preferences.";

    //[Tooltip("Supported tags {company}, {age}")]
    //[TextArea(1,6)]
    [FieldData("Политика CCOPA", "Supported tags {company}, {age}")]
    [SerializeField] private string _POLICY_CCOPA_TEXT_EN = "By checking ON the box above, I accept that I`ve read and agreed with Privacy Policy and I accept that my age is older than {age}. I understand that I can always withdraw my consent at any time from within Settings in the game.";
    public string POLICY_CCOPA_TEXT_EN => _POLICY_CCOPA_TEXT_EN;
    //public string POLICY_CCOPA_TEXT_EN = "By checking ON the box above, I accept that I`ve read and agreed with Privacy Policy and I accept that my age is older than {age}. I understand that I can always withdraw my consent at any time from within Settings in the game.";


    // ========== LOG ==========
    //[Space(8)]
    //[Header("- LOG -----------------------------------------------------------------")]

    [FieldData("Отображать Лог Info", "Отображать выбранный тип сообщений логирования")]
    [SerializeField] private bool _LOG_SHOW_INFO = false;
    public bool LOG_SHOW_INFO => _LOG_SHOW_INFO;

    [FieldData("Отображать Лог Debug", "Отображать выбранный тип сообщений логирования")]
    [SerializeField] private bool _LOG_SHOW_DEBUG = false;
    public bool LOG_SHOW_DEBUG => _LOG_SHOW_DEBUG;

    [FieldData("Отображать Лог Warning", "Отображать выбранный тип сообщений логирования")]
    [SerializeField] private bool _LOG_SHOW_WARNING = true;
    public bool LOG_SHOW_WARNING => _LOG_SHOW_WARNING;

    [FieldData("Отображать Лог Error", "Отображать выбранный тип сообщений логирования")]
    [SerializeField] private bool _LOG_SHOW_ERROR = true;
    public bool LOG_SHOW_ERROR => _LOG_SHOW_ERROR;


    // ========== PERFOMANCE ==========
    //[Tooltip("Ожидание после выгрузки/загрузки сцены (мс)")]
    [FieldData("Ожидание после выгрузки/загрузки сцены (мс)", "Ожидание после выгрузки/загрузки сцены (мс)")]
    [SerializeField] private int _LEVEL_WAIT_AFTER_LOADING = 50;
    public int LEVEL_WAIT_AFTER_LOADING => _LEVEL_WAIT_AFTER_LOADING;

    //[Space(8)]
    //[Header("- HARDWARE -----------------------------------------------------------------")]
    //[Tooltip("Желаемая частота кадров. 0=по умолчанию устройства.")]
    [FieldData("Желаемая частота кадров.", "Установите '0' по умолчанию устройства.")]
    [SerializeField] private int _HARD_TARGET_FRAMERATE = 60;
    public int HARD_TARGET_FRAMERATE => _HARD_TARGET_FRAMERATE;
    //public int HARD_TARGET_FRAMERATE = 60;


    // ========== DATA ==========
    //[Space(8)]
    //[Header("- Saved user data -----------------------------------------------------------------")]

    [FieldData("Префикс имени файла данных", "Не желательно менять после выпуска очередной версии Вашего приложения")]
    [SerializeField] private string _DATA_SAVE_PREFIX = "GGTeam";
    public string DATA_SAVE_PREFIX => _DATA_SAVE_PREFIX;
    //public string DATA_SAVE_PREFIX = "GGTeam";

    [FieldData("Суффикс имени файла данных", "Не желательно менять после выпуска очередной версии Вашего приложения")]
    [SerializeField] private string _DATA_SAVE_SUFFIX = "MobileCore";
    public string DATA_SAVE_SUFFIX => _DATA_SAVE_SUFFIX;
    //public string DATA_SAVE_SUFFIX = "MobileCore";

    // ========== Analytics ==========
    //[Space(8)]
    //[Header("- Analytics. Support default Yandex.Metrica api -----------------------------------------------------------------")]
    //[Tooltip("Уникальный идентификатор приложения, который выдается при добавлении приложения в веб-интерфейсе AppMetrica. (https://appmetrika.yandex.ru/application/new)")]
    [FieldData("Уникальный идентификатор приложения", "выдается при добавлении приложения в веб-интерфейсе AppMetrica. (https://appmetrika.yandex.ru/application/new)")]
    [SerializeField] private string _ANALYTICS_APP_KEY = "1ebc6e45-a6de-4acc-bfc3-c340ea9d8235";
    public string ANALYTICS_APP_KEY => _ANALYTICS_APP_KEY;
    //public string ANALYTICS_APP_KEY = "1ebc6e45-a6de-4acc-bfc3-c340ea9d8235";

    //[Tooltip("включить/отключить передачу данных о местоположении. Для включения закоментируйте строку #define APP_METRICA_TRACK_LOCATION_DISABLED в Analytics.cs")]
    [FieldData("Передача данных о местоположении", "Для включения дополнительно надо закоментировать строку #define APP_METRICA_TRACK_LOCATION_DISABLED в Analytics.cs")]
    [SerializeField] private bool _ANALYTICS_LOCATION_TRACKING = false;
    public bool ANALYTICS_LOCATION_TRACKING => _ANALYTICS_LOCATION_TRACKING;
    //public bool ANALYTICS_LOCATION_TRACKING = false;

    //[Tooltip("включить/отключить логирование работы библиотеки")]
    public bool ANALYTICS_LOGS = false;

    //[Tooltip("включить/отключить отправку ошибок")]
    [FieldData("Отправка ошибок", "Включить/отключить отправку ошибок в сервис аналитики")]
    [SerializeField] private bool _ANALYTICS_EXCEPTIONS_REPORTING = true;
    public bool ANALYTICS_EXCEPTIONS_REPORTING => _ANALYTICS_EXCEPTIONS_REPORTING;
    //public bool ANALYTICS_EXCEPTIONS_REPORTING = true;

    // ========== ADS ==========
    //[Space(8)]
    //[Header("- ADS. Support default IronSource api -----------------------------------------------------------------")]
    //[Tooltip("Support default IronSource")]
    [FieldData("Ключ IronSource SDK", "Можно получить на сайте IronSource (https://www.ironsrc.com/)")]
    [SerializeField] private string _ADS_APP_KEY = "b4fa68fd";
    public string ADS_APP_KEY => _ADS_APP_KEY;
    //public string ADS_APP_KEY = "b4fa68fd";

    [FieldData("Отображать 'Видео' рекламу", "")]
    [SerializeField] private bool _ADS_ENABLE_VIDEO = true;
    public bool ADS_ENABLE_VIDEO => _ADS_ENABLE_VIDEO;
    //public bool ADS_ENABLE_VIDEO = true;

    [FieldData("Отображать 'Баннеры'", "")]
    [SerializeField] private bool _ADS_ENABLE_BANNER = false;
    public bool ADS_ENABLE_BANNER => _ADS_ENABLE_BANNER;
    //public bool ADS_ENABLE_BANNER = false;

    //[Tooltip("Начинать показывать рекламу с уровня N")]
    [FieldData("Начинать показывать рекламу с уровня номер", "")]
    [SerializeField] private int _ADS_START_VIDEO_FROM_LEVEL = 4;
    public int ADS_START_VIDEO_FROM_LEVEL => _ADS_START_VIDEO_FROM_LEVEL;
    //public int ADS_START_VIDEO_FROM_LEVEL = 4;

    //[Tooltip("Отображать рекламу не чаще этого времени (сек)")]
    [FieldData("Отображать рекламу не чаще чем через (сек)", "Минимальный интервал показа рекламы")]
    [SerializeField] private int _ADS_SHOW_TIME_MIN_SEC = 120;
    public int ADS_SHOW_TIME_MIN_SEC => _ADS_SHOW_TIME_MIN_SEC;
    //public int ADS_SHOW_TIME_MIN_SEC = 120;

    // ========== Publishing ==========
    //[Space(8)]
    //[Header("- Publishing -----------------------------------------------------------------")]
    //[Tooltip("Публикация")]
    [FieldData("Ссылка на App в маркете (Android)", "Необходима для оценки приложения игроками")]
    [SerializeField] private string _MARKET_URL_ANDROID = "";
    public string MARKET_URL_ANDROID => _MARKET_URL_ANDROID;
    //public string MARKET_URL_ANDROID = "";
    [FieldData("Ссылка на App в маркете (iOS)", "Необходима для оценки приложения игроками")]
    [SerializeField] private string _MARKET_URL_IOS = "";
    public string MARKET_URL_IOS => _MARKET_URL_IOS;
    //public string MARKET_URL_IOS = "";

}
