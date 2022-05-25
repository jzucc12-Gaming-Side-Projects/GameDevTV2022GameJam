using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region //UI components
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timeText = null;
    private TextMeshProUGUI[] elements => GetComponentsInChildren<TextMeshProUGUI>();
    private Button button = null;
    #endregion

    #region //Color
    [Header("Color")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color hoverColor = Color.yellow;
    #endregion

    #region //Level info
    [Header("Level Info")]
    [SerializeField] private int timeValue = 0;
    [SerializeField] int levelNumber = -1;
    #endregion


    #region //Monobehaviour
    private void OnValidate()
    {
        timeText.text = $"{timeValue}s";
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        timeText.text = $"{timeValue}s";
        if(levelNumber < 2) return;
        if(PlayerPrefs.GetInt($"Level {levelNumber - 1}", 0) == 1) return;
        button.interactable = false;
        SetColor(inactiveColor);
    }
    #endregion

    #region //Pointer methods
    public void OnClick()
    {
        Timer.SetMaxTime(timeValue);
        LevelManager.currentLevel = levelNumber;
        SceneManager.LoadScene($"Level");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!button.interactable) return;
        SetColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        SetColor(defaultColor);
    }

    private void SetColor(Color color)
    {
        foreach (var element in elements)
            element.color = color;
    }
    #endregion
}