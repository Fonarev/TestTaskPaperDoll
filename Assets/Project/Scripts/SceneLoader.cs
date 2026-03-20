using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Project
{
    public class SceneLoader 
    {
        private const float MINIMUM_LOADING_TIME = 2.0f;

        public static event Action OnLoadingFinished;
        public static event LoadingCallback OnLoading;
        public delegate void LoadingCallback(float state, string message);
        
        private static int taskIndex;
        private static string loadingMessage;
        private static AsyncOperation loadingOperation;
        private static readonly List<ILoadingData> loadingTasks = new();
       
        private static float Progress => taskIndex / (loadingTasks.Count + 1);

        public static void LoadScene(Action sceneLoadedCallback = null)
        {
            SetLoadingMessage("Loading...");
            CoroutineHandler.StartRoutine(LoadSceneStartup(sceneLoadedCallback));
        }

        public static void SimpleLoadScene(Action sceneLoadedCallback = null)
        {
            CoroutineHandler.StartRoutine(SimpleLoadCoroutine(sceneLoadedCallback));
        }

        public static void AddTask(ILoadingData loadingData)
        {
            loadingTasks.Add(loadingData);
        }

        private static IEnumerator LoadSceneStartup(Action sceneLoadedCallback = null)
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;

            yield return  LoadingTasksProgress(OnLoading);

            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCount < sceneIndex)
                Debug.LogError("[Loading]: First scene is missing!");

            float minimumFinishTime = realtimeSinceStartup + MINIMUM_LOADING_TIME;

            loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingOperation.allowSceneActivation = false;

            while (!loadingOperation.isDone || realtimeSinceStartup < minimumFinishTime)
            {
                yield return null;

                realtimeSinceStartup = Time.realtimeSinceStartup;

                OnLoading?.Invoke(1.0f, "LoadingScene...");

                if (loadingOperation.progress >= 0.9f)
                {
                    loadingOperation.allowSceneActivation = true;
                }
            }

            yield return LoadingTasksProgress();

            OnLoading?.Invoke(1.0f, "Done");

            yield return null;

            sceneLoadedCallback?.Invoke();
            OnLoadingFinished?.Invoke();
        }

        private static IEnumerator SimpleLoadCoroutine(Action sceneLoadedCallback = null)
        {
            yield return LoadingTasksProgress();

            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCount < sceneIndex)
                Debug.LogError("[Loading]: First scene is missing!");


            loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingOperation.allowSceneActivation = false;

            while (!loadingOperation.isDone)
            {
                yield return null;

                if (loadingOperation.progress >= 0.9f)
                {
                    loadingOperation.allowSceneActivation = true;
                }
            }

            sceneLoadedCallback?.Invoke();
            OnLoadingFinished?.Invoke();
        }

        private static IEnumerator LoadingTasksProgress(LoadingCallback Callback = null)
        {
            taskIndex = 0;

            while (taskIndex < loadingTasks.Count)
            {
                if (loadingTasks[taskIndex].IsLoadedData)
                {
                    taskIndex++;
                    Callback?.Invoke(Progress, loadingMessage);
                }

                yield return null;
            }

            loadingTasks.Clear();
        }

        public static void SetLoadingMessage(string message)
        {
            loadingMessage = message;

            float progress = 0.0f;
            if (loadingOperation != null)
                progress = loadingOperation.progress;

            OnLoading?.Invoke(progress, message);
        }
    }
}