using System;
using Microsoft.VisualBasic;

namespace BitLab.Message
{
    public class MessageCenter
    {
        public static void WelcomeTitle()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\t\t                        ");

            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\t\t                        \n" +
                              "\t\t     Program BitLab     \n" +
                              "\t\t                        ");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\t          UKSW          \n\n");
            Console.ResetColor();

            Console.Write("Pierwszym krokiem jest wyszukanie peer'a. \n");

            Console.Write("Wprowadz ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("dns-default");
            Console.ResetColor();
            Console.Write(", żeby użyć domyślnego DNS'a (seed.bitcoin.sipa.be). \n");

            Console.Write("Wprowadź");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" dns-custom");
            Console.ResetColor();
            Console.Write(", żeby podać własnego DNS'a.\n"); 
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Jezeli chcesz zakonczyc dzialanie programu wpisz 'koniec'.\n");
            Console.WriteLine("\n");
            Console.ResetColor();
        }
        public static string ErrorRef(String input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPodałeś zły string. Koniec programu.");
            Console.WriteLine("\n Sprobuj ponownie, wpisujac dns-default lub dns-custom \n");
            Console.WriteLine(" Mozesz rowniez wyjsc z programu wpisujac 'esc'\n");
            Console.ResetColor();
            input = Console.ReadLine();
            return input;
        }
        public static void statusMsg()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nWysyłam getAddr");
            Console.WriteLine("Process ....");
            Console.ResetColor();
        }
        public static void powMsg()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("powiodło sie.\n");
            Console.ResetColor();
        }
        public static void npowMsg()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("nie powiodło się.\n");
            Console.ResetColor();
        }
        public static void qdnsMsg()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPodaj nazwe DNS od którego chcesz uzyskać informacje.");
            Console.ResetColor();
        }
        public static void qdnshMsg()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPodaj host DNS od którego chcesz uzyskać informacje.");
            Console.ResetColor();
        }
        public static void agMsg()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("powiodło sie.\n");
            Console.ResetColor();
        }
        public static void abMsg()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("nie powiodło się.\n");
            Console.ResetColor();
        }
        public static void acconectMsg(String dnsName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nie udało się połączyć z " + dnsName);
            Console.ResetColor();
            return;
        }
    }
}