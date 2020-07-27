using SQLite;

namespace Finances.Service
{
    public class SqlService : ISqlService
    {
        public SQLiteConnection Factory()
        {
            return new SQLiteConnection(App.DataBasePath);
        }
    }

    public interface ISqlService
    {
        SQLiteConnection Factory();
    }
}