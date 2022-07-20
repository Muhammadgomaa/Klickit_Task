using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Klickit_Task
{
    public static class GlobalVariables
    {
        public static HttpClient WebAPIcLinet = new HttpClient();

        static GlobalVariables()
        {
            WebAPIcLinet.BaseAddress = new Uri("http://localhost:1996/");
            WebAPIcLinet.DefaultRequestHeaders.Clear();
            WebAPIcLinet.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}