using System;
using System.IO;
using BitLab.Serializator;

namespace BitLab.Helpers
{
    public class BitcoinInt : ISerializable
    {
        private ulong _wartosc;

        public BitcoinInt(ulong value)
        {
            this.SetValue(value);
        }

        internal void SetValue(ulong value)
        {
            this._wartosc = value;
        }

        public static void StaticWrite(BitcoinStream bs, ulong length)
        {
            Stream inner = bs.Inner;
            if (length < 253UL)
                inner.WriteByte((byte) length);
            else if (length <= (ulong) ushort.MaxValue)
            {
                ushort data = (ushort) length;
                inner.WriteByte((byte) 253);
                bs.ReadWrite(ref data);
            }
            else if (length <= (ulong) uint.MaxValue)
            {
                uint data = (uint) length;
                inner.WriteByte((byte) 254);
                bs.ReadWrite(ref data);
            }
            else
            {
                ulong data = length;
                inner.WriteByte(byte.MaxValue);
                bs.ReadWrite(ref data);
            }
        }

        public static ulong StaticRead(BitcoinStream bs)
        {
            if (bs.Serializing)
            {
                throw new InvalidOperationException("Stream nie może się serializować");
            }
            
            int num = bs.Inner.ReadByte();
            if (num == -1)
            {
                throw new EndOfStreamException("Brak byte'ow do czytania");
            }

            if (num < 253)
            {
                return (ulong) (byte) num;
            }
            if (num == 253)
            {
                ushort data = 0;
                bs.ReadWrite(ref data);
                return (ulong) data;
            }
            if (num == 254)
            {
                uint data = 0;
                bs.ReadWrite(ref data);
                return (ulong) data;
            }
            ulong data1 = 0;
            bs.ReadWrite(ref data1);
            return data1;
        }

        public ulong ToLong()
        {
            return this._wartosc;
        }

        public void ReadWrite(BitcoinStream stream)
        {
            if (stream.Serializing)
            {
                BitcoinInt.StaticWrite(stream, this._wartosc);
            }
            else
            {
                this._wartosc = BitcoinInt.StaticRead(stream);
            }
        }
    }
}
