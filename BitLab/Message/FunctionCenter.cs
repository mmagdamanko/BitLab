using System;
using System.Net.Mime;
using System.Net.NetworkInformation;
using BitLab.DNS;

namespace BitLab.Message
{
    public class FunctionCenter
    {
        public static string isStrOK(string input)
        {
            for (;;)
            {
                if (input == "dns-default")
                {
                    return input;
                }
                else if (input == "dns-custom")
                {
                    return input;
                }
                else
                {
                    input = MessageCenter.ErrorRef(input);
                    if (input == "esc")
                        break;
                }
            }
            return null;
        }
        public static void customDNS(DNS.DNS dns)
        {
            MessageCenter.statusMsg();
            var dataFromDns = dns.GetAddressNodesAsync(5000); //getaddr
            Console.Write($"Pobranie peer'ów ");
            if (dataFromDns.IsCanceled == false)
            {
                MessageCenter.agMsg();
            }
            else
            {
                MessageCenter.abMsg();
            }
            if (!dataFromDns.IsCanceled)
            {
                PeersHelperExtension.PrintPeersFromDns(dataFromDns);
            }
        }
        public static Boolean isConnect()
        {
            try { 
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
