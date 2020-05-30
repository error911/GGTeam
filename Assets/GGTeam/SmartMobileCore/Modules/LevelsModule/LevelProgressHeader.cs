using UnityEngine;

namespace GGTeam.SmartMobileCore
{

    public sealed class LevelProgressHeader
    {
        string MES_ERROR_NOT_INIT = "Уровень не инициализирован";
        readonly Level level;
        readonly LevelData levelData;
        readonly GameManager Game;
        public LevelProgressHeader(Level level, LevelData levelData, GameManager gameManager)
        {
            this.level = level;
            this.levelData = levelData;
            this.levelData.money = 0;
            this.Game = gameManager;
        }

        /// <summary>
        /// Добавить очки
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public int ScoreAdd(int score)
        {
            if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return 0; }
            if (score <= 0) return levelData.score;
            int newScore = levelData.score + score;
            levelData.score = newScore;
            Game.Levels.OnScoreChanged?.Invoke(newScore);
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
            if (score == 0) return levelData.score;
            int newScore = levelData.score - score;
            if (newScore < 0) newScore = 0;
            levelData.score = newScore;
            Game.Levels.OnScoreChanged?.Invoke(newScore);
            return newScore;
        }

        /// <summary>
        /// Обнулить очки текущего уровня
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public void ScoreClear()
        {
            if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return; }
            levelData.score = 0;
            Game.Levels.OnScoreChanged?.Invoke(0);
        }

        /// <summary>
        /// Добавить игровую валюту (деньги/кристаллы...) в текущий уровень.
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public int MoneyAdd(int money)
        {
            if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return 0; }
            if (money <= 0) return levelData.money;
            int newMoney = levelData.money + money;
            levelData.money = newMoney;
            //Game.Config.GameSetup.GAMEPLAY_USER_MONEY += money;
            //Game.Config.GameSetup.Save();
            Game.Levels.OnMoneyChanged?.Invoke(newMoney);
            return newMoney;
        }

        /// <summary>
        /// Обнулить игровую валюту текущего уровня
        /// </summary>
        public void MoneyClear()
        {
            if (level == null) { Debug.LogWarning(MES_ERROR_NOT_INIT); return; }
            levelData.money = 0;
            Game.Levels.OnMoneyChanged?.Invoke(0);
        }

    }
}