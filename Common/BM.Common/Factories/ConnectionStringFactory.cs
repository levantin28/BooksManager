using BM.Common.Constants;

namespace BM.Common.Factories
{
    public class ConnectionStringFactory
    {
        public static class ConnectionStringsFactory
        {
            public static string GetDbContextConnectionString()
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DbConnectionString) ?? "MIGRATION";
            }
        }
    }
}
