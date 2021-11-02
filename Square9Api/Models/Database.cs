using System.Collections.Generic;

namespace Square9.QuickstartSample.Square9Api.Models
{
    public class Database
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DatabaseResponse
    {
        public List<Database> Databases { get; set; }
    }
}