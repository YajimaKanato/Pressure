using UnityEngine;

/// <summary>
/// �p�������邱�Ƃŋ��ʕ�������邽�߂̃N���X
/// �����ȃC���^�[�t�F�[�X�����Ƃ���ǉ��ł���
/// </summary>
public class ObjectBase : MonoBehaviour, IPause,IGameFlow
{
    /// <summary>�|�[�Y����̃t���O</summary>
    protected bool _isPause = false;
    /// <summary>�X�^�[�g����̃t���O</summary>
    protected bool _isStart = false;
    /// <summary>�I������̃t���O</summary>
    protected bool _isEnd = false;

    public void GameEnd()
    {
        _isEnd = true;
    }

    public void GameStart()
    {
        _isStart = true;
    }

    public virtual void Pause()
    {
        _isPause = true;
    }

    public virtual void Resume()
    {
        _isPause = false;
    }
}

/// <summary>�|�[�Y���i��C���^�[�t�F�[�X</summary>
interface IPause
{
    /// <summary>�|�[�Y����Ƃ��ɌĂ΂��֐�</summary>
    public void Pause();

    /// <summary>�|�[�Y����߂�Ƃ��ɌĂ΂��֐�</summary>
    public void Resume();
}

/// <summary>�Q�[���̐i�s���i��C���^�[�t�F�[�X</summary>
interface IGameFlow
{
    /// <summary>�Q�[�����J�n�����Ƃ��ɌĂяo�����֐�</summary>
    public void GameStart();

    /// <summary>�Q�[�����I�������Ƃ��ɌĂяo�����֐�</summary>
    public void GameEnd();
}
