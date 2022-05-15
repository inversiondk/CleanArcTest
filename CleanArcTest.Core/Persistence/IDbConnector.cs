using System.Data;

namespace CleanArcTest.Core.Persistence
{
    public interface IDbConnector
    {
        IDbConnection CreateConnection();
    }
}
