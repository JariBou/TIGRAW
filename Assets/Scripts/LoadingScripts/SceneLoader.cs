using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScripts
{
    public class SceneLoader : MonoBehaviour
    {
        public Canvas loadingScreen;
        public Image loadingBarFill;
        public static SceneLoader Instance;

    

        public void LoadScene(int sceneId)
        {
            StartCoroutine(LoadSceneAsync(sceneId));
        }
    
        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

            loadingScreen.enabled = true;

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

                loadingBarFill.fillAmount = progressValue;

                yield return null;
            }

            loadingScreen.enabled = false;
        }
    }
}
