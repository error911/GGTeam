// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessWindow : UIScreen
{
    public Text textLvlValue;
    public Text textScoreValue;
    float duration = 1.0f;

    int curScore = 0;
    int tId = -1;

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();
    }

    public override void OnOpen()
    {
        curScore = Game.Levels.Current.Data.score;
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
        int ssc = curScore;
        Tween.StopTween(tId);
        tId = Tween.TweenInt((x) => { textScoreValue.text = x.ToString(); }, ssc, score, duration);
        curScore = score;
        //textScoreValue.text = score.ToString();
    }
}
