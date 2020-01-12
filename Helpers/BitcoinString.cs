using System;
using System.Collections.Generic;
using System.Linq;
using BitLab.Serializator;

namespace BitLab.Helpers
{
    public class BitcoinString : ISerializable
    {
        private byte[] _bytes;

        public BitcoinString()
        {
            this._bytes = new byte[0];
        }

        public int Length
        {
            get { return this._bytes.Length; }
        }

        public BitcoinString(byte[] bytes)
        {
            this._bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public byte[] GetString()
        {
            return ((IEnumerable<byte>) this._bytes).ToArray<byte>();
        }

        public void ReadWrite(BitcoinStream stream)
        {
            BitcoinInt data = new BitcoinInt((ulong) this._bytes.Length);
            stream.ReadWrite<BitcoinInt>(ref data);
            if (!stream.Serializing)
            {
                this._bytes = new byte[data.ToLong()];
            }

            stream.ReadWrite(ref this._bytes);
        }

        internal static void StaticWrite(BitcoinStream bs, byte[] bytes)
        {
            ulong length = bytes == null ? 0UL : (ulong) bytes.Length;
            BitcoinInt.StaticWrite(bs, length);
            if (bytes == null)
                return;
            bs.ReadWrite(ref bytes);
        }

        internal static void StaticRead(BitcoinStream bs, ref byte[] bytes)
        {
            ulong length = BitcoinInt.StaticRead(bs);
            bytes = new byte[length];
            bs.ReadWrite(ref bytes);
        }
        
    }

}
