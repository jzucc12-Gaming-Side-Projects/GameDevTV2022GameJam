using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private PlayerControls controls = null;

    #region //UI Components
    [SerializeField] private GameObject gameOverUI = null;
    [SerializeField] private GameObject victoryUI = null;
    [SerializeField] private GameObject pauseUI = null;
    [SerializeField] private Transform resetBar = null;
    #endregion

    #region //End game variables
    [SerializeField] private float gameOverDelayTime = 0.5f;
    private AudioSource terribleBackgroundMusic = null;
    #endregion

    #region //Level State
    private Timer timer = null;
    public static int currentLevel = -1;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        controls = new PlayerControls();
        timer = FindObjectOfType<Timer>();
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
        terribleBackgroundMusic = GetComponent<AudioSource>();
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        CharacterCollider.OnDeath += OnDeath;
        Timer.OnVictory += OnVictory;
        controls.Enable();
        controls.Player.Pause.started += OnPause;
    }

    private void OnDisable()
    {
        CharacterCollider.OnDeath -= OnDeath;
        Timer.OnVictory -= OnVictory;
        controls.Disable();
        controls.Player.Pause.started -= OnPause;
    }
    #endregion

    #region //Game over
    private void OnDeath()
    {
        controls.Disable();
        Time.timeScale = 0;
        terribleBackgroundMusic.Stop();
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(gameOverDelayTime);
        gameOverUI.SetActive(true);
        resetBar.localScale = new Vector3(timer.GetPercentageLeft(), 1, 0);
    }
    #endregion

    #region //Victory
    private void OnVictory()
    {
        controls.Disable();
        Time.timeScale = 0;
        victoryUI.SetActive(true);
        PlayerPrefs.SetInt($"Level {currentLevel}", 1);
    }

    #endregion

    #region //Pause
    private void OnPause(InputAction.CallbackContext context)
    {
        bool isPaused = Time.timeScale == 0;
        Time.timeScale = isPaused ? 1 : 0;
        pauseUI.SetActive(!isPaused);
    }
    #endregion
}
