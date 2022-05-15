using CleanArcTest.Core.Base.Contracts;

namespace CleanArcTest.Core.Repositories.Command.Base
{
    public interface ICommandRepository<T> where T : IAggregateRoot
    {

    }
}
