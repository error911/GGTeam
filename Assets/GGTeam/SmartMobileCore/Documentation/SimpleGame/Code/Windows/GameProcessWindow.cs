// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessWindow : UIScreen
{
    public Text textLvlValue;
    public Text textScoreValue;
    public Transform rtBtnPause;
    public Transform trIngameMenu;
    public Image imgPause;
    public Image imgPlay;
    public Image menuBackgroundImg;
    
    public Image menuSetupSoundOn;
    public Image menuSetupSoundOff;
    public Image menuSetupVibroOn;
    public Image menuSetupVibroOff;

    private float duration = 1.0f;
    private int curScore = 0;
    private int tId = -1;
    private bool opened = false;
    private bool pauseProcess = false;
    private string rateUrl = "";


    public void OnBtnOpen()
    {
        if (pauseProcess) return;
        float speed = 0.15f;

        if (!opened)
        {
            SetPause(true);
            //Time.timeScale = 0;
            // Открываем игровое меню
            opened = true;
            pauseProcess = true;
            trIngameMenu.gameObject.SetActive(true);
            Tween.TweenFloat((a) => {
                imgPause.color = new Color(imgPause.color.r, imgPause.color.g, imgPause.color.b, 1 - a);
                imgPlay.color = new Color(imgPlay.color.r, imgPlay.color.g, imgPlay.color.b, a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0, 0.5f, speed);

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
            opened = false;
            pauseProcess = true;
            Tween.TweenFloat((a) => {
                imgPause.color = new Color(imgPause.color.r, imgPause.color.g, imgPause.color.b, a);
                imgPlay.color = new Color(imgPlay.color.r, imgPlay.color.g, imgPlay.color.b, 1-a);
            }, 0, 1, speed * 2);

            // Фон
            Tween.TweenFloat((a) => { menuBackgroundImg.color = new Color(menuBackgroundImg.color.r, menuBackgroundImg.color.g, menuBackgroundImg.color.b, a); }, 0.5f, 0.0f, speed);

            Vector3 posSt = rtBtnPause.localPosition;
            Vector3 posEn = rtBtnPause.localPosition + new Vector3(869, 0, 0);
            Tween.TweenVector3((p) => { rtBtnPause.localPosition = p; }, posSt, posEn, speed, 0, OnPauseHideCallback2);

            void OnPauseHideCallback2()
            {
                trIngameMenu.gameObject.SetActive(false);
                pauseProcess = false;
            }
        }
    }


    public void OnBtnRestart()
    {
        OnBtnOpen();
        Game.Levels.LoadCurrent();
    }

    public void OnBtnSound()
    {
        Game.Config.SETUP_SOUND_ENABLED = !Game.Config.SETUP_SOUND_ENABLED;
        RenderButtonsImage();
    }

    public void OnBtnVibro()
    {
        Game.Config.SETUP_VIBRO_ENABLED = !Game.Config.SETUP_VIBRO_ENABLED;
        RenderButtonsImage();
    }

    public void OnBtnRate()
    {
        Application.OpenURL(rateUrl);
    }

    public void OnBtnLevelSelect()
    {
        OnBtnOpen();
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
    }

    public override void OnClose()
    {
        Game.Levels.OnScoreChanged -= ScoreChange;
    }

    public override void OnInit()
    {
        textLvlValue.text = "-";
        textScoreValue.text = "-";
        trIngameMenu.gameObject.SetActive(false);

        if (Application.platform == RuntimePlatform.Android) rateUrl = Game.Config.Current.MARKET_URL_ANDROID;
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) rateUrl = Game.Config.Current.MARKET_URL_IOS;

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
