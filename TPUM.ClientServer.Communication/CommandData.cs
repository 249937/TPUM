namespace TPUM.ClientServer.Communication
{
    public enum Command
    {
        Add,
        Get,
        GetAll,
        Remove
    }

    public class CommandData
    {
        public Command command;
        public byte[] data;
    }
}
