using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region //Variables
    [SerializeField] private float inspectorTime = 30;
    private static float maxTime = 0;
    private TextMeshProUGUI timerText = null;
    private float currentTime;
    public static event Action OnVictory;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        currentTime = GetMaxTime();
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        currentTime = Mathf.Max(0, currentTime - Time.deltaTime);
        timerText.text = currentTime.ToString("F2");

        if(currentTime == 0)
        {
            OnVictory?.Invoke();
            enabled = false;
        }
    }
    #endregion

    #region //Getters and setters
    public float GetPercentageLeft()
    {
        return currentTime / GetMaxTime();
    }

    public float GetTimeLeft()
    {
        return currentTime;
    }

    public static void SetMaxTime(float newMaxTime)
    {
        maxTime = newMaxTime;
    }

    private float GetMaxTime()
    {
        return maxTime == 0 ? inspectorTime : maxTime;
    }
    #endregion
}
