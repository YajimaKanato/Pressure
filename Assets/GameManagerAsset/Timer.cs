using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイマーを司るスクリプト
/// </summary>
public class Timer : ObjectBase
{
    [SerializeField] float _timeLimit = 60;
    [SerializeField] Text _timerText;

    float _delta = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _delta = _timeLimit;
        _timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStart && !_isEnd)
        {
            if (!_isPause)
            {
                if (_delta > 0)
                {
                    _delta -= Time.deltaTime;
                    _timerText.text = _delta.ToString("00.00");
                }
                else
                {
                    var objs = FindObjectsByType<ObjectBase>(FindObjectsSortMode.None);
                    foreach (var obj in objs)
                    {
                        obj.GameEnd();
                    }
                    SceneManager.LoadScene("Result");
                }
            }
        }
    }
}
