namespace Infiniminer
{
    public enum NetMessageType : byte
    {
        ServerDiscovered,
        StatusChanged,
        ConnectionRejected,
        Data
    }
}