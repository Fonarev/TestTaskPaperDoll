using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.UI
{
    public class UIPopupWindow : MonoBehaviour
    {
        public event Action<string, bool> OnClose;

        public string nameAsset;
        [SerializeField] protected Button closeButton;

        protected GlobalCanvas localCanvas;

        public virtual void Init(GlobalCanvas localCanvas, bool isRelease)
        {
            this.localCanvas = localCanvas;

            closeButton?.onClick.AddListener(() =>
            {
                //AudioManager.Instance.PlayAudioEffect(Clips.CLICK_BUTTON); 
                OnClose.Invoke(nameAsset, isRelease);
            });
        }
    }
}