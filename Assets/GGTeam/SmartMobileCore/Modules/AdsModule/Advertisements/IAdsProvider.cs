// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;

namespace GGTeam.SmartMobileCore
{
    public interface IAdsProvider
    {
        /// <summary>
        /// Имя провайдера
        /// </summary>
        string ProviderName { get; }
        bool RewardedReady { get; }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <returns>успех инициализации</returns>
        //bool Init();
        bool Init(string APP_KEY, bool ENABLE_VIDEO, bool ENABLE_BANNER);

        /// <summary>
        /// Начать предварительно фоново загружать обычную рекламу
        /// </summary>
        void Preload();

        /// <summary>
        /// Показ рекламы по завершении уровня
        /// </summary>
        /// <param name="levelNum">Номер уровня</param>
        /// <param name="OnComplete">При завершении показа (удачно или нет)</param>
        void Show(int levelNum, Action<bool> OnComplete);

        /// <summary>
        /// Показ рекламы за вознаграждение
        /// </summary>
        /// <param name="OnSuccess">При успешном просмотре</param>
        /// <param name="OnCansel">При отмене просмотра</param>
        void ShowRewarded(Action OnSuccess, Action OnCansel);

        void ShowBanner();

    }


    public enum BannerSizes
    {
        /// <summary>
        /// 320 x 50
        /// </summary>
        BANNER,

        /// <summary>
        /// 320 x 90
        /// </summary>
        LARGE,

        /// <summary>
        /// 300 x 250
        /// </summary>
        RECTANGLE,

        /// <summary>
        /// If (screen height ≤ 720) 320 x 50
        /// If (screen height > 720) 728 x 90
        /// </summary>
        SMART,
    }

    public enum BannerPosition
    {
        TOP = 1,
        BOTTOM = 2
    }

}