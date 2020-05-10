// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteWindow : UIScreen
{
    [SerializeField] Text lvlNum = null;
    [SerializeField] Text textScores = null;
    [SerializeField] Image imgStar1 = null;
    [SerializeField] Image imgStar2 = null;
    [SerializeField] Image imgStar3 = null;

    public void OnBtnContinue()
    {
        Game.Levels.LoadNext();
    }

    public void OnBtnRestart()
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
        textScores.text = Game.Levels.Current.Data.score.ToString();

        float stars = Game.Levels.Current.Data.stars;

        if (stars > 0 && stars <= 1)
        {
            imgStar1.fillAmount = stars;
            imgStar2.fillAmount = 0;
            imgStar3.fillAmount = 0;
        }
        else
        if (stars > 1 && stars <= 2)
        {
            imgStar1.fillAmount = 1;
            imgStar2.fillAmount = stars - 1;
            imgStar3.fillAmount = 0;
        }
        else
        if (stars > 2 && stars <= 3)
        {
            imgStar1.fillAmount = 1;
            imgStar2.fillAmount = 1;
            imgStar3.fillAmount = stars - 2;
        }
        else
        {
            imgStar1.fillAmount = 1;
            imgStar2.fillAmount = 1;
            imgStar3.fillAmount = 1;
        }


    }
}
