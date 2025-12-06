using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : BaseScreen
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private string gameSceneName = "Game";

    public override void Show()
    {
        base.Show();
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync()
    {
        loadingSlider.value = 0f;
        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);

        // Don't let the scene activate until you allow it
        asyncLoad.allowSceneActivation = false;

        // Update the slider's value as the scene loads
        while (!asyncLoad.isDone)
        {
            // loading progress is between 0 and 0.9
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;

            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                //Wait a brief moment to show 100%
                loadingSlider.value = 1f;
                
                // Activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
