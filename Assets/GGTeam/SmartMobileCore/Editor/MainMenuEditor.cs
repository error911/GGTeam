using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public class MainMenuEditor : Editor
    {
        [MenuItem("GGTeam/SmartMobileCore/GameManager")]
        public static void PlaceGameManager()
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) { return; }
            GameObject go = new GameObject("[GameManager]");
            go.AddComponent<GameManager>();
            go.transform.SetAsFirstSibling();
        }

        /*
        [MenuItem("GGTeam/SmartMobileCore/GameManager")]
        public static void PlaceGameManager()
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) { return; }
            GameObject go = new GameObject("[GameManager]");
            go.AddComponent<GameManager>();
            go.transform.SetAsFirstSibling();
        }
        */

    }
}