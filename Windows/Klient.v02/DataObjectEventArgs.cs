using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klient.v02
{
    public class DataObjectEventArgs : EventArgs
    {
        // Add as many properties as you wish.  
        // A collection of properties works, too.  
        private object obj;
        public object Obj
        {
            get
            {
                return obj;
            }
            set
            {
                obj = value;
            }
        }

        public DataObjectEventArgs(Object obj)
        {
            this.obj = obj;
        }

    }
    public class DataObject
    {
        private object sender;

        // Add as many properties as you wish.  
        private object obj;
        public object Obj
        {
            get
            {
                return obj;
            }
            set
            {
                obj = value;
            }
        }
        public event EventHandler<DataObjectEventArgs> Updated;

        public DataObject()
        {
            this.sender = this;
            this.obj = new object();
        }
        private void OnUpdated(DataObjectEventArgs doea)
        {
            if (this.Updated != null)
            {
                this.Updated.Invoke(sender, doea);
            }
        }
        internal void InvokeUpdated(
            object sender,
            DataObjectEventArgs doea)
        {
            this.sender = sender;
            OnUpdated(doea);
        }
    } 
}
