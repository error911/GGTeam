// ================================
// Free license: CC BY Murnik Roman
// ================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public sealed class ConfigHeader
    {
        /// <summary>
        /// Текущая, используемая конфигурация (только для чтения)
        /// </summary>
        public GameConfigSO GameConfig{ get; private set; }
        /*
        public GameConfigSO GameConfig
        {
            get
            {
                if (_GameConfig == null) _GameConfig = Resources.Load<GameConfigSO>("SmartMobileCore/MainConfig");
                return _GameConfig;
            }
            set
            {
                _GameConfig = value;
            }
        }
        GameConfigSO _GameConfig;
        */

        /// <summary>
        /// Сохраняемые настройки и данные игры. При изменении, необходимо сохранить Save()
        /// </summary>
        public GameData GameSetup { get; private set; }
        
        //~ConfigHeader()
        //{
        //    Debug.Log("GameData saved...");
        //    CurrentGameData.Save();
        //    Debug.Log("GameData saved - ok");
        //}

        private GameManager Game;

        /*
        string s_data_level_lastplayed = "";
        string s_data_sound_enabled = "";
        string s_data_vibro_enabled = "";
        string s_data_ads_disabled_by_user = "";
        */

        public ConfigHeader(GameManager gameManager, GameConfigSO gameConfig)
        {
            this.Game = gameManager;
            this.GameConfig = gameConfig;
            this.GameSetup = new GameData();
            GameSetup.Load();

//            if (this.GameConfig == null) this.GameConfig = ScriptableObject.CreateInstance<GameConfigSO>();

            /*
            s_data_level_lastplayed = GameConfig.DATA_SAVE_PREFIX + "." + GameConfig.DATA_SAVE_SUFFIX + ".Config.Level.lastplayed";
            s_data_sound_enabled = GameConfig.DATA_SAVE_PREFIX + "." + GameConfig.DATA_SAVE_SUFFIX + ".Config.Setup.Sound.enabled";
            s_data_vibro_enabled = GameConfig.DATA_SAVE_PREFIX + "." + GameConfig.DATA_SAVE_SUFFIX + ".Config.Setup.Vibro.enabled";
            s_data_ads_disabled_by_user = GameConfig.DATA_SAVE_PREFIX + "." + GameConfig.DATA_SAVE_SUFFIX + ".Config.Setup.Ads.useroff";
            */
        }

        // ========== SAVED CORE DATA ==========
/*        
        /// <summary>
        /// Номер уровня, в который играли последний раз
        /// </summary>
        public int SAVED_LEVEL_LASTPLAYED
        {
            get
            {
                return PlayerPrefs.GetInt(s_data_level_lastplayed, 0);
            }

            set
            {
                PlayerPrefs.SetInt(s_data_level_lastplayed, value);
            }
        }

        /// <summary>
        /// Звук: Вкл/Откл
        /// </summary>
        public bool SETUP_SOUND_ENABLED
        {
            get
            {
                int i = PlayerPrefs.GetInt(s_data_sound_enabled, 1);
                if (i <= 0) return false;
                return true;
            }

            set
            {
                int i = 0;
                if (value == true) i = 1;
                PlayerPrefs.SetInt(s_data_sound_enabled, i);
            }
        }

        /// <summary>
        /// Вибро: Вкл/Откл
        /// </summary>
        public bool SETUP_VIBRO_ENABLED
        {
            get
            {
                int i = PlayerPrefs.GetInt(s_data_vibro_enabled, 0);
                if (i <= 0) return false;
                return true;
            }

            set
            {
                int i = 0;
                if (value == true) i = 1;
                PlayerPrefs.SetInt(s_data_vibro_enabled, i);
            }
        }

        /// <summary>
        /// Ads: true-отключен пользователем (например куплен)
        /// </summary>
        public bool SETUP_ADS_USEROFF
        {
            get
            {
                int i = PlayerPrefs.GetInt(s_data_ads_disabled_by_user, 0);
                if (i <= 0) return false;
                return true;
            }

            set
            {
                int i = 0;
                if (value == true) i = 1;
                PlayerPrefs.SetInt(s_data_ads_disabled_by_user, i);
            }
        }
*/

        /*
        public Dictionary<int, bool> SAVED_LEVEL_COMPLETED_LISTQQQ
        {
            get
            {
                Dictionary<int, bool> tmp = new Dictionary<int, bool>();
                string s = PlayerPrefs.GetString(s_data_level_completed_list, "");
                tmp = JsonUtility.FromJson<Dictionary<int, bool>>(s);
Debug.Log("Load> " + s);
                if (tmp == null) tmp = new Dictionary<int, bool>();
                return tmp;
            }
            set
            {
                string s = JsonUtility.ToJson(value);
Debug.Log("!!!Save> " + s);
                if (s != null && s.Length > 0) PlayerPrefs.SetString(s_data_level_completed_list, s);
            }
        }
        */





    }
}