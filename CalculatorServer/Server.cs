using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.IO;


namespace CalculatorServer
{
    class Server
    {
        public Server()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();

            listener.BeginGetContext(ar =>
            {
                HttpListener l = (HttpListener)ar.AsyncState;

                while (true)
                {
                    HttpListenerContext context = l.GetContext();
                    context.Response.StatusCode = 200;
                    string num1str = context.Request.QueryString["num1"];
                    string num2str = context.Request.QueryString["num2"];
                    string op = context.Request.QueryString["op"];
                    string calc = Calc(Int32.Parse(num1str), Int32.Parse(num2str), op).ToString();

                    //context.Response.ContentType = "text/plain";
                    //context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    //context.Response.ContentLength64 = calc.Length;

                    StreamWriter writer = new StreamWriter(context.Response.OutputStream);
                    writer.WriteLine(calc);
                    Console.WriteLine(num1str + op + num2str + "=" + calc);
                    writer.Close();
                    context.Response.Close();
                    l.Stop();
                }
                
            }, listener);
        }

        public int Calc(int num1, int num2, string op)
        {
            int res = 0;

            switch(op)
            {
                case ("p"):
                    res = num1 + num2;
                    break;
                case ("-"):
                    res = num1 - num2;
                    break;
                case ("*"):
                    res = num1 * num2;
                    break;
                case ("/"):
                    res = num1 / num2;
                    break;
            }

            return res;
        }
    }
}
