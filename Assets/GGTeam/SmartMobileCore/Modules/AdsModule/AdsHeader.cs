// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public sealed class AdsHeader
    {
        /// <summary>
        /// Отображать после каждого уровня
        /// </summary>
        public bool Show_each_level { get; private set; } = false;
        public bool ADS_ENABLED { get; private set; } = false;
        public bool IsInited { get; private set; } = false;
        public bool IsEnabled { get; private set; } = false;
        public bool EnabledDebugDraw { get; private set; } = false;

        private readonly Config config;
        private readonly GameManager Game;
        private readonly IAdsProvider adwareProvider;

        // Сколько секунд назад успешно отобразилась реклама (видео not-rewarded)
        public int Last_show_sec
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
            /// <summary>
            /// Начинать показывать рекламу с уровня N
            /// </summary>
            public int show_video_start_level = 4;

            /// <summary>
            /// Отображать рекламу не чаще этого времени
            /// </summary>
            public int show_time_min_sec = 120;
        }

        public void AcceptPolicy(bool ok)
        {
            IronSource.Agent.setConsent(ok);
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Init(string APP_KEY, bool enable_video, bool enable_banner, int show_video_start_level, int show_time_min_sec)
        {
            if (enable_video || enable_banner) EnabledDebugDraw = true;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (!enable_video && !enable_banner) { IsInited = true; IsEnabled = false; } else IsEnabled = true;
                this.config.enable_video = enable_video;
                this.config.enable_banner = enable_banner;
                this.config.show_video_start_level = show_video_start_level;
                this.config.show_time_min_sec = show_time_min_sec;
                IsInited = false;
                if (IsEnabled)
                {
                    IsInited = adwareProvider.Init(APP_KEY, enable_video, enable_banner);
                    if (!IsInited) Game.Log.Warning("Ads", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]");
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
            if (Game.Config.GameSetup.SETUP_ADS_USEROFF) { OnComplete?.Invoke(false); return; }
            if (!IsEnabled) { OnComplete?.Invoke(false); return; }
            if (!IsInited) { Game.Log.Warning("Ads", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]"); OnComplete?.Invoke(false); return; }
            if (levelNum > config.show_video_start_level && Game.ADS != null && Game.ADS.IsEnabled)
            {
                if (Game.ADS.Last_show_sec > config.show_time_min_sec)
                {
                    adwareProvider.Show(levelNum, OnEndShow);
                }
                else
                {
                    if (Show_each_level)
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
        /// <param name="OnCancel"></param>
        public void ShowRewarded(Action OnSuccess, Action OnCancel)
        {
            if (!IsEnabled) { OnSuccess?.Invoke(); return; }
            if (!IsInited) { Game.Log.Warning("Ads", "Не удалось проинициализировать провайдера источника рекламы [" + adwareProvider.ProviderName + "]"); OnCancel?.Invoke(); return; }
            adwareProvider.ShowRewarded(OnSuccess, OnCancel);
        }

        public void HideBanner()
        {
            if (!IsEnabled) return;
            if (!config.enable_banner) return;
            IronSource.Agent.hideBanner();
        }


        public void ReloadBanner()
        {
            if (!IsEnabled) return;
            if (!config.enable_banner) return;
            IronSource.Agent.destroyBanner();
            ShowBanner(predBannerPosition);
        }

        BannerPosition predBannerPosition = BannerPosition.BOTTOM;
        public void ShowBanner(BannerPosition bannerPosition = BannerPosition.BOTTOM)
        {
#if UNITY_EDITOR
            RenderDebugBanner();
#endif

            predBannerPosition = bannerPosition;
            //IronSource.Agent.displayBanner();// BannerLayout banner = IronSource.createBanner(Activity, new ISBannerSize(320, 50));
            if (Game.Config.GameSetup.SETUP_ADS_USEROFF) return;
            if (!IsEnabled) return;
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
            if (!EnabledDebugDraw) return;
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