using y.Extends.SQL.SQLite.DLLs;

namespace y.Extends.SQL.SQLite
{
    public static class SQLiteHelper
    {
        private static bool Initialized;

        public static void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            Initialized = true;
            DllLoader.Loader();
            new InitializeConfig().Initialize();
        }


    }
}