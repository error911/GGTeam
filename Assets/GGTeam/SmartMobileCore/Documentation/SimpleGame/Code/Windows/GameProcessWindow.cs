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
    public Text textMoneyValue;
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

    public GameObject RowSkinPref;

    private float duration = 1.0f;
    private int curScore = 0;
    private int tId = -1;
    private bool opened_menu = false;
    private bool pauseProcess = false;
    private string rateUrl = "";


    bool btn_skin_defState = false;
    bool btn_pause_defState = false;


    #region === Работа со скинами ===
    public Transform trSkinsMenuContent;
    public Image imgPauseSkins;
    public Image imgPlaySkins;
    public Transform rtBtnSkins;

    public Action<SkinPack> OnSelectAviableSkin;
    public Action<SkinPack> OnSelectUnaviableSkin;

    Dictionary<int, SkinPack> skinPack = new Dictionary<int, SkinPack>();
    bool skinlist_inited = false;
    private bool opened_skins = false;
    private int selected = 0;

    internal void BtnClickSelectSkin(int id)
    {
        if (!skinPack.ContainsKey(id)) { Debug.Log("Скин #" + id + " не найден"); return; }

        if (skinPack[id].closed) { SelectSkin(id); OnSelectAviableSkin?.Invoke(skinPack[id]); }
        else OnSelectUnaviableSkin?.Invoke(skinPack[id]);
    }

    /// <summary>
    /// Установить свойства пака
    /// </summary>
    /// <param name="id"></param>
    /// <param name="opened"></param>
    /// <param name="aviable"></param>
    public void SkinPack_SetState(int id, bool opened, bool aviable = true)
    {
        if (!skinPack.ContainsKey(id)) { Debug.LogError("SkinPack_SetState> Не найден id:" + id); return; }

        skinPack[id].closed = aviable;
        if (!aviable)
        {
            skinPack[id].img_closed.enabled = true;
            skinPack[id].img_off.enabled = false;
            skinPack[id].img_on.enabled = false;
        }
        else
        {
            skinPack[id].img_closed.enabled = false;
            skinPack[id].img_off.enabled = !opened;
            skinPack[id].img_on.enabled = opened;
        }
    }

    /// <summary>
    /// Получить свойства пака скинов
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SkinPack SkinPack_GetState(int id)
    {
        if (!skinPack.ContainsKey(id)) { Debug.LogError("SkinPack_GetState> Не найден id:" + id); return null; }
        return skinPack[id];
    }

    public void SkinPack_SetColor(int id, List<Color> colors)
    {
        if (!skinPack.ContainsKey(id)) { Debug.LogError("SkinPack_SetColor> Не найден id:" + id); return; }
        int i = 0;
        foreach (var item in skinPack[id].skinPreviewList)
        {
            if (colors.Count >= i + 1) item.color = colors[i];
            i++;
        }
    }

    public class SkinPack
    {
        public int id = 0;
        public bool closed = false;
        public Button btn;
        public Image img_on;
        public Image img_off;
        public Image img_closed;
        public List<Image> skinPreviewList = new List<Image>();
    }

    void SelectSkin(int id)
    {
        foreach (var item in skinPack)
        {
            if (item.Key == id)
            {
                item.Value.img_on.enabled = true;
                item.Value.img_off.enabled = false;
            }
            else
            {
                item.Value.img_on.enabled = false;
                item.Value.img_off.enabled = true;
            }
        }
    }

    /// <summary>
    /// Задать список скинов
    /// </summary>
    /// <param name="skinList"></param>
    public void SkinPack_Init(int selected, Dictionary<int, List<Sprite>> skinList)
    {
        if (skinlist_inited) return;

        skinPack = new Dictionary<int, SkinPack>();
        this.selected = selected;

        rtBtnSkins.gameObject.SetActive(true);

        int skinN = 0;
        foreach (var skinItem in skinList)
        {
            if (skinItem.Value == null) continue;
            if (skinItem.Value.Count == 0) continue;

            SkinPack skinPackHeader = new SkinPack();
            skinPackHeader.skinPreviewList = new List<Image>();
            skinPackHeader.id = skinN;

            GameObject rowGo = Instantiate(RowSkinPref, RowSkinPref.transform.parent);
            rowGo.SetActive(true);
            Transform buttonTr = rowGo.transform.Find("Button");

            skinPackHeader.img_on = buttonTr.Find("ImageOn").GetComponent<Image>();
            skinPackHeader.img_off = buttonTr.Find("ImageOff").GetComponent<Image>();
            skinPackHeader.img_closed = buttonTr.Find("ImageClosed").GetComponent<Image>();

            skinPackHeader.btn = buttonTr.GetComponent<Button>();

            Transform contentTr = buttonTr.Find("content");
            Transform imgTr1 = contentTr.Find("Image_1");
            Transform imgTr2 = contentTr.Find("Image_2");
            Transform imgTr3 = contentTr.Find("Image_3");
            Transform imgTr4 = contentTr.Find("Image_4");
            Image img1 = imgTr1.GetComponent<Image>(); img1.enabled = false; skinPackHeader.skinPreviewList.Add(img1);
            Image img2 = imgTr2.GetComponent<Image>(); img2.enabled = false; skinPackHeader.skinPreviewList.Add(img2);
            Image img3 = imgTr3.GetComponent<Image>(); img3.enabled = false; skinPackHeader.skinPreviewList.Add(img3);
            Image img4 = imgTr4.GetComponent<Image>(); img4.enabled = false; skinPackHeader.skinPreviewList.Add(img4);

            if (selected == skinN)
            {
                skinPackHeader.img_on.enabled = true;
                skinPackHeader.img_off.enabled = false;
                skinPackHeader.img_closed.enabled = false;
            }
            else
            {
                skinPackHeader.img_on.enabled = false;
                skinPackHeader.img_off.enabled = true;
                skinPackHeader.img_closed.enabled = false;
            }

            int i = 1;
            foreach (var spr in skinItem.Value)
            {
                if (i > 4) continue;

                if (i == 1)
                {
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

            //btn.onClick.RemoveAllListeners();
            int nn = skinN;
            skinPackHeader.btn.onClick.AddListener(() => { BtnClickSelectSkin(nn); });

            skinPack.Add(skinN, skinPackHeader);

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
            btn_pause_defState = rtBtnPause.gameObject.activeSelf;
            rtBtnPause.gameObject.SetActive(false);

            RowSkinPref.SetActive(false);

            //2Game.Metrica.Report_MenuOpen();
            SetPause(true);
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
                rtBtnPause.gameObject.SetActive(btn_pause_defState);
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
            btn_skin_defState = rtBtnSkins.gameObject.activeSelf;
            rtBtnSkins.gameObject.SetActive(false);

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

                rtBtnSkins.gameObject.SetActive(btn_skin_defState);
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
        Game.Config.GameSetup.SETUP_SOUND_ENABLED = !Game.Config.GameSetup.SETUP_SOUND_ENABLED;
        Game.Config.GameSetup.Save();
        Game.Metrica.Report_MenuSound(Game.Config.GameSetup.SETUP_SOUND_ENABLED);

        RenderButtonsImage();
    }

    public void OnBtnVibro()
    {
        Game.Config.GameSetup.SETUP_VIBRO_ENABLED = !Game.Config.GameSetup.SETUP_VIBRO_ENABLED;
        Game.Config.GameSetup.Save();
        Game.Metrica.Report_MenuVibro(Game.Config.GameSetup.SETUP_VIBRO_ENABLED);
        RenderButtonsImage();
    }

    public void OnBtnRate()
    {
        Game.Metrica.Report_MenuRate();
        Application.OpenURL(rateUrl);
    }

    public void OnBtnAdsUserOff()
    {
        Game.Config.GameSetup.SETUP_ADS_USEROFF = true;
        Game.Config.GameSetup.Save();
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
        if (Game.Config.GameSetup.SETUP_SOUND_ENABLED) { menuSetupSoundOn.enabled = true; menuSetupSoundOff.enabled = false; }
        else { menuSetupSoundOn.enabled = false; menuSetupSoundOff.enabled = true; }

        if (Game.Config.GameSetup.SETUP_VIBRO_ENABLED) { menuSetupVibroOn.enabled = true; menuSetupVibroOff.enabled = false; }
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

        //textMoneyValue.text = Game.Levels.OnMoneyChanged;
        textMoneyValue.text = Game.Config.GameSetup.GAMEPLAY_USER_MONEY.ToString();
        Game.Levels.OnMoneyChanged += OnMoneyChanged;

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



        if (Application.platform == RuntimePlatform.Android) rateUrl = Game.Config.GameConfig.MARKET_URL_ANDROID;
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) rateUrl = Game.Config.GameConfig.MARKET_URL_IOS;
        else
        {
            rateUrl = "http://greatgalaxy.ru/";
            if (Game.Config.GameConfig.MARKET_URL_ANDROID.Length > 1) rateUrl = Game.Config.GameConfig.MARKET_URL_ANDROID;
            else if (Game.Config.GameConfig.MARKET_URL_IOS.Length > 1) rateUrl = Game.Config.GameConfig.MARKET_URL_IOS;
        }


        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(false);
#if UNITY_PURCHASING
        if (rtBtnNoAds) rtBtnNoAds.gameObject.SetActive(EnableBtn_NoAds);
#endif

        if (rateUrl.Length <= 1) if (rtBtnRate) rtBtnRate.gameObject.SetActive(false);


        RenderButtonsImage();

    }

    private void OnMoneyChanged(int obj)
    {
        textMoneyValue.text = obj.ToString();  // Game.Config.GameSetup.GAMEPLAY_USER_MONEY.ToString();
    }

    void ScoreChange(int score)
    {
        int ssc = curScore;
        Tween.StopTween(tId);
        tId = Tween.TweenInt((x) => { textScoreValue.text = x.ToString(); }, ssc, score, duration);
        curScore = score;
    }
}
