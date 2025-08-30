using UnityEngine;

/// <summary>
/// �|�[�Y���i��N���X
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField, Tooltip("�|�[�Y����Ƃ��ɃA�N�e�B�u�ɂ���I�u�W�F�N�g")] GameObject _pauseObject;

    /// <summary>�|�[�Y������Ƃ��ɌĂяo���I�u�W�F�N�g��ۑ�����z��</summary>
    ObjectBase[] _pauseObjects;
    /// <summary>�|�[�Y���ǂ����̃t���O</summary>
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
            //�I�u�W�F�N�g�Ƀ|�[�Y����m�点��
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

            //�|�[�Y�؂�ւ�
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
