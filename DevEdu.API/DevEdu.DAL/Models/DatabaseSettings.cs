using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DevEdu.DAL.Models
{
    public class DatabaseSettings 
    {
        public DatabaseSettings(){ }
        public  string ConnectionString { get; set; }
        
    }
}
