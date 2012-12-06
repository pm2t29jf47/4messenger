using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecialWords
{
    public static class SpecialWords
    {
        private static string leftPointyBracket = "<",
            rightPointyBracket = ">",
            semicolon = ";",
            space = " ",
            leftSquareBrackets = "[",
            rightSquareBracket = "]",
            leftRoundBracket = "(",
            rightRoundBracket = ")",
            colon = ":",
            exclamationMark = "!",
            asterisk = "*";

        private static string message = "Message",
            caller = "Caller",
            handledException = "Handled exception";

        public static string Message
        {
            get
            {
                return message;
            }
        }

        public static string HandledException
        {
            get
            {
                return handledException;
            }
        }

        public static string Caller
        {
            get
            {
                return caller;
            }
        }

        public static string LeftPointyBracket
        {
            get
            {
                return leftPointyBracket;
            }
        }

        public static string RightPointyBracket
        {
            get
            {
                return rightPointyBracket;
            }
        }

        public static string Semicolon
        {
            get
            {
                return semicolon;
            }
        }

        public static string Space
        {
            get
            {
                return space;
            }
        }
        
        public static string LeftSquareBrackets
        {
            get
            {
                return leftSquareBrackets;
            }
        }
        
        public static string RightSquareBracket
        {
            get
            {
                return rightSquareBracket;
            }
        }
        
        public static string LeftRoundBracket
        {
            get
            {
                return leftRoundBracket;
            }
        }
        
        public static string RightRoundBracket
        {
            get
            {
                return rightRoundBracket;
            }
        }

        public static string Colon
        {
            get
            {
                return colon;
            }
        }
        
        public static string ExclamationMark
        {
            get
            {
                return exclamationMark;
            }
        }
        
        public static string Asterisk
        {
            get
            {
                return asterisk;
            }
        }
    }
}