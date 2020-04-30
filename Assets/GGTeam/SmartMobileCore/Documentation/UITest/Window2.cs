using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window2 : UIScreen
{
    public override void OnClose()
    {
        
    }

    public override void OnInit()
    {
        
    }

    public override void OnOpen()
    {
        
    }

    public void OnBtnClose()
    {
        Close(true);
        Game.UI.Open(FindObjectOfType<Window1>(), true);
    }
}
