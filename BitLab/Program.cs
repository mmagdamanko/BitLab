using System;
using BitLab.DNS;
using BitLab.Message;
namespace BitLab
{
    class Program
    {
        static void Main(string[] args)
        {
            for (;;)
            {
                MessageCenter.WelcomeTitle();
                if (!FunctionCenter.isConnect())
                {
                    Console.WriteLine("\n Niestety nie masz aktywnego polaczenia internegowego, sprobuj pozniej \n");
                    return;
                }

                string input = Console.ReadLine();
                input = FunctionCenter.isStrOK(input);
                DNS.DNS dns;

                if (input == "dns-default")
                {
                    dns = new DNS.DNS(DnsHelperExtension.defaultDnsName,
                        DnsHelperExtension.defaultDnsHost); //from hardwire seeds,
                    MessageCenter.statusMsg();

                    var dataFromDns = dns.GetAddressNodesAsync(5000); //getaddr
                    Console.Write($"Pobranie peer'ów ");
                    if (dataFromDns.IsCanceled == false)
                    {
                        MessageCenter.powMsg();
                    }
                    else
                    {
                        MessageCenter.npowMsg();
                    }

                    if (!dataFromDns.IsCanceled)
                    {
                        PeersHelperExtension.PrintPeersFromDns(dataFromDns);
                    }
                }
                else if (input == "dns-custom")
                {
                    MessageCenter.qdnsMsg();
                    string dnsName = Console.ReadLine();
                    MessageCenter.qdnshMsg();
                    string dnsHost = Console.ReadLine();

                    try
                    {
                        dns = new DNS.DNS(dnsName, dnsHost); //DNS lookup
                        FunctionCenter.customDNS(dns);
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
                    catch (Exception ex)
                    {
                        MessageCenter.acconectMsg(dnsName);
                    }
                }
                
                //
            }
        }
    }
}
        
    
