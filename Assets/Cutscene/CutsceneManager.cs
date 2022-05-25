using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image blackBox = null;
    [SerializeField] private float fadeTime = 1;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Exit();
    }

    public void Exit()
    {
        Time.timeScale = 0;
        foreach(var source in FindObjectsOfType<AudioSource>())
            source.Stop();
            
        StartCoroutine(Fade());
    }
    
    private IEnumerator Fade()
    {
        float currentTime = 0;
        Color currentColor = blackBox.color;
        while(currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;
            currentColor.a = Mathf.Lerp(0, 1, currentTime / fadeTime);
            blackBox.color = currentColor;
            yield return null;
        }
        currentColor.a = 1;
        blackBox.color = currentColor;

        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
