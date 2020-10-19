// ================================
// Free license: CC BY Murnik Roman
// ================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public class SimpleGameplay : Level
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) LevelComplete();
            if (Input.GetKeyDown(KeyCode.F)) LevelFailed();
            if (Input.GetKeyDown(KeyCode.R)) LevelRestart();
        }

        public override void OnLevelStart()
        {
            Game.Log.Info("OnLevelStart " + Data.number, "Simple gameplay works!");
            Game.Log.Warning("Создайте класс с логикой уровня и сделайте его наследником от Level");
        }

        public override void OnLevelComplete(LevelData levelData)
        {
            Game.Log.Info("OnLevelComplete", "Simple gameplay Level Complete! You score: " + levelData.score + " You stars: " + levelData.stars);
        }

        public override void OnLevelFailed(LevelData levelData)
        {
            Game.Log.Info("OnLevelFailed", "Simple gameplay Level Failed! You score: " + levelData.score);
        }

        void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            GUI.Label(new Rect(10, 10, 150, 20), "LEVEL: " + Data.number);
            GUI.Label(new Rect(10, 30, 200, 20), "Press key: 'C' to Complete level");
            GUI.Label(new Rect(78, 45, 150, 20), "'F' to Failed level");
            GUI.Label(new Rect(78, 60, 150, 20), "'R' to Restart level");

            guiStyle.fontSize = 11;
            GUI.Label(new Rect(10, Screen.height - 30, 150, 20), "Create custom class. This is simple Demo level logic.", guiStyle);
        }
    }
}