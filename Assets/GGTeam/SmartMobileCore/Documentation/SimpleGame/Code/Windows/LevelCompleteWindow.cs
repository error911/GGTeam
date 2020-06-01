// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using Boo.Lang;
using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteWindow : UIScreen
{
    // collect reward
    [SerializeField] bool use_collect_reward = false;
    [SerializeField] int collectX_module = 3;

    [Space(32)]

    [SerializeField] Text textLvlNum = null;
    [SerializeField] Text textScores = null;
    [SerializeField] Text textCrystals = null;
    [SerializeField] Image imgStar1 = null;
    [SerializeField] Image imgStar2 = null;
    [SerializeField] Image imgStar3 = null;

    //[SerializeField] Image imgCrystal = null;
    [SerializeField] GameObject goCrystal = null;
    [SerializeField] Transform contentCrystals = null;
    //[SerializeField] Image imgCollect2 = null;
    //[SerializeField] Image imgCollect3 = null;

    [SerializeField] GameObject fxStarShow = null;
    [SerializeField] GameObject fxCollectShow = null;

    [SerializeField] Button btnContinue = null;
    [SerializeField] Button btnCollect = null;
    [SerializeField] Button btnCollectX = null;
    [SerializeField] Text textCollectX = null;

    [SerializeField] GameObject panelCollectReward = null;

    public void OnBtnCollect()
    {
        int m = Game.Levels.Current.Data.money;
        if (m > 0)
        {
            Game.Config.GameSetup.GAMEPLAY_USER_MONEY += m;
            Game.Config.GameSetup.Save();
        }
        Game.Levels.LoadNext();
    }

    public void OnBtnCollectX()
    {
        int m = Game.Levels.Current.Data.money;
        if (m > 0)
        {
            Game.Config.GameSetup.GAMEPLAY_USER_MONEY += m;
            Game.Config.GameSetup.Save();
        }

        Game.ADS.ShowRewarded(OnRewOk, OnRevCancel);
    }

    private void OnRevCancel()
    {
        OnBtnCollect();
    }

    private void OnRewOk()
    {
        int m = Game.Levels.Current.Data.money * collectX_module;
        if (m > 0)
        {
            Game.Config.GameSetup.GAMEPLAY_USER_MONEY += m;
            Game.Config.GameSetup.Save();
        }
        Game.Levels.LoadNext();
    }

    public void OnBtnContinue()
    {
        Game.Levels.LoadNext();
    }

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();
    }

    

    public override void OnInit()
    {
        if (use_collect_reward)
        {
            textScores.gameObject.SetActive(false);
            panelCollectReward.SetActive(true);
            btnContinue.gameObject.SetActive(false);
            //btnCollect.gameObject.SetActive(true);
            //if (Game.ADS.ADS_ENABLED) btnCollectX.gameObject.SetActive(true);
            btnCollect.gameObject.SetActive(false);
            btnCollectX.gameObject.SetActive(false);

            textCollectX.text = "COLLECT<color=orange>X</color>" + collectX_module.ToString();
            goCrystal.SetActive(false);
            textCrystals.text = "";
        }
        else
        {
            textScores.gameObject.SetActive(true);
            panelCollectReward.SetActive(false);
            btnContinue.gameObject.SetActive(true);
            btnCollect.gameObject.SetActive(false);
            btnCollectX.gameObject.SetActive(false);
        }
    }

    public override void OnClose()
    {
        if (use_collect_reward)
        {
            textCrystals.text = "";
            foreach (Transform item in contentCrystals)
            {
                if (item != goCrystal.transform) Destroy(item.gameObject);
            }

            btnCollect.gameObject.SetActive(false);
            btnCollectX.gameObject.SetActive(false);
        }
    }

    public override void OnOpen()
    {
        textLvlNum.text = "LEVEL " + Game.Levels.Current.Data.number;
        if (Game.Levels.Current.Data.score > 0)
        {
            textScores.text = Game.Levels.Current.Data.score.ToString();
            Tween.TweenInt((n) => { textScores.text = n.ToString(); }, 0, Game.Levels.Current.Data.score, 0.8f);
        }


        if (use_collect_reward)
        {
            btnCollectX.gameObject.SetActive(true);
            btnCollectX.transform.localScale = Vector3.zero;
            Tween.TweenVector3((s) => { btnCollectX.transform.localScale = s; }, Vector3.zero, Vector3.one, 0.25f, 0, null, false, TweenType.Overshoot);

            float delay = 2.0f;
            btnCollect.gameObject.SetActive(true);
            btnCollect.transform.localScale = Vector3.zero;
            Tween.TweenVector3((s) => { btnCollect.transform.localScale = s; }, Vector3.zero, Vector3.one, 0.25f, delay, null, false, TweenType.Overshoot);
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

        if (use_collect_reward) Invoke("ShowCollect", 0.25f);
    }

    
    void ShowCollect()
    {
        if (!goCrystal) return;
        int MAX_SHOW = 8;
        int m = Game.Levels.Current.Data.money;
        int c = m;
        if (c > MAX_SHOW) c = MAX_SHOW;

        Tween.TweenInt((t) => { textCrystals.text = t.ToString(); }, 0, m, 0.5f, 0.5f);

        if (m > 0)
        {
            for (int i = 0; i < c; i++)
            {
                GameObject go = Instantiate(goCrystal, contentCrystals);
                go.SetActive(true);
                Image img = go.GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

                float n = (c-i) * 0.20f;
                Vector3 pos = img.transform.localPosition;
                Tween.TweenFloat((u) => { img.color = new Color(img.color.r, img.color.g, img.color.b, u); }, 0, 1, 0.25f, n);  //()=> FXCollect(img)
                Tween.TweenFloat((u) => { }, 0, 1, 0.1f, n, () => FXCollect(img));
            }
        }

    }
    

    void FXStar(Transform pos)
    {
        if (fxStarShow == null) return;
        Vector3 newPos = new Vector3(pos.position.x, pos.position.y, 0);
        Instantiate(fxStarShow, newPos, Quaternion.identity);
    }

    void FXCollect(Image img)
    {
        if (img == null) return;
        if (fxCollectShow == null) return;
        var g = Instantiate(fxCollectShow, img.transform);
        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, -300);
    }
    
}
