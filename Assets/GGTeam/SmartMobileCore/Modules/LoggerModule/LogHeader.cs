// ================================
// Free license: CC BY Murnik Roman
// ================================

namespace GGTeam.SmartMobileCore
{
    public sealed class LogHeader
    {
        private GameManager Game;

        public LogHeader(GameManager gameManager)
        {
            this.Game = gameManager;
        }

        public void Debug(string message)
        {
            RenderLogDebug("<color=gray><b>GAME> </b></color>" + message);
        }

        public void Debug(string caption, string message)
        {
            RenderLogDebug("<color=gray><b>" + caption + "> </b></color>" + message);
        }

        public void Info(string message)
        {
            RenderLogInfo("<color=green><b>GAME> </b></color>" + message);
        }

        public void Info(string caption, string message)
        {
            RenderLogInfo("<color=green><b>" + caption + "> </b></color>" + message);
        }

        public void Warning(string message)
        {
            RenderLogWarning("<color=yellow><b>GAME> </b></color>" + message);
        }

        public void Warning(string caption, string message)
        {
            RenderLogWarning("<color=yellow><b>" + caption + "> </b></color>" + message);
        }

        public void Error(string message)
        {
            RenderLogError("<color=red><b>GAME> </b></color>" + message);
        }

        public void Error(string caption, string message)
        {
            RenderLogError("<color=red><b>" + caption + "> </b></color>" + message);
        }

        void RenderLogDebug(string mes)
        {
            if (Game.Config.main.LOG_SHOW_DEBUG) UnityEngine.Debug.Log(mes);
        }

        void RenderLogInfo(string mes)
        {
            if (Game.Config.main.LOG_SHOW_INFO) UnityEngine.Debug.Log(mes);
        }

        void RenderLogWarning(string mes)
        {
            if (Game.Config.main.LOG_SHOW_WARNING) UnityEngine.Debug.LogWarning(mes);
        }

        void RenderLogError(string mes)
        {
            if (Game.Config.main.LOG_SHOW_ERROR) UnityEngine.Debug.LogError(mes);
        }
    }
}