using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite.EF6.Migrations;

namespace y.Extends.SQL.SQLite
{
    public abstract class SQLiteDatabase<T> : DbContext where T: DbContext
    {
        private DbModelBuilder _modelBuilder;

        protected SQLiteDatabase() : base("SQLiteConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion <T, Configuration <T>>());
            //Database.SetInitializer<DbContext>(null);
        }
         
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
            ExcuteInitialize();
            base.OnModelCreating(modelBuilder);
        }

        public abstract void ExcuteInitialize();

        public void WaitExcuteInitialize()
        {
            _modelBuilder.Conventions.Remove <PluralizingTableNameConvention>();
            _modelBuilder.Configurations.AddFromAssembly(typeof (T).Assembly);
        }
    }

    public class Configuration <T> : DbMigrationsConfiguration <T> where T : DbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(T context)
        {
        }
    }
}