using System;
using System.Collections.Generic;
using System.Linq;

namespace BitLab.Message
{
    public class Message
    {
        private byte[] command = new byte[12];
        private uint magic;
        private Payload _PayloadObject;
        
        public uint Magic
        {
            get
            {
                return this.magic;
            }
            set
            {
                this.magic = value;
            }
        }

        public string Command
        {
            get
            {
                return EncodeData(this.command , 0, this.command.Length);
            }
            private set
            {
                this.command = DecodeData(value.Trim().PadRight(12, char.MinValue));
            }
        }

        public Payload Payload
        {
            get
            {
                return this._PayloadObject;
            }
            set
            {
                this._PayloadObject = value;
                this.Command = this._PayloadObject.Command;
            }
        }
        
        private byte[] DecodeData(string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
                return new byte[0];
            return ((IEnumerable<char>) encoded.ToCharArray()).Select<char, byte>((Func<char, byte>) (o => (byte) o)).ToArray<byte>();
        }

        private string EncodeData(byte[] data, int offset, int count)
        {
            return new string(((IEnumerable<byte>) data).Skip<byte>(offset).Take<byte>(count).Select<byte, char>((Func<byte, char>) (o => (char) o)).ToArray<char>()).Replace("\0", "");
        }
    }
}