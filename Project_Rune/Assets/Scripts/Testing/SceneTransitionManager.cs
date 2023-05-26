using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    //===================================================================================================================================//

    public ScreenFade fadeScreen;

    //===================================================================================================================================//

    private void Start()
    {
        //PlayerHealth.onPlayerDeath += LoadGameOverScene;
    }

    //===================================================================================================================================//

    public void GoToScene(int SceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(SceneIndex));
    }

    //===================================================================================================================================//

    void LoadGameOverScene()
    {
        StartCoroutine(GoToSceneRoutine(1)); // index has to be checked if scenes in build settings are changed
    }

    //===================================================================================================================================//

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        SceneManager.LoadScene(sceneIndex);
    }

    //===================================================================================================================================//

}