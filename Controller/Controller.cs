using HW_Server_TCP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_Server_TCP.Controller
{
    internal class Controller
    {
        MySocket mySocket;
        Form1 myForm;
        public Controller(Form1 form1) 
        {
             myForm = form1;
             mySocket = new MySocket(this);
        }


        public void GetMessage(string str)
        {
            myForm.IniListBox(str);
        }
        public void StartThreadForAccept()
        {
            mySocket.ThreadForAccept();
        }
    }
}
