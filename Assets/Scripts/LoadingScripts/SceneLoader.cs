using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScripts
{
    public class SceneLoader : MonoBehaviour
    {
        public Slider slider;
        
        
        // TODO: Ask maybe going to a lightweight loading screen not asycly scene that loads asyncly the scene we want to load?
        
        public void LoadScene(int sceneId)
        {
            StartCoroutine(LoadSceneAsync(sceneId));
        }
    
        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            
            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

                slider.value = progressValue;

                yield return null;
            }

        }
    }
}
