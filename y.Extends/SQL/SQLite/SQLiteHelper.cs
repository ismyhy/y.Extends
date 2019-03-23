using System.Runtime.InteropServices.ComTypes;

using y.Extends.SQL.SQLite.DLLs;

namespace y.Extends.SQL.SQLite
{
    public static class SQLiteHelper
    {
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            DllLoader.Loader();
            new InitializeConfig().Initialize();
        }


        public static TContext CreateContext<TContext>() where TContext : SQLiteDatabase<TContext>, new()
        { 
            return new TContext(); 
        }


    }
}