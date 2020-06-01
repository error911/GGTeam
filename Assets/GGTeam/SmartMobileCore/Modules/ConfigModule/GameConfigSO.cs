﻿// ================================
// Free license: CC BY Murnik Roman
// ================================

using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "GGTeam/SmartMobileCore/GameConfig", order = 1)]
public class GameConfigSO : ScriptableObject
{
    // ========== GAMEPLAY ==========
    [Space(8)]
    [Header("- GAMEPLAY -----------------------------------------------------------------")]
    [Tooltip("Обнулять очки текущего уровня")]
    public bool GAMEPLAY_CLEAR_SCORE_ON_START_LEVEL = true;

    // ========== POLICY ==========
    [Space(8)]
    [Header("- POLICY (Supported tags:{company},{age}) -----------------------------------------------------------------")]
    public bool POLICY_ENABLED = true;

    public int POLICY_AGE_MIN = 16;

    [Tooltip("Supported tags {company}, {age}")]
    [TextArea(1,6)]
    public string POLICY_GDRP_TEXT_EN = "I hereby consent to {company} processing of my personal data to personalize and improve the game and serving targeted advertisements in the game through advertising networks and their partners based on my personal preferences.";

    [Tooltip("Supported tags {company}, {age}")]
    [TextArea(1,6)]
    public string POLICY_CCOPA_TEXT_EN = "By checking ON the box above, I accept that I`ve read and agreed with Privacy Policy and I accept that my age is older than {age}. I understand that I can always withdraw my consent at any time from within Settings in the game.";

    // ========== LEVEL ==========
    [Space(8)]
    [Header("- LEVEL -----------------------------------------------------------------")]
    [Tooltip("Ожидание после выгрузки/загрузки сцены (мс)")]
    public int LEVEL_WAIT_AFTER_LOADING = 50;
    [Tooltip("Использовать одну сцену для всех уровней")]
    public bool LEVEL_USE_ONE_SCENE_FOR_ALL = false;
    [Tooltip("Кол-во уровней. Внимание! Имеет значение только если параметр выше=true")]
    public int LEVEL_ONE_SCENE_LEVELS_COUNT = 0;

    // ========== LOG ==========
    [Space(8)]
    [Header("- LOG -----------------------------------------------------------------")]
    public bool LOG_SHOW_DEBUG = false;
    public bool LOG_SHOW_WARNING = true;
    public bool LOG_SHOW_ERROR = true;
    public bool LOG_SHOW_INFO = false;

    // ========== HARDWARE ==========
    [Space(8)]
    [Header("- HARDWARE -----------------------------------------------------------------")]
    [Tooltip("Желаемая частота кадров. 0=по умолчанию устройства.")]
    public int HARD_TARGET_FRAMERATE = 60;

    // ========== DATA ==========
    [Space(8)]
    [Header("- Saved user data -----------------------------------------------------------------")]
    public string DATA_SAVE_PREFIX = "GGTeam";
    public string DATA_SAVE_SUFFIX = "MobileCore";

    // ========== Analytics ==========
    [Space(8)]
    [Header("- Analytics. Support default Yandex.Metrica api -----------------------------------------------------------------")]
    [Tooltip("Уникальный идентификатор приложения, который выдается при добавлении приложения в веб-интерфейсе AppMetrica. (https://appmetrika.yandex.ru/application/new)")]
    public string ANALYTICS_APP_KEY = "1ebc6e45-a6de-4acc-bfc3-c340ea9d8235";

    [Tooltip("включить/отключить передачу данных о местоположении. Для включения закоментируйте строку #define APP_METRICA_TRACK_LOCATION_DISABLED в Analytics.cs")]
    public bool ANALYTICS_LOCATION_TRACKING = false;

    [Tooltip("включить/отключить логирование работы библиотеки")]
    public bool ANALYTICS_LOGS = false;

    [Tooltip("включить/отключить отправку ошибок")]
    public bool ANALYTICS_EXCEPTIONS_REPORTING = true;

    // ========== ADS ==========
    [Space(8)]
    [Header("- ADS. Support default IronSource api -----------------------------------------------------------------")]
    [Tooltip("Support default IronSource")]
    public string ADS_APP_KEY = "b4fa68fd";
    public bool ADS_ENABLE_VIDEO = true;
    public bool ADS_ENABLE_BANNER = false;
    [Tooltip("Начинать показывать рекламу с уровня N")]
    public int ADS_START_VIDEO_FROM_LEVEL = 4;
    [Tooltip("Отображать рекламу не чаще этого времени (сек)")]
    public int ADS_SHOW_TIME_MIN_SEC = 120;

    // ========== Publishing ==========
    [Space(8)]
    [Header("- Publishing -----------------------------------------------------------------")]
    [Tooltip("Публикация")]
    public string MARKET_URL_ANDROID = "";
    public string MARKET_URL_IOS = "";

}
