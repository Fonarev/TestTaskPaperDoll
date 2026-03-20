
using UnityEngine;

namespace Assets.Project
{
    public class Entrypoint : MonoBehaviour
    {
        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            SceneLoader.LoadScene(() => Debug.Log("Loaded Scene")); 
        }

    }
}