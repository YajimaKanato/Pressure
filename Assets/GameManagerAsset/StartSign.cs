using UnityEngine;

/// <summary>
/// 開始の合図を司るスクリプト
/// </summary>
public class StartSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var objs = FindObjectsByType<ObjectBase>(FindObjectsSortMode.None);
        foreach(var obj in objs)
        {
            obj.GameStart();
        }
    }
}
