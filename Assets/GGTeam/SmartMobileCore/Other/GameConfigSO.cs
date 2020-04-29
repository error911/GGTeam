using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "SmartMobileCore/GameConfig", order = 1)]
public class GameConfigSO : ScriptableObject
{
    // ========== POLICY ==========
    public bool POLICY_ENABLED = true;

    public int POLICY_AGE_MIN = 16;
    [TextArea(1,6)]
    public string POLICY_GDRP_TEXT = "I hereby consent to {company} processing of my personal data to personalize and improve the game and serving targeted advertisements in the game through advertising networks and their partners based on my personal preferences.";
    [TextArea(1,6)]
    public string POLICY_CCOPA_TEXT = "By checking ON the box above, I accept that I`ve read and agreed with Privacy Policy and I accept that my age is older than 16. I understand that I can always withdraw my consent at any time from within Settings in the game.";

    // ========== LEVEL ==========

    [Tooltip("Ожидание после выгрузки/загрузки сцены (мс)")]
    public int LEVEL_WAIT_AFTER_LOADING = 0;

    // ========== LOG ==========
    public bool LOG_SHOW_DEBUG = false;
    public bool LOG_SHOW_WARNING = true;
    public bool LOG_SHOW_ERROR = true;
    public bool LOG_SHOW_INFO = false;

    // ========== HARDWARE ==========
    [Tooltip("Желаемая частота кадров. 0=по умолчанию устройства.")]
    public int HARD_TARGET_FRAMERATE = 60;

    // ========== DATA ==========
    public string DATA_SAVE_PREFIX = "GGTeam";
    public string DATA_SAVE_SUFFIX = "MobileCore";

    // ========== ADS ==========
    [Tooltip("Support only IronSource")]
    public string ADS_APP_KEY = "b4fa68fd";
    public bool ADS_ENABLE_VIDEO = true;
    public bool ADS_ENABLE_BANNER = false;

}
