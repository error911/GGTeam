using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : UIScreen
{
    [SerializeField] Text textGameName = null;
    [SerializeField] Text textCompanyName = null;
    public void OnBtnPlay()
    {
        Game.Levels.LoadNext(OnLoaded);
    }

    public void OnBtnNewGame()
    {
        Game.Levels.Load(1, OnLoaded);
    }


    

    //public void OnBtnGFX()
    //{
    //    Game.UI.GFX.Show();
    //}

    private void OnLoaded()
    {
        //if (text1 != null) text1.text = "Level " + Game.Levels.CurrentNumber.ToString();
    }

    public override void OnInit()
    {
        
    }

    public override void OnOpen()
    {
        string productName = Application.productName;
        string companyName = "by " + Application.companyName + " v." + Application.version;

        if (textGameName != null) textGameName.text = productName;
        if (textCompanyName != null) textCompanyName.text = companyName;
    }

    public override void OnClose()
    {

    }
}
