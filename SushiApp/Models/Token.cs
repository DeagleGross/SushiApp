using System;
using SQLite;

namespace SushiApp.Models
{
    public class Token
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string access_token { get; set; }
        public string error_description { get; set; }
        public DateTime expire_date { get; set; }

        // constructor by default
        public Token(){}
    }
}
