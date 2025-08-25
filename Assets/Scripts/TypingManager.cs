using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [SerializeField] Text _outputTextJ;
    [SerializeField] Text _outputTextE;

    WordBaseData _wordBaseData;
    /// <summary>���͂��ꂽ������𕪊���������</summary>
    List<string> _inputList = new List<string>();
    /// <summary>�����̕�����𕪊��������̂̃Z�b�g���X�g</summary>
    List<(string japanese, string english, bool isTyping)> _answerList = new List<(string, string, bool)>();
    /// <summary>���ݓ��͂�҂��Ă��镶����</summary>
    Queue<char> _currentWaitTyping = new Queue<char>();

    /// <summary>���͂��ꂽ������</summary>
    char _input;
    /// <summary>�����ƂȂ镶����i���{��j</summary>
    string _answerJ = "����ɂ���";
    /// <summary>�����ƂȂ镶����i���[�}���j</summary>
    string _answerE;
    /// <summary>���݂܂łɓ��͊�������������</summary>
    string _currentInput;
    /// <summary>���͂�҂��Ă�����{�ꂪ����_answerList�̃C���f�b�N�X</summary>
    int _typingIndex = 0;
    /// <summary>���ɎQ�Ƃ���C���f�b�N�X�Ƃ̍�</summary>
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
        //��������̓��͂��󂯕t����
        if (Input.anyKeyDown)
        {
            InputKeyNotBack();
        }
    }

    /// <summary>
    /// ������ݒ肷��֐�
    /// </summary>
    void AnswerSet()
    {
        //�������X�V
        int rand = UnityEngine.Random.Range(0, TypingList.Typing.Count);
        _answerJ = TypingList.Typing[rand];

        //�����̕����񕪊����X�g���X�V
        _answerList?.Clear();

        for (int i = 0; i < _answerJ.Length; i++)
        {
            switch (_answerJ[i].ToString())
            {
                case "��":
                    _answerList.Add((_answerJ[i].ToString() + _answerJ[i + 1].ToString(), _wordBaseData.WordBaseJtoE[_answerJ[i].ToString() + _answerJ[i + 1].ToString()][0], false));
                    i++;
                    break;
                case "��" or "��" or "��" or "��" or "��" or "��" or "��" or "��":
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

        //�����ɑΉ����镶����i���[�}���j���X�V
        _answerE = "";
        foreach (var e in _answerList)
        {
            _answerE += e.english;
        }

        //�����̃e�L�X�g���X�V
        _outputTextJ.text = _answerJ;
        _outputTextE.text = _answerE;

        //���͑҂��̃C���f�b�N�X��������
        _typingIndex = 0;
    }

    /// <summary>
    /// BackSpace�ɑΉ����Ă��Ȃ��^�C�s���O�����m����֐�
    /// </summary>
    void InputKeyNotBack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //�^�C�s���O�������̂�
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
            //�^�C�s���O���������̂�
            if (_answerE != _currentInput)
            {
                foreach (var key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown((KeyCode)key))
                    {
                        //_inputList.Add(((KeyCode)key).ToString());
                        _input = (char)((KeyCode)key);
                        CheckWord();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// BackSpace�ɑΉ������^�C�s���O�����m����֐�
    /// </summary>
    void InputKeyBack()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && _inputList.Count > 0)
        {
            _inputList.RemoveAt(_inputList.Count - 1);
            _input = ' ';
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
                        //_inputList.Add(((KeyCode)key).ToString());
                        _input = (char)((KeyCode)key);
                        CheckWord();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���͂��ꂽ�����ɂ��Ă̏������s���֐�
    /// </summary>
    void CheckWord()
    {
        //���͑҂��̓��{�����͂��n�߂Ă��邩�ǂ���
        if (!_answerList[_typingIndex].isTyping)
        {
            foreach(var s in _answerList[_typingIndex].japanese)
            {

            }

            _nextIndex = 0;
            if (_answerList[_typingIndex].japanese.Contains("��"))
            {

            }

            //�����̓��͑҂��P��ɑΉ����郍�[�}���ϊ����擾
            var check = _wordBaseData.WordBaseJtoE[_answerList[_typingIndex].japanese];
            foreach (var c in check)
            {
                //������������n�܂郍�[�}����̏ꍇ�ɕςȋ����ɂȂ�
                if (c[0] == _input)
                {
                    //�����ɑΉ����郍�[�}�����A���͂��ꂽ���[�}������n�܂���̂ɕύX����
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

            //�����̃��[�}���̕�������X�V����
            _answerE = "";
            foreach (var c in _answerList)
            {
                _answerE += c.english;
            }
        }

        if (_answerList[_typingIndex].isTyping)
        {
            //���͂��������̐��딻��
            if (_input == _currentWaitTyping.Peek())
            {
                _currentInput += _input;
                _currentWaitTyping.Dequeue();

                if (_currentWaitTyping.Count <= 0)
                {
                    _typingIndex += _nextIndex;
                }

                //�����ɑΉ����郍�[�}���̕\�����X�V
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
                //�ԈႦ��������ł�����
            }
        }
        else
        {
            //�ԈႦ��������ł�����

        }
    }

    /// <summary>
    /// ���[�}���ϊ��̑Ή��\�����N���X
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
            //���s
            _wordBaseEtoJ["a"] = "��";
            _wordBaseEtoJ["i"] = "��";
            _wordBaseEtoJ["u"] = "��";
            _wordBaseEtoJ["e"] = "��";
            _wordBaseEtoJ["o"] = "��";
            _wordBaseEtoJ["yi"] = "��";
            _wordBaseEtoJ["yyi"] = "����";
            _wordBaseEtoJ["ye"] = "����";
            _wordBaseEtoJ["yye"] = "������";
            _wordBaseEtoJ["wi"] = "����";
            _wordBaseEtoJ["wu"] = "��";
            _wordBaseEtoJ["we"] = "����";
            _wordBaseEtoJ["wwi"] = "������";
            _wordBaseEtoJ["wwu"] = "����";
            _wordBaseEtoJ["wwe"] = "������";
            _wordBaseEtoJ["wha"] = "����";
            _wordBaseEtoJ["whi"] = "����";
            _wordBaseEtoJ["whu"] = "��";
            _wordBaseEtoJ["whe"] = "����";
            _wordBaseEtoJ["who"] = "����";
            _wordBaseEtoJ["wwha"] = "������";
            _wordBaseEtoJ["wwhi"] = "������";
            _wordBaseEtoJ["wwhu"] = "����";
            _wordBaseEtoJ["wwhe"] = "������";
            _wordBaseEtoJ["wwho"] = "������";


            //���s
            _wordBaseEtoJ["ka"] = "��";
            _wordBaseEtoJ["ki"] = "��";
            _wordBaseEtoJ["ku"] = "��";
            _wordBaseEtoJ["ke"] = "��";
            _wordBaseEtoJ["ko"] = "��";
            _wordBaseEtoJ["kka"] = "����";
            _wordBaseEtoJ["kki"] = "����";
            _wordBaseEtoJ["kku"] = "����";
            _wordBaseEtoJ["kke"] = "����";
            _wordBaseEtoJ["kko"] = "����";
            _wordBaseEtoJ["ca"] = "��";
            _wordBaseEtoJ["cu"] = "��";
            _wordBaseEtoJ["co"] = "��";
            _wordBaseEtoJ["cca"] = "����";
            _wordBaseEtoJ["ccu"] = "����";
            _wordBaseEtoJ["cco"] = "����";
            _wordBaseEtoJ["kya"] = "����";
            _wordBaseEtoJ["kyi"] = "����";
            _wordBaseEtoJ["kyu"] = "����";
            _wordBaseEtoJ["kye"] = "����";
            _wordBaseEtoJ["kyo"] = "����";
            _wordBaseEtoJ["kkya"] = "������";
            _wordBaseEtoJ["kkyi"] = "������";
            _wordBaseEtoJ["kkyu"] = "������";
            _wordBaseEtoJ["kkye"] = "������";
            _wordBaseEtoJ["kkyo"] = "������";
            _wordBaseEtoJ["qya"] = "����";
            _wordBaseEtoJ["qyi"] = "����";
            _wordBaseEtoJ["qyu"] = "����";
            _wordBaseEtoJ["qye"] = "����";
            _wordBaseEtoJ["qyo"] = "����";
            _wordBaseEtoJ["qqya"] = "������";
            _wordBaseEtoJ["qqyi"] = "������";
            _wordBaseEtoJ["qqyu"] = "������";
            _wordBaseEtoJ["qqye"] = "������";
            _wordBaseEtoJ["qqyo"] = "������";
            _wordBaseEtoJ["qwa"] = "����";
            _wordBaseEtoJ["qwi"] = "����";
            _wordBaseEtoJ["qwu"] = "����";
            _wordBaseEtoJ["qwe"] = "����";
            _wordBaseEtoJ["qwo"] = "����";
            _wordBaseEtoJ["qqwa"] = "������";
            _wordBaseEtoJ["qqwi"] = "������";
            _wordBaseEtoJ["qqwu"] = "������";
            _wordBaseEtoJ["qqwe"] = "������";
            _wordBaseEtoJ["qqwo"] = "������";
            _wordBaseEtoJ["qa"] = "����";
            _wordBaseEtoJ["qi"] = "����";
            _wordBaseEtoJ["qu"] = "��";
            _wordBaseEtoJ["qe"] = "����";
            _wordBaseEtoJ["qo"] = "����";
            _wordBaseEtoJ["qqa"] = "������";
            _wordBaseEtoJ["qqi"] = "������";
            _wordBaseEtoJ["qqu"] = "����";
            _wordBaseEtoJ["qqe"] = "������";
            _wordBaseEtoJ["qqo"] = "������";
            _wordBaseEtoJ["xka"] = "��";
            _wordBaseEtoJ["lka"] = "��";
            _wordBaseEtoJ["xke"] = "��";
            _wordBaseEtoJ["lke"] = "��";
            _wordBaseEtoJ["xxka"] = "����";
            _wordBaseEtoJ["llka"] = "����";
            _wordBaseEtoJ["xxke"] = "����";
            _wordBaseEtoJ["llke"] = "����";

            //���s
            _wordBaseEtoJ["sa"] = "��";
            _wordBaseEtoJ["si"] = "��";
            _wordBaseEtoJ["su"] = "��";
            _wordBaseEtoJ["se"] = "��";
            _wordBaseEtoJ["so"] = "��";
            _wordBaseEtoJ["ssa"] = "����";
            _wordBaseEtoJ["ssi"] = "����";
            _wordBaseEtoJ["ssu"] = "����";
            _wordBaseEtoJ["sse"] = "����";
            _wordBaseEtoJ["sso"] = "����";
            _wordBaseEtoJ["sha"] = "����";
            _wordBaseEtoJ["shi"] = "��";
            _wordBaseEtoJ["shu"] = "����";
            _wordBaseEtoJ["she"] = "����";
            _wordBaseEtoJ["sho"] = "����";
            _wordBaseEtoJ["ssha"] = "������";
            _wordBaseEtoJ["sshi"] = "����";
            _wordBaseEtoJ["sshu"] = "������";
            _wordBaseEtoJ["sshe"] = "������";
            _wordBaseEtoJ["ssho"] = "������";
            _wordBaseEtoJ["sya"] = "����";
            _wordBaseEtoJ["syi"] = "��";
            _wordBaseEtoJ["syu"] = "����";
            _wordBaseEtoJ["sye"] = "����";
            _wordBaseEtoJ["syo"] = "����";
            _wordBaseEtoJ["ssya"] = "������";
            _wordBaseEtoJ["ssyi"] = "����";
            _wordBaseEtoJ["ssyu"] = "������";
            _wordBaseEtoJ["ssye"] = "������";
            _wordBaseEtoJ["ssyo"] = "������";
            _wordBaseEtoJ["ci"] = "��";
            _wordBaseEtoJ["ce"] = "��";
            _wordBaseEtoJ["cci"] = "����";
            _wordBaseEtoJ["cce"] = "����";

            //���s
            _wordBaseEtoJ["ta"] = "��";
            _wordBaseEtoJ["ti"] = "��";
            _wordBaseEtoJ["tu"] = "��";
            _wordBaseEtoJ["te"] = "��";
            _wordBaseEtoJ["to"] = "��";
            _wordBaseEtoJ["tta"] = "����";
            _wordBaseEtoJ["tti"] = "����";
            _wordBaseEtoJ["ttu"] = "����";
            _wordBaseEtoJ["tte"] = "����";
            _wordBaseEtoJ["tto"] = "����";
            _wordBaseEtoJ["tya"] = "����";
            _wordBaseEtoJ["tyi"] = "����";
            _wordBaseEtoJ["tyu"] = "����";
            _wordBaseEtoJ["tye"] = "����";
            _wordBaseEtoJ["tyo"] = "����";
            _wordBaseEtoJ["ttya"] = "������";
            _wordBaseEtoJ["ttyi"] = "������";
            _wordBaseEtoJ["ttyu"] = "������";
            _wordBaseEtoJ["ttye"] = "������";
            _wordBaseEtoJ["ttyo"] = "������";
            _wordBaseEtoJ["cha"] = "����";
            _wordBaseEtoJ["chi"] = "��";
            _wordBaseEtoJ["chu"] = "����";
            _wordBaseEtoJ["che"] = "����";
            _wordBaseEtoJ["cho"] = "����";
            _wordBaseEtoJ["ccha"] = "������";
            _wordBaseEtoJ["cchi"] = "����";
            _wordBaseEtoJ["cchu"] = "������";
            _wordBaseEtoJ["cche"] = "������";
            _wordBaseEtoJ["ccho"] = "������";
            _wordBaseEtoJ["cya"] = "����";
            _wordBaseEtoJ["cyi"] = "����";
            _wordBaseEtoJ["cyu"] = "����";
            _wordBaseEtoJ["cye"] = "����";
            _wordBaseEtoJ["cyo"] = "����";
            _wordBaseEtoJ["ccya"] = "������";
            _wordBaseEtoJ["ccyi"] = "������";
            _wordBaseEtoJ["ccyu"] = "������";
            _wordBaseEtoJ["ccye"] = "������";
            _wordBaseEtoJ["ccyo"] = "������";
            _wordBaseEtoJ["tsa"] = "��";
            _wordBaseEtoJ["tsi"] = "��";
            _wordBaseEtoJ["tsu"] = "��";
            _wordBaseEtoJ["tse"] = "��";
            _wordBaseEtoJ["tso"] = "��";
            _wordBaseEtoJ["ttsa"] = "����";
            _wordBaseEtoJ["ttsi"] = "����";
            _wordBaseEtoJ["ttsu"] = "����";
            _wordBaseEtoJ["ttse"] = "����";
            _wordBaseEtoJ["ttso"] = "����";
            _wordBaseEtoJ["tha"] = "�Ă�";
            _wordBaseEtoJ["thi"] = "�Ă�";
            _wordBaseEtoJ["thu"] = "�Ă�";
            _wordBaseEtoJ["the"] = "�Ă�";
            _wordBaseEtoJ["tho"] = "�Ă�";
            _wordBaseEtoJ["ttha"] = "���Ă�";
            _wordBaseEtoJ["tthi"] = "���Ă�";
            _wordBaseEtoJ["tthu"] = "���Ă�";
            _wordBaseEtoJ["tthe"] = "���Ă�";
            _wordBaseEtoJ["ttho"] = "���Ă�";
            _wordBaseEtoJ["twa"] = "�Ƃ�";
            _wordBaseEtoJ["twi"] = "�Ƃ�";
            _wordBaseEtoJ["twu"] = "�Ƃ�";
            _wordBaseEtoJ["twe"] = "�Ƃ�";
            _wordBaseEtoJ["two"] = "�Ƃ�";
            _wordBaseEtoJ["ttwa"] = "���Ƃ�";
            _wordBaseEtoJ["ttwi"] = "���Ƃ�";
            _wordBaseEtoJ["ttwu"] = "���Ƃ�";
            _wordBaseEtoJ["ttwe"] = "���Ƃ�";
            _wordBaseEtoJ["ttwo"] = "���Ƃ�";

            //�ȍs
            _wordBaseEtoJ["na"] = "��";
            _wordBaseEtoJ["ni"] = "��";
            _wordBaseEtoJ["nu"] = "��";
            _wordBaseEtoJ["ne"] = "��";
            _wordBaseEtoJ["no"] = "��";
            _wordBaseEtoJ["nna"] = "����";
            _wordBaseEtoJ["nni"] = "����";
            _wordBaseEtoJ["nnu"] = "����";
            _wordBaseEtoJ["nne"] = "����";
            _wordBaseEtoJ["nno"] = "����";
            _wordBaseEtoJ["nya"] = "�ɂ�";
            _wordBaseEtoJ["nyi"] = "�ɂ�";
            _wordBaseEtoJ["nyu"] = "�ɂ�";
            _wordBaseEtoJ["nye"] = "�ɂ�";
            _wordBaseEtoJ["nyo"] = "�ɂ�";

            //�͍s
            _wordBaseEtoJ["ha"] = "��";
            _wordBaseEtoJ["hi"] = "��";
            _wordBaseEtoJ["hu"] = "��";
            _wordBaseEtoJ["he"] = "��";
            _wordBaseEtoJ["ho"] = "��";
            _wordBaseEtoJ["hha"] = "����";
            _wordBaseEtoJ["hhi"] = "����";
            _wordBaseEtoJ["hhu"] = "����";
            _wordBaseEtoJ["hhe"] = "����";
            _wordBaseEtoJ["hho"] = "����";
            _wordBaseEtoJ["hya"] = "�Ђ�";
            _wordBaseEtoJ["hyi"] = "�Ђ�";
            _wordBaseEtoJ["hyu"] = "�Ђ�";
            _wordBaseEtoJ["hye"] = "�Ђ�";
            _wordBaseEtoJ["hyo"] = "�Ђ�";
            _wordBaseEtoJ["hhya"] = "���Ђ�";
            _wordBaseEtoJ["hhyi"] = "���Ђ�";
            _wordBaseEtoJ["hhyu"] = "���Ђ�";
            _wordBaseEtoJ["hhye"] = "���Ђ�";
            _wordBaseEtoJ["hhyo"] = "���Ђ�";
            _wordBaseEtoJ["fa"] = "�ӂ�";
            _wordBaseEtoJ["fi"] = "�ӂ�";
            _wordBaseEtoJ["fu"] = "��";
            _wordBaseEtoJ["fe"] = "�ӂ�";
            _wordBaseEtoJ["fo"] = "�ӂ�";
            _wordBaseEtoJ["ffa"] = "���ӂ�";
            _wordBaseEtoJ["ffi"] = "���ӂ�";
            _wordBaseEtoJ["ffu"] = "����";
            _wordBaseEtoJ["ffe"] = "���ӂ�";
            _wordBaseEtoJ["ffo"] = "���ӂ�";
            _wordBaseEtoJ["fwa"] = "�ӂ�";
            _wordBaseEtoJ["fwi"] = "�ӂ�";
            _wordBaseEtoJ["fwu"] = "�ӂ�";
            _wordBaseEtoJ["fwe"] = "�ӂ�";
            _wordBaseEtoJ["fwo"] = "�ӂ�";
            _wordBaseEtoJ["ffwa"] = "���ӂ�";
            _wordBaseEtoJ["ffwi"] = "���ӂ�";
            _wordBaseEtoJ["ffwu"] = "���ӂ�";
            _wordBaseEtoJ["ffwe"] = "���ӂ�";
            _wordBaseEtoJ["ffwo"] = "���ӂ�";
            _wordBaseEtoJ["fya"] = "�ӂ�";
            _wordBaseEtoJ["fyi"] = "�ӂ�";
            _wordBaseEtoJ["fyu"] = "�ӂ�";
            _wordBaseEtoJ["fye"] = "�ӂ�";
            _wordBaseEtoJ["fyo"] = "�ӂ�";
            _wordBaseEtoJ["ffya"] = "���ӂ�";
            _wordBaseEtoJ["ffyi"] = "���ӂ�";
            _wordBaseEtoJ["ffyu"] = "���ӂ�";
            _wordBaseEtoJ["ffye"] = "���ӂ�";
            _wordBaseEtoJ["ffyo"] = "���ӂ�";

            //�܍s
            _wordBaseEtoJ["ma"] = "��";
            _wordBaseEtoJ["mi"] = "��";
            _wordBaseEtoJ["mu"] = "��";
            _wordBaseEtoJ["me"] = "��";
            _wordBaseEtoJ["mo"] = "��";
            _wordBaseEtoJ["mma"] = "����";
            _wordBaseEtoJ["mmi"] = "����";
            _wordBaseEtoJ["mmu"] = "����";
            _wordBaseEtoJ["mme"] = "����";
            _wordBaseEtoJ["mmo"] = "����";
            _wordBaseEtoJ["mya"] = "�݂�";
            _wordBaseEtoJ["myi"] = "�݂�";
            _wordBaseEtoJ["myu"] = "�݂�";
            _wordBaseEtoJ["mye"] = "�݂�";
            _wordBaseEtoJ["myo"] = "�݂�";
            _wordBaseEtoJ["mmya"] = "���݂�";
            _wordBaseEtoJ["mmyi"] = "���݂�";
            _wordBaseEtoJ["mmyu"] = "���݂�";
            _wordBaseEtoJ["mmye"] = "���݂�";
            _wordBaseEtoJ["mmyo"] = "���݂�";

            //��s
            _wordBaseEtoJ["ya"] = "��";
            _wordBaseEtoJ["yu"] = "��";
            _wordBaseEtoJ["yo"] = "��";
            _wordBaseEtoJ["yya"] = "����";
            _wordBaseEtoJ["yyu"] = "����";
            _wordBaseEtoJ["yyo"] = "����";

            //��s
            _wordBaseEtoJ["ra"] = "��";
            _wordBaseEtoJ["ri"] = "��";
            _wordBaseEtoJ["ru"] = "��";
            _wordBaseEtoJ["re"] = "��";
            _wordBaseEtoJ["ro"] = "��";
            _wordBaseEtoJ["rra"] = "����";
            _wordBaseEtoJ["rri"] = "����";
            _wordBaseEtoJ["rru"] = "����";
            _wordBaseEtoJ["rre"] = "����";
            _wordBaseEtoJ["rro"] = "����";
            _wordBaseEtoJ["rya"] = "���";
            _wordBaseEtoJ["ryi"] = "�股";
            _wordBaseEtoJ["ryu"] = "���";
            _wordBaseEtoJ["rye"] = "�肥";
            _wordBaseEtoJ["ryo"] = "���";
            _wordBaseEtoJ["rrya"] = "�����";
            _wordBaseEtoJ["rryi"] = "���股";
            _wordBaseEtoJ["rryu"] = "�����";
            _wordBaseEtoJ["rrye"] = "���肥";
            _wordBaseEtoJ["rryo"] = "�����";

            //��s
            _wordBaseEtoJ["wa"] = "��";
            _wordBaseEtoJ["wo"] = "��";
            _wordBaseEtoJ["wwa"] = "����";
            _wordBaseEtoJ["wwo"] = "����";
            _wordBaseEtoJ["xwa"] = "��";
            _wordBaseEtoJ["lwa"] = "��";
            _wordBaseEtoJ["xxwa"] = "����";
            _wordBaseEtoJ["llwa"] = "����";
            _wordBaseEtoJ["nn"] = "��";
            _wordBaseEtoJ["xn"] = "��";
            _wordBaseEtoJ["n"] = "��";

            //���s
            _wordBaseEtoJ["ga"] = "��";
            _wordBaseEtoJ["gi"] = "��";
            _wordBaseEtoJ["gu"] = "��";
            _wordBaseEtoJ["ge"] = "��";
            _wordBaseEtoJ["go"] = "��";
            _wordBaseEtoJ["gga"] = "����";
            _wordBaseEtoJ["ggi"] = "����";
            _wordBaseEtoJ["ggu"] = "����";
            _wordBaseEtoJ["gge"] = "����";
            _wordBaseEtoJ["ggo"] = "����";
            _wordBaseEtoJ["gya"] = "����";
            _wordBaseEtoJ["gyi"] = "����";
            _wordBaseEtoJ["gyu"] = "����";
            _wordBaseEtoJ["gye"] = "����";
            _wordBaseEtoJ["gyo"] = "����";
            _wordBaseEtoJ["ggya"] = "������";
            _wordBaseEtoJ["ggyi"] = "������";
            _wordBaseEtoJ["ggyu"] = "������";
            _wordBaseEtoJ["ggye"] = "������";
            _wordBaseEtoJ["ggyo"] = "������";
            _wordBaseEtoJ["gwa"] = "����";
            _wordBaseEtoJ["gwi"] = "����";
            _wordBaseEtoJ["gwu"] = "����";
            _wordBaseEtoJ["gwe"] = "����";
            _wordBaseEtoJ["gwo"] = "����";
            _wordBaseEtoJ["ggwa"] = "������";
            _wordBaseEtoJ["ggwi"] = "������";
            _wordBaseEtoJ["ggwu"] = "������";
            _wordBaseEtoJ["ggwe"] = "������";
            _wordBaseEtoJ["ggwo"] = "������";

            //���s
            _wordBaseEtoJ["za"] = "��";
            _wordBaseEtoJ["zi"] = "��";
            _wordBaseEtoJ["zu"] = "��";
            _wordBaseEtoJ["ze"] = "��";
            _wordBaseEtoJ["zo"] = "��";
            _wordBaseEtoJ["zza"] = "����";
            _wordBaseEtoJ["zzi"] = "����";
            _wordBaseEtoJ["zzu"] = "����";
            _wordBaseEtoJ["zze"] = "����";
            _wordBaseEtoJ["zzo"] = "����";
            _wordBaseEtoJ["zya"] = "����";
            _wordBaseEtoJ["zyi"] = "����";
            _wordBaseEtoJ["zyu"] = "����";
            _wordBaseEtoJ["zye"] = "����";
            _wordBaseEtoJ["zyo"] = "����";
            _wordBaseEtoJ["zzya"] = "������";
            _wordBaseEtoJ["zzyi"] = "������";
            _wordBaseEtoJ["zzyu"] = "������";
            _wordBaseEtoJ["zzye"] = "������";
            _wordBaseEtoJ["zzyo"] = "������";
            _wordBaseEtoJ["ja"] = "����";
            _wordBaseEtoJ["ji"] = "��";
            _wordBaseEtoJ["ju"] = "����";
            _wordBaseEtoJ["je"] = "����";
            _wordBaseEtoJ["jo"] = "����";
            _wordBaseEtoJ["jja"] = "������";
            _wordBaseEtoJ["jji"] = "����";
            _wordBaseEtoJ["jju"] = "������";
            _wordBaseEtoJ["jje"] = "������";
            _wordBaseEtoJ["jjo"] = "������";
            _wordBaseEtoJ["jya"] = "����";
            _wordBaseEtoJ["jyi"] = "����";
            _wordBaseEtoJ["jyu"] = "����";
            _wordBaseEtoJ["jye"] = "����";
            _wordBaseEtoJ["jyo"] = "����";
            _wordBaseEtoJ["jjya"] = "������";
            _wordBaseEtoJ["jjyi"] = "������";
            _wordBaseEtoJ["jjyu"] = "������";
            _wordBaseEtoJ["jjye"] = "������";
            _wordBaseEtoJ["jjyo"] = "������";

            //���s
            _wordBaseEtoJ["da"] = "��";
            _wordBaseEtoJ["di"] = "��";
            _wordBaseEtoJ["du"] = "��";
            _wordBaseEtoJ["de"] = "��";
            _wordBaseEtoJ["do"] = "��";
            _wordBaseEtoJ["dda"] = "����";
            _wordBaseEtoJ["ddi"] = "����";
            _wordBaseEtoJ["ddu"] = "����";
            _wordBaseEtoJ["dde"] = "����";
            _wordBaseEtoJ["ddo"] = "����";
            _wordBaseEtoJ["dya"] = "����";
            _wordBaseEtoJ["dyi"] = "����";
            _wordBaseEtoJ["dyu"] = "����";
            _wordBaseEtoJ["dye"] = "����";
            _wordBaseEtoJ["dyo"] = "����";
            _wordBaseEtoJ["ddya"] = "������";
            _wordBaseEtoJ["ddyi"] = "������";
            _wordBaseEtoJ["ddyu"] = "������";
            _wordBaseEtoJ["ddye"] = "������";
            _wordBaseEtoJ["ddyo"] = "������";
            _wordBaseEtoJ["dha"] = "�ł�";
            _wordBaseEtoJ["dhi"] = "�ł�";
            _wordBaseEtoJ["dhu"] = "�ł�";
            _wordBaseEtoJ["dhe"] = "�ł�";
            _wordBaseEtoJ["dho"] = "�ł�";
            _wordBaseEtoJ["ddha"] = "���ł�";
            _wordBaseEtoJ["ddhi"] = "���ł�";
            _wordBaseEtoJ["ddhu"] = "���ł�";
            _wordBaseEtoJ["ddhe"] = "���ł�";
            _wordBaseEtoJ["ddho"] = "���ł�";
            _wordBaseEtoJ["dwa"] = "�ǂ�";
            _wordBaseEtoJ["dwi"] = "�ǂ�";
            _wordBaseEtoJ["dwu"] = "�ǂ�";
            _wordBaseEtoJ["dwe"] = "�ǂ�";
            _wordBaseEtoJ["dwo"] = "�ǂ�";
            _wordBaseEtoJ["ddwa"] = "���ǂ�";
            _wordBaseEtoJ["ddwi"] = "���ǂ�";
            _wordBaseEtoJ["ddwu"] = "���ǂ�";
            _wordBaseEtoJ["ddwe"] = "���ǂ�";
            _wordBaseEtoJ["ddwo"] = "���ǂ�";

            //�΍s
            _wordBaseEtoJ["ba"] = "��";
            _wordBaseEtoJ["bi"] = "��";
            _wordBaseEtoJ["bu"] = "��";
            _wordBaseEtoJ["be"] = "��";
            _wordBaseEtoJ["bo"] = "��";
            _wordBaseEtoJ["bba"] = "����";
            _wordBaseEtoJ["bbi"] = "����";
            _wordBaseEtoJ["bbu"] = "����";
            _wordBaseEtoJ["bbe"] = "����";
            _wordBaseEtoJ["bbo"] = "����";
            _wordBaseEtoJ["bya"] = "�т�";
            _wordBaseEtoJ["byi"] = "�т�";
            _wordBaseEtoJ["byu"] = "�т�";
            _wordBaseEtoJ["bye"] = "�т�";
            _wordBaseEtoJ["byo"] = "�т�";
            _wordBaseEtoJ["bbya"] = "���т�";
            _wordBaseEtoJ["bbyi"] = "���т�";
            _wordBaseEtoJ["bbyu"] = "���т�";
            _wordBaseEtoJ["bbye"] = "���т�";
            _wordBaseEtoJ["bbyo"] = "���т�";
            _wordBaseEtoJ["va"] = "����";
            _wordBaseEtoJ["vi"] = "����";
            _wordBaseEtoJ["vu"] = "��";
            _wordBaseEtoJ["ve"] = "����";
            _wordBaseEtoJ["vo"] = "����";
            _wordBaseEtoJ["vva"] = "������";
            _wordBaseEtoJ["vvi"] = "������";
            _wordBaseEtoJ["vvu"] = "����";
            _wordBaseEtoJ["vve"] = "������";
            _wordBaseEtoJ["vvo"] = "������";
            _wordBaseEtoJ["vya"] = "����";
            _wordBaseEtoJ["vyi"] = "����";
            _wordBaseEtoJ["vyu"] = "����";
            _wordBaseEtoJ["vye"] = "����";
            _wordBaseEtoJ["vyo"] = "����";
            _wordBaseEtoJ["vvya"] = "������";
            _wordBaseEtoJ["vvyi"] = "������";
            _wordBaseEtoJ["vvyu"] = "������";
            _wordBaseEtoJ["vvye"] = "������";
            _wordBaseEtoJ["vvyo"] = "������";

            //�ύs
            _wordBaseEtoJ["pa"] = "��";
            _wordBaseEtoJ["pi"] = "��";
            _wordBaseEtoJ["pu"] = "��";
            _wordBaseEtoJ["pe"] = "��";
            _wordBaseEtoJ["po"] = "��";
            _wordBaseEtoJ["ppa"] = "����";
            _wordBaseEtoJ["ppi"] = "����";
            _wordBaseEtoJ["ppu"] = "����";
            _wordBaseEtoJ["ppe"] = "����";
            _wordBaseEtoJ["ppo"] = "����";
            _wordBaseEtoJ["pya"] = "�҂�";
            _wordBaseEtoJ["pyi"] = "�҂�";
            _wordBaseEtoJ["pyu"] = "�҂�";
            _wordBaseEtoJ["pye"] = "�҂�";
            _wordBaseEtoJ["pyo"] = "�҂�";
            _wordBaseEtoJ["ppya"] = "���҂�";
            _wordBaseEtoJ["ppyi"] = "���҂�";
            _wordBaseEtoJ["ppyu"] = "���҂�";
            _wordBaseEtoJ["ppye"] = "���҂�";
            _wordBaseEtoJ["ppyo"] = "���҂�";

            //����������
            _wordBaseEtoJ["la"] = "��";
            _wordBaseEtoJ["li"] = "��";
            _wordBaseEtoJ["lu"] = "��";
            _wordBaseEtoJ["le"] = "��";
            _wordBaseEtoJ["lo"] = "��";
            _wordBaseEtoJ["xa"] = "��";
            _wordBaseEtoJ["xi"] = "��";
            _wordBaseEtoJ["xu"] = "��";
            _wordBaseEtoJ["xe"] = "��";
            _wordBaseEtoJ["xo"] = "��";
            _wordBaseEtoJ["lla"] = "����";
            _wordBaseEtoJ["lli"] = "����";
            _wordBaseEtoJ["llu"] = "����";
            _wordBaseEtoJ["lle"] = "����";
            _wordBaseEtoJ["llo"] = "����";
            _wordBaseEtoJ["xxa"] = "����";
            _wordBaseEtoJ["xxi"] = "����";
            _wordBaseEtoJ["xxu"] = "����";
            _wordBaseEtoJ["xxe"] = "����";
            _wordBaseEtoJ["xxo"] = "����";
            _wordBaseEtoJ["lya"] = "��";
            _wordBaseEtoJ["lyu"] = "��";
            _wordBaseEtoJ["lyo"] = "��";
            _wordBaseEtoJ["xya"] = "��";
            _wordBaseEtoJ["xyu"] = "��";
            _wordBaseEtoJ["xyo"] = "��";
            _wordBaseEtoJ["llya"] = "����";
            _wordBaseEtoJ["llyu"] = "����";
            _wordBaseEtoJ["llyo"] = "����";
            _wordBaseEtoJ["xxya"] = "����";
            _wordBaseEtoJ["xxyu"] = "����";
            _wordBaseEtoJ["xxyo"] = "����";
            _wordBaseEtoJ["ltu"] = "��";
            _wordBaseEtoJ["xtu"] = "��";
            _wordBaseEtoJ["ltsu"] = "��";
            _wordBaseEtoJ["xtsu"] = "��";
            _wordBaseEtoJ["lltu"] = "����";
            _wordBaseEtoJ["xxtu"] = "����";
            _wordBaseEtoJ["lltsu"] = "����";
            _wordBaseEtoJ["xxtsu"] = "����";

            //�L�΂��_
            _wordBaseEtoJ["-"] = "�[";
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