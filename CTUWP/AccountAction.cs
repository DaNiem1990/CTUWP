using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTUWP
{
    class AccountAction
    {
        string apiAddress = "http://ct.zobacztu.pl";
        string endPoint = "/api/login";
        string[] messages = { "Trwa logowanie proszę czekać", "Trwa rejestracja proszę czekać" };
        string waitingMessage;
        bool isLogin = true;

        public AccountAction(string endPoint = "")
        {
            initType(endPoint);
        }

        private void initType(string endPoint)
        {
            if (String.IsNullOrEmpty(endPoint))
            {
                isLogin = true;
                waitingMessage = messages[0];
                endPoint = "/api/login";
            }
            else
            {
                isLogin = false;
                waitingMessage = messages[1];
                this.endPoint = endPoint;
            }
        } 
    }
}
