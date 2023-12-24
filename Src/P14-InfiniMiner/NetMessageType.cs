namespace GameManager
{
    public enum NetMessageType : byte
    {
        ServerDiscovered,
        StatusChanged,
        ConnectionRejected,
        Data
    }
}