using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Socket Listener acts as a server and listens to the incoming
// messages on the specified port and protocol.
public class SocketListener
{
    private static readonly string[] questions = {
        "cosa solitamente viene detto dopo 'non è tutto oro quel che'?",
        "quale e' il campione del mondo di VGC Pokémon?",
        "che malattia aveva Freddie Mercury?"
    };

    private static readonly string[] answers = {
        "luccica",
        "Ceribelli",
        "AIDS"
    };
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }

    public static void StartServer()
    {
        // Get Host IP Address that is used to establish a connection
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        // If a host has multiple addresses, you will get a list of addresses
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2021);

        try
        {

            // Create a Socket that will use Tcp protocol
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.
            // We will listen 10 requests at a time
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            Socket handler = listener.Accept();

            // Incoming data from the client.
            /*string data = null;
            byte[] bytes = null;*/

            byte[] bytes = new byte[1024];

            //while (true)
            //{
                int score = 0;
                for(int i = 0; i < questions.Length; i++)
                {
                    byte[] questionBytes = Encoding.UTF8.GetBytes(questions[i]);
                    handler.Send(questionBytes);
                    Console.WriteLine($"Domanda inviata: {questions[i]}");

                    
                    int bytesRec = handler.Receive(bytes);
                    string data = Encoding.UTF8.GetString(bytes, 0, bytesRec).Trim();
                    /*if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }*/
                    
                    if (data.Equals(answers[i], StringComparison.OrdinalIgnoreCase))
                    {
                        score++;
                        handler.Send(Encoding.UTF8.GetBytes("Corretto!"));
                    }
                    else
                    {
                        handler.Send(Encoding.UTF8.GetBytes("Sbagliato!"));
                    }

                }

                string scoreMessage = $"Il tuo punteggio finale è: {score} su {questions.Length}";
                handler.Send(Encoding.UTF8.GetBytes(scoreMessage));


                //byte[] msg = Encoding.ASCII.GetBytes(scoreMessage);
               // handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            //}  
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }
}