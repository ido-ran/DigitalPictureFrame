namespace DigitalFrame.Core.Interfaces
{
    public interface IRepository<T>
    {
        T Load();
        void Save(T value);
    }
}