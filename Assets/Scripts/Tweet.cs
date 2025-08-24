using UnityEngine;
using TweetData;

public class Tweet : MonoBehaviour
{
    [SerializeField] float _score;
    [SerializeField] float _readSpeed;
    [SerializeField] float _mentalDamage;
    [SerializeField] TweetState _tweetState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

namespace TweetData
{
    public enum TweetState
    {
        Good,
        Bad
    }
}
