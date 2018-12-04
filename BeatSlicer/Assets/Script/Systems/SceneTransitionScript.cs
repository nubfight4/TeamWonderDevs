using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour {

    public Animator transitionAnimation;
    public string sceneName;
    public bool startTransit;
    public bool startTransitWhite;

    private void Update()
    {
        if (startTransit)
            StartCoroutine(SceneTransit());
        else if (startTransitWhite)
            StartCoroutine(SceneTransitWhite());
    }

    IEnumerator SceneTransit()
    {
        transitionAnimation.SetTrigger("transit");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator SceneTransitWhite()
    {
        transitionAnimation.SetTrigger("transitWhite");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
