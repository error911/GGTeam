// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceAdsProvider : IAdsProvider
{
    public string ProviderName => "IronSource";

    /// <summary>
    /// Готовы показать рекламу
    /// </summary>
    public bool RewardedReady => _RewardedReady();

    //private string APP_KEY = "";
    private bool ENABLE_VIDEO = false;
    private bool ENABLE_BANNER = false;

    bool ready_interst = false;
    bool ready_rewarded = false;

    Action<bool> OnInterstEnded;
    //Action OnInterstError;

    Action OnRewardedOk;
    Action OnRewardedError;

    bool inited = false;

    //public IronSourceAdsProvider(string APP_KEY, bool ENABLE_VIDEO = true, bool ENABLE_BANNER = true)
    //{
    //    this.APP_KEY = APP_KEY;
    //    this.ENABLE_VIDEO = ENABLE_VIDEO;
    //    this.ENABLE_BANNER = ENABLE_BANNER;
    //    Init();
    //}

    public bool Init(string APP_KEY, bool ENABLE_VIDEO, bool ENABLE_BANNER)
    {
        if (APP_KEY.Length == 0) return false;
        if (!ENABLE_VIDEO && !ENABLE_BANNER) return false;
        this.ENABLE_VIDEO = ENABLE_VIDEO;
        this.ENABLE_BANNER = ENABLE_BANNER;
        EventsSubs();

        // ===== Настройка вручную =====
        if (ENABLE_VIDEO)
        {
            //For Interstitial
            IronSource.Agent.init(APP_KEY, IronSourceAdUnits.INTERSTITIAL);

            //For Rewarded Video
            IronSource.Agent.init(APP_KEY, IronSourceAdUnits.REWARDED_VIDEO);
        }
        
        //For Offerwall
        //IronSource.Agent.init(APP_KEY, IronSourceAdUnits.OFFERWALL);

        if (ENABLE_BANNER)
        {
            //For Banners
            IronSource.Agent.init(APP_KEY, IronSourceAdUnits.BANNER);
        }

        // простой способ убедиться, что ваша интеграция вознагражденного видео-посредничества была успешно завершена
        IronSource.Agent.validateIntegration();

        IronSource.Agent.shouldTrackNetworkState(true);

        inited = true;
        Preload();

        return true;
    }



    public void Show(int levelNum, Action<bool> OnComplete)
    {
        if (!inited) { Debug.LogWarning("Перед использованием рекламы, необходимо сначала ее инициализировать Init()"); OnComplete?.Invoke(false); return; }
        this.OnInterstEnded = OnComplete;
        ready_interst = false;
        IronSource.Agent.showInterstitial();
    }

    /*
    private IEnumerator SkipFrame_old(Action onComplete)
        {
            yield return new WaitForSecondsRealtime(0.25f); // Ожидание после выгрузки/загрузки новой сцены
            onComplete?.Invoke();
        }
    */

    public void ShowBanner()
    {
        if (!inited) Debug.LogWarning("Перед использованием рекламы, необходимо сначала ее инициализировать Init()");
        throw new NotImplementedException();
    }

    public void ShowRewarded(Action OnSuccess, Action OnCansel)
    {
        if (!inited) { Debug.LogWarning("Перед использованием рекламы, необходимо сначала ее инициализировать Init()"); OnCansel?.Invoke(); return; }
        this.OnRewardedOk = OnSuccess;
        this.OnRewardedError = OnCansel;

        IronSource.Agent.showRewardedVideo();
    }


    bool subscribed = false;
    private void EventsSubs()
    {
        if (subscribed) return;
        subscribed = true;

        if (ENABLE_VIDEO)
        {
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        }

        if (ENABLE_BANNER)
        {

        }
    }

    #region ===== Interstitial Events =====
    // удачно или нет показалась реклама
    void InterstitialEnded(bool okey)
    {
        OnInterstEnded?.Invoke(okey);
        OnInterstEnded = null;
    }

    //Вызывается при сбое процесса инициализации.
    //@param description - string - contains information about the failure.
    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        //Debug.Log(">>InterstitialAdLoadFailedEvent");
        Preload();  //? В другое место
        InterstitialEnded(false);
    }

    // [2] Вызывается непосредственно перед открытием экрана Interstitial.
    void InterstitialAdShowSucceededEvent()
    {
        //Debug.Log(">>InterstitialAdShowSucceededEvent");
    }

    //Вызывается, когда объявление не показывается.
    //@param description - string - contains information about the failure.
    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        //Debug.Log(">>InterstitialAdShowFailedEvent");
        InterstitialEnded(false);
    }

    // Вызывается, когда конечный пользователь нажимает на промежуточную рекламу.
    void InterstitialAdClickedEvent()
    {
        //Debug.Log(">>InterstitialAdClickedEvent");
    }

    // [3] Вызывается, когда промежуточная реклама закрывается и пользователь возвращается к экрану приложения.
    void InterstitialAdClosedEvent()
    {
        //Debug.Log("!>>InterstitialAdClosedEvent");
        InterstitialEnded(true);
        Preload();  //? В другое место
    }

    //Вызывается, когда вставка готова к показу после вызова функции загрузки.
    void InterstitialAdReadyEvent()
    {
        //Debug.Log(">>InterstitialAdReadyEvent");
        ready_interst = true;
    }

    // [1] Вызывается, когда открыт рекламный блок Interstitial.
    void InterstitialAdOpenedEvent()
    {
        //Debug.Log(">>InterstitialAdOpenedEvent");
    }

    #endregion

    #region ===== Rewarded Events =====

    //Вызывается, когда открылось RewardedVideo.
    //Приложение потеряет фокус.
    //Пожалуйста, избегайте выполнения тяжелых задач, пока видеообъявление не будет закрыто.
    void RewardedVideoAdOpenedEvent()
    {
        //Debug.Log("> RewardedVideoAdOpenedEvent");
    }

    //Вызывается, когда показ рекламы RewardedVideo будет закрыт.
    //Ваша деятельность теперь восстановит фокус
    void RewardedVideoAdClosedEvent()
    {
        //Debug.Log("> RewardedVideoAdClosedEvent");
    }

    //Вызывается при изменении статуса доступности рекламы.
    //@param - available - значение изменится на true, когда будут доступны видео с вознаграждением.
    //Затем вы можете показать видео, вызвав showRewardedVideo().
    //Значение изменится на false, когда нет доступных видео.
    void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //        ttt.text += "Changed:"+ available +"\r\n";
        //Change the in-app 'Traffic Driver' state according to availability.
        //bool rewardedVideoAvailability = available;

        //Debug.Log("> RewardedVideoAvailabilityChangedEvent " + available);
        ready_rewarded = available;
    }

    // Примечание. Указанные ниже события доступны не для всех поддерживаемых сетей видео-рекламы с вознаграждением.
    // Проверьте, какие события доступны для рекламной сети, которую вы решили включить в свою сборку.
    // Мы рекомендуем использовать только события, которые регистрируются во ВСЕХ рекламных сетях, которые вы включаете в свою сборку.
    // Вызывается, когда начинается показ видеообъявления.
    void RewardedVideoAdStartedEvent()
    {
        //Debug.Log("> RewardedVideoAdStartedEvent");
    }

    // Вызывается, когда видеообъявление заканчивается.
    void RewardedVideoAdEndedEvent()
    {
        //Debug.Log("> RewardedVideoAdEndedEvent");
    }

    // Вызывается, когда пользователь завершил видео и должен быть вознагражден.
    // При использовании обратных вызовов от сервера к серверу вы можете игнорировать эти события и ждать обратного вызова с сервера ironSource.
    //@param - placement - объект размещения, который содержит данные вознаграждения
    //
    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        OnRewardedOk?.Invoke();
    }

    //Вызывается, когда видео с вознаграждением не удалось показать 
    //@param description - string - содержит информацию о сбое.
    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        OnRewardedError?.Invoke();
    }

    #endregion

    #region ===== Функции Interstitial =====

    /// <summary>
    /// Проверить, готовы ли мы показать рекламу?
    /// </summary>
    /// <returns></returns>
    public bool ReadyInterst()
    {
        return ready_interst;
    }

    // Заранее вызовем загрузку видео
    public void Preload()
    {
        IronSource.Agent.loadInterstitial();
    }
    #endregion

    #region ===== Функции Rewarded =====

    /// <summary>
    /// Проверить, готовы ли мы показать рекламу?
    /// </summary>
    private bool _RewardedReady()
    {
        bool available = IronSource.Agent.isRewardedVideoAvailable();
        return available;
    }

    //void IAdsProvider.Preload()
    //{
    //    throw new NotImplementedException();
    //}
    #endregion



}
