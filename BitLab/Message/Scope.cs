using System;

namespace BitLab.Message
{
    public class Scope : IDisposable
    {
        private Action close;

        public Scope(Action open, Action close)
        {
            this.close = close;
            open();
        }

        public void Dispose()
        {
            this.close();
        }
    }
}