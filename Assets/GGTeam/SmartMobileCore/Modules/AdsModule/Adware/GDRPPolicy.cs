using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore
{
    public class GDRPPolicy : MonoBehaviour
    {
        const string gdrp_name = "ggteam.gdrp";
        public Button GDRPButtonAccept;
        public Button GDRPButtonOk;
        public Text textPolicy_GDRP;
        public Text textPolicy_CCOPA;
        public Image checkOn;
        public Image checkOff;
        public GameObject background;
        bool accepted = false;
        List<GameObject> all_root_go = new List<GameObject>();
        Animator animator;
        public GameManager Game
        {
            get
            {
                if (_Game != null) return _Game;
                _Game = FindObjectOfType<GameManager>();
                if (_Game == null) { Debug.LogError("[GameManager] not found in root scene."); }
                return _Game;
            }
        }
        private GameManager _Game;

        void Awake()
        {
            foreach (Transform item in transform) item.gameObject.SetActive(false);
        }

        public void Init(string textPolicy_GDRP, string textPolicy_CCOPA, Action OnComplete)
        {
            int si = transform.GetSiblingIndex();
            if (si != 0)
            {
                //Game.Log.Warning("GDRPPolicy", "Please, add [GDRPPolicy] prefab on the top of first loading scene (build index = 0).");
                transform.SetAsFirstSibling();
            }

            if (textPolicy_GDRP != null)
                if (textPolicy_GDRP.Length > 0)
                    this.textPolicy_GDRP.text = textPolicy_GDRP;

            if (textPolicy_CCOPA != null)
                if (textPolicy_CCOPA.Length > 0)
                    this.textPolicy_CCOPA.text = textPolicy_CCOPA;


            int gdrp_i = PlayerPrefs.GetInt(gdrp_name, 0);
            if (gdrp_i == 1) accepted = true; else accepted = false;

            animator = GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

            foreach (Transform item in transform) all_root_go.Add(item.gameObject);

            if (!accepted) background.SetActive(true);
            StartCoroutine(WaitFrame(()=>Started()));


            void Started()
            {
                if (accepted)
                {
                    foreach (var item in all_root_go) item.SetActive(false);
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    GDRPButtonAccept.onClick.AddListener(() => { SwichAccept(); });
                    GDRPButtonOk.onClick.AddListener(() => { OnButtonOk(OnComplete); });
                    this.textPolicy_GDRP.text = this.textPolicy_GDRP.text.Replace("{company}", Application.companyName);

                    foreach (var item in all_root_go) item.SetActive(true);

                    if (animator != null) animator.enabled = true;
                }
            }
        }

        

        IEnumerator WaitFrame(Action OnComplete)
        {
            int n = 3;
            while (n > 0)
            {
                yield return new WaitForEndOfFrame();
                n--;
            }
            OnComplete?.Invoke();
        }


        void SwichAccept()
        {
            accepted = !accepted;
            checkOn.enabled = accepted;
            checkOff.enabled = !accepted;
            GDRPButtonOk.interactable = accepted;
        }

        void OnButtonOk(Action OnComplete)
        {
            Game.ADS.AcceptPolicy(true);
            PlayerPrefs.SetInt(gdrp_name, 1);
            foreach (var item in all_root_go) item.SetActive(false);
            OnComplete?.Invoke();
            Destroy(gameObject);
        }

    }
}