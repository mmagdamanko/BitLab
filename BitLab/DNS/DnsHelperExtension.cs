using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BitLab.DNS
{
    public static class DnsHelperExtension
    {
        public static string defaultDnsName = "seed.bitcoin.sipa";
        public static string defaultDnsHost = "seed.bitcoin.sipa.be";
        public static async Task<IPEndPoint[]> ResolveToIPEndpointsAsync(
            this EndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint)); //endpoint nie zawierał wartości
            }

            IPEndPoint ipEndPoint1 = endpoint as IPEndPoint;  //sprawdzenie czy endpoint jest IPendpointem
            if (ipEndPoint1 != null)
            {
                return new IPEndPoint[1] {ipEndPoint1};
            }

            DnsEndPoint dns = endpoint as DnsEndPoint;  //sprawdzanie czy endpoint jest DnsEndPointem
            if (dns == null)
            {
                throw new NotSupportedException(endpoint.ToString());
            }

            return ((IEnumerable<IPAddress>) await Dns.GetHostAddressesAsync(dns.Host).ConfigureAwait(false)).Select<IPAddress, IPEndPoint>((Func<IPAddress, IPEndPoint>) (i => new IPEndPoint(i, dns.Port))).ToArray<IPEndPoint>();
        }
    }
}