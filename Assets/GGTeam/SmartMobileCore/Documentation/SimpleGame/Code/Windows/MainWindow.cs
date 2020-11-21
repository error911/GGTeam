// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.SmartMobileCore.Modules.UIScreenModule;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : UIScreen
{
    [SerializeField] Text textGameName = null;
    [SerializeField] Text textCompanyName = null;
    [SerializeField] Button btnSelectLevel = null;

    public void OnBtnPlay()
    {
        Game.Levels.LoadNext(OnLoadedCallback);
    }

    public void OnBtnNewGame()
    {
        Game.Levels.Load(1, OnLoadedCallback);
    }

    public void OnBtnSelectLevel()
    {
        Game.UI.Open(UITypes.ScreenLevelSelect);
    }

    private void OnLoadedCallback()
    {

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

        var complLvls = Game.Levels.CompletedLevels();
        if (complLvls != null)
            if (complLvls.Count > 0)
                btnSelectLevel.gameObject.SetActive(true);
    }

    public override void OnClose()
    {

    }
}
