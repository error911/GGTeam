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
        /// Текущая, используемая конфигурация
        /// </summary>
        public GameConfigSO Current { get; private set; }

        private GameManager Game;
        string s_data_level_current = "";
        string s_data_level_completed_list = "";

        public ConfigHeader(GameManager gameManager, GameConfigSO gameConfig)
        {
            this.Game = gameManager;
            this.Current = gameConfig;
            if (this.Current == null) this.Current = ScriptableObject.CreateInstance<GameConfigSO>();

            s_data_level_current = Current.DATA_SAVE_PREFIX + "." + Current.DATA_SAVE_SUFFIX + ".Config.Level.current";
            s_data_level_completed_list = Current.DATA_SAVE_PREFIX + "." + Current.DATA_SAVE_SUFFIX + ".Config.Level.completedlist";
        }

        // ========== SAVED CORE DATA ==========
        public int SAVED_LEVEL_CURRENT
        {
            get
            {
                return PlayerPrefs.GetInt(s_data_level_current, 0);
            }

            set
            {
                PlayerPrefs.SetInt(s_data_level_current, value);
            }
        }



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