using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public class AddressableManager : MonoBehaviour
{
    [Header("Asset Keys/Labels")]
    [Tooltip("The specific addressable key for the environment asset.")]
    [SerializeField] private string environmentAssetKey;
    [Tooltip("The specific addressable key for the character prefab.")]
    [SerializeField] private string characterPrefabKey;
    [Tooltip("A label shared by multiple assets for group loading.")]
    [SerializeField] private string criticalAssetsLabel = "CriticalAssets";

    [Header("UI")]
    [SerializeField] private Button loadEnvironmentButton;
    [SerializeField] private Button loadCharactersButton;
    [SerializeField] private Button loadLabeledButton;
    [SerializeField] private Button unloadAllButton;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    private readonly List<AsyncOperationHandle> _loadedAssetHandles = new List<AsyncOperationHandle>();
    private readonly List<GameObject> _instantiatedObjects = new List<GameObject>();

    private void Start()
    {
        loadEnvironmentButton.onClick.AddListener(() => LoadAndSpawn(environmentAssetKey,"Environment Asset"));

        loadCharactersButton.onClick.AddListener(() => LoadAndSpawn(characterPrefabKey, "character"));
        
        loadLabeledButton.onClick.AddListener(() => LoadAssetsByLabel<GameObject>(criticalAssetsLabel));
        unloadAllButton.onClick.AddListener(UnloadAllAssets);

        if(progressBar) progressBar.gameObject.SetActive(false);
        if(progressText) progressText.gameObject.SetActive(false);
    }

    private void LoadAndSpawn(string key, string logDescription)
    {
        LoadSpecificAsset<GameObject>(key, asset =>
        {
            if (asset != null)
            {
                var instance = Instantiate(asset, Vector3.zero, Quaternion.identity);
                _instantiatedObjects.Add(instance);
                Debug.Log($"Successfully loaded and instantiated {logDescription}: {asset.name}");
            }
        });
    }

    #region Public API

    public void LoadSpecificAsset<T>(string key, Action<T> onAssetLoaded) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        // The lambda here is a "closure" that captures the 'handle' and 'onAssetLoaded' variables.
        StartCoroutine(LoadRoutine(handle, () => onAssetLoaded?.Invoke(handle.Result)));
    }

    public void LoadAssetsByLabel<T>(string label) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(label, null);
        // This lambda also captures the 'handle' variable.
        StartCoroutine(LoadRoutine(handle, () =>
        {
            foreach (var asset in handle.Result)
            {
                if (asset is GameObject prefab)
                {
                    var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    _instantiatedObjects.Add(instance);
                }
            }
        }));
    }

    public void UnloadAllAssets()
    {
        foreach (var obj in _instantiatedObjects)
        {
            Destroy(obj);
        }
        _instantiatedObjects.Clear();

        foreach (var handle in _loadedAssetHandles)
        {
            if(handle.IsValid()) Addressables.Release(handle);
        }
        _loadedAssetHandles.Clear();

        Debug.Log("All Addressable assets unloaded and instances destroyed.");
    }

    #endregion

    #region Core Loading Routine

    /// <summary>
    /// The single routine that handles all loading operations.
    /// The onSucceeded action is a closure that captures the specific handle.
    /// </summary>
    private IEnumerator LoadRoutine(AsyncOperationHandle handle, Action onSucceeded)
    {
        if(progressBar) progressBar.gameObject.SetActive(true);
        if(progressText) progressText.gameObject.SetActive(true);

        _loadedAssetHandles.Add(handle);

        while (!handle.IsDone)
        {
            if(progressBar) progressBar.value = handle.PercentComplete;
            if(progressText) progressText.text = $"Loading... {handle.PercentComplete * 100:F0}%";
            yield return null;
        }

        if(progressBar) progressBar.value = 1f;
        if(progressText) progressText.text = "Load Complete!";

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onSucceeded?.Invoke();
        }
        else
        {
            Debug.LogError($"Failed to load asset. Status: {handle.Status}, Exception: {handle.OperationException}");
        }
        
        yield return new WaitForSeconds(1.5f);
        if(progressBar) progressBar.gameObject.SetActive(false);
        if(progressText) progressText.gameObject.SetActive(false);
    }

    #endregion

    private void OnDestroy()
    {
        UnloadAllAssets();
    }
}
