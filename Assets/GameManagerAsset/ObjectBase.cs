using UnityEngine;

/// <summary>
/// 継承させることで共通部分を作るためのクラス
/// いろんなインターフェースをあとから追加できる
/// </summary>
public class ObjectBase : MonoBehaviour, IPause,IGameFlow
{
    /// <summary>ポーズ判定のフラグ</summary>
    protected bool _isPause = false;
    /// <summary>スタート判定のフラグ</summary>
    protected bool _isStart = false;
    /// <summary>終了判定のフラグ</summary>
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

/// <summary>ポーズを司るインターフェース</summary>
interface IPause
{
    /// <summary>ポーズするときに呼ばれる関数</summary>
    public void Pause();

    /// <summary>ポーズから戻るときに呼ばれる関数</summary>
    public void Resume();
}

/// <summary>ゲームの進行を司るインターフェース</summary>
interface IGameFlow
{
    /// <summary>ゲームが開始されるときに呼び出される関数</summary>
    public void GameStart();

    /// <summary>ゲームが終了されるときに呼び出される関数</summary>
    public void GameEnd();
}
