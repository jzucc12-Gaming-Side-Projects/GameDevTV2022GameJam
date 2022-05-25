using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoryText = null;
    [SerializeField] private AudioClip beatGameClip = null;
    [SerializeField] private AudioSource victorySource = null;
    [SerializeField] private string beatGameText = "";


    private void Awake()
    {
        if(LevelManager.currentLevel != 5) return;
        victoryText.text = beatGameText;
        victorySource.clip = beatGameClip;
        victorySource.Play();
    }
}
