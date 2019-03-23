using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

using y.Extends.SQL;
using y.Extends.SQL.SQLite;
using y.Extends.SQL.SQLite.Attrbutes;

namespace UnitTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SQLiteHelper.Initialize();

            using (var ere = new Ere())
            {
                ere.T44t1s.Add(new T44t1
                {
                    Organization = "4343",
                    DatetimeLastUpdate = DateTime.Now.ToLongDateString()
                });
                ere.SaveChanges();

                var ll = ere.T44t1s.ToList();
            }
        }
    }

    [Table("testTable")]
    public class T44t1 : EntityBase
    {
        [MaxLength(100)]
        public string DatetimeLastUpdate { get; set; }

        [Autoincrement]
        [Key]
        [NotNull] 
        public int Id { get; set; }

        [NotNull]
        [Column] 
        public bool IsDeleted { get; set; }

        [Ignore]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Organization { get; set; }
    }

    public class Ere : SQLiteDatabase <Ere>
    {
        public DbSet <T44t1> T44t1s { get; set; }

        public override void ExcuteInitialize()
        {
            WaitExcuteInitialize();
        }
    }
}