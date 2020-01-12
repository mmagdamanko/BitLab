using System;
using System.IO;
using System.Threading;

namespace BitLab.Serializator
{
    public class BitcoinStream
    {
        private int _MaxArraySize = 1048576;
        private readonly Stream _Inner;
        private readonly bool _Serializing;


        public int MaxArraySize
        {
            get { return this._MaxArraySize; }
            set { this._MaxArraySize = value; }
        }

        public Stream Inner
        {
            get { return this._Inner; }
        }

        public bool Serializing
        {
            get { return this._Serializing; }
        }

        public bool IsBigEndian { get; set; }

        private void ReadWriteNumber(ref ulong value, int size)
        {
            byte[] data = new byte[size];
            for (int index = 0; index < size; ++index)
                data[index] = (byte) (value >> index * 8);
            if (this.IsBigEndian)
                Array.Reverse<byte>(data);
            this.ReadWriteBytes(ref data, 0, -1);
            if (this.IsBigEndian)
                Array.Reverse<byte>(data);
            ulong num1 = 0;
            for (int index = 0; index < data.Length; ++index)
            {
                ulong num2 = (ulong) data[index];
                num1 += num2 << index * 8;
            }

            value = num1;
        }

        private void ReadWriteBytes(ref byte[] data, int offset = 0, int count = -1)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return;
            }
            
            count = count == -1 ? data.Length : count;
            
            if (count == 0)
            {
                return;
            }
            
            this.ReadWriteBytes(new Span<byte>(data, offset, count));
        }

        private void ReadWriteBytes(Span<byte> data)
        {
            if (this.Serializing)
            {
                this.Inner.Write((ReadOnlySpan<byte>) data);
            }
            else
            {
                int num = ReadStream(Inner,data);
                if (num == 0)
                {
                    throw new EndOfStreamException("No more byte to read");
                }
            }
        }
        
        public static int ReadStream( Stream stream, Span<byte> buffer, CancellationToken cancellation = default (CancellationToken))
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof (stream));
            }
            
            int num = 0;
            while (!buffer.IsEmpty)
            {
                cancellation.ThrowIfCancellationRequested();
                int start = stream.Read(buffer);
                if (start == 0)
                    return 0;
                buffer = buffer.Slice(start);
                num += start;
            }
            return num;
        }
        

        public void ReadWrite(ref ushort data)
        {
            ulong num = (ulong) data;
            this.ReadWriteNumber(ref num, 2);
            if (this.Serializing)
            {
                return;
            }
            data = (ushort) num;
        }
        
        public void ReadWrite(ref uint data)
        {
            ulong num = (ulong) data;
            this.ReadWriteNumber(ref num, 4);
            if (this.Serializing)
            {
                return;
            }
            data = (uint) num;
        }
        
        public void ReadWrite(ref ulong data)
        {
            ulong num = data;
            this.ReadWriteNumber(ref num, 8);
            
            if (this.Serializing)
            {
                return;
            }
            data = num;
        }
        
        public void ReadWrite(ref byte[] arr)
        {
            this.ReadWriteBytes(ref arr, 0, -1);
        }
        
        public void ReadWrite<T>(ref T data) where T : ISerializable
        {
            T result = data;

            result.ReadWrite(this);
            
            
            if (this.Serializing)
            {
                return;
            }
            data = result;
        }
    }
}