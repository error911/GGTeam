// ================================
// Free license: CC BY Murnik Roman
// ================================

using GGTeam.Tools.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore
{
    public class Loading : MonoBehaviour
    {
        public Image progress;

        private GameManager _Game;
        float dur = 1.0f;
        int tId = 0;
        float pr = 0.0f;
        public void StartProcess(GameManager game)
        {
            _Game = game;
            int si = transform.GetSiblingIndex();
            if (si != 0) transform.SetAsFirstSibling();
            tId = Tween.TweenFloat((x) => { progress.fillAmount = x; pr = x; }, 0.0f, 1.0f, dur, 0.0f, End, false, TweenType.Linear);
        }

        void End()
        {
            Invoke("Complete", 8.0f);
        }

        public void Complete()
        {
            CancelInvoke();
            Tween.StopTween(tId);
            progress.fillAmount = 1.0f;
            float d = 1.0f - pr;
            Destroy(this.gameObject, d / 2);
        }
        
    }
}