using UnityEngine;

namespace Assets.Project.UI
{
    public class UIScreen : MonoBehaviour
    {
        private bool isRelease;
        protected GameStateProxy gameState;
        protected GlobalCanvas localCanvas;
        
        public bool IsRelease => isRelease;

        public virtual void Init(GameStateProxy gameState, GlobalCanvas localCanvas, bool isRelease)
        {
            this.gameState = gameState;
            this.localCanvas = localCanvas;
            this.isRelease = isRelease;
        }
    }
}