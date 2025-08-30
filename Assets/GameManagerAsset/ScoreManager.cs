using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアを司るスクリプト
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;

    TypingManager _typingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typingManager = FindFirstObjectByType<TypingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score : " + _typingManager.Score.ToString();
    }
}
