using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Project
{
    public class LoaderAsset 
    {
        public static IEnumerator Load<T>(AssetReference assetReference, Action<T> callback = null)
        {
            AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();

            yield return handle;

            var result = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(result);
            }
        }

        public static IEnumerator Load<T>(string name, Action<T> callback = null)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);

            yield return handle;

            var result = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(result);
            }
            yield return new WaitForSeconds(2);
            Addressables.Release(handle);
        }

        public static IEnumerator LoadList<T>(string assetName, Action<T> callback = null)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(assetName, op =>
            {
                if (op != null)
                    callback.Invoke(op);
            });
            yield return new WaitForSeconds(2);
            //Addressables.Release(handle);
        }

        public static IEnumerator InstantiateAsset(AssetReference reference, Transform parent = null, Action<GameObject> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = reference.InstantiateAsync(parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                callback?.Invoke(asset);
            else
                throw new NullReferenceException($"Failed to load Asset: {reference}");
        }

        public static IEnumerator InstantiateAsset(string assetName, Transform parent = null, Action<GameObject> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                callback?.Invoke(asset);
            else
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
        }

        public static IEnumerator InstantiateAsset<T>(AssetReference reference, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(reference, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!asset.TryGetComponent(out T loadinged))
                    throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                callback?.Invoke(loadinged);
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {reference}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(AssetReference reference, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(reference, pos, rot, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {

                if (!asset.TryGetComponent(out T loadinged))
                    throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                callback?.Invoke(loadinged);
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {reference}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(string assetName, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!asset.TryGetComponent(out T loadinged))
                    throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                callback?.Invoke(loadinged);
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(string assetName, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, pos, rot, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!asset.TryGetComponent(out T loadinged))
                    throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                callback?.Invoke(loadinged);
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
            }
        }

        public static void ReleaseInstance(GameObject go)
        {
            go.SetActive(false);
            Addressables.ReleaseInstance(go);
        }
    }
}