using System.Threading.Tasks;
using System;

namespace TPUM.ClientServer.Communication
{
    public enum Command
    {
        Add,
        Find,
        FindAll,
        Remove
    }

    public struct CommandData
    {
        public Command command;
        public byte[] data;
    }

    public abstract class WebSocketConnection
    {
        public virtual Action<CommandData> OnMessage
        {
            set;
            protected get;
        } = message => { };

        public virtual Action OnClose
        {
            set;
            protected get;
        } = () => { };

        public virtual Action OnError
        {
            set;
            protected get;
        } = () => { };

        public async Task SendAsync(CommandData commandData)
        {
            await SendTask(commandData);
        }

        public abstract Task DisconnectAsync();

        protected abstract Task SendTask(CommandData commandData);
    }
}
