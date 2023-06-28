namespace GameManager
{

    public interface IPrototype
    {
        IPrototype ShallowClone();
        IPrototype DeepClone();
    }
}
