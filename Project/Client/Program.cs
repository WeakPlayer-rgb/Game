using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void SendMessageFromSocket(int port)
        {
            
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством
            
            // Устанавливаем удаленную точку для сокета
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[1];
            var ipEndPoint = new IPEndPoint(ipAddr, port);
            
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            // Console.Write("Введите сообщение: ");
            //string message = Console.ReadLine();
            //
            // Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint);
            // byte[] msg = Encoding.UTF8.GetBytes(message);
            //
            // Отправляем данные через сокет
            // int bytesSent = sender.Send(msg);
            
            // Получаем ответ от сервера
            while (true)
            {
                var bytesRec = sender.Receive(bytes);
                Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));
                sender.Send(Encoding.UTF8.GetBytes("receive"));
            }
            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            // if (message != null && !message.Contains("<TheEnd>"))
            //     SendMessageFromSocket(port);
            
            // Освобождаем сокет
            // sender.Shutdown(SocketShutdown.Both);
            // sender.Close();
        }
    }
}