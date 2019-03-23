using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using y.Extends.SQL.SQLite;
using y.Extends.SQL.SQLite.Attrbutes;

namespace UnitTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SQLiteHelper.Initialize();

            using (var ere = new ere())
            {
                ere.t44t1s.Add(new t44t1()
                {
                    Organization="4343",
                });
                ere.SaveChanges();
            }


        }
    }

    public class t44t1  
    {
        public string DatetimeLastUpdate { get; set; }

        [Autoincrement]
        [Key]
        public int Id { get; set; }

        [NotNull]
        public bool IsDeleted { get; set; }

        [IgnoreColumn]
        public string Name { get; set; }

        public string Organization { get; set; }
    }


    public class ere : SQLiteDatabase<ere>
    {
        public DbSet <t44t1> t44t1s { get; set; }

        public override void ExcuteInitialize()
        {
            WaitExcuteInitialize();
        }
    }


}