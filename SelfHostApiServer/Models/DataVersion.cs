using System;

namespace SelfHostApiServer.Models
{
    public class DataVersion
    {
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

		public override string ToString()
		{
            if (this == null)
                return "wtf";
            return $"{this.TypeName}={this.Version}\n";
		}
	}
}
