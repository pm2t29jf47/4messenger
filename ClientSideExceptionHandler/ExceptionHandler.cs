using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SpecialWords;


namespace ClientSideExceptionHandler
{
    public static class ExceptionHandler
    {      
        public static void HandleExcepion(Exception ex, string caller)
        {            
            string exceptionTitle = SpecialWords.SpecialWords.HandledException
                + SpecialWords.SpecialWords.ExclamationMark;

            string exceptionMessage = SpecialWords.SpecialWords.Caller
                + SpecialWords.SpecialWords.Colon
                + SpecialWords.SpecialWords.LeftSquareBrackets
                + caller
                + SpecialWords.SpecialWords.RightSquareBracket;

            string callerName = SpecialWords.SpecialWords.Message
                + SpecialWords.SpecialWords.Colon
                + SpecialWords.SpecialWords.LeftSquareBrackets
                + ex.Message
                + SpecialWords.SpecialWords.RightSquareBracket;

            string lineSplitter = SpecialWords.SpecialWords.Asterisk
                + SpecialWords.SpecialWords.Space
                + SpecialWords.SpecialWords.Asterisk
                + SpecialWords.SpecialWords.Space
                + SpecialWords.SpecialWords.Asterisk;

            Trace.WriteLine(exceptionTitle);
            Trace.WriteLine(exceptionMessage);
            Trace.WriteLine(callerName);
            Trace.WriteLine(lineSplitter); 
        }
    }
}
