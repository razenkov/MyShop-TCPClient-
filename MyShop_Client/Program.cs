using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClientOfMyShop
{
    class Program
    {
        public static bool isNewUser = false;
        public static string LocalId;

        static void Main(string[] args)
        {
            Console.WriteLine("Client Starts;");
            Socket ClientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 1080);

            ClientSocket.Connect(endPoint);


            User CurrentUser = new User();//create temp user

            try
            {
                StreamReader sr = new StreamReader(@"C:\Users\adm1n\Documents\Visual Studio 2017\Projects\MyShop\ClientOfMyShop\Id.txt");
                char[] buff = new char[512];
                LocalId = sr.ReadLine().ToString();
            }
            catch { }

            if (LocalId != null)
            {
                Console.WriteLine("Start to restore your data. Welcome.");
                BinaryRestoreId(CurrentUser);
                //restore data from server by LocalId
            }
            else
            {
                CurrentUser = RegistrateNewUser();
                BinarySaveId(CurrentUser);
                ClientSocket.Send(ConvertUserToByteArray(CurrentUser), 0, ConvertUserToByteArray(CurrentUser).Length, 0); //send new user to server


            }

            //=============================START=================================================



            while (true)

            {
                Console.WriteLine("Enter msg: ");

                string msg = Console.ReadLine();

                byte[] msgBuffer = Encoding.Default.GetBytes(msg);

                ClientSocket.Send(msgBuffer, 0, msgBuffer.Length, 0);

                byte[] buffer = new byte[255];

                int recive = ClientSocket.Receive(buffer, 0, buffer.Length, 0);

                Array.Resize(ref buffer, recive);

                Console.WriteLine("Resived: {0}", Encoding.Default.GetString(buffer));


            }

            ClientSocket.Close();//TODO: Find good way to close socket
        }
        //=================================END=============================================

        public static void BinarySaveId(User user)
        {
            Console.WriteLine("BinarySaveId");
            string path = @"C:\Users\adm1n\Documents\Visual Studio 2017\Projects\MyShop\ClientOfMyShop\Id.txt";

            BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
            bw.Write(user.Id);
            bw.Close();
        }

        public static void BinaryRestoreId(User user)
        {
            Console.WriteLine("BinaryRestoreId");
            string path = @"C:\Users\adm1n\Documents\Visual Studio 2017\Projects\MyShop\ClientOfMyShop\Id.txt";

            BinaryReader br = new BinaryReader(File.Open(path, FileMode.OpenOrCreate));
            user.Id = br.ReadString();
            br.Close();
        }

        public static User RegistrateNewUser()
        {
            Console.WriteLine("You are new user, we need you to registrate. Please enter your name:");
            User newUser = new User();
            newUser.Name = Console.ReadLine();

            Console.WriteLine("Please enter your soname");
            newUser.Soname = Console.ReadLine();

            Console.WriteLine("Please enter your age");
            newUser.Age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter your sex");
            newUser.Sex = Console.ReadLine();

            return newUser;
        }

        public static byte[] ByteArrayToSend(User user)
        {
            if (user == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, user);
                return ms.ToArray();
            }
        }

        public static User ConvertByteArrayToUser(byte[] array)
        {
            User user = new User();

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(array, 0, array.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return user;
        }

        public static byte[] ConvertUserToByteArray(User user)
        {
            if (user == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, user);

            return ms.ToArray();
        }

        public static void UserDataRequest(string Id)
        {

        }

    }
}
