using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Cubeman.Manager
{
    public sealed class SceneLoaderManager : MonoBehaviour
    {
        internal delegate void StartLoadScene();
        internal event StartLoadScene OnStartLoadScene;

        public void LoadActiveScene()
        {
            if(OnStartLoadScene != null) { OnStartLoadScene(); }

            var currentScene = SceneManager.GetActiveScene();
            StartCoroutine(LoadingSceneCoroutine(currentScene.buildIndex));
        }

        public void LoadScene(int sceneIndex)
        {
            if (OnStartLoadScene != null) { OnStartLoadScene(); }

            StartCoroutine(LoadingSceneCoroutine(sceneIndex));
        }

        public void LoadScene(string sceneName)
        {
            if (OnStartLoadScene != null) { OnStartLoadScene(); }

            StartCoroutine(LoadingSceneCoroutine(sceneName));
        }

        IEnumerator LoadingSceneCoroutine(int sceneIndex)
        {
            yield return new WaitForSeconds(0.6f);

            var sceneLoading = SceneManager.LoadSceneAsync(sceneIndex);

            while (!sceneLoading.isDone)
            {
                var progress = Mathf.Clamp01(sceneLoading.progress / 0.9f);
                Debug.Log("loading: " + progress);
                yield return null;
            }
        }

        IEnumerator LoadingSceneCoroutine(string sceneName)
        {
            yield return new WaitForSeconds(0.6f);

            var sceneLoading = SceneManager.LoadSceneAsync(sceneName);

            while (!sceneLoading.isDone)
            {
                var progress = Mathf.Clamp01(sceneLoading.progress / 0.9f);
                Debug.Log("loading: " + progress);
                yield return null;
            }
        }
    }
}