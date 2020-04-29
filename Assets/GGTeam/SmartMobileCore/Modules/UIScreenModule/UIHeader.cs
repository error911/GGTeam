// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public sealed class UIHeader
    {
        GameManager Game;
        List<UIScreen> screenList = new List<UIScreen>();
        public GFXHeader GFX;

        public UIHeader(GameManager game)
        {
            this.Game = game;
            GFX = new GFXHeader(this);
        }

        // Открывает начальные интерфейсы
        public void Init()
        {
            WaitSeconds(()=> OnCompleteWait());

            void OnCompleteWait()
            {
                int preloadedLvlNuim = GetLevelPreloaded();

                foreach (var item in screenList)
                {
                    if (preloadedLvlNuim == 0)
                    {
                        // Если нет предзагруженых уровней
                        if (item.showOnStart || item.UIType == UITypes.ScreenMainMenu)
                        {
                            item.Open();
                        }
                    }
                    else
                    {
                        // Если есть предзагруженый уровень
                        // то не показываем начальные интерфейсы
                    }
                }
            }

            int GetLevelPreloaded()
            {
                if (Game == null) return 0;
                if (Game.Levels == null) return 0;
                return Game.Levels.GetLevelPreloaded();
            }


            //void WaitSeconds(float seconds, Action onComplete)
            void WaitSeconds(Action _OnComplete)
            {
                Game.StartCoroutine(Routine());

                IEnumerator Routine()
                {
                    //yield return new WaitForSeconds(seconds);
                    int n = 2;
                    while (n > 0)
                    {
                        yield return new WaitForEndOfFrame();
                        n--;
                    }
                    _OnComplete?.Invoke();
                }
            }
        }

        /// <summary>
        /// Отобразить интерфейс
        /// </summary>
        /// <param name="screen"></param>
        public void Open(UIScreen screen, bool use_animate = true)
        {
            screen.Open(use_animate);
        }

        /// <summary>
        /// Отобразить все интерфейсы типа
        /// </summary>
        /// <param name="uiType"></param>
        public void Open(UITypes uiType, bool use_animate = true)
        {
            foreach (var item in screenList)
            {
                if (item == null) { continue; }
                if (item.UIType == uiType) { item.Open(use_animate); }
            }
        }

        /// <summary>
        /// Скрыть все интерфейсы типа
        /// </summary>
        /// <param name="uiType"></param>
        public void Close(UITypes uiType, bool use_animate = true)
        {
            foreach (var item in screenList)
            {
                if (item == null) { continue; }
                if (item.UIType == uiType)
                {
                    item.Close(use_animate);
                }
            }
        }


        #region === Приватные методы ===
        internal void Register(UIScreen screen)
        {
            if (!screenList.Contains(screen)) { screenList.Add(screen); }
        }

        internal void UnRegister(UIScreen screen)
        {
            if (screenList.Contains(screen)) screenList.Remove(screen);
        }

        #endregion

        public class GFXHeader
        {
            int timeShow_ms = 125;
            int timeHide_ms = 125;
            //Color colorShowIn = new Color(1,1,1, 0);
            //Color colorShowOut = new Color(1,1,1, 1);
            //Color colorHideIn = new Color(1,1,1, 1);
            //Color colorHideOut = new Color(1,1,1, 0);
            Color colorShowIn = new Color(0,0,0, 0);
            Color colorShowOut = new Color(0,0,0, 1);
            Color colorHideIn = new Color(0,0,0, 1);
            Color colorHideOut = new Color(0,0,0, 0);

            Color color_current;

            bool process = false;

            //UIHeader UI;
            UnityEngine.UI.Image image;
            
            
            internal GFXHeader(UIHeader uI)
            {
                //UI = uI;
//                image = CreateToolsCanvas();
//                color_current = colorShowIn;
//                image.color = color_current;
            }
            

            List<LerpStruct> lerps = new List<LerpStruct>();
            public struct LerpStruct
            {
                public bool onoff;
                public Action OnComplete;
                public LerpStruct(bool onoff, Action onComplete)
                {
                    this.onoff = onoff;
                    this.OnComplete = onComplete;
                }
            }

            public void Show(Action OnComplete = null)
            {
                if (image == null) image = CreateToolsCanvas();
                lerps.Add(new LerpStruct(false, OnComplete));
                _LerpQueue();
            }

            public void Hide(Action OnComplete = null)
            {
                if (image == null) image = CreateToolsCanvas();
                lerps.Add(new LerpStruct(true, OnComplete));
                _LerpQueue();
            }

            void _LerpQueue()
            {
                if (lerps.Count == 0) return;
                bool _onoff = lerps[0].onoff;
                Action _OnComplete = lerps[0].OnComplete;
                lerps.RemoveAt(0);

                if (!process)
                {
                    if (!_onoff) Lerp(timeShow_ms, colorShowIn, colorShowOut, false, _OnComplete);
                    else Lerp(timeHide_ms, colorHideIn, colorHideOut, true, _OnComplete);
                }
            }

            async void Lerp(int delay_ms, Color color_start, Color color_end, bool onoff, Action onComplete = null)
            {
                int w = 60 * delay_ms / 1000;
                float step = 1.0f/w;
                int step_time_ms = delay_ms / w;
                image.color = color_start;
                image.enabled = true;
                process = true;
                for (int i = 0; i <= w; i++)
                {
                    await System.Threading.Tasks.Task.Delay(step_time_ms);
                    if (!process) { break; /*End(); return;*/ }
                    color_current = Color.Lerp(color_start, color_end, step * (float)i);
                    if (image != null) image.color = color_current;
                }
                process = false;
                image.enabled = !onoff;
                onComplete?.Invoke();

                _LerpQueue();
                return;
            }

            UnityEngine.UI.Image CreateToolsCanvas()
            {
                var actS = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                var actS2 = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0);
                if (actS2!= null) UnityEngine.SceneManagement.SceneManager.SetActiveScene(actS2);

                GameObject canvasGo = new GameObject();
                canvasGo.name = "[Interface]";
                Canvas  canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                UnityEngine.UI.CanvasScaler canvasScaler = canvasGo.AddComponent<UnityEngine.UI.CanvasScaler>();
                UnityEngine.UI.GraphicRaycaster canvasRaycaster = canvasGo.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                GameObject backgroundGo = new GameObject();
                backgroundGo.name = "fx-background";
                backgroundGo.transform.SetParent(canvasGo.transform);
                RectTransform rectTr = backgroundGo.AddComponent<RectTransform>();
                rectTr.anchorMin = new Vector2(0, 0);
                rectTr.anchorMax = new Vector2(1, 1);
                rectTr.pivot = new Vector2(0.5f, 0.5f);
                rectTr.anchoredPosition = new Vector2(0, 0);
                rectTr.sizeDelta = Vector2.zero;
                rectTr.ForceUpdateRectTransforms();
                CanvasRenderer canvasRenderer = backgroundGo.AddComponent<CanvasRenderer>();
                UnityEngine.UI.Image image = backgroundGo.AddComponent<UnityEngine.UI.Image>();
                image.enabled = false;
                canvasGo.transform.SetAsLastSibling();

                if (actS != null) UnityEngine.SceneManagement.SceneManager.SetActiveScene(actS);
                return image;
            }

        }

    }
}