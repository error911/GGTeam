// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailedWindow : UIScreen
{
    public Text lvlNum;

    public void OnBtnRetry()
    {
        Game.Levels.LoadCurrent();
    }

    public override void OnClose()
    {
    }

    public override void OnInit()
    {
        
    }

    public override void OnOpen()
    {
        lvlNum.text = "LEVEL " + Game.Levels.CurrentNumber;
    }
}
