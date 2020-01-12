using System;
using System.Collections.Generic;
using BitLab.Message;

namespace BitLab.Node
{
    public class NodeListener : BeetwenMessageListener<IncomingMessage>  ,IDisposable
    {
        private readonly Node _node;
        private IDisposable _subskrypcja;
        private List<Func<IncomingMessage, bool>> _OgloszoneWiadomosci = new List<Func<IncomingMessage, bool>>();

        
        public NodeListener(Node node)
        {
            this._subskrypcja = node.MessageCreator.AddMessageListener((IMessageListener<IncomingMessage>) this);
            this._node = node;
        }
        
        public NodeListener Where(Func<IncomingMessage, bool> param)
        {
            this._OgloszoneWiadomosci.Add(param);
            return this;
        }
        
        public void Dispose()
        {
            if (this._subskrypcja == null)
            {
                return;
            }
            
            this._subskrypcja.Dispose();
        }
    }
}