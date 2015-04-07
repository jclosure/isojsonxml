using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsondemo
{
    public class DynamicMessage : IMessageHeaders
    {
        public Guid Id { get; set; }
        public string ReplyTo { get; set; }
        public int Priority { get; set; }
        public int RetryAttempts { get; set; }
        public object Body { get; set; }

        public Type Type { get; set; }
        public object GetBody()
        {
            //When deserialized this.Body is a string so use the serilaized
            //this.Type to deserialize it back into the original type
            return this.Body is string
            ? TypeSerializer.DeserializeFromString((string)this.Body, this.Type)
            : this.Body;
        }
    }

    public class GenericMessage<T> : IMessageHeaders
    {
        public Guid Id { get; set; }
        public string ReplyTo { get; set; }
        public int Priority { get; set; }
        public int RetryAttempts { get; set; }
        public T Body { get; set; }
    }

    public class StrictMessage : IMessageHeaders
    {
        public Guid Id { get; set; }
        public string ReplyTo { get; set; }
        public int Priority { get; set; }
        public int RetryAttempts { get; set; }
        public MessageBody Body { get; set; }
    }

    public class MessageBody
    {
        public MessageBody()
        {
            this.Arguments = new List<string>();
        }

        public string Action { get; set; }
        public List<string> Arguments { get; set; }
    }

    /// Common interface not required, used only to simplify validation
    public interface IMessageHeaders
    {
        Guid Id { get; set; }
        string ReplyTo { get; set; }
        int Priority { get; set; }
        int RetryAttempts { get; set; }
    }
}
