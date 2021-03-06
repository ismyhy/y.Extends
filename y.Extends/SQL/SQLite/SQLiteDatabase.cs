﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite.EF6.Migrations;
using System.Linq;

namespace y.Extends.SQL.SQLite
{
    public abstract class SQLiteDatabase <T> : DbContext where T : DbContext
    {
        private DbModelBuilder _modelBuilder;

        protected SQLiteDatabase(bool lazyLoading = true) : base("SQLiteConnectionString")
        {
            Configuration.LazyLoadingEnabled = lazyLoading;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion <T, Configuration <T>>());

            //Database.SetInitializer<DbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
            ExcuteInitialize();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        ///     方法实现之后需要执行 WaitExcuteInitialize() 方法来初始化数据
        ///     <see cref="WaitExcuteInitialize" />
        /// </summary>
        public abstract void ExcuteInitialize();

        public void WaitExcuteInitialize()
        {
            _modelBuilder.Conventions.Remove <PluralizingTableNameConvention>();
            _modelBuilder.Configurations.AddFromAssembly(typeof (T).Assembly);
        }

        /// <summary>
        ///     保存修改项
        ///     <para>参数为 true 表示：在执行保存之后释放该数据上下文</para>
        ///     <para>参数为 false 表示：在执行保存之后数据上下文不进行释放</para>
        /// </summary>
        /// <param name="releaseAfterSaveChanges">在执行保存之后是否释放该数据上下文</param>
        /// <returns></returns>
        public int SaveChanges(bool releaseAfterSaveChanges = false)
        {
            try
            {
                bool saveFailed;
                var result = 0;
                do
                {
                    saveFailed = false;
                    try
                    {
                        result = base.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        ex.Entries.Single().Reload();
                    }
                } while (saveFailed);

                return result;
            }
            finally
            {
                if (releaseAfterSaveChanges)
                {
                    Dispose();
                }
            }
        }
    }

    internal class Configuration <T> : DbMigrationsConfiguration <T> where T : DbContext
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