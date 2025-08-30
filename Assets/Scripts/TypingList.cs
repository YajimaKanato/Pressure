using UnityEngine;
using System.Collections.Generic;
using TweetData;


[CreateAssetMenu(fileName = "TypingList", menuName = "ScriptableObjects/TypingList")]
public class TypingList : ScriptableObject
{
    [SerializeField] List<TypingData> _typingLists;
    public List<TypingData> TypingLists { get { return _typingLists; } }
}

[System.Serializable]
public class TypingData
{
    [SerializeField] string _tweetText;
    [SerializeField] string _teewtOutput;
    [SerializeField] TweetState _tweetState;
    [Header("Score")]
    [SerializeField] int _tweetScore;
    [SerializeField] int _banScore;

    public string TweetText { get { return _tweetText; } }
    public string TweetOutput { get { return _teewtOutput; } }
    public TweetState TweetState { get { return _tweetState; } }
    public int TweetScore { get { return _tweetScore; } }
    public int BanScore { get { return _banScore; } }
}

namespace TweetData
{
    public enum TweetState
    {
        Good,
        Bad
    }
}
