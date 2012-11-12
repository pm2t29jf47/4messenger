using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSideExceptionHandler
{
    public class ExceptionHandler
    {
        public void HandleExcepion(Exception ex,string caller)
        {
            int a = 10;
            a++;
            //trace right line//listener
        }
    }
}