using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private AsyncOperation async = null;
    public void LoadScene(int pIndex)
    {
        SceneManager.LoadScene(pIndex);
    }
    public void LoadSceneAsync(int pIndex, float pWaitTime)
    {
        StartCoroutine(CoroutineLoadSceneAsync(pIndex, pWaitTime));
    }
    private IEnumerator CoroutineLoadSceneAsync(int pIndex, float pWaitTime)
    {
        async = SceneManager.LoadSceneAsync(pIndex);
        async.allowSceneActivation = false;


        yield return new WaitForSeconds(pWaitTime);
        Camera.main.transform.SetParent(gameObject.transform);
        async.allowSceneActivation = true;

        // COMMENT if the async load is not complete but the start button was pressed, show the loading screen and loading progress bar. When its complete return.
        
    }
}
