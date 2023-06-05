using DatingApp.API.Data.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TracNghiem.Domain;

namespace TracNghiem.WebAPI
{
    public class Program
    {
        private static byte[] result = new byte[1024];
        private static int myPort = 6969;
        static Socket serverSocket;
        public static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // conver a ip address to IpAddress Instance
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ipAddress, myPort));
            serverSocket.Listen(10); //quantity of client can listen
            Console.WriteLine("Enpoint dung de giao tiep cua server: ", serverSocket.LocalEndPoint.ToString());
            // khi co 1 client connect toi thi khoi tao 1 thread moi
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();

            CreateHostBuilder(args).Build().Run();


            var app = CreateHostBuilder(args).Build();
            using var scope = app.Services.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;

            //khong lam anh huong den app. Update dc thi tot, ko thi thoi
            try
            {
                var context = serviceProvider.GetRequiredService<TracnghiemContext>();
                context.Database.Migrate();
                Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Migration failed!");
            }
            Console.ReadLine();
        }
        /// <summary>
        /// while a client connect with server and server accept with create a socket instance 
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello to Client : "
                    + clientSocket.AddressFamily.ToString()));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }
        private static void ReceiveMessage(object clientSocket) // client get message from server
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocket.Receive(result); // get data receive length(server send)
                    Console.WriteLine("client endpoint: ", myClientSocket.RemoteEndPoint.ToString(),
                        Encoding.ASCII.GetString(result, 0, receiveNumber)); // 0: offset 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both); // disable two socket client and server while exception occur
                    myClientSocket.Close(); // release all resource associate with this socket
                    break;
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
