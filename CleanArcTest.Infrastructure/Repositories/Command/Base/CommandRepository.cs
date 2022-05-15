using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Repositories.Command.Base;

namespace CleanArcTest.Infrastructure.Repositories.Command.Base
{
    public class CommandRepository<T> : ICommandRepository<T> where T : IAggregateRoot
    {

    }
}
