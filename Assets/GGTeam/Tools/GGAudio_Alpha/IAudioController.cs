using System;

namespace GGTeam.Tools.Audio
{
    public interface IAudioController : IDisposable
    {
        /// <summary>
        /// Включает или отключает все звуки в игре. Все аудио источники устанавливают громкость = 0 и останавливают их воспроизведение.
        /// </summary>
        bool SoundEnabled { get; set; }
        /// <summary>
        /// Включает или отключает всю музыку в игре. Все музыкальные источники устанавливают громкость = 0 или значение MusicVolume;
        /// </summary>
        bool MusicEnabled { get; set; }
        /// <summary>
        /// Громкость звуков. (диапазон 1 - 0)
        /// </summary>
        float SoundVolume { get; set; }
        /// <summary>
        /// Громкость музыки. (диапазон 1 - 0)
        /// </summary>
        float MusicVolume { get; set; }
    }
}
