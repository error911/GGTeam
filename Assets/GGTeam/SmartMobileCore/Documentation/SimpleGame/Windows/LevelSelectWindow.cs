using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectWindow : UIScreen
{
    [SerializeField] GameObject CellPref;
    [SerializeField] Text lvlNum = null;

    public override void OnInit()
    {
        //if (gameObject == null) Debug.Log("11111111");
        //if (gameObject.transform == null) Debug.Log("222222222222");

        //if (CellPref == null) CellPref = gameObject.transform.Find("Cell").gameObject;

        for (int i = 1; i <= Game.Levels.Count; i++)
        {
            var data = Game.Levels.LevelData(i);
            Debug.Log(data.num);
        }
        
        
    }




    public void OnBtnContinue()
    {
        Game.Levels.LoadNext(); //OnLoaded
    }

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();  //OnLoaded
    }

    public override void OnClose()
    {

    }

    

    public override void OnOpen()
    {
        lvlNum.text = "LEVEL " + Game.Levels.CurrentNumber;
    }
}
