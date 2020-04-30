using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessWindow : UIScreen
{
    public Text lvlNumLext;

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();
    }

    public override void OnOpen()
    {
        lvlNumLext.text = "LEVEL " + Game.Levels.CurrentNumber;
    }

    public override void OnClose()
    {
        
    }

    public override void OnInit()
    {
        //Debug.LogError(Game.Levels.CurrentNumber + " OnInit");
        //if (Game == null) Debug.LogError("1");
        //if (Game.Levels == null) Debug.LogError("2");
        //        Game.Levels.OnLevelChanged += OnLevelChange;
    }
}
