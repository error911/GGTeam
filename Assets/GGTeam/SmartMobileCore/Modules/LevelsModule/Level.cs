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
 
        #region Подсчет очков
        public class ScoreHeader
        {
            string MES_ERROR_NOT_INIT = "Уровень не инициализирован";
            readonly Level level;
            readonly GameManager gameManager;
            public ScoreHeader(Level level, GameManager gameManager)
            {
                this.level = level;
                this.gameManager = gameManager;
            }

            /// <summary>
            /// Добавить очки
            /// </summary>
            /// <param name="score"></param>
            /// <returns></returns>
            public int ScoreAdd(int score)
            {
                if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return 0; }
                if (score <= 0) return level.Data.score;
                int newScore = level.Data.score + score;
                level.Data.score = newScore;
                //level.OnScoreUpdate?.Invoke(newScore);
                gameManager.Levels.OnScoreChanged?.Invoke(newScore);
                return newScore;
            }

            /// <summary>
            /// Отнять очки
            /// </summary>
            /// <param name="score"></param>
            /// <returns></returns>
            public int ScoreRemove(int score)
            {
                if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return 0; }
                if (score == 0) return level.Data.score;
                int newScore = level.Data.score - score;
                if (newScore < 0) newScore = 0;
                level.Data.score = newScore;
                return newScore;
            }
        }
        #endregion

        /// <summary>
        /// Очки
        /// </summary>
        public ScoreHeader Score;

        // <summary>
        // Текущий номер уровеня
        // </summary>
//1        public int CurrentNumber { get; private set; } = 0;
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
//1            CurrentNumber = levelNumber;
            _Game = gameManager;
            _Data = new LevelData(levelNumber);
            Score = new ScoreHeader(this, gameManager);

_Data.played = true; _Data.Save();

            StartCoroutine(SkipFrame(OnOk));
            void OnOk()
            {
//1                CurrentNumber = gameManager.Levels.CurrentNumber;
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

        // <param name="score">набранные очки</param>
        // <param name="stars">полученные звезды (0-3 с шагом 0.5)</param>
        //public abstract void OnLevelComplete(int score, float stars);
        /// <summary>
        /// Уровень пройден
        /// </summary>
        /// <param name="levelData">прогресс</param>
        public abstract void OnLevelComplete(LevelData levelData);

        // <param name="score">набранные очки</param>
        //public abstract void OnLevelFailed(int score);
        /// <summary>
        /// Уровень провален
        /// </summary>
        /// <param name="levelData"></param>
        public abstract void OnLevelFailed(LevelData levelData);

        //void SaveProgress()
        //{
            //string s_name = Game.Config.Current.DATA_SAVE_PREFIX + "." + Game.Config.Current.DATA_SAVE_SUFFIX + ".Config.Level.completedlist";
            //GGTeam.SmartMobileCore.DataModel.LevelData
        //    Game.Levels.Current.Data.Save(Data.number.ToString());
        //}

        /// <summary>
        /// Завершить уровень
        /// </summary>
        // <param name="score">набранные очки</param>
        /// <param name="stars">полученные звезды (0-3 с шагом 0.5)</param>
        public void LevelComplete(float stars = 0)  //int score = 0, 
        {
            if (Game == null) return;
            if (Game.UI == null) return;

            // Сохраняем прогресс
            Data.stars = stars;
            //if (Data.stars < stars) { Data.stars = stars; }
            Data.played = true;
            Data.completed = true;
            Data.Save();

            var nextData = Game.Levels.LevelData(Data.number + 1);
            if (nextData != null) { nextData.played = true; nextData.Save(); } 


            Game.UI.Close(UITypes.ScreenMainMenu);
            Game.UI.Close(UITypes.InterfaceInGame);
            Game.UI.Open(UITypes.ScreenLevelComplete);

//!            _ScoreUpdate(score);
//!if (Data.score < score) Data.score = score;
            

            //Debug.Log("num " + Data.number);
            //Debug.Log("played " + Data.played);
            //Debug.Log("completed " + Data.completed);
            //Debug.Log("score " + Data.score);
            //Debug.Log("stars " + Data.stars);


            // ТУТ СОХРАНИТЬ ПРОГРЕСС
            //            Game.Levels.Current.Data.Save();
            //=======================
            //            SaveProgress();

            OnLevelComplete(Data); // Data.score, stars
        }

        /// <summary>
        /// Провалить уровень
        /// </summary>
        // <param name="score"></param>
        public void LevelFailed()   //int score = 0
        {
            if (Game == null) return;
            if (Game.UI == null) return;
            Game.UI.Close(UITypes.ScreenMainMenu);
            Game.UI.Close(UITypes.InterfaceInGame);
            Game.UI.Open(UITypes.ScreenLevelFailed);
//! if (Data.score < score) Data.score = score;
            OnLevelFailed(Data);
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