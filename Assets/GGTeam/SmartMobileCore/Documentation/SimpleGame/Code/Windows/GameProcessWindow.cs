// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessWindow : UIScreen
{
    public bool EnableBtn_NoAds = true;
    [Space(16)]

    public Text textLvlValue;
    public Text textScoreValue;
    public Transform rtBtnPause;
    public Transform rtBtnNoAds;
    public Transform rtBtnRate;
    public Transform trIngameMenuContent;
    public Image imgPauseMenu;
    public Image imgPlayMenu;
    public Image menuBackgroundImg;

    public Image menuSetupSoundOn;
    public Image menuSetupSoundOff;
    public Image menuSetupVibroOn;
    public Image menuSetupVibroOff;

    //public Button[] BntSelectSkinObj;
    public GameObject RowSkinPref;

    private float duration = 1.0f;
    private int curScore = 0;
    private int tId = -1;
    private bool opened_menu = false;
    private bool pauseProcess = false;
    private string rateUrl = "";




    #region === Работа со скинами ===
    public Transform trSkinsMenuContent;
    public Image imgPauseSkins;
    public Image imgPlaySkins;
    public Transform rtBtnSkins;

    public Action<int> OnSelectSkin;
    Dictionary<int, List<Sprite>> skinList = new Dictionary<int, List<Sprite>>();
    bool skinlist_inited = false;
    private bool opened_skins = false;
    internal void BtnClickSelectSkin(int id)
    {
        if (!skinList.ContainsKey(id)) { Debug.Log("Скин #" + id + " не найден"); return; }
        OnSelectSkin?.Invoke(id);
    }

    /// <summary>
    /// Задать список скинов
    /// </summary>
    /// <param name="skinList"></param>
    public void SkinListInit(Dictionary<int, List<Sprite>> skinList)
    {
        if (skinlist_inited) return;

        this.skinList = skinList;

        rtBtnSkins.gameObject.SetActive(true);

        //RowSkinPref.SetActive(false);

        int skinN = 0;
        foreach (var skinItem in skinList)
        {
            if (skinItem.Value == null) continue;
            if (skinItem.Value.Count == 0) continue;

            GameObject rowGo = Instantiate(RowSkinPref, RowSkinPref.transform.parent);
            rowGo.SetActive(true);
            Transform buttonTr = rowGo.transform.Find("Button");
          Button btn = buttonTr.GetComponent<Button>();
            Transform contentTr = buttonTr.Find("content");
            Transform imgTr1 = contentTr.Find("Image_1");
            Transform imgTr2 = contentTr.Find("Image_2");
            Transform imgTr3 = contentTr.Find("Image_3");
            Transform imgTr4 = contentTr.Find("Image_4");
          Image img1 = imgTr1.GetComponent<Image>();    img1.enabled = false;
          Image img2 = imgTr2.GetComponent<Image>();    img2.enabled = false;
          Image img3 = imgTr3.GetComponent<Image>();    img3.enabled = false;
          Image img4 = imgTr4.GetComponent<Image>();    img4.enabled = false;
            int i = 1;
            foreach (var spr in skinItem.Value)
            {
                if (i > 4) continue;
                
                if (i == 1) {
                    img1.sprite = spr; img1.enabled = true;
                }

                if (i == 2)
                {
                    img2.sprite = spr; img2.enabled = true;
                }

                if (i == 3)
                {
                    img3.sprite = spr; img3.enabled = true;
                }

                if (i == 4)
                {
                    img4.sprite = spr; img4.enabled = true;
                }

                i++;
            }

            btn.onClick.AddListener(() => { BtnClickSelectSkin(skinN); });

            skinN++;
        }

        skinlist_inited = true;
    }

    public void OnBtnOpen_Skins()
    {
        //if (pauseProcess) return;
        float speed = 0.15f;

        if (!opened_skins)
        {
            RowSkinPref.SetActive(false);

//2Game.Metrica.Report_MenuOpen();
//2SetPause(true);
            //Time.timeScale = 0;
            // Открываем игровое меню
            opened_skins = true;
//2pauseProcess = true;
            trSkinsMenuContent.gameObject.SetActive(true);
            Tween.TweenFloat((a) => {
                imgPauseSkins.color = new Color(imgPauseSkins.color.r, imgPauseSkins.color.g, imgPauseSkins.color.b, 1 - a);
                imgPlaySkins.color = new Color(imgPlaySkins.color.r, imgPlaySkins.color.g, imgPlaySkins.color.b, a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0, 0.8f, speed);

            Vector3 posSt = rtBtnSkins.localPosition;
            Vector3 posEn = rtBtnSkins.localPosition + new Vector3(869, 0, 0);
            Tween.TweenVector3((p) => { rtBtnSkins.localPosition = p; }, posSt, posEn, speed, 0, OnSkinsHideCallback1);
            void OnSkinsHideCallback1()
            {
//2                pauseProcess = false;
            }
        }
        else
        {
            // Закрываем игровое меню
            SetPause(false);
            //Time.timeScale = 1;
            opened_skins = false;
//2pauseProcess = true;
            Tween.TweenFloat((a) => {
                imgPauseSkins.color = new Color(imgPauseSkins.color.r, imgPauseSkins.color.g, imgPauseSkins.color.b, a);
                imgPlaySkins.color = new Color(imgPlaySkins.color.r, imgPlaySkins.color.g, imgPlaySkins.color.b, 1 - a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0.8f, 0.0f, speed);

            Vector3 posSt = rtBtnSkins.localPosition;
            Vector3 posEn = rtBtnSkins.localPosition - new Vector3(869, 0, 0);
            Tween.TweenVector3((p) => { rtBtnSkins.localPosition = p; }, posSt, posEn, speed, 0, OnSkinsHideCallback2);

            void OnSkinsHideCallback2()
            {
                trSkinsMenuContent.gameObject.SetActive(false);
//2                pauseProcess = false;
            }
        }
    }

    #endregion

    public void OnBtnOpen_Menu()
    {
        if (pauseProcess) return;
        float speed = 0.15f;

        if (!opened_menu)
        {
Game.Metrica.Report_MenuOpen();
            SetPause(true);
            //Time.timeScale = 0;
            // Открываем игровое меню
            opened_menu = true;
            pauseProcess = true;
            trIngameMenuContent.gameObject.SetActive(true);
            Tween.TweenFloat((a) => {
                imgPauseMenu.color = new Color(imgPauseMenu.color.r, imgPauseMenu.color.g, imgPauseMenu.color.b, 1 - a);
                imgPlayMenu.color = new Color(imgPlayMenu.color.r, imgPlayMenu.color.g, imgPlayMenu.color.b, a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0, 0.8f, speed);

            Vector3 posSt = rtBtnPause.localPosition;
            Vector3 posEn = rtBtnPause.localPosition - new Vector3(869, 0, 0);
            Tween.TweenVector3((p) => { rtBtnPause.localPosition = p; }, posSt, posEn, speed, 0, OnPauseHideCallback1);
            void OnPauseHideCallback1()
            {
                pauseProcess = false;
            }
        }
        else
        {
            // Закрываем игровое меню
            SetPause(false);
            //Time.timeScale = 1;
            opened_menu = false;
            pauseProcess = true;
            Tween.TweenFloat((a) => {
                imgPauseMenu.color = new Color(imgPauseMenu.color.r, imgPauseMenu.color.g, imgPauseMenu.color.b, a);
                imgPlayMenu.color = new Color(imgPlayMenu.color.r, imgPlayMenu.color.g, imgPlayMenu.color.b, 1 - a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0.8f, 0.0f, speed);

            Vector3 posSt = rtBtnPause.localPosition;
            Vector3 posEn = rtBtnPause.localPosition + new Vector3(869, 0, 0);
            Tween.TweenVector3((p) => { rtBtnPause.localPosition = p; }, posSt, posEn, speed, 0, OnPauseHideCallback2);

            void OnPauseHideCallback2()
            {
                trIngameMenuContent.gameObject.SetActive(false);
                pauseProcess = false;
            }
        }
    }


    public void OnBtnRestart()
    {
        OnBtnOpen_Menu();
        Game.Levels.LoadCurrent();
    }

    public void OnBtnSound()
    {
        Game.Config.SETUP_SOUND_ENABLED = !Game.Config.SETUP_SOUND_ENABLED;
Game.Metrica.Report_MenuSound(Game.Config.SETUP_SOUND_ENABLED);

        RenderButtonsImage();
    }

    public void OnBtnVibro()
    {
        Game.Config.SETUP_VIBRO_ENABLED = !Game.Config.SETUP_VIBRO_ENABLED;
Game.Metrica.Report_MenuVibro(Game.Config.SETUP_VIBRO_ENABLED);
        RenderButtonsImage();
    }

    public void OnBtnRate()
    {
Game.Metrica.Report_MenuRate();
        Application.OpenURL(rateUrl);
    }

    public void OnBtnAdsUserOff()
    {
        Game.Config.SETUP_ADS_USEROFF = true;
        //RenderButtonsImage();
    }

    public void OnBtnLevelSelect()
    {
Game.Metrica.Report_MenuSelectLevel();
        OnBtnOpen_Menu();
        Game.UI.Close(UITypes.InterfaceInGame);
        Game.UI.Open(UITypes.ScreenLevelSelect, OnLevelSelectCallback);
    }

    void OnLevelSelectCallback()
    {
        Game.UI.Open(UITypes.InterfaceInGame);
    }

    void SetPause(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }



    void RenderButtonsImage()
    {
        if (Game.Config.SETUP_SOUND_ENABLED) { menuSetupSoundOn.enabled = true; menuSetupSoundOff.enabled = false; }
        else { menuSetupSoundOn.enabled = false; menuSetupSoundOff.enabled = true; }

        if (Game.Config.SETUP_VIBRO_ENABLED) { menuSetupVibroOn.enabled = true; menuSetupVibroOff.enabled = false; }
        else { menuSetupVibroOn.enabled = false; menuSetupVibroOff.enabled = true; }
    }



    public override void OnOpen()
    {
        curScore = Game.Levels.Current.Data.score;
        textLvlValue.text = Game.Levels.Current.Data.number.ToString();
        textScoreValue.text = Game.Levels.Current.Data.score.ToString();
        Game.Levels.OnScoreChanged += ScoreChange;

        if (!skinlist_inited) rtBtnSkins.gameObject.SetActive(false);
        else rtBtnSkins.gameObject.SetActive(true);


        /*
        if (Application.platform == RuntimePlatform.Android) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        else
        {
            rateUrl = "http://greatgalaxy.ru/";
            if (Game.Config.Current.MARKET_URL_ANDROID.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
            else if (Game.Config.Current.MARKET_URL_IOS.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        }


        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(false);
#if UNITY_PURCHASING
        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(EnableBtn_NoAds);
#endif

        if (rateUrl.Length <= 1) if (rtBtnRate) rtBtnRate.gameObject.SetActive(false);
        */
    }

    public override void OnClose()
    {
        Game.Levels.OnScoreChanged -= ScoreChange;
    }

    public override void OnInit()
    {
        textLvlValue.text = "-";
        textScoreValue.text = "-";
        trIngameMenuContent.gameObject.SetActive(false);

        //        if (Application.platform == RuntimePlatform.Android) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
        //        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        //        else
        //        {
        //            rateUrl = "http://greatgalaxy.ru/";
        //            if (Game.Config.Current.MARKET_URL_ANDROID.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
        //            else if (Game.Config.Current.MARKET_URL_IOS.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        //        }


        //        rtBtnNoAds.gameObject.SetActive(false);
        //#if UNITY_PURCHASING
        //        rtBtnNoAds.gameObject.SetActive(EnableBtn_NoAds);
        //#endif

        //        if (rateUrl.Length <= 1) rtBtnRate.gameObject.SetActive(false);



        if (Application.platform == RuntimePlatform.Android) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        else
        {
            rateUrl = "http://greatgalaxy.ru/";
            if (Game.Config.Current.MARKET_URL_ANDROID.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
            else if (Game.Config.Current.MARKET_URL_IOS.Length > 1) rateUrl = Game.Config.Current.MARKET_URL_IOS;
        }


        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(false);
#if UNITY_PURCHASING
        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(EnableBtn_NoAds);
#endif

        if (rateUrl.Length <= 1) if (rtBtnRate) rtBtnRate.gameObject.SetActive(false);


        RenderButtonsImage();

    }

    void ScoreChange(int score)
    {
        int ssc = curScore;
        Tween.StopTween(tId);
        tId = Tween.TweenInt((x) => { textScoreValue.text = x.ToString(); }, ssc, score, duration);
        curScore = score;
    }
}
