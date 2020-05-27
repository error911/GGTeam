// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGTeam.SmartMobileCore
{
    public sealed class LevelsHeader
    {
        /// <summary>
        /// Общее количество уровней
        /// </summary>
        //1public int Count => LevelsData.Data.Count;    // allList.Count;
        public int Count => _Levels.Count;    // allList.Count;

        /// <summary>
        /// Текущий номер уровеня
        /// </summary>
        //[Obsolete("Use 'Current'")]
        public int CurrentNumber { get; private set; } = 0;  //private set; 

        /// <summary>
        /// Текущий уровень
        /// </summary>
        public Level Current { get; private set; }

        // <summary>
        // Максимальный уровень
        // </summary>
        //public int MaxNumber => Count;

        public Action<int> OnLevelChanged;
        public Action<int> OnScoreChanged;

        private readonly GameManager Game;
        private bool loadingProcess = false;
        private int ended_last_level_num = 0;

        private List<int> allLevelsNumList = new List<int>();  // Список номеров уровней

        private List<LevelData> _Levels = new List<LevelData>();   // Список уровней
        
        private bool progressLevelsLoaded = false;


        // ====================================================

        // Конструктор
        public LevelsHeader(GameManager gameManager)
        {
            this.Game = gameManager;
            allLevelsNumList = GetAllLevelsNumList();

            CurrentNumber = gameManager.Config.SAVED_LEVEL_LASTPLAYED;
            _Levels = LevelsProgressLoadAll(allLevelsNumList);

            if (_Levels.Count == 0 && allLevelsNumList.Count > 0)
                if (allLevelsNumList.Count > 0)
                {
                    foreach (var item in allLevelsNumList)
                    {
                        LevelData d = new LevelData(item);
                        _Levels.Add(d);
                    }
                }

//Debug.Log("R"+allLevelsNumList.Count);
            
            int predloaded_level = GetLevelPreloaded();
            if (predloaded_level != 0)
            {
                CurrentNumber = predloaded_level;
                WaitSceneLoad(predloaded_level);
            }
            
        }

        // ====================================================

        /// <summary>
        /// Доступ к данным прогресса уровня
        /// </summary>
        /// <param name="lvlNum"></param>
        /// <returns></returns>
        public LevelData LevelData(int lvlNum)
        {
            if (!progressLevelsLoaded) _Levels = LevelsProgressLoadAll(allLevelsNumList);
            if (lvlNum > Count) lvlNum = 1;

            var lvl = _Levels.Where(x => x.number == lvlNum).SingleOrDefault();
            if (lvl == null) { lvl = new LevelData(lvlNum); _Levels.Add(lvl); }

            return lvl;
        }

        /// <summary>
        /// Список пройденных уровней
        /// </summary>
        /// <returns>Список пройденных уровней</returns>
        public List<LevelData> CompletedLevels()
        {
            if (!progressLevelsLoaded) _Levels = LevelsProgressLoadAll(allLevelsNumList);
            return _Levels.Where(x => x.completed == true).ToList();
        }


        // TODO ===== Вернуть ссылку на класс логики уровня! =====
        /// <summary>
        /// Загрузить следующий уровень. Если текущий уровень последний, то загрузиться первый.
        /// </summary>
        /// <param name="OnLoaded"></param>
        public void LoadNext(Action OnLoaded = null)
        {
            if (loadingProcess) return;
            loadingProcess = true;

            int need_num = CurrentNumber + 1;
            if (need_num > Count) need_num = 1;
            if (need_num == 0) need_num = 1;

            _HideUI();
            Game.ADS.HideBanner();
            Game.UI.GFX.Show(_OnShowComplete);
            void _OnShowComplete()
            {
                
                ReplaceCurrentLevel(need_num, _AdShow);

                // Реклама - Старт
                void _AdShow()
                {
                    Game.ADS.Show(Current.Data.number, _AdsEnd);

                    Game.ADS.ReloadBanner();    // ShowBanner();
                }

                // Реклама - Завершена
                void _AdsEnd(bool ok)
                {
                    _ShowUI(CurrentNumber);
                    Game.UI.GFX.Hide(_OnHided);
                    void _OnHided()
                    {
                        if (LevelData(need_num) != null)
                        { LevelData(need_num).opened = true; /*Debug.Log("SAVE! #NULL " + need_num);*/ LevelsProgressSave(); }

                        loadingProcess = false;
                        OnLoaded?.Invoke();
                        OnLevelChanged?.Invoke(CurrentNumber);
                    }
                }
            }
        }

        // TODO ===== Вернуть ссылку на класс логики уровня! (например Action<Level> OnLoaded)=====
        /// <summary>
        /// Загрузить уровень (по номеру 1..MaxNumber)
        /// </summary>
        /// <param name="levelNumber">номер уровня 1...MaxLevelNum</param>
        /// <param name="OnLoaded"></param>
        public void Load(int levelNumber, Action OnLoaded = null)
        {
            if (loadingProcess) return;
            loadingProcess = true;

            Game.UI.GFX.Show(_OnShowComplete);
            void _OnShowComplete()
            {
                _HideUI();
                if (CurrentNumber > 0)
                {
                    Unload(CurrentNumber, _OnUnloadComplete);
                }
                else _OnUnloadComplete();

                void _OnUnloadComplete()
                {
                    OnlyLoad(levelNumber, _OnLoadComplete);
                    void _OnLoadComplete()
                    {
                        Game.UI.GFX.Hide(_OnHideComplete);
                        void _OnHideComplete()
                        {
                            _ShowUI(CurrentNumber);
                            loadingProcess = false;

                            OnLoaded?.Invoke();
                            OnLevelChanged?.Invoke(CurrentNumber);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Перезагрузить текущий уровень
        /// </summary>
        /// <param name="OnLoaded"></param>
        public void LoadCurrent(Action OnLoaded = null)
        {
//1            Load(CurrentNumber, OnLoaded);//
            Load(Current.Data.number, OnLoaded);
            
            /*
            Load(CurrentNumber, _OnLoadComplete);
            void _OnLoadComplete()
            {
                {
                    OnLoaded?.Invoke();
                    OnLevelChanged?.Invoke(CurrentNumber);
                }
            }
            */
        }


        #region === Приватные методы ===

        private void LevelsProgressSave(int lvlNum = 0, bool completeState = true, int newScore = 0, float newStars = 0)
        {
            //1if (!progressLevelsLoaded) LevelsData = LevelsProgressLoadAll(allLevelsNumList);
            if (!progressLevelsLoaded) _Levels = LevelsProgressLoadAll(allLevelsNumList);
//!            string s_data_level_completed_list = Game.Config.Current.DATA_SAVE_PREFIX + "." + Game.Config.Current.DATA_SAVE_SUFFIX + ".Config.Level.completedlist";

            if (lvlNum > 0)
            {
                var findingLvl = _Levels.Where(x => x.number == lvlNum).SingleOrDefault();
                if (findingLvl == null)
                {
                    LevelData d = new LevelData(lvlNum);
                    d.completed = completeState;
                    d.score = newScore;
                    d.stars = newStars;
                    d.opened = true;
                    _Levels.Add(d);
                }
                else
                {
                    findingLvl.completed = completeState;
                    if (newScore > findingLvl.score) findingLvl.score = newScore;
                    if (newStars > findingLvl.stars) findingLvl.stars = newStars;
                }
            }
            /*
            var test = LevelsData.Data.Where(x => x.number == lvlNum).SingleOrDefault();
            if (test == null) Debug.Log("SAVE #NULL " + lvlNum);
            else Debug.Log("SAVE #" + test.number + " sc:" + test.score + ", st:" + test.stars);

            string s = JsonUtility.ToJson(LevelsData);
            if (s != null && s.Length > 0) PlayerPrefs.SetString(s_data_level_completed_list, s);
            */

        }

        private List<int> GetAllLevelsNumList()
        {
            List<int> tmp = new List<int>();
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (Game.Config.Current.LEVEL_USE_ONE_SCENE_FOR_ALL) sceneCount = Game.Config.Current.LEVEL_ONE_SCENE_LEVELS_COUNT + 1;
            if (sceneCount == 0) Game.Log.Warning("Не указано кол-во уровней. Или параметр [LEVEL_USE_ONE_SCENE_FOR_ALL] установлен ошибочно.");

            for (int i = 0; i < sceneCount; i++) if (i != 0) tmp.Add(i);

            return tmp;
        }

        // TODO: Сделать кеширование результата метода
        private List<LevelData> LevelsProgressLoadAll(List<int> levelsNums)
        {
            List<LevelData> _LevelsTmp = new List<LevelData>();
            foreach (var num in levelsNums)
            {
                _LevelsTmp.Add(new LevelData(num));
            }
            progressLevelsLoaded = true;
            return _LevelsTmp;

            /*
            string s_data_level_completed_list = Game.Config.Current.DATA_SAVE_PREFIX + "." + Game.Config.Current.DATA_SAVE_SUFFIX + ".Config.Level.completedlist";

            progressLevelsLoaded = true;
            string s = PlayerPrefs.GetString(s_data_level_completed_list, "");
            LevelsDataHeader tmp = JsonUtility.FromJson<LevelsDataHeader>(s);
            if (tmp == null) tmp = new LevelsDataHeader();
            return tmp;
            */
        }

        // Изменить номер уровня [-и сохранить предыдущий, как пройденный]
        private void ChangeCurrentNumber(int n)
        {
            if (Game == null) return;
            if (n <= 0) return;
            CurrentNumber = n;

            if (n - 1 >= 0)
            {
                ended_last_level_num = n - 1;
//                LevelsProgressSave(ended_last_level_num, true);
            }
            Game.Config.SAVED_LEVEL_LASTPLAYED = ended_last_level_num;
        }

        // Ожидаем загрузки сцены (в редакторе)
        private async void WaitSceneLoad(int predloaded_level)
        {
            if (predloaded_level == 0) return;
            Scene s = SceneManager.GetSceneByBuildIndex(predloaded_level);
            if (s == null) return;
            int i = 20;
            bool b = false;
            while (!b)
            {
                await Task.Delay(150);
                i--;
                if (s.isLoaded) b = true;
                if (s == null) b = true;
                if (i == 0) b = true;
            }

            if (i == 0) return;
            if (s == null) return;
            SceneManager.SetActiveScene(s);
            _LevelInit();
            return;
        }

        private void _ShowUI(int levelNumber)
        {
            // Отображаем главный интерфейс
            if (levelNumber == 0) Game.UI.Open(UITypes.ScreenMainMenu);

            // Отображаем интерфейс в игре
            if (levelNumber != 0) Game.UI.Open(UITypes.InterfaceInGame);
        }

        private void _HideUI()
        {
            // Скрываем главный интерфейс
            Game.UI.Close(UITypes.ScreenMainMenu, true);

            // Скрываем окна завершения уровня
            Game.UI.Close(UITypes.ScreenLevelComplete, true);
            Game.UI.Close(UITypes.ScreenLevelFailed, true);
            Game.UI.Close(UITypes.InterfaceInGame, true);
            Game.UI.Close(UITypes.ScreenLevelSelect, true);
        }

        // Загрузить уровень (по номеру 1..MaxNumber)
        private void OnlyLoad(int levelNumber, Action OnLoaded = null)
        {

            if (!Game.Config.Current.LEVEL_USE_ONE_SCENE_FOR_ALL)
            {

#pragma warning disable CS0618 // Тип или член устарел
                Game.Scenes.LoadScene(levelNumber, _OnLoadComplete);
#pragma warning restore CS0618 // Тип или член устарел
            }
            else
            {
                Game.Scenes.LoadScene(1, _OnLoadComplete);
            }

            void _OnLoadComplete()
            {
Debug.Log("Уровень #" + levelNumber + " загружен.");
                Game.Log.Debug("Level", "Уровень #" + levelNumber + " загружен.");
                ChangeCurrentNumber(levelNumber);
                _LevelInit();
                OnLoaded?.Invoke();
            }
        }

        private void _LevelInit()
        {
            /*
            bool ok = false;
            var scene = SceneManager.GetActiveScene();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (scene.buildIndex == i) { Debug.Log(i); ok = true; }
            }
            if (!ok) return;
            */

//1            var gm = GameObject.FindObjectOfType<GameManager>();
//1            if (gm != null) return;

            Level lvl = _FindLevelObject();
            lvl.Init(Game, CurrentNumber);
        }

        private Level _FindLevelObject()
        {
            Level lvl = UnityEngine.Object.FindObjectOfType<Level>();
            if (lvl != null) { Current = lvl; }
            else
            {
                GameObject go = new GameObject("[Gameplay]");
                go.name = "[Gameplay]";
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.SetSiblingIndex(0);
                lvl = go.AddComponent<SimpleGameplay>();
            }
            return lvl;
        }

        // Получить список всех уровней из build-settings
        /*
        private List<int> GetAllLevels()
        {
            List<int> tmp = new List<int>();
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++) tmp.Add(i);
            return tmp;
        }
        */

        /// <summary>
        /// Получить номер предзагруженного уровеня (EditorMode)
        /// </summary>
        /// <returns></returns>
        internal int GetLevelPreloaded()
        {
            List<int> tmp = new List<int>();
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                int bi = SceneManager.GetSceneAt(i).buildIndex;
                if (bi != 0) tmp.Add(bi);
            }
            if (tmp.Count == 0) return 0;
            tmp.Sort();
            if (tmp.Count > 1) Game.Log.Warning("Levels", "Предзагружено более одного уровня");
            return tmp[0];
        }

        // Выгрузить
        private void Unload(int levelNumber, Action OnUnloaded = null)
        {
            if (!allLevelsNumList.Contains(levelNumber) || levelNumber < 1) { Game.Log.Error("Level", "Уровень #" + levelNumber + " не найден."); return; }
            int scene_current = levelNumber;    // allList[levelNumber];
#pragma warning disable CS0618 // Тип или член устарел
            Game.Scenes.UnloadScene(levelNumber, _OnUnloadComplete);
#pragma warning restore CS0618 // Тип или член устарел

            void _OnUnloadComplete(bool ok)
            {
                if (ok) Game.Log.Debug("Level", "Уровень #" + levelNumber + " выгружен.");
                OnUnloaded?.Invoke();
            }
        }

        // Заменить уровень
        private void ReplaceCurrentLevel(int newLevelNumber, Action OnComplete = null)
        {
            if (CurrentNumber != 0) Unload(CurrentNumber, _OnUnloadComplete);
            else _OnUnloadComplete();
            void _OnUnloadComplete()
            {
                OnlyLoad(newLevelNumber, OnComplete);
            }
        }

        #endregion

    }
}