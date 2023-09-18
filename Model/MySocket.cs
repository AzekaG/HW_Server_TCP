using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HW_Server_TCP.Controller;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HW_Server_TCP.Model
{
    internal class MySocket
    {
        Random random = new Random();
        Controller.Controller controller;
       public MySocket(Controller.Controller controller) 
        {
           this.controller = controller;
        }
        

        //Receive получатель
        public void ThreadForRecieve(object param)
        {
           
            List<string> res = new List<string>();
            Socket handler = (Socket)param;
            try
            {
                string client = null;
                string data = null;
                byte[] bytes = new byte[1024];

                int byteRec = handler.Receive(bytes);
                client = Encoding.UTF8.GetString(bytes, 0, byteRec);
                client += "(" + handler.RemoteEndPoint.ToString() + ")";

                while (true)
                {
                    byteRec = handler.Receive(bytes);
                    if (byteRec == 0)
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        return;
                    }
                    data = Encoding.UTF8.GetString(bytes, 0, byteRec);
                    controller.GetMessage(client);
                    controller.GetMessage(data);
                    GetAnswerToClient(handler , data);


                    if (data.IndexOf("<end>") > -1) break;
                }

                string theReply = "Я завершаю обработку сообщений";
                byte[] msg = Encoding.UTF8.GetBytes(theReply);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                   
             }
            catch (Exception ex) 
            {
                controller.GetMessage("Сервер: " + ex.Message);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

           
         }

        void GetAnswerToClient(Socket sender , string data)
        {
             
            byte[] msg = Encoding.UTF8.GetBytes(HandlerAnswer(data));
            sender.Send(msg);
        }

        public void ThreadForAccept()
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 49152);

                Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sListener.Bind(iPEndPoint);

                sListener.Listen(10);
                while(true)
                {
                    MessageBox.Show("Сокер открыт , сервер в работе");
                    Socket handler = sListener.Accept();
                    Thread thread = new Thread(new ParameterizedThreadStart(ThreadForRecieve));
                    thread.IsBackground = true;
                    thread.Start(handler);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сервер : " + ex.Message);
            }
        }
        public string HandlerAnswer(string dataMSG)
        {
            string data = dataMSG.ToLower();
            string res = "Сервер : ";
            if (data.Contains("привет"))
                res+= "Привет)";
            if (data.Contains("дела"))
                res += "У меня дела нормально , а твои как?";
            if (data.Contains("погода"))
                res += "та вроде бы у нас сегодня ночью прохладно , но завтра обещают тепло)";
            if (data.Contains("анекдот"))
                res += AnecdotRand();
            if (data.Contains("зовут"))
                res += "ну..скуажем так , мне пока имя не давали)";
            
            return res;
        }



        public string AnecdotRand()
        {
            List<string> anecdots = new List<string>();

            anecdots.Add("Мужчины без материальных проблем - это те, кого женщины ищут. С материальными проблемами- те, кого женщины уже нашли.");
            anecdots.Add("Приходит Сара домой из поликлиники. Абрам, ты знаешь, то что мы с тобой тридцать лет имели за оргазм, на самом деле астма!");
            anecdots.Add("- Вы случайно не сын портного Изи?- Таки да, но почему случайно?");
            anecdots.Add("- Скажите, Додик, расстегай - это рыба или мясо?- Фирочка, это глагол.");
            anecdots.Add("- Сёма, а ты приснился мне в эротическом сне…. - Да? Ну и шо я там вытворял?- Пришёл и тока всё испортил!");
            anecdots.Add("Как говорит заслуженный учитель младших классов Изольда Давидовна, если вас знают только с хорошей стороны, так сидите и не вертитесь.");




            
            return anecdots.ElementAt( random.Next(0,anecdots.Count-1));
        }
    }





  
}
