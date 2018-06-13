using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Observer
{
    /// <summary>
    /// Delegates the message processing
    /// </summary>
    /// <param name="massage"></param>
    public delegate void MessageHandler(MessageEventArgs massage); 
    /// <summary>
    /// A single entry point to manage messages.
    /// </summary>
    public class MessageObserver:Singeton<MessageObserver>
    {
        protected MessageObserver() : base()
        {
            SetMessage("Create observer");
        }


        public event MessageHandler ReceivedMessage;

        /// <summary>
        /// Alerting about new massages all subscribers
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage (string message, MessageType messageType =  MessageType.System)
        {
            if (this.ReceivedMessage != null)
                this.ReceivedMessage(new MessageEventArgs { Message = message , Type = messageType, time = DateTime.Now /* new DateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)*/ });
        }        
    }

    public class MessageEventArgs
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public DateTime time { get; set; }
    }


  public  enum MessageType {System = 0, Warning = 2, Error = 3, Success } 
}
