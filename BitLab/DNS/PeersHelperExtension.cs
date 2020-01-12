using System;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace BitLab.DNS
{
    public static class PeersHelperExtension
    {
        public static void PrintPeersFromDns(Task<IPEndPoint[]> endpoints)
        {
            int i = 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Pokazanie pobranych peer'ów"); //print IP addresses of peers\
            Console.ResetColor();

            foreach (var peer in endpoints.Result)
            {
                Console.WriteLine($"Peer {i} : {peer}");
                i++;
                Thread.Sleep(100);
            }
        }
    }
}