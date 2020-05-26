// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGTeam.SmartMobileCore
{
    public sealed class ScenesHeader
    {
        /// <summary>
        /// Общее количество сцен
        /// </summary>
        public int Count => scenes.Length;

        private Dictionary<int, Scene> LoadedScenes = new Dictionary<int, Scene>();

        private string[] scenes;
        GameManager Game;

        public ScenesHeader(GameManager gameManager)
        {
            this.Game = gameManager;
            scenes = GetScenesInBuild();

            #region === Получим список уже загруженных сцен перед запуском ===
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene n_scene = SceneManager.GetSceneAt(i);
                int n_buildIndex = n_scene.buildIndex;
                LoadedScenes.Add(n_buildIndex, n_scene);
            }
            #endregion

        }

        /// <summary>
        /// Загрузить сцену по ее (build_index)
        /// </summary>
        /// <param name="sceneID">build index</param>
        /// <param name="OnComplete">выполнить по завершении загрузки</param>
        public void LoadScene(int sceneID, Action OnComplete)
        {
            if (sceneID > 1) if (Game.Config.Current.LEVEL_USE_ONE_SCENE_FOR_ALL) sceneID = 1;

            if (!CheckExist(sceneID)) { /* OnComplete?.Invoke(); */ Game.Log.Error("Scene", "Ошибка загрузки сцены #" + sceneID + " в память."); return; }
            var loading = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);

            loading.completed += (x) => _SetActiveScene();
            void _SetActiveScene()
            {
//                if (Game.Config.Current.LEVEL_USE_ONE_SCENE_FOR_ALL) sceneID = 1;

                Scene sc = SceneManager.GetSceneByBuildIndex(sceneID);
                SceneManager.SetActiveScene(sc);
                bool ok = ListAdd(sceneID, SceneManager.GetActiveScene());
                if (!ok) { Game.Log.Error("Scene", "Ошибка загрузки сцены #" + sceneID + " в память."); return; }
                OnComplete?.Invoke();
            }
        }

        // Проверка на существование сцены с таким id
        bool CheckExist(int sceneID)
        {
            if (sceneID < 0) return false;
            if (sceneID >= Count) return false;
            return true;
        }


        /// <summary>
        /// Выгрузить сцену по ее (build_index) в словаре
        /// </summary>
        /// <param name="sceneID">build index</param>
        /// <param name="OnComplete">выполнить по завершении выгрузки</param>
        public void UnloadScene(int sceneID, Action<bool> OnComplete)
        {
            if (sceneID > 1) if (Game.Config.Current.LEVEL_USE_ONE_SCENE_FOR_ALL) sceneID = 1;

            if (LoadedScenes.ContainsKey(sceneID))
            {
                var unloading = SceneManager.UnloadSceneAsync(LoadedScenes[sceneID]);
                unloading.completed += (x) => OnUnloadComplete();
                void OnUnloadComplete()
                {
                    bool ok = ListRemove(sceneID);
                    if (!ok) { Game.Log.Error("Scene", "Ошибка выгрузки сцены #" + sceneID + " из памяти."); return; }
                    WaitTime(Game.Config.Current.LEVEL_WAIT_AFTER_LOADING, _AfterWait);
                    void _AfterWait()
                    {
                        OnComplete?.Invoke(true);
                    }
                }
            }
            else
            {
                OnComplete?.Invoke(false);
            }
        }

        async void WaitTime(int delay_ms, Action onComplete)
        {
            await Task.Delay(delay_ms);
            onComplete?.Invoke();
            return;
        }

        /*
        private IEnumerator SkipFrame_old(Action onComplete)
        {
            yield return new WaitForSecondsRealtime(0.25f); // Ожидание после выгрузки/загрузки новой сцены
            onComplete?.Invoke();
        }
        */

        bool ListAdd(int sceneID, Scene scene)
        {
            if (LoadedScenes.ContainsKey(sceneID)) return false;
            LoadedScenes.Add(sceneID, scene);
            return true;
        }

        bool ListRemove(int sceneID)
        {
            if (!LoadedScenes.ContainsKey(sceneID)) return false;
            LoadedScenes.Remove(sceneID);
            return true;
        }

        // Получить список сцен из build-settings
        string[] GetScenesInBuild()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
            if (scenes.Length <= 1) { Game.Log.Warning("Scene", "В BuildSettings необходимо добавить главную сцену и одну или несколько сцен с уровнями игры."); }
            return scenes;
        }

    }
}