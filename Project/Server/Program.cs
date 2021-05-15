using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NewGame.Properties;

namespace SocketServer
{
    internal static class Program
    {
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.String")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.Char[]")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.RuntimeMethodInfoStub")]
        [SuppressMessage("ReSharper.DPA", "DPA0004: Closure object allocation")]
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        static void Main(string[] args)
        {
            // Устанавливаем для сокета локальную конечную точку
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[1];
            var ipEndPoint = new IPEndPoint(ipAddr, 11000);
            
            // Создаем сокет Tcp/Ip
            var sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                var sendDictionary = new Dictionary<Socket, Task<int>>();
                var forReceive = new Dictionary<Socket, Task<int>>();
                var clientData = new Dictionary<Socket, byte[]>();
                
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                var task = sListener.AcceptAsync();

                // Начинаем слушать соединения
                while (true)
                {
                    switch (task.IsCompleted)
                    {
                        case true when !sendDictionary.ContainsKey(task.Result):
                            sendDictionary[task.Result] =
                                task.Result.SendAsync(Encoding.UTF8.GetBytes("send"), SocketFlags.Broadcast);
                            task = sListener.AcceptAsync();
                            break;
                        case true:
                            task = sListener.AcceptAsync();
                            break;
                    }

                    foreach (var socket in sendDictionary.Keys.Where(socket =>
                        sendDictionary[socket].IsCompleted &&
                        (!forReceive.ContainsKey(socket) || forReceive[socket].IsCompleted)))
                    {
                        socket.SendAsync(Encoding.UTF8.GetBytes("send"), SocketFlags.None);
                        if (!forReceive.ContainsKey(socket))
                        {
                            clientData.Add(socket,new byte[1024]);
                            forReceive.Add(socket,socket.ReceiveAsync(clientData[socket],SocketFlags.None));
                        }
                        else
                            forReceive[socket] = socket.ReceiveAsync(clientData[socket], SocketFlags.None);
                    }

                    foreach (var socket in forReceive.Keys.Where(socket => forReceive[socket].IsCompleted))
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(clientData[socket]));
                        if (socket.Blocking)
                        {
                            clientData.Remove(socket);
                            forReceive.Remove(socket);
                        }
                    }
                    // foreach (var task in TaskList)
                    // {
                    //     if(task.IsCompleted) ClientList.Add(task.Result);
                    // }

                    // Мы дождались клиента, пытающегося с нами соединиться

                    // byte[] bytes = new byte[1024];
                    // int bytesRec = handler.Receive(bytes);
                    //
                    // data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    //
                    // // Показываем данные на консоли
                    // Console.Write("Полученный текст: " + data + "\n\n");
                    //
                    // // Отправляем ответ клиенту\
                    // string reply = "Спасибо за запрос в " + data.Length.ToString()
                    //         + " символов";
                    // byte[] msg = Encoding.UTF8.GetBytes(reply);
                    // handler.Send(msg);
                    //
                    // if (data.IndexOf("<TheEnd>") > -1)
                    // {
                    //     Console.WriteLine("Сервер завершил соединение с клиентом.");
                    //     break;
                    // }

                    // handler.Shutdown(SocketShutdown.Both);
                    // handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}