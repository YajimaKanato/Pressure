using UnityEngine;
using System.Collections.Generic;

public static class TypingList
{
    static List<string> _typingList = new List<string>
    { 
    "‚ ‚è‚ª‚Æ‚¤",
    "‚±‚ñ‚É‚¿‚Í",
    "‚³‚æ‚¤‚È‚ç",
    "‚¨‚Í‚æ‚¤",
    "‚ ‚ñ‚Ï‚ñ‚Ü‚ñ",
    "‚Ï‚¢‚È‚Á‚Õ‚é",
    "‚±‚ñ‚è‚ñ‚´‚¢",
    "‚µ‚Á‚Ï‚¢‚Í‚¹‚¢‚±‚¤‚Ì‚à‚Æ",
    "‚¹‚ñ‚ë‚Á‚Ò‚á‚­"
    };

    public static List<string> Typing {  get { return _typingList; } }
}
