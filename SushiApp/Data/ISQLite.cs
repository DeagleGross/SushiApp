using System;
using SQLite;

namespace SushiApp.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
