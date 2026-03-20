using UnityEngine;

namespace Assets.Project
{
    public class AbstractDI : MonoBehaviour
    {
        [SerializeField] protected bool isMessage;

        [SerializeField] protected Installer[] installers;

        protected void Message(string message)
        {
           if(isMessage) Debug.Log(message);
        }
    }
}