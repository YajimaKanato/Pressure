using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [SerializeField] Text _outputText;

    string _input;
    List<char> _inputList = new List<char>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _outputText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            InputKey();
        }
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && _inputList.Count > 0)
        {
            _inputList.RemoveAt(_inputList.Count - 1);
            _input = "";
            foreach (char c in _inputList)
            {
                _input += c;
            }
            _outputText.text = _input;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(_input);
            _input = "";
            _outputText.text = _input;
            _inputList.Clear();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {

        }
        else
        {
            foreach (var key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown((KeyCode)key))
                {
                    //_input += ((KeyCode)key).ToString();
                    _inputList.Add((char)((KeyCode)key));
                    _input = "";
                    foreach (char c in _inputList)
                    {
                        _input += c;
                    }
                    _outputText.text = _input;
                    break;
                }
            }
        }
    }

    //タイピングのすべてを調べる
}