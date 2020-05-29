using UnityEngine;

namespace GGTeam.SmartMobileCore
{

    public sealed class LevelProgressHeader
    {
        string MES_ERROR_NOT_INIT = "Уровень не инициализирован";
        readonly Level level;
        readonly LevelData levelData;
        readonly GameManager gameManager;
        public LevelProgressHeader(Level level, LevelData levelData, GameManager gameManager)
        {
            this.level = level;
            this.levelData = levelData;
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
            if (score <= 0) return levelData.score;
            int newScore = levelData.score + score;
            levelData.score = newScore;
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
            if (score == 0) return levelData.score;
            int newScore = levelData.score - score;
            if (newScore < 0) newScore = 0;
            levelData.score = newScore;
            gameManager.Levels.OnScoreChanged?.Invoke(newScore);
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
            gameManager.Levels.OnScoreChanged?.Invoke(0);
        }

    }
}