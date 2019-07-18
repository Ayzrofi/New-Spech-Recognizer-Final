using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearnGameController : MonoBehaviour {
    public AudioSource As;
    [Header("Transitions Animation")]
    public Animator anim;

    public void LoadLearnScene(string SceneName)
    {
        StartCoroutine(loadLearnScene(SceneName));
    }

    public IEnumerator loadLearnScene(string scene)
    {
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.6f);
        Debug.Log(scene);
        SceneManager.LoadScene(scene);
    }

	public void PlaySfx(AudioClip Clip)
    {
        if (As.isPlaying)
            As.Stop();

        As.PlayOneShot(Clip);
    }
}
