// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public abstract class Level : MonoBehaviour
    {
        /// <summary>
        /// Текущий номер уровеня
        /// </summary>
        public int CurrentNumber { get; private set; } = 0;
        public LevelData Data { get { return _Data; } set { _Data = value; } }
        [HideInInspector] [SerializeField] LevelData _Data;

        public GameManager Game
        {
            get
            {
                if (_Game != null) return _Game;
                _Game = FindObjectOfType<GameManager>();
                if (_Game == null) { Debug.LogError("No [GameManager] component found."); return null; }
                return _Game;
            }
        }
        private GameManager _Game;

        public void Init(GameManager gameManager, int levelNumber)
        {
            CurrentNumber = levelNumber;
            _Game = gameManager;

            _Data = new LevelData(levelNumber);

            StartCoroutine(SkipFrame(OnOk));
            void OnOk()
            {
                CurrentNumber = gameManager.Levels.CurrentNumber;
                OnLevelStart();
            }
        }

        private IEnumerator SkipFrame(Action onComplete = null)
        {
            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }

        /*
        async void WaitTime(int delay_ms, Action onComplete)
        {
            await Task.Delay(delay_ms);
            onComplete?.Invoke();
            return;
        }
        */

        /// <summary>
        /// Уровень загрузился
        /// </summary>
        public abstract void OnLevelStart();    //TODO НЕ СРАБАТЫВАЕТ ПРИ ОТЛАДКЕ УРОВНЯ

        /// <summary>
        /// Уровень пройден
        /// </summary>
        /// <param name="score">набранные очки</param>
        /// <param name="stars">полученные звезды (0-3 с шагом 0.5)</param>
        public abstract void OnLevelComplete(int score, float stars);

        /// <summary>
        /// Уровень провален
        /// </summary>
        /// <param name="score">набранные очки</param>
        public abstract void OnLevelFailed(int score);

        /// <summary>
        /// Завершить уровень
        /// </summary>
        /// <param name="score">набранные очки</param>
        /// <param name="stars">полученные звезды (0-3 с шагом 0.5)</param>
        public void LevelComplete(int score = 0, float stars = 0)
        {
            if (Game == null) return;
            if (Game.UI == null) return;
            Game.UI.Close(UITypes.ScreenMainMenu);
            Game.UI.Close(UITypes.InterfaceInGame);
            Game.UI.Open(UITypes.ScreenLevelComplete);
if (Data.score < score) Data.score = score;
if (Data.stars < stars) Data.stars = stars;
            OnLevelComplete(score, stars);
        }

        /// <summary>
        /// Провалить уровень
        /// </summary>
        /// <param name="score"></param>
        public void LevelFailed(int score = 0)
        {
            if (Game == null) return;
            if (Game.UI == null) return;
            Game.UI.Close(UITypes.ScreenMainMenu);
            Game.UI.Close(UITypes.InterfaceInGame);
            Game.UI.Open(UITypes.ScreenLevelFailed);
if (Data.score < score) Data.score = score;
            OnLevelFailed(score);
        }

        /// <summary>
        /// Перезапустить уровень
        /// </summary>
        public void LevelRestart()
        {
            if (Game == null) return;
            if (Game.UI == null) return;
            Game.Levels.LoadCurrent();
        }
    }

    

}