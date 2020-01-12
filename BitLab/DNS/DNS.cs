using System.Net;
using System.Threading.Tasks;

namespace BitLab.DNS
{
    public class DNS
    {
        private string name;
        private string host;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Host
        {
            get
            {
                return this.host;
            }
        }

        /// <summary>
        /// Konstruktor , podczas tworzenia obiektu przypisujemy odrazu nazwe i host danego DNS'a
        /// </summary>
        /// <param name="name"></param>
        /// <param name="host"></param>
        public DNS(string name, string host)
        {
            this.name = name;
            this.host = host;
        }

        public Task<IPEndPoint[]> GetAddressNodesAsync(int port)
        {
            return new DnsEndPoint(this.Host, port).ResolveToIPEndpointsAsync();
        }

        public override string ToString()
        {
            return this.name + " (" + this.host + ")";
        }
    }
}