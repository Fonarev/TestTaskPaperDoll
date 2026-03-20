using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.UI
{
    [RequireComponent(typeof(Canvas))]
    public class GlobalCanvas : MonoBehaviour
    {
        [SerializeField] protected Transform screenContainer;
        [SerializeField] protected Transform popupContainer;

        protected GameStateProxy gameState;
        protected Dictionary<string, UIScreen> screens = new();
        protected Dictionary<string, UIPopupWindow> popups = new();
        protected Dictionary<string, GameObject> popupsObject = new();
        
        private UIScreen oldScreen;
        private UIPopupWindow oldWindow;

        public virtual void Initialize(GameStateProxy gameState)
        {
            this.gameState = gameState;
        }

        public void ShowScreen(string name, bool isRelease = true)
        {
            if (screens.TryGetValue(name, out UIScreen screen))
            {
                screen.gameObject.SetActive(true);
            }
            else
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIScreen>(name, screenContainer, op =>
                {
                    screens[name] = op;
                    op.Init(gameState, this, isRelease);
                    op.gameObject.SetActive(true);
                }));
            }
        }

        public IEnumerator ShowScreenRoutine(string name, bool isRelease = true)
        {
            if (screens.TryGetValue(name, out UIScreen screen))
            {
                screen.gameObject.SetActive(true);
            }
            else
            {
                yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIScreen>(name, screenContainer, op =>
                {
                    screens[name] = op;
                    op.Init(gameState, this, isRelease);
                    op.gameObject.SetActive(true);
                }));
            }
            yield return null;
        }

        public void HideScreen(string name, bool isRelease = true)
        {
            if (screens.TryGetValue(name, out UIScreen screen))
            {
                if (screen.IsRelease)
                {
                    if (isRelease)
                    {
                        screen.gameObject.SetActive(false);
                        Addressables.ReleaseInstance(screen.gameObject);
                        screens.Remove(name);
                    }
                    else
                    {
                        screen.gameObject.SetActive(false);
                    }
                }
                else
                {
                    screen.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log($"No Screen: {name}");
            }
        }

        public void ShowPopupWindows(string name, bool isRelease = true)
        {
            if (popups.TryGetValue(name, out UIPopupWindow popup))
            {
                popup.gameObject.SetActive(true);
            }
            else
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIPopupWindow>(name, popupContainer, op =>
                {
                    popups[name] = op;
                    op.Init(this, isRelease);
                    op.gameObject.SetActive(true);
                    op.OnClose += ClosePopup;
                    gameState.OpenedWindows = CheckEnable();
                }));
            }

        }

        public void ClosePopup(string name, bool isRelease = true)
        {
            if (popups.TryGetValue(name, out UIPopupWindow popup))
            {
                if (isRelease)
                {
                    popup.gameObject.SetActive(false);
                    popup.OnClose -= ClosePopup;
                    Addressables.ReleaseInstance(popup.gameObject);
                    popups.Remove(name);
                }
                else
                {
                    popup.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log($"No PopupWindow: {name}");
            }
            gameState.OpenedWindows = CheckEnable();
        }

        public void ShowObjectPopup(string name)
        {
            if (popupsObject.TryGetValue(name, out GameObject popup))
            {
                popup.SetActive(true);
            }
            else
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(name, popupContainer, op =>
                {
                    op.SetActive(true);
                    popupsObject[name] = op;
                }));
            }
        }

        public void CloseObjectPopup(string name, bool isRelease = true)
        {
            if (popupsObject.TryGetValue(name, out GameObject popup))
            {
                if (isRelease)
                {
                    popup.gameObject.SetActive(false);
                    Addressables.ReleaseInstance(popup);
                    popupsObject.Remove(name);
                    
                }
                else
                {
                    popup.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError($"No PopupWindow: {name}");
            }
        }

        public void CloseAllPopup(string name, bool isRelease = true)
        {
            foreach (var pop in popups)
            {
                ClosePopup(pop.Value.nameAsset, isRelease);
            }
        }

        private bool CheckEnable()
        {
            List<UIPopupWindow> enablePopup = new();

            foreach (var popup in popups)
            {
                if (popup.Value.gameObject.activeSelf)
                    enablePopup.Add(popup.Value);
            }

            return enablePopup.Count > 0;
        }

    }
}