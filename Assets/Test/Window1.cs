using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window1 : UIScreen
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
        Game.UI.Open(FindObjectOfType<Window2>(), true);
    }

}
