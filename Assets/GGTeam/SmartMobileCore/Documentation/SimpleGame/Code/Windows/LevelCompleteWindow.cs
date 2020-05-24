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

    [SerializeField] GameObject fxStarShow = null;

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
        textLvlNum.text = "LEVEL " + Game.Levels.Current.Data.number;
        if (Game.Levels.Current.Data.score > 0)
        {
            textScores.text = Game.Levels.Current.Data.score.ToString();
            Tween.TweenInt((n) => { textScores.text = n.ToString(); }, 0, Game.Levels.Current.Data.score, 0.8f);
        }

        float stars = Game.Levels.Current.Data.stars;
        float anSpeed = 0.25f;
        float anDel = anSpeed - 0.1f;

        imgStar1.fillAmount = 0;
        imgStar2.fillAmount = 0;
        imgStar3.fillAmount = 0;

        if (stars > 0 && stars <= 1)
        {
            FXStar(imgStar1.transform);
            imgStar1.fillAmount = stars;
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, stars - 1, anSpeed);
        }
        else
        if (stars > 1 && stars <= 2)
        {
            FXStar(imgStar1.transform);
            FXStar(imgStar2.transform);
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, stars - 1, anSpeed, anDel);
        }
        else
        if (stars > 2 && stars <= 3)
        {
            FXStar(imgStar1.transform);
            FXStar(imgStar2.transform);
            FXStar(imgStar3.transform);
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, 1, anSpeed, anDel);
            Tween.TweenFloat((f) => { imgStar3.fillAmount = f; }, 0, stars - 2, anSpeed, anDel * 2);
        }
        else
        {
            FXStar(imgStar1.transform);
            FXStar(imgStar2.transform);
            FXStar(imgStar3.transform);
            Tween.TweenFloat((f) => { imgStar1.fillAmount = f; }, 0, 1, anSpeed);
            Tween.TweenFloat((f) => { imgStar2.fillAmount = f; }, 0, 1, anSpeed, anSpeed - 0.1f);
            Tween.TweenFloat((f) => { imgStar3.fillAmount = f; }, 0, 1, anSpeed, anSpeed * 2 - 0.1f);
        }


    }

    void FXStar(Transform pos)
    {
        if (fxStarShow == null) return;
        Vector3 newPos = new Vector3(pos.position.x, pos.position.y, 300);
        Instantiate(fxStarShow, newPos, Quaternion.identity);
    }
}
