// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteWindow : UIScreen
{
    [SerializeField] Text textLvlNum = null;
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
        textLvlNum.text = "LEVEL " + Game.Levels.CurrentNumber;
        if (Game.Levels.Current.Data.score > 0)
        {
            textScores.text = Game.Levels.Current.Data.score.ToString();
            Tween.TweenInt((n) => { textScores.text = n.ToString(); }, 0, Game.Levels.Current.Data.score, 0.8f);
        }

        float stars = Game.Levels.Current.Data.stars;
        float anSpeed = 0.3f;
        float anDel = anSpeed - 0.1f;

        imgStar1.fillAmount = 0;
        imgStar2.fillAmount = 0;
        imgStar3.fillAmount = 0;

        if (stars > 0 && stars <= 1)
        {
            imgStar1.fillAmount = stars;
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, stars - 1, anSpeed);
            //imgStar2.fillAmount = 0;
            //imgStar3.fillAmount = 0;
        }
        else
        if (stars > 1 && stars <= 2)
        {
            //imgStar1.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            //imgStar2.fillAmount = stars - 1;
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, stars - 1, anSpeed, anDel);
            //imgStar3.fillAmount = 0;
        }
        else
        if (stars > 2 && stars <= 3)
        {
            //imgStar1.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            //imgStar2.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, 1, anSpeed, anDel);
            //imgStar3.fillAmount = stars - 2;
            Tween.TweenFloat((f) => { imgStar3.fillAmount = f; }, 0, stars - 2, anSpeed, anDel * 2);
        }
        else
        {
            //imgStar1.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            //imgStar2.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, 1, anSpeed, anSpeed - 0.1f);
            //imgStar3.fillAmount = 1;
            Tween.TweenFloat((f) => { imgStar3.fillAmount = f; }, 0, 1, anSpeed, anSpeed * 2 - 0.1f);
        }


    }
}
