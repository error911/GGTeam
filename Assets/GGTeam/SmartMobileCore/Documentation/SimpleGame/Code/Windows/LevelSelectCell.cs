// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectCell : UIScreen
{
    [SerializeField] Text lvlNum = null;

    public void OnBtnContinue()
    {
        //Close(true);
        Game.Levels.LoadNext(); //OnLoaded
    }

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();  //OnLoaded
    }

    public override void OnClose()
    {

    }

    public override void OnInit()
    {

    }

    public override void OnOpen()
    {
        lvlNum.text = "LEVEL " + Game.Levels.Current.Data.number;
    }
}
