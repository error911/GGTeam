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

        /// <summary>
        /// Сохраняемые настройки и данные игры. При изменении, необходимо сохранить Save()
        /// </summary>
        public GameData GameSetup { get; private set; }

        public ConfigHeader(GameConfigSO gameConfig)
        {
            this.GameConfig = gameConfig;
            this.GameSetup = new GameData();
            GameSetup.Load();
        }
    }
}