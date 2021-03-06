﻿// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.Tools.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore.Modules.UIScreenModule
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] UITypes uiType = UITypes.Custom;
        public bool showOnStart = false;
        public bool animate = false;
        public TweenType animateTypeOpen = TweenType.SoftEaseOutQuint;
        public TweenType animateTypeClose = TweenType.SoftEaseOutQuint;
        public GameObject content;

        #region === Публичные ===

        /// <summary>
        /// Тип
        /// </summary>
        public UITypes UIType => uiType;

        /// <summary>
        /// Состояние
        /// </summary>
        public bool IsOpen => uiscreen_opened;
        bool uiscreen_opened = false;

        //private Action OnOpenAction;
        private Action OnCloseAction;

        public GameManager Game
        {
            get
            {
                if (_Game != null) return _Game;
                _Game = FindObjectOfType<GameManager>();
                if (_Game == null) { if (!ui_screen_inited) Debug.LogError("Не найден главный компонент GameManager."); return null; }
                return _Game;
            }
        }
        private GameManager _Game;

        #endregion

        #region === Приватные ===
        private float _anim_open_duration = 0.35f;
        private float _anim_close_duration = 0.25f;
        private bool ui_screen_inited = false;
        private Image contentImg
        {
            get
            {
                if (_contentImg != null) return _contentImg;
                if (content == null) return null;
                _contentImg = content.GetComponent<Image>();
                return _contentImg;
            }
        }
        Image _contentImg;
        Color contentImgStartColor = new Color(1, 1, 1, 1);
        Color contentImgEndColor = new Color(1, 1, 1, 0.2f);
        #endregion

        #region === Редактор ===
        string contName = "content";
#if UNITY_EDITOR
        private void Reset()
        {
            var c = transform.Find(contName);
            if (c == null)
            {
                CanvasRenderer cr = GetComponent<CanvasRenderer>();
                if (cr == null) gameObject.AddComponent<CanvasRenderer>();
                var go = new GameObject(contName, typeof(RectTransform));
                go.transform.SetParent(transform);
                RectTransform rt = go.GetComponent<RectTransform>();
                RectTransform p_rt = transform.GetComponent<RectTransform>();
                rt.anchoredPosition = p_rt.anchoredPosition;
                rt.sizeDelta = p_rt.sizeDelta;
                rt.pivot = p_rt.pivot;
                rt.anchorMax = p_rt.anchorMax;
                rt.anchorMin = p_rt.anchorMin;
                go.AddComponent<CanvasRenderer>();
                go.name = contName;
                content = go;
            }
            else content = c.gameObject;
            uiscreen_opened = content.activeSelf;
        }

        [EditorButton("Open / Close")]
        public void ButtonShowHide()
        {
            if (content == null)
            {
                var c = transform.Find(contName);
                if (c == null) return;
                content = c.gameObject;
            }

            uiscreen_opened = content.activeSelf;
            if (Application.isPlaying)
            {
                if (!uiscreen_opened) Open();
                else Close();
            }
            else
            {

                uiscreen_opened = !uiscreen_opened;
                content.SetActive(IsOpen);
            }
        }

