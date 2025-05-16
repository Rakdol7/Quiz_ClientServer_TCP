using System;
using System.Net.Sockets;
using System.Text;

class QuizClient
{
    static void Main()
    {
        Console.WriteLine(" CLIENT QUIZ ");
        Console.WriteLine("Connessione al server...");

        try
        {

            using (TcpClient client = new TcpClient("127.0.0.1", 2021))
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Connesso al server. Inizia il quiz!\n");

                byte[] buffer = new byte[1024];
                int bytesRead;

                for (int i = 1; i <= 3; i++)
                {

                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string question = Encoding.UTF8.GetString(buffer, 0, bytesRead);

             
                    Console.WriteLine($"DOMANDA {i}: {question}");
                    Console.Write("La tua risposta: ");

               
                    string answer = Console.ReadLine();

       
                    byte[] answerBytes = Encoding.UTF8.GetBytes(answer);
                    stream.Write(answerBytes, 0, answerBytes.Length);


                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string feedback = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"--> {feedback}\n");
                }

    
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string scoreMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(" RISULTATO FINALE ");
                Console.WriteLine(scoreMessage);
            }
        }
        catch (SocketException)
        {
            Console.WriteLine("Impossibile connettersi al server. Assicurati che il server sia in esecuzione.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }

        Console.WriteLine("\nPremi un tasto per uscire...");
        Console.ReadKey();
    }
}