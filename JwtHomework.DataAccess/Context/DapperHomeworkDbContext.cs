using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace JwtHomework.DataAccess
{
    public class DapperHomeworkDbContext
    {

        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DapperHomeworkDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            //Appsettings.json içerisindeki connectionString icerisindeki "PostgreSql" degerini okuyoruz ve connectionString atıyoruz.
            connectionString = this.configuration.GetConnectionString("PosgreSql");
        }

        public IDbConnection CreateConnection()
        {

            return new NpgsqlConnection(connectionString);
        }
    }
}