#endif
        #endregion

        /// <summary>
        /// Инициализация окна.
        /// </summary>
        public abstract void OnInit();

        /// <summary>
        /// При отображении
        /// </summary>
        public abstract void OnOpen();

        /// <summary>
        /// При закрытии
        /// </summary>
        public abstract void OnClose();

        /// <summary>
        /// Отобразить интерфейс
        /// </summary>
        public virtual void Open(Action OnClose = null, bool use_animate = true)
        {
            if (content == null) { _Game.Log.Error("Content GameObject not found"); return; }
            if (uiscreen_opened) return;

            uiscreen_opened = !uiscreen_opened;
            OnCloseAction = OnClose;

            Vector3 s_sc = new Vector3(1, 1, 1);

            content.SetActive(true);
            if (use_animate && animate)
            {
                int n = 0;
                if (contentImg != null) Tween.TweenColor((c) => contentImg.color = c, contentImgStartColor, contentImgEndColor, _anim_open_duration, 0, null, false, TweenType.EaseOutQuint);
                foreach (Transform item in content.transform)
                {
                    item.localScale = Vector3.zero;
                    if (n == 0)
                        Tween.TweenVector3((a) => item.localScale = a, Vector3.zero, s_sc, _anim_open_duration, 0, EndAnim1, false, animateTypeOpen);
                    else
                        Tween.TweenVector3((a) => item.localScale = a, Vector3.zero, s_sc, _anim_open_duration, 0, null, false, animateTypeOpen);
                    n++;
                }

                //content.transform.localScale = Vector3.zero;
                //twId = Tween.TweenVector3((a) => content.transform.localScale = a, Vector3.zero, s_sc, _anim_open_duration, 0, EndAnim1, false, animateTypeOpen);
            }
            else
            {
                EndAnim1();
            }

            void EndAnim1()
            {
                OnOpen();
            }
        }

        /// <summary>
        /// Скрыть интерфейс
        /// </summary>
        public virtual void Close(bool use_animate = true)
        {
            if (content == null) { _Game.Log.Error("Content GameObject not found"); return; }
            if (!uiscreen_opened) return;
            uiscreen_opened = !uiscreen_opened;
            Vector3 s_sc = new Vector3(1, 1, 1);

            if (use_animate && animate)
            {
                if (contentImg != null) Tween.TweenColor((c) => contentImg.color = c, contentImgEndColor, contentImgStartColor, _anim_open_duration, 0, null, false, TweenType.EaseOutQuint);
                int n = 0;
                foreach (Transform item in content.transform)
                {
                    if (n == 0)
                        Tween.TweenVector3((a) => item.localScale = a, s_sc, Vector3.zero, _anim_close_duration, 0, EndAnim, false, animateTypeClose);
                    else
                        Tween.TweenVector3((a) => item.localScale = a, s_sc, Vector3.zero, _anim_close_duration, 0, null, false, animateTypeClose);
                    n++;
                }
            }
            else
            {
                EndAnim();
            }
            void EndAnim()
            {
                content.SetActive(false);
                if (use_animate && animate) content.transform.localScale = s_sc;
                OnClose();
                OnCloseAction?.Invoke();
            }
        }

        protected void OnEnable()
        {
            if (Game == null) return;
            if (contentImg != null)
            {
                contentImgStartColor = contentImg.color;
                contentImgEndColor = new Color(contentImgStartColor.r, contentImgStartColor.g, contentImgStartColor.b, contentImgStartColor.a);
                contentImgStartColor = new Color(contentImgStartColor.r, contentImgStartColor.g, contentImgStartColor.b, 0.0f);
            }

            StartCoroutine(SkipFrameAndReg());
            StartCoroutine(SkipFrameAndInit());
        }

        private IEnumerator SkipFrameAndInit()
        {
            yield return new WaitForEndOfFrame();
            while (!Game.IsInited)
            {
                yield return new WaitForEndOfFrame();
            }
            ui_screen_inited = true;
            OnInit();
        }


        private IEnumerator SkipFrameAndReg()
        {
            yield return new WaitForEndOfFrame();
            Game.UI.Register(this);
        }

        private void OnDisable()
        {
            if (Game != null) if (Game.UI != null) Game.UI.UnRegister(this);
        }

        void Awake()
        {
            // Ожидаем инициализации ядра
            if (!Game.IsInited) { Invoke("Awake", 0.25f); return; }

            if (content == null) { Game.Log.Error("Нет ссылки на Content (корневой элемент ui панели)"); return; }
            //if (anim == null) anim = content.GetComponent<Animator>();
            uiscreen_opened = content.activeSelf;
            if (showOnStart) Open();
            else if (uiType != UITypes.ScreenMainMenu) Close();
        }
    }

    [Serializable]
    public enum UITypes
    {
        Custom = 0,
        InterfaceInGame = 1,
        ScreenMainMenu = 2,
        ScreenLevelComplete = 3,
        ScreenLevelFailed = 4,
        ScreenLevelSelect = 5,
        ScreenShop = 6,
    }

    [Serializable]
    public enum UIOrientation
    {
        Auto = 0,
        Portrait = 1,
        Landscape = 2,
    }

}