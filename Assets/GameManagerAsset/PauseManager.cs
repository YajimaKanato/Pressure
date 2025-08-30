using UnityEngine;

/// <summary>
/// ポーズを司るクラス
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField, Tooltip("ポーズするときにアクティブにするオブジェクト")] GameObject _pauseObject;

    /// <summary>ポーズをするときに呼び出すオブジェクトを保存する配列</summary>
    ObjectBase[] _pauseObjects;
    /// <summary>ポーズかどうかのフラグ</summary>
    bool _isPause = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pauseObject.SetActive(false);
        _pauseObjects = FindObjectsByType<ObjectBase>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //オブジェクトにポーズ情報を知らせる
            foreach (var obj in _pauseObjects)
            {
                if (!_isPause)
                {
                    obj.Pause();
                }
                else
                {
                    obj.Resume();
                }
            }

            //ポーズ切り替え
            if (!_isPause)
            {
                Debug.Log("Pause");
            }
            else
            {
                Debug.Log("Resume");
            }
            _isPause = !_isPause;
            _pauseObject.SetActive(_isPause);
        }
    }
}
