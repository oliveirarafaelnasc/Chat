namespace RO.Chat.IO.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        int Commit();
    }
}