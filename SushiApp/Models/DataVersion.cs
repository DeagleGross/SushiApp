using System;
using SQLite;

namespace SushiApp.Data
{
    public class DataVersion
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Version { get; set; }

        // constructor by default
        public DataVersion() { }

        public DataVersion(string typename, string version)
        {
            TypeName = typename;
            Version = version;
        }

        public DataVersion(string typename, string version, int id)
        {
            TypeName = typename;
            Version = version;
            Id = id;
        }
    }
}
