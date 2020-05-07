// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessWindow : UIScreen
{
    public Text textLvlValue;
    public Text textScoreValue;

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();
    }

    public override void OnOpen()
    {
        textLvlValue.text = Game.Levels.CurrentNumber.ToString();
        textScoreValue.text = Game.Levels.Current.Data.score.ToString();
        Game.Levels.OnScoreChanged += ScoreChange;
    }

    public override void OnClose()
    {
        Game.Levels.OnScoreChanged -= ScoreChange;
    }

    public override void OnInit()
    {
        textLvlValue.text = "-";
        textScoreValue.text = "-";
    }

    void ScoreChange(int score)
    {
        textScoreValue.text = score.ToString();
    }
}
