using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [SerializeField] Text _outputTextJ;
    [SerializeField] Text _outputTextE;

    WordBaseData _wordBaseData;
    /// <summary>入力された文字列を分割したもの</summary>
    List<string> _inputList = new List<string>();
    /// <summary>答えの文字列を分割したもののセットリスト</summary>
    List<(string japanese, string english, bool isTyping)> _answerList = new List<(string, string, bool)>();
    /// <summary>現在入力を待っている文字列</summary>
    Queue<char> _currentWaitTyping = new Queue<char>();
    /// <summary>次の入力の候補となる文字</summary>
    List<char> _nextInput = new List<char>();
    /// <summary>入力待ちの文字に対するローマ字の入力候補</summary>
    List<string> _answerCandidate = new List<string>();


    /// <summary>入力された文字列</summary>
    string _input;
    /// <summary>答えとなる文字列（日本語）</summary>
    string _answerJ = "こんにちは";
    /// <summary>答えとなる文字列（ローマ字）</summary>
    string _answerE;
    /// <summary>現在までに入力完了した文字列</summary>
    string _currentInput;
    /// <summary>入力を待っている日本語がある_answerListのインデックス</summary>
    int _typingIndex = 0;
    /// <summary>次に参照するインデックスとの差</summary>
    int _nextIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _wordBaseData = new WordBaseData();
        _outputTextE.text = "";
        AnswerSet();
    }

    // Update is called once per frame
    void Update()
    {
        //何かしらの入力を受け付ける
        if (Input.anyKeyDown)
        {
            InputKeyNotBack();
        }
    }

    /// <summary>
    /// 答えを設定する関数
    /// </summary>
    void AnswerSet()
    {
        //答えを更新
        int rand = UnityEngine.Random.Range(0, TypingList.Typing.Count);
        _answerJ = TypingList.Typing[rand];

        //答えの文字列分割リストを更新
        _answerList?.Clear();

        for (int i = 0; i < _answerJ.Length; i++)
        {
            switch (_answerJ[i].ToString())
            {
                case "っ":
                    _answerList.Add((_answerJ[i].ToString() + _answerJ[i + 1].ToString(), _wordBaseData.WordBaseJtoE[_answerJ[i].ToString() + _answerJ[i + 1].ToString()][0], false));
                    i++;
                    break;
                case "ゃ" or "ゅ" or "ょ" or "ぁ" or "ぃ" or "ぅ" or "ぇ" or "ぉ":
                    _answerList[_answerList.Count - 1] = (_answerList[_answerList.Count - 1].japanese + _answerJ[i].ToString(), _wordBaseData.WordBaseJtoE[_answerList[_answerList.Count - 1].japanese + _answerJ[i].ToString()][0], false);
                    break;
                default:
                    _answerList.Add((_answerJ[i].ToString(), _wordBaseData.WordBaseJtoE[_answerJ[i].ToString()][0], false));
                    break;
            }
        }

        /*
        foreach (var s in _answerJ)
        {
            _answerList.Add((s.ToString(), _wordBaseData.WordBaseJtoE[s.ToString()][0], false));
        }*/

        //答えに対応する文字列（ローマ字）を更新
        _answerE = "";
        foreach (var e in _answerList)
        {
            _answerE += e.english;
        }

        //答えのテキストを更新
        _outputTextJ.text = _answerJ;
        _outputTextE.text = _answerE;

        //入力待ちのインデックスを初期化
        _typingIndex = 0;
    }

    /// <summary>
    /// BackSpaceに対応していないタイピングを検知する関数
    /// </summary>
    void InputKeyNotBack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //タイピング完了時のみ
            if (_answerE == _currentInput)
            {
                _currentInput = "";
                AnswerSet();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {

        }
        else
        {
            //タイピング未完了時のみ
            if (_answerE != _currentInput)
            {
                foreach (var key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown((KeyCode)key))
                    {
                        _inputList.Add(((KeyCode)key).ToString());
                        //_input = (char)((KeyCode)key);
                        CheckWord();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// BackSpaceに対応したタイピングを検知する関数
    /// </summary>
    void InputKeyBack()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && _inputList.Count > 0)
        {
            _inputList.RemoveAt(_inputList.Count - 1);
            //_input = ' ';
            foreach (var c in _inputList)
            {
                //_input += c;
            }
            //_outputTextE.text = _input;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_answerE == _currentInput)
            {
                _currentInput = "";
                AnswerSet();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {

        }
        else
        {
            if (_answerE != _currentInput)
            {
                foreach (var key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown((KeyCode)key))
                    {
                        _inputList.Add(((KeyCode)key).ToString());
                        //_input = (char)((KeyCode)key);
                        CheckWord();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 入力された文字についての処理を行う関数
    /// </summary>
    void CheckWord()
    {
        //小文字を含むかどうか
        if (_answerList[_typingIndex].japanese.Length >= 2)
        {
            _answerCandidate.Clear();

            //小文字のうち"っ"かそれ以外か
            if (_answerList[_typingIndex].japanese.Contains("っ"))
            {
                //"っ"を含んだ文字列に対するローマ字を入力しようしているかどうかを調べる
                foreach (var s in _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese])
                {
                    if (s.Contains(_input))
                    {
                        _answerCandidate.Add(s);
                    }
                }

                //"っ"を含んだ文字列に対するローマ字を入力しようとしていなかったら
                if (_answerCandidate.Count == 0)
                {
                    foreach (var s in _wordBaseData.WordBaseJtoE["っ"])
                    {
                        if (s.Contains(_input))
                        {
                            _answerCandidate.Add(s);
                        }
                    }

                    //"っ"の入力を受け付けられそうなら文字列を分割
                    if (_answerCandidate.Count > 0)
                    {
                        string substr1 = _answerList[_typingIndex].japanese[0].ToString();
                        string substr2 = _answerList[_typingIndex].japanese.Substring(1, _answerList[_typingIndex].japanese.Length - 1);

                        _answerList.Insert(_typingIndex + 1, (substr1, _answerCandidate[0], true));
                        _answerList.Insert(_typingIndex + 2, (substr2, _wordBaseData.WordBaseJtoE[substr2][0], false));
                        _answerList.RemoveAt(_typingIndex);
                    }
                }
            }
            else
            {
                //"っ"以外の小さい文字について調べる
                foreach (var s in _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese])
                {
                    if (s.Contains(_input))
                    {
                        _answerCandidate.Add(s);
                    }
                }

                //小さい文字を含んだ文字列に対するローマ字の候補に適したものを入力しようとしていなかったら
                if (_answerCandidate.Count == 0 && _input.Length >= 2)
                {
                    foreach (var s in _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese[0].ToString()])
                    {
                        if (s.Contains(_input))
                        {
                            _answerCandidate.Add(s);
                        }
                    }

                    //文字列を分割する
                    if (_answerCandidate.Count > 0)
                    {
                        string substr1 = _answerList[_typingIndex].japanese[0].ToString();
                        string substr2 = _answerList[_typingIndex].japanese.Substring(1, _answerList[_typingIndex].japanese.Length - 1);

                        _answerList.Insert(_typingIndex + 1, (substr1, _answerCandidate[0], true));
                        _answerList.Insert(_typingIndex + 2, (substr2, _wordBaseData.WordBaseJtoE[substr2][0], false));
                        _answerList.RemoveAt(_typingIndex);
                    }
                }
            }
        }
        else
        {
            foreach (var s in _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese])
            {
                if (s.Contains(_input))
                {
                    _answerCandidate.Add(s);
                }
            }
        }

        if (_answerCandidate.Count == 0)
        {
            //ローマ字の頭文字のタイピング失敗

        }
        else
        {
            //リストの（ディープ）コピー
            var candidateCopy = new List<string>(_answerCandidate);
            //ローマ字候補の更新
            _answerCandidate.Clear();
            foreach (var s in candidateCopy)
            {
                if (s.Contains(_input))
                {
                    _answerCandidate.Add(s);
                }
            }

            //入力候補に適した入力をしなかったら
            if (_answerCandidate.Count == 0)
            {
                //候補を元に戻す
                _answerCandidate = new List<string>(candidateCopy);
                //入力した文字を消す
                _input = _input.Substring(0, _input.Length - 1);
            }

            //表示する文字列の更新
        }

        /*
        //入力待ちの日本語を入力し始めているかどうか
        if (!_answerList[_typingIndex].isTyping)
        {
            if (_answerList[_typingIndex].japanese[0].ToString() == "っ")
            {
                _nextInput.Clear();
                _answerCandidate.Clear();
                foreach (var s in _wordBaseData.WordBaseJtoE["っ"])
                {
                    if (s[0].ToString() == _input)
                    {
                        _answerCandidate.Add(s);
                    }
                }

                //入力した文字に対して正常に候補を得られたら
                if (_answerCandidate.Count > 0)
                {
                    //"っ"を含む文字列を分割
                    _answerList.Insert(_typingIndex + 1, (_answerList[_typingIndex].japanese[0].ToString(), _answerCandidate[0], false));
                    _answerList.Insert(_typingIndex + 2, (_answerList[_typingIndex].japanese.Substring(1, _answerList[_typingIndex].japanese.Length - 1), _answerCandidate[0], false));
                    //もともとの文字列を削除
                    _answerList.RemoveAt(_typingIndex);
                }
            }
            else if (_answerList[_typingIndex].japanese.Contains("ゃ") ||
                _answerList[_typingIndex].japanese.Contains("ゅ") ||
                _answerList[_typingIndex].japanese.Contains("ょ") ||
                _answerList[_typingIndex].japanese.Contains("ぁ") ||
                _answerList[_typingIndex].japanese.Contains("ぃ") ||
                _answerList[_typingIndex].japanese.Contains("ぅ") ||
                _answerList[_typingIndex].japanese.Contains("ぇ") ||
                _answerList[_typingIndex].japanese.Contains("ぉ"))
            {
                _answerCandidate.Clear();
                string[] words = { "ゃ", "ゅ", "ょ", "ぁ", "ぃ", "ぅ", "ぇ", "ぉ" };
                foreach (string word in words)
                {
                    foreach (var s in _wordBaseData.WordBaseJtoE[word])
                    {
                        if (s[0].ToString() == _input)
                        {
                            _answerCandidate.Add(s);
                        }
                    }

                    if (_answerCandidate.Count > 0)
                    {
                        break;
                    }
                }

                //入力した文字に対して正常に候補を得られたら
                if (_answerCandidate.Count > 0)
                {
                    //"っ"以外の小さい文字を含む文字列を分割
                    _answerList.Insert(_typingIndex + 1, (_answerList[_typingIndex].japanese[_answerList[_typingIndex].japanese.Length - 1].ToString(), _answerCandidate[0], false));
                    _answerList.Insert(_typingIndex + 1, (_answerList[_typingIndex].japanese.Substring(0, _answerList[_typingIndex].japanese.Length - 1), _answerCandidate[0], false));
                    //もともとの文字列を削除
                    _answerList.RemoveAt(_typingIndex);
                }
            }
            else
            {
                //小さい文字を含む文字列以外の文字に対する候補を取得
                _answerCandidate.Clear();
                foreach (var s in _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese])
                {
                    if (s[0].ToString() == _input)
                    {
                        _answerCandidate.Add(s);
                    }
                }
            }
        }*/

        /*
        //入力待ちの日本語を入力し始めているかどうか
        if (!_answerList[_typingIndex].isTyping)
        {
            _nextIndex = 0;
            //"っ"を含んだ文字列の時の処理
            if (_answerList[_typingIndex].japanese.Contains("っ"))
            {
                //"っ"に対応する文字を入力したかどうかを判定
                var lxtu = _wordBaseData.WordBaseJtoE["っ"];
                foreach(var j in lxtu)
                {
                    if (_input == j[0])
                    {

                        break;
                    }
                }
            }

            //答えの入力待ち単語に対応するローマ字変換を取得
            var check = _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese];
            foreach (var c in check)
            {
                //同じ文字から始まるローマ字列の場合に変な挙動になる
                if (c[0] == _input)
                {
                    //答えに対応するローマ字を、入力されたローマ字から始まるものに変更する
                    _answerList[_typingIndex] = (_answerList[_typingIndex].japanese, c, true);
                    _currentWaitTyping.Clear();
                    foreach (var c2 in c)
                    {
                        _currentWaitTyping.Enqueue(c2);
                    }
                    _nextIndex++;
                    break;
                }
            }

            //答えのローマ字の文字列を更新する
            _answerE = "";
            foreach (var c in _answerList)
            {
                _answerE += c.english;
            }
        }

        if (_answerList[_typingIndex].isTyping)
        {
            //入力した文字の正誤判定
            if (_input == _currentWaitTyping.Peek())
            {
                _currentInput += _input;
                _currentWaitTyping.Dequeue();

                if (_currentWaitTyping.Count <= 0)
                {
                    _typingIndex += _nextIndex;
                }

                //答えに対応するローマ字の表示を更新
                _outputTextE.text = "";
                for (int i = 0; i < _answerE.Length; i++)
                {
                    if (i < _currentInput.Length)
                    {
                        if (_currentInput[i] == _answerE[i])
                        {
                            _outputTextE.text += $"<color=black>{_answerE[i]}</color>";
                        }
                        else
                        {
                            _outputTextE.text += $"<color=white>{_answerE[i]}</color>";
                        }
                    }
                    else
                    {
                        _outputTextE.text += $"<color=white>{_answerE[i]}</color>";
                    }
                }
            }
            else
            {
                //間違えた文字を打った時
            }
        }
        else
        {
            //間違えた文字を打った時

        }*/
    }

    /// <summary>
    /// ローマ字変換の対応表を持つクラス
    /// </summary>
    class WordBaseData
    {
        Dictionary<string, string> _wordBaseEtoJ;
        public Dictionary<string, string> WordBaseEtoJ { get { return _wordBaseEtoJ; } }

        Dictionary<string, List<string>> _wordBaseJtoE;
        public Dictionary<string, List<string>> WordBaseJtoE { get { return _wordBaseJtoE; } }

        public WordBaseData()
        {
            _wordBaseEtoJ = new Dictionary<string, string>();
            _wordBaseJtoE = new Dictionary<string, List<string>>();
            EnglishToJapanese();
            JapaneseToEnglish();
        }

        public void EnglishToJapanese()
        {
            //あ行
            _wordBaseEtoJ["a"] = "あ";
            _wordBaseEtoJ["i"] = "い";
            _wordBaseEtoJ["u"] = "う";
            _wordBaseEtoJ["e"] = "え";
            _wordBaseEtoJ["o"] = "お";
            _wordBaseEtoJ["yi"] = "い";
            _wordBaseEtoJ["yyi"] = "っい";
            _wordBaseEtoJ["ye"] = "いぇ";
            _wordBaseEtoJ["yye"] = "っいぇ";
            _wordBaseEtoJ["wi"] = "うぃ";
            _wordBaseEtoJ["wu"] = "う";
            _wordBaseEtoJ["we"] = "うぇ";
            _wordBaseEtoJ["wwi"] = "っうぃ";
            _wordBaseEtoJ["wwu"] = "っう";
            _wordBaseEtoJ["wwe"] = "っうぇ";
            _wordBaseEtoJ["wha"] = "うぁ";
            _wordBaseEtoJ["whi"] = "うぃ";
            _wordBaseEtoJ["whu"] = "う";
            _wordBaseEtoJ["whe"] = "うぇ";
            _wordBaseEtoJ["who"] = "うぉ";
            _wordBaseEtoJ["wwha"] = "っうぁ";
            _wordBaseEtoJ["wwhi"] = "っうぃ";
            _wordBaseEtoJ["wwhu"] = "っう";
            _wordBaseEtoJ["wwhe"] = "っうぇ";
            _wordBaseEtoJ["wwho"] = "っうぉ";


            //か行
            _wordBaseEtoJ["ka"] = "か";
            _wordBaseEtoJ["ki"] = "き";
            _wordBaseEtoJ["ku"] = "く";
            _wordBaseEtoJ["ke"] = "け";
            _wordBaseEtoJ["ko"] = "こ";
            _wordBaseEtoJ["kka"] = "っか";
            _wordBaseEtoJ["kki"] = "っき";
            _wordBaseEtoJ["kku"] = "っく";
            _wordBaseEtoJ["kke"] = "っけ";
            _wordBaseEtoJ["kko"] = "っこ";
            _wordBaseEtoJ["ca"] = "か";
            _wordBaseEtoJ["cu"] = "く";
            _wordBaseEtoJ["co"] = "こ";
            _wordBaseEtoJ["cca"] = "っか";
            _wordBaseEtoJ["ccu"] = "っく";
            _wordBaseEtoJ["cco"] = "っこ";
            _wordBaseEtoJ["kya"] = "きゃ";
            _wordBaseEtoJ["kyi"] = "きぃ";
            _wordBaseEtoJ["kyu"] = "きゅ";
            _wordBaseEtoJ["kye"] = "きぇ";
            _wordBaseEtoJ["kyo"] = "きょ";
            _wordBaseEtoJ["kkya"] = "っきゃ";
            _wordBaseEtoJ["kkyi"] = "っきぃ";
            _wordBaseEtoJ["kkyu"] = "っきゅ";
            _wordBaseEtoJ["kkye"] = "っきぇ";
            _wordBaseEtoJ["kkyo"] = "っきょ";
            _wordBaseEtoJ["qya"] = "くゃ";
            _wordBaseEtoJ["qyi"] = "くぃ";
            _wordBaseEtoJ["qyu"] = "くゅ";
            _wordBaseEtoJ["qye"] = "くぇ";
            _wordBaseEtoJ["qyo"] = "くょ";
            _wordBaseEtoJ["qqya"] = "っくゃ";
            _wordBaseEtoJ["qqyi"] = "っくぃ";
            _wordBaseEtoJ["qqyu"] = "っくゅ";
            _wordBaseEtoJ["qqye"] = "っくぇ";
            _wordBaseEtoJ["qqyo"] = "っくょ";
            _wordBaseEtoJ["qwa"] = "くぁ";
            _wordBaseEtoJ["qwi"] = "くぃ";
            _wordBaseEtoJ["qwu"] = "くぅ";
            _wordBaseEtoJ["qwe"] = "くぇ";
            _wordBaseEtoJ["qwo"] = "くぉ";
            _wordBaseEtoJ["qqwa"] = "っくぁ";
            _wordBaseEtoJ["qqwi"] = "っくぃ";
            _wordBaseEtoJ["qqwu"] = "っくぅ";
            _wordBaseEtoJ["qqwe"] = "っくぇ";
            _wordBaseEtoJ["qqwo"] = "っくぉ";
            _wordBaseEtoJ["qa"] = "くぁ";
            _wordBaseEtoJ["qi"] = "くぃ";
            _wordBaseEtoJ["qu"] = "く";
            _wordBaseEtoJ["qe"] = "くぇ";
            _wordBaseEtoJ["qo"] = "くぉ";
            _wordBaseEtoJ["qqa"] = "っくぁ";
            _wordBaseEtoJ["qqi"] = "っくぃ";
            _wordBaseEtoJ["qqu"] = "っく";
            _wordBaseEtoJ["qqe"] = "っくぇ";
            _wordBaseEtoJ["qqo"] = "っくぉ";
            _wordBaseEtoJ["xka"] = "ヵ";
            _wordBaseEtoJ["lka"] = "ヵ";
            _wordBaseEtoJ["xke"] = "ヶ";
            _wordBaseEtoJ["lke"] = "ヶ";
            _wordBaseEtoJ["xxka"] = "っヵ";
            _wordBaseEtoJ["llka"] = "っヵ";
            _wordBaseEtoJ["xxke"] = "っヶ";
            _wordBaseEtoJ["llke"] = "っヶ";

            //さ行
            _wordBaseEtoJ["sa"] = "さ";
            _wordBaseEtoJ["si"] = "し";
            _wordBaseEtoJ["su"] = "す";
            _wordBaseEtoJ["se"] = "せ";
            _wordBaseEtoJ["so"] = "そ";
            _wordBaseEtoJ["ssa"] = "っさ";
            _wordBaseEtoJ["ssi"] = "っし";
            _wordBaseEtoJ["ssu"] = "っす";
            _wordBaseEtoJ["sse"] = "っせ";
            _wordBaseEtoJ["sso"] = "っそ";
            _wordBaseEtoJ["sha"] = "しゃ";
            _wordBaseEtoJ["shi"] = "し";
            _wordBaseEtoJ["shu"] = "しゅ";
            _wordBaseEtoJ["she"] = "しぇ";
            _wordBaseEtoJ["sho"] = "しょ";
            _wordBaseEtoJ["ssha"] = "っしゃ";
            _wordBaseEtoJ["sshi"] = "っし";
            _wordBaseEtoJ["sshu"] = "っしゅ";
            _wordBaseEtoJ["sshe"] = "っしぇ";
            _wordBaseEtoJ["ssho"] = "っしょ";
            _wordBaseEtoJ["sya"] = "しゃ";
            _wordBaseEtoJ["syi"] = "し";
            _wordBaseEtoJ["syu"] = "しゅ";
            _wordBaseEtoJ["sye"] = "しぇ";
            _wordBaseEtoJ["syo"] = "しょ";
            _wordBaseEtoJ["ssya"] = "っしゃ";
            _wordBaseEtoJ["ssyi"] = "っし";
            _wordBaseEtoJ["ssyu"] = "っしゅ";
            _wordBaseEtoJ["ssye"] = "っしぇ";
            _wordBaseEtoJ["ssyo"] = "っしょ";
            _wordBaseEtoJ["ci"] = "し";
            _wordBaseEtoJ["ce"] = "せ";
            _wordBaseEtoJ["cci"] = "っし";
            _wordBaseEtoJ["cce"] = "っせ";

            //た行
            _wordBaseEtoJ["ta"] = "た";
            _wordBaseEtoJ["ti"] = "ち";
            _wordBaseEtoJ["tu"] = "つ";
            _wordBaseEtoJ["te"] = "て";
            _wordBaseEtoJ["to"] = "と";
            _wordBaseEtoJ["tta"] = "った";
            _wordBaseEtoJ["tti"] = "っち";
            _wordBaseEtoJ["ttu"] = "っつ";
            _wordBaseEtoJ["tte"] = "って";
            _wordBaseEtoJ["tto"] = "っと";
            _wordBaseEtoJ["tya"] = "ちゃ";
            _wordBaseEtoJ["tyi"] = "ちぃ";
            _wordBaseEtoJ["tyu"] = "ちゅ";
            _wordBaseEtoJ["tye"] = "ちぇ";
            _wordBaseEtoJ["tyo"] = "ちょ";
            _wordBaseEtoJ["ttya"] = "っちゃ";
            _wordBaseEtoJ["ttyi"] = "っちぃ";
            _wordBaseEtoJ["ttyu"] = "っちゅ";
            _wordBaseEtoJ["ttye"] = "っちぇ";
            _wordBaseEtoJ["ttyo"] = "っちょ";
            _wordBaseEtoJ["cha"] = "ちゃ";
            _wordBaseEtoJ["chi"] = "ち";
            _wordBaseEtoJ["chu"] = "ちゅ";
            _wordBaseEtoJ["che"] = "ちぇ";
            _wordBaseEtoJ["cho"] = "ちょ";
            _wordBaseEtoJ["ccha"] = "っちゃ";
            _wordBaseEtoJ["cchi"] = "っち";
            _wordBaseEtoJ["cchu"] = "っちゅ";
            _wordBaseEtoJ["cche"] = "っちぇ";
            _wordBaseEtoJ["ccho"] = "っちょ";
            _wordBaseEtoJ["cya"] = "ちゃ";
            _wordBaseEtoJ["cyi"] = "ちぃ";
            _wordBaseEtoJ["cyu"] = "ちゅ";
            _wordBaseEtoJ["cye"] = "ちぇ";
            _wordBaseEtoJ["cyo"] = "ちょ";
            _wordBaseEtoJ["ccya"] = "っちゃ";
            _wordBaseEtoJ["ccyi"] = "っちぃ";
            _wordBaseEtoJ["ccyu"] = "っちゅ";
            _wordBaseEtoJ["ccye"] = "っちぇ";
            _wordBaseEtoJ["ccyo"] = "っちょ";
            _wordBaseEtoJ["tsa"] = "つぁ";
            _wordBaseEtoJ["tsi"] = "つぃ";
            _wordBaseEtoJ["tsu"] = "つ";
            _wordBaseEtoJ["tse"] = "つぇ";
            _wordBaseEtoJ["tso"] = "つぉ";
            _wordBaseEtoJ["ttsa"] = "っつぁ";
            _wordBaseEtoJ["ttsi"] = "っつぃ";
            _wordBaseEtoJ["ttsu"] = "っつ";
            _wordBaseEtoJ["ttse"] = "っつぇ";
            _wordBaseEtoJ["ttso"] = "っつぉ";
            _wordBaseEtoJ["tha"] = "てゃ";
            _wordBaseEtoJ["thi"] = "てぃ";
            _wordBaseEtoJ["thu"] = "てゅ";
            _wordBaseEtoJ["the"] = "てぇ";
            _wordBaseEtoJ["tho"] = "てょ";
            _wordBaseEtoJ["ttha"] = "ってゃ";
            _wordBaseEtoJ["tthi"] = "ってぃ";
            _wordBaseEtoJ["tthu"] = "ってゅ";
            _wordBaseEtoJ["tthe"] = "ってぇ";
            _wordBaseEtoJ["ttho"] = "ってょ";
            _wordBaseEtoJ["twa"] = "とぁ";
            _wordBaseEtoJ["twi"] = "とぃ";
            _wordBaseEtoJ["twu"] = "とぅ";
            _wordBaseEtoJ["twe"] = "とぇ";
            _wordBaseEtoJ["two"] = "とぉ";
            _wordBaseEtoJ["ttwa"] = "っとぁ";
            _wordBaseEtoJ["ttwi"] = "っとぃ";
            _wordBaseEtoJ["ttwu"] = "っとぅ";
            _wordBaseEtoJ["ttwe"] = "っとぇ";
            _wordBaseEtoJ["ttwo"] = "っとぉ";

            //な行
            _wordBaseEtoJ["na"] = "な";
            _wordBaseEtoJ["ni"] = "に";
            _wordBaseEtoJ["nu"] = "ぬ";
            _wordBaseEtoJ["ne"] = "ね";
            _wordBaseEtoJ["no"] = "の";
            _wordBaseEtoJ["nna"] = "っな";
            _wordBaseEtoJ["nni"] = "っに";
            _wordBaseEtoJ["nnu"] = "っぬ";
            _wordBaseEtoJ["nne"] = "っね";
            _wordBaseEtoJ["nno"] = "っの";
            _wordBaseEtoJ["nya"] = "にゃ";
            _wordBaseEtoJ["nyi"] = "にぃ";
            _wordBaseEtoJ["nyu"] = "にゅ";
            _wordBaseEtoJ["nye"] = "にぇ";
            _wordBaseEtoJ["nyo"] = "にょ";

            //は行
            _wordBaseEtoJ["ha"] = "は";
            _wordBaseEtoJ["hi"] = "ひ";
            _wordBaseEtoJ["hu"] = "ふ";
            _wordBaseEtoJ["he"] = "へ";
            _wordBaseEtoJ["ho"] = "ほ";
            _wordBaseEtoJ["hha"] = "っは";
            _wordBaseEtoJ["hhi"] = "っひ";
            _wordBaseEtoJ["hhu"] = "っふ";
            _wordBaseEtoJ["hhe"] = "っへ";
            _wordBaseEtoJ["hho"] = "っほ";
            _wordBaseEtoJ["hya"] = "ひゃ";
            _wordBaseEtoJ["hyi"] = "ひぃ";
            _wordBaseEtoJ["hyu"] = "ひゅ";
            _wordBaseEtoJ["hye"] = "ひぇ";
            _wordBaseEtoJ["hyo"] = "ひょ";
            _wordBaseEtoJ["hhya"] = "っひゃ";
            _wordBaseEtoJ["hhyi"] = "っひぃ";
            _wordBaseEtoJ["hhyu"] = "っひゅ";
            _wordBaseEtoJ["hhye"] = "っひぇ";
            _wordBaseEtoJ["hhyo"] = "っひょ";
            _wordBaseEtoJ["fa"] = "ふぁ";
            _wordBaseEtoJ["fi"] = "ふぃ";
            _wordBaseEtoJ["fu"] = "ふ";
            _wordBaseEtoJ["fe"] = "ふぇ";
            _wordBaseEtoJ["fo"] = "ふぉ";
            _wordBaseEtoJ["ffa"] = "っふぁ";
            _wordBaseEtoJ["ffi"] = "っふぃ";
            _wordBaseEtoJ["ffu"] = "っふ";
            _wordBaseEtoJ["ffe"] = "っふぇ";
            _wordBaseEtoJ["ffo"] = "っふぉ";
            _wordBaseEtoJ["fwa"] = "ふぁ";
            _wordBaseEtoJ["fwi"] = "ふぃ";
            _wordBaseEtoJ["fwu"] = "ふぅ";
            _wordBaseEtoJ["fwe"] = "ふぇ";
            _wordBaseEtoJ["fwo"] = "ふぉ";
            _wordBaseEtoJ["ffwa"] = "っふぁ";
            _wordBaseEtoJ["ffwi"] = "っふぃ";
            _wordBaseEtoJ["ffwu"] = "っふぅ";
            _wordBaseEtoJ["ffwe"] = "っふぇ";
            _wordBaseEtoJ["ffwo"] = "っふぉ";
            _wordBaseEtoJ["fya"] = "ふゃ";
            _wordBaseEtoJ["fyi"] = "ふぃ";
            _wordBaseEtoJ["fyu"] = "ふゅ";
            _wordBaseEtoJ["fye"] = "ふぇ";
            _wordBaseEtoJ["fyo"] = "ふょ";
            _wordBaseEtoJ["ffya"] = "っふゃ";
            _wordBaseEtoJ["ffyi"] = "っふぃ";
            _wordBaseEtoJ["ffyu"] = "っふゅ";
            _wordBaseEtoJ["ffye"] = "っふぇ";
            _wordBaseEtoJ["ffyo"] = "っふょ";

            //ま行
            _wordBaseEtoJ["ma"] = "ま";
            _wordBaseEtoJ["mi"] = "み";
            _wordBaseEtoJ["mu"] = "む";
            _wordBaseEtoJ["me"] = "め";
            _wordBaseEtoJ["mo"] = "も";
            _wordBaseEtoJ["mma"] = "っま";
            _wordBaseEtoJ["mmi"] = "っみ";
            _wordBaseEtoJ["mmu"] = "っむ";
            _wordBaseEtoJ["mme"] = "っめ";
            _wordBaseEtoJ["mmo"] = "っも";
            _wordBaseEtoJ["mya"] = "みゃ";
            _wordBaseEtoJ["myi"] = "みぃ";
            _wordBaseEtoJ["myu"] = "みゅ";
            _wordBaseEtoJ["mye"] = "みぇ";
            _wordBaseEtoJ["myo"] = "みょ";
            _wordBaseEtoJ["mmya"] = "っみゃ";
            _wordBaseEtoJ["mmyi"] = "っみぃ";
            _wordBaseEtoJ["mmyu"] = "っみゅ";
            _wordBaseEtoJ["mmye"] = "っみぇ";
            _wordBaseEtoJ["mmyo"] = "っみょ";

            //や行
            _wordBaseEtoJ["ya"] = "や";
            _wordBaseEtoJ["yu"] = "ゆ";
            _wordBaseEtoJ["yo"] = "よ";
            _wordBaseEtoJ["yya"] = "っや";
            _wordBaseEtoJ["yyu"] = "っゆ";
            _wordBaseEtoJ["yyo"] = "っよ";

            //ら行
            _wordBaseEtoJ["ra"] = "ら";
            _wordBaseEtoJ["ri"] = "り";
            _wordBaseEtoJ["ru"] = "る";
            _wordBaseEtoJ["re"] = "れ";
            _wordBaseEtoJ["ro"] = "ろ";
            _wordBaseEtoJ["rra"] = "っら";
            _wordBaseEtoJ["rri"] = "っり";
            _wordBaseEtoJ["rru"] = "っる";
            _wordBaseEtoJ["rre"] = "っれ";
            _wordBaseEtoJ["rro"] = "っろ";
            _wordBaseEtoJ["rya"] = "りゃ";
            _wordBaseEtoJ["ryi"] = "りぃ";
            _wordBaseEtoJ["ryu"] = "りゅ";
            _wordBaseEtoJ["rye"] = "りぇ";
            _wordBaseEtoJ["ryo"] = "りょ";
            _wordBaseEtoJ["rrya"] = "っりゃ";
            _wordBaseEtoJ["rryi"] = "っりぃ";
            _wordBaseEtoJ["rryu"] = "っりゅ";
            _wordBaseEtoJ["rrye"] = "っりぇ";
            _wordBaseEtoJ["rryo"] = "っりょ";

            //わ行
            _wordBaseEtoJ["wa"] = "わ";
            _wordBaseEtoJ["wo"] = "を";
            _wordBaseEtoJ["wwa"] = "っわ";
            _wordBaseEtoJ["wwo"] = "っを";
            _wordBaseEtoJ["xwa"] = "ゎ";
            _wordBaseEtoJ["lwa"] = "ゎ";
            _wordBaseEtoJ["xxwa"] = "っゎ";
            _wordBaseEtoJ["llwa"] = "っゎ";
            _wordBaseEtoJ["nn"] = "ん";
            _wordBaseEtoJ["xn"] = "ん";
            _wordBaseEtoJ["n"] = "ん";

            //が行
            _wordBaseEtoJ["ga"] = "が";
            _wordBaseEtoJ["gi"] = "ぎ";
            _wordBaseEtoJ["gu"] = "ぐ";
            _wordBaseEtoJ["ge"] = "げ";
            _wordBaseEtoJ["go"] = "ご";
            _wordBaseEtoJ["gga"] = "っが";
            _wordBaseEtoJ["ggi"] = "っぎ";
            _wordBaseEtoJ["ggu"] = "っぐ";
            _wordBaseEtoJ["gge"] = "っげ";
            _wordBaseEtoJ["ggo"] = "っご";
            _wordBaseEtoJ["gya"] = "ぎゃ";
            _wordBaseEtoJ["gyi"] = "ぎぃ";
            _wordBaseEtoJ["gyu"] = "ぎゅ";
            _wordBaseEtoJ["gye"] = "ぎぇ";
            _wordBaseEtoJ["gyo"] = "ぎょ";
            _wordBaseEtoJ["ggya"] = "っぎゃ";
            _wordBaseEtoJ["ggyi"] = "っぎぃ";
            _wordBaseEtoJ["ggyu"] = "っぎゅ";
            _wordBaseEtoJ["ggye"] = "っぎぇ";
            _wordBaseEtoJ["ggyo"] = "っぎょ";
            _wordBaseEtoJ["gwa"] = "ぐぁ";
            _wordBaseEtoJ["gwi"] = "ぐぃ";
            _wordBaseEtoJ["gwu"] = "ぐぅ";
            _wordBaseEtoJ["gwe"] = "ぐぇ";
            _wordBaseEtoJ["gwo"] = "ぐぉ";
            _wordBaseEtoJ["ggwa"] = "っぐぁ";
            _wordBaseEtoJ["ggwi"] = "っぐぃ";
            _wordBaseEtoJ["ggwu"] = "っぐぅ";
            _wordBaseEtoJ["ggwe"] = "っぐぇ";
            _wordBaseEtoJ["ggwo"] = "っぐぉ";

            //ざ行
            _wordBaseEtoJ["za"] = "ざ";
            _wordBaseEtoJ["zi"] = "じ";
            _wordBaseEtoJ["zu"] = "ず";
            _wordBaseEtoJ["ze"] = "ぜ";
            _wordBaseEtoJ["zo"] = "ぞ";
            _wordBaseEtoJ["zza"] = "っざ";
            _wordBaseEtoJ["zzi"] = "っじ";
            _wordBaseEtoJ["zzu"] = "っず";
            _wordBaseEtoJ["zze"] = "っぜ";
            _wordBaseEtoJ["zzo"] = "っぞ";
            _wordBaseEtoJ["zya"] = "じゃ";
            _wordBaseEtoJ["zyi"] = "じぃ";
            _wordBaseEtoJ["zyu"] = "じゅ";
            _wordBaseEtoJ["zye"] = "じぇ";
            _wordBaseEtoJ["zyo"] = "じょ";
            _wordBaseEtoJ["zzya"] = "っじゃ";
            _wordBaseEtoJ["zzyi"] = "っじぃ";
            _wordBaseEtoJ["zzyu"] = "っじゅ";
            _wordBaseEtoJ["zzye"] = "っじぇ";
            _wordBaseEtoJ["zzyo"] = "っじょ";
            _wordBaseEtoJ["ja"] = "じゃ";
            _wordBaseEtoJ["ji"] = "じ";
            _wordBaseEtoJ["ju"] = "じゅ";
            _wordBaseEtoJ["je"] = "じぇ";
            _wordBaseEtoJ["jo"] = "じょ";
            _wordBaseEtoJ["jja"] = "っじゃ";
            _wordBaseEtoJ["jji"] = "っじ";
            _wordBaseEtoJ["jju"] = "っじゅ";
            _wordBaseEtoJ["jje"] = "っじぇ";
            _wordBaseEtoJ["jjo"] = "っじょ";
            _wordBaseEtoJ["jya"] = "じゃ";
            _wordBaseEtoJ["jyi"] = "じぃ";
            _wordBaseEtoJ["jyu"] = "じゅ";
            _wordBaseEtoJ["jye"] = "じぇ";
            _wordBaseEtoJ["jyo"] = "じょ";
            _wordBaseEtoJ["jjya"] = "っじゃ";
            _wordBaseEtoJ["jjyi"] = "っじぃ";
            _wordBaseEtoJ["jjyu"] = "っじゅ";
            _wordBaseEtoJ["jjye"] = "っじぇ";
            _wordBaseEtoJ["jjyo"] = "っじょ";

            //だ行
            _wordBaseEtoJ["da"] = "だ";
            _wordBaseEtoJ["di"] = "ぢ";
            _wordBaseEtoJ["du"] = "づ";
            _wordBaseEtoJ["de"] = "で";
            _wordBaseEtoJ["do"] = "ど";
            _wordBaseEtoJ["dda"] = "っだ";
            _wordBaseEtoJ["ddi"] = "っぢ";
            _wordBaseEtoJ["ddu"] = "っづ";
            _wordBaseEtoJ["dde"] = "っで";
            _wordBaseEtoJ["ddo"] = "っど";
            _wordBaseEtoJ["dya"] = "ぢゃ";
            _wordBaseEtoJ["dyi"] = "ぢぃ";
            _wordBaseEtoJ["dyu"] = "ぢゅ";
            _wordBaseEtoJ["dye"] = "ぢぇ";
            _wordBaseEtoJ["dyo"] = "ぢょ";
            _wordBaseEtoJ["ddya"] = "っぢゃ";
            _wordBaseEtoJ["ddyi"] = "っぢぃ";
            _wordBaseEtoJ["ddyu"] = "っぢゅ";
            _wordBaseEtoJ["ddye"] = "っぢぇ";
            _wordBaseEtoJ["ddyo"] = "っぢょ";
            _wordBaseEtoJ["dha"] = "でゃ";
            _wordBaseEtoJ["dhi"] = "でぃ";
            _wordBaseEtoJ["dhu"] = "でゅ";
            _wordBaseEtoJ["dhe"] = "でぇ";
            _wordBaseEtoJ["dho"] = "でょ";
            _wordBaseEtoJ["ddha"] = "っでゃ";
            _wordBaseEtoJ["ddhi"] = "っでぃ";
            _wordBaseEtoJ["ddhu"] = "っでゅ";
            _wordBaseEtoJ["ddhe"] = "っでぇ";
            _wordBaseEtoJ["ddho"] = "っでょ";
            _wordBaseEtoJ["dwa"] = "どぁ";
            _wordBaseEtoJ["dwi"] = "どぃ";
            _wordBaseEtoJ["dwu"] = "どぅ";
            _wordBaseEtoJ["dwe"] = "どぇ";
            _wordBaseEtoJ["dwo"] = "どぉ";
            _wordBaseEtoJ["ddwa"] = "っどぁ";
            _wordBaseEtoJ["ddwi"] = "っどぃ";
            _wordBaseEtoJ["ddwu"] = "っどぅ";
            _wordBaseEtoJ["ddwe"] = "っどぇ";
            _wordBaseEtoJ["ddwo"] = "っどぉ";

            //ば行
            _wordBaseEtoJ["ba"] = "ば";
            _wordBaseEtoJ["bi"] = "び";
            _wordBaseEtoJ["bu"] = "ぶ";
            _wordBaseEtoJ["be"] = "べ";
            _wordBaseEtoJ["bo"] = "ぼ";
            _wordBaseEtoJ["bba"] = "っば";
            _wordBaseEtoJ["bbi"] = "っび";
            _wordBaseEtoJ["bbu"] = "っぶ";
            _wordBaseEtoJ["bbe"] = "っべ";
            _wordBaseEtoJ["bbo"] = "っぼ";
            _wordBaseEtoJ["bya"] = "びゃ";
            _wordBaseEtoJ["byi"] = "びぃ";
            _wordBaseEtoJ["byu"] = "びゅ";
            _wordBaseEtoJ["bye"] = "びぇ";
            _wordBaseEtoJ["byo"] = "びょ";
            _wordBaseEtoJ["bbya"] = "っびゃ";
            _wordBaseEtoJ["bbyi"] = "っびぃ";
            _wordBaseEtoJ["bbyu"] = "っびゅ";
            _wordBaseEtoJ["bbye"] = "っびぇ";
            _wordBaseEtoJ["bbyo"] = "っびょ";
            _wordBaseEtoJ["va"] = "ヴぁ";
            _wordBaseEtoJ["vi"] = "ヴぃ";
            _wordBaseEtoJ["vu"] = "ヴ";
            _wordBaseEtoJ["ve"] = "ヴぇ";
            _wordBaseEtoJ["vo"] = "ヴぉ";
            _wordBaseEtoJ["vva"] = "っヴぁ";
            _wordBaseEtoJ["vvi"] = "っヴぃ";
            _wordBaseEtoJ["vvu"] = "っヴ";
            _wordBaseEtoJ["vve"] = "っヴぇ";
            _wordBaseEtoJ["vvo"] = "っヴぉ";
            _wordBaseEtoJ["vya"] = "ヴゃ";
            _wordBaseEtoJ["vyi"] = "ヴぃ";
            _wordBaseEtoJ["vyu"] = "ヴゅ";
            _wordBaseEtoJ["vye"] = "ヴぇ";
            _wordBaseEtoJ["vyo"] = "ヴょ";
            _wordBaseEtoJ["vvya"] = "っヴゃ";
            _wordBaseEtoJ["vvyi"] = "っヴぃ";
            _wordBaseEtoJ["vvyu"] = "っヴゅ";
            _wordBaseEtoJ["vvye"] = "っヴぇ";
            _wordBaseEtoJ["vvyo"] = "っヴょ";

            //ぱ行
            _wordBaseEtoJ["pa"] = "ぱ";
            _wordBaseEtoJ["pi"] = "ぴ";
            _wordBaseEtoJ["pu"] = "ぷ";
            _wordBaseEtoJ["pe"] = "ぺ";
            _wordBaseEtoJ["po"] = "ぽ";
            _wordBaseEtoJ["ppa"] = "っぱ";
            _wordBaseEtoJ["ppi"] = "っぴ";
            _wordBaseEtoJ["ppu"] = "っぷ";
            _wordBaseEtoJ["ppe"] = "っぺ";
            _wordBaseEtoJ["ppo"] = "っぽ";
            _wordBaseEtoJ["pya"] = "ぴゃ";
            _wordBaseEtoJ["pyi"] = "ぴぃ";
            _wordBaseEtoJ["pyu"] = "ぴゅ";
            _wordBaseEtoJ["pye"] = "ぴぇ";
            _wordBaseEtoJ["pyo"] = "ぴょ";
            _wordBaseEtoJ["ppya"] = "っぴゃ";
            _wordBaseEtoJ["ppyi"] = "っぴぃ";
            _wordBaseEtoJ["ppyu"] = "っぴゅ";
            _wordBaseEtoJ["ppye"] = "っぴぇ";
            _wordBaseEtoJ["ppyo"] = "っぴょ";

            //小さい文字
            _wordBaseEtoJ["la"] = "ぁ";
            _wordBaseEtoJ["li"] = "ぃ";
            _wordBaseEtoJ["lu"] = "ぅ";
            _wordBaseEtoJ["le"] = "ぇ";
            _wordBaseEtoJ["lo"] = "ぉ";
            _wordBaseEtoJ["xa"] = "ぁ";
            _wordBaseEtoJ["xi"] = "ぃ";
            _wordBaseEtoJ["xu"] = "ぅ";
            _wordBaseEtoJ["xe"] = "ぇ";
            _wordBaseEtoJ["xo"] = "ぉ";
            _wordBaseEtoJ["lla"] = "っぁ";
            _wordBaseEtoJ["lli"] = "っぃ";
            _wordBaseEtoJ["llu"] = "っぅ";
            _wordBaseEtoJ["lle"] = "っぇ";
            _wordBaseEtoJ["llo"] = "っぉ";
            _wordBaseEtoJ["xxa"] = "っぁ";
            _wordBaseEtoJ["xxi"] = "っぃ";
            _wordBaseEtoJ["xxu"] = "っぅ";
            _wordBaseEtoJ["xxe"] = "っぇ";
            _wordBaseEtoJ["xxo"] = "っぉ";
            _wordBaseEtoJ["lya"] = "ゃ";
            _wordBaseEtoJ["lyu"] = "ゅ";
            _wordBaseEtoJ["lyo"] = "ょ";
            _wordBaseEtoJ["xya"] = "ゃ";
            _wordBaseEtoJ["xyu"] = "ゅ";
            _wordBaseEtoJ["xyo"] = "ょ";
            _wordBaseEtoJ["llya"] = "っゃ";
            _wordBaseEtoJ["llyu"] = "っゅ";
            _wordBaseEtoJ["llyo"] = "っょ";
            _wordBaseEtoJ["xxya"] = "っゃ";
            _wordBaseEtoJ["xxyu"] = "っゅ";
            _wordBaseEtoJ["xxyo"] = "っょ";
            _wordBaseEtoJ["ltu"] = "っ";
            _wordBaseEtoJ["xtu"] = "っ";
            _wordBaseEtoJ["ltsu"] = "っ";
            _wordBaseEtoJ["xtsu"] = "っ";
            _wordBaseEtoJ["lltu"] = "っっ";
            _wordBaseEtoJ["xxtu"] = "っっ";
            _wordBaseEtoJ["lltsu"] = "っっ";
            _wordBaseEtoJ["xxtsu"] = "っっ";

            //伸ばし棒
            _wordBaseEtoJ["-"] = "ー";
        }

        public void JapaneseToEnglish()
        {
            foreach (var dic in _wordBaseEtoJ)
            {
                if (!_wordBaseJtoE.ContainsKey(dic.Value))
                {
                    _wordBaseJtoE[dic.Value] = new List<string>();
                }
                _wordBaseJtoE[dic.Value].Add(dic.Key);
            }
        }
    }
}