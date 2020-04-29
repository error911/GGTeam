using GGTeam.Tools.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] UITypes uiType = UITypes.Custom;
        public bool showOnStart = false;
        public bool animate = false;
        public TweenType animateTypeOpen = TweenType.SoftEaseOutQuint;// Bounce;
        public TweenType animateTypeClose = TweenType.SoftEaseOutQuint; // Bounce;
        public GameObject content;
        

        #region === Публичные ===

        /// <summary>
        /// Тип
        /// </summary>
        public UITypes UIType => uiType;

        /// <summary>
        /// Состояние
        /// </summary>
        public bool IsOpen => opened;
        bool opened = false;

        public Action OnOpenAction;
        public Action OnCloseAction;

        public GameManager Game
        {
            get
            {
                if (_Game != null) return _Game;
                _Game = FindObjectOfType<GameManager>();
                if (_Game == null) { if (!inited) Debug.LogError("Не найден главный компонент GameManager."); return null; }
                return _Game;
            }
        }
        private GameManager _Game;

        #endregion


        #region === Приватные ===
        private float _anim_open_duration = 0.25f;
        private float _anim_close_duration = 0.15f;
        private bool inited = false;
        //private Animator anim;
        #endregion


        #region === Редактор ===
#if UNITY_EDITOR
        private void Reset()
        {
            var c = transform.Find("content");
            if (c == null)
            {
                CanvasRenderer cr = GetComponent<CanvasRenderer>();
                if (cr == null) gameObject.AddComponent<CanvasRenderer>();
                var go = new GameObject("content", typeof(RectTransform));
                go.transform.SetParent(transform);
                RectTransform rt = go.GetComponent<RectTransform>();
                RectTransform p_rt = transform.GetComponent<RectTransform>();
                rt.anchoredPosition = p_rt.anchoredPosition;
                rt.sizeDelta = p_rt.sizeDelta;
                rt.pivot = p_rt.pivot;
                rt.anchorMax = p_rt.anchorMax;
                rt.anchorMin = p_rt.anchorMin;
                go.AddComponent<CanvasRenderer>();
                go.name = "content";
                content = go;
            }
            else content = c.gameObject;
        }

        [EditorButton("Open / Close")]
        public void ButtonShowHide()
        {
            if (content == null) return;
            opened = content.activeSelf;
            opened = !opened;
            content.SetActive(IsOpen);
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
        public virtual void Open(bool use_animate = true)
        {
            if (content == null) { _Game.Log.Error("Content GO not found"); return; }
            opened = !opened;

            Vector3 s_sc = new Vector3(1, 1, 1);// tr.localScale;

            content.SetActive(true);
            if (use_animate && animate)
            {
                content.transform.localScale = Vector3.zero;
                //Tween.StopTween(twId);
                twId = Tween.TweenVector3((a) => content.transform.localScale = a, Vector3.zero, s_sc, _anim_open_duration, 0, EndAnim1, false, animateTypeOpen);
            }
            else
            {
                EndAnim1();
            }

            void EndAnim1()
            {
                OnOpen();
                OnOpenAction?.Invoke();
            }
        }
        int twId = -1;

        /// <summary>
        /// Скрыть интерфейс
        /// </summary>
        public virtual void Close(bool use_animate = true)
        {
            //if (!opened) return;
            if (content == null) { _Game.Log.Error("Content GO not found"); return; }
            opened = !opened;

            Vector3 s_sc = new Vector3(1, 1, 1);// tr.localScale;

            if (use_animate && animate)
            {
                //Tween.StopTween(twId);
                twId = Tween.TweenVector3((a) => content.transform.localScale = a, s_sc, Vector3.zero, _anim_close_duration, 0, EndAnim, false, animateTypeClose);
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


        private async void WaitForInit()
        {
            //var game = Game;

//if (game == null) game = GameManager.api;

            await Task.Run(() =>
            {
                if (!Game.inited)
                {
                    WaitForInit();
                    return;
                }
            });
            inited = true;
            OnInit();
        }

        protected void OnEnable()
        {
            if (Game == null) return;
            StartCoroutine(SkipFrameAndReg());
            WaitForInit();
        }

        private IEnumerator SkipFrameAndReg()
        {
            yield return new WaitForEndOfFrame();
            //GameManager.api.UI.Register(this);
            Game.UI.Register(this);
        }

        private void OnDisable()
        {
            if (Game != null) if (Game.UI != null) Game.UI.UnRegister(this);
        }

        void Awake()
        {
            if (content == null) { Game.Log.Error("Нет ссылки на Content (корневой элемент ui панели)"); return; }
//if (anim == null) anim = content.GetComponent<Animator>();
            opened = content.activeSelf;
            if (showOnStart) Open();
            else Close();
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
    }

}