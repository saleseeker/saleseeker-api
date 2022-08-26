using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace saleseeker_data
{
    public class SSDbContextFactory : IDesignTimeDbContextFactory<SSDbContext>
    {
        public SSDbContext CreateDbContext(string[] args)
        {
            return new SSDbContext();
        }
    }
}
