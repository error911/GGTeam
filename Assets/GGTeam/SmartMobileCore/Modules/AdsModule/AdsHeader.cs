// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public class AdsHeader
    {
        /// <summary>
        /// Отображать рекламу не чаще этого времени
        /// </summary>
        public int show_time_min_sec = 120;

        /// <summary>
        /// Начинать показывать рекламу с уровня N
        /// </summary>
        public int show_start_level = 3;
        
        /// <summary>
        /// Отображать после каждого уровня
        /// </summary>
        public bool show_each_level = true;


        public bool ADS_ENABLED { get; private set; } = false;
        public Config config;
        private GameManager Game;
        private IAdsProvider adwareProvider;
        public bool isInited { get; private set; } = false;
        public bool isEnabled { get; private set; } = false;
        public bool enabledDebugDraw { get; private set; } = false;

        // Сколько секунд назад успешно отобразилась реклама (нативная)
        public int last_show_sec
        {
            get
            {
                var ts = DateTime.Now - last_show_time;
                return (int)ts.TotalSeconds;
            }
        }
        private DateTime last_show_time = DateTime.Now;

        public AdsHeader(GameManager game, IAdsProvider adwareProvider)
        {
            Game = game;
            config = new Config();

            if (adwareProvider == null)
            {
                ADS_ENABLED = false;
            }
            else
            {
                ADS_ENABLED = true;
                this.adwareProvider = adwareProvider;
            }
        }

        public class Config
        {
            public BannerSizes bannerSizes = BannerSizes.SMART;
            public bool enable_video = true;   // Включить видео рекламу
            public bool enable_banner = true;  // Включить баннеры
        }

        public void AcceptPolicy(bool ok)
        {
            IronSource.Agent.setConsent(ok);
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Init(string APP_KEY, bool enable_video, bool enable_banner)
        {
            if (enable_video || enable_banner) enabledDebugDraw = true;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (!enable_video && !enable_banner) { isInited = true; isEnabled = false; } else isEnabled = true;
                this.config.enable_video = enable_video;
                this.config.enable_banner = enable_banner;
                isInited = false;
                if (isEnabled)
                {
                    isInited = adwareProvider.Init(APP_KEY, enable_video, enable_banner);
                    if (!isInited) Game.Log.Warning("Adv", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]");
                }
            }
        }

        /// <summary>
        /// Показ рекламы по завершении уровня
        /// </summary>
        /// <param name="levelNum">Номер уровня</param>
        /// <param name="OnComplete">При завершении показа</param>
        public void Show(int levelNum, Action<bool> OnComplete)
        {
            if (!isEnabled) { OnComplete?.Invoke(false); return; }
            if (!isInited) { Game.Log.Warning("Adv", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]"); OnComplete?.Invoke(false); return; }

            if (levelNum >= show_start_level && Game.ADS != null && Game.ADS.isEnabled)
            {
                if (Game.ADS.last_show_sec > show_time_min_sec)
                {
                    adwareProvider.Show(levelNum, OnEndShow);
                }
                else
                {
                    if (show_each_level)
                    {
                        adwareProvider.Show(levelNum, OnEndShow);
                    }
                    else OnEndShow(false);
                }
            }
            else OnEndShow(false);

            void OnEndShow(bool ok)
            {
                if (ok) last_show_time = DateTime.Now;
                OnComplete?.Invoke(ok);
            }
        }

        /// <summary>
        /// Показ рекламы за вознаграждение
        /// </summary>
        /// <param name="OnSuccess"></param>
        /// <param name="OnCansel"></param>
        public void ShowRewarded(Action OnSuccess, Action OnCansel)
        {
            if (!isEnabled) { OnSuccess?.Invoke(); return; }
            if (!isInited) { Game.Log.Warning("Adv", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]"); OnCansel?.Invoke(); return; }
            adwareProvider.ShowRewarded(OnSuccess, OnCansel);
        }

        public void HideBanner()
        {
            if (!isEnabled) return;
            if (!config.enable_banner) return;
            IronSource.Agent.hideBanner();
        }

        public void ShowBanner(BannerPosition bannerPosition = BannerPosition.BOTTOM)
        {
#if UNITY_EDITOR
            RenderDebugBanner();
#endif

            //IronSource.Agent.displayBanner();// BannerLayout banner = IronSource.createBanner(Activity, new ISBannerSize(320, 50));
            if (!isEnabled) return;
            if (!config.enable_banner) return;

            IronSourceBannerSize _bannerSize = IronSourceBannerSize.BANNER;
            IronSourceBannerPosition _bannerPosition = IronSourceBannerPosition.BOTTOM;
            if (bannerPosition == BannerPosition.BOTTOM) _bannerPosition = IronSourceBannerPosition.BOTTOM;
            if (bannerPosition == BannerPosition.TOP) _bannerPosition = IronSourceBannerPosition.TOP;

            IronSource.Agent.loadBanner(_bannerSize, _bannerPosition);
            IronSource.Agent.displayBanner();
        }

#if UNITY_EDITOR
        private void RenderDebugBanner()
        {
            if (!enabledDebugDraw) return;
            int SW = Screen.width;
            float time = 30.0f;
            float bw = 728;
            float bh = 90;

            float cw = SW / 2;
            float cbw = bw / 2;
            float al = cw - cbw;
            Vector3 pos = new Vector3(al, bh, 1);

            Color c = Color.yellow;

            Debug.DrawLine(pos, new Vector3(pos.x + bw, pos.y, pos.z), c, time, true); //, false
            Debug.DrawLine(new Vector3(pos.x + bw, pos.y, pos.z), new Vector3(pos.x + bw, pos.y - bh, pos.z), c, time);
            Debug.DrawLine(new Vector3(pos.x + bw, pos.y - bh, pos.z), new Vector3(pos.x, pos.y - bh, pos.z), c, time); //, false
            Debug.DrawLine(new Vector3(pos.x, pos.y, pos.z), new Vector3(pos.x, pos.y - bh, pos.z), c, time);

            Debug.DrawLine(CToW(pos), CToW(new Vector3(pos.x + bw, pos.y, pos.z)), c, time, false);
            Debug.DrawLine(CToW(new Vector3(pos.x + bw, pos.y, pos.z)), CToW(new Vector3(pos.x + bw, pos.y - bh, pos.z)), c, time);
            Debug.DrawLine(CToW(new Vector3(pos.x + bw, pos.y - bh, pos.z)), CToW(new Vector3(pos.x, pos.y - bh, pos.z)), c, time);
            Debug.DrawLine(CToW(new Vector3(pos.x, pos.y, pos.z)), CToW(new Vector3(pos.x, pos.y - bh, pos.z)), c, time);
        }
#endif

        private Vector3 CToW(Vector3 pos)
        {
            if (Camera.main != null)
                return Camera.main.ScreenToWorldPoint(pos);
            else return Vector3.zero;
        }

    }

}