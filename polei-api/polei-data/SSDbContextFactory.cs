using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace polei_data
{
    public class SSDbContextFactory : IDesignTimeDbContextFactory<SSDbContext>
    {
        public SSDbContext CreateDbContext(string[] args)
        {
            return new SSDbContext();
        }
    }
}
