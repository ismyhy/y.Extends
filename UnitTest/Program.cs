using System;
using System.Collections.Generic;
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
                    Organization = "22243",
                    DatetimeLastUpdate = DateTime.Now.ToLongDateString(),
                    TestListClass = new TestListClass
                    {
                        Organization = "444444",
                        DatetimeLastUpdate = DateTime.Now.ToLongDateString()
                    },
                    T44t2List = new List <TestListClass2>
                    {
                        new TestListClass2
                        {
                            Organization = "55555",
                            DatetimeLastUpdate = DateTime.Now.Ticks.ToString()
                        },

                        new TestListClass2
                        {
                            Organization = "6666",
                            DatetimeLastUpdate = DateTime.Now.Ticks.ToString()
                        },
                        new TestListClass2
                        {
                            Organization = "777",
                            DatetimeLastUpdate = DateTime.Now.Ticks.ToString()
                        }
                    }
                });
                ere.SaveChanges();

                var ll = ere.T44t1s.ToList();
                var t1 = new T44t1();

                ll.First().CopyTo(t1);
            var y11=    t1.T44t2List.ToList();
                var t2 = ll.Last().TestListClass;
            }
        }
    }

    [Table("testTable")]
    public class T44t1 : EntityBase<T44t1>
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

        public int T44t2Id { get; set; }

        [ForeignKey("T44t2Id")]
        public virtual TestListClass TestListClass { get; set; }

        //[Ignore]
        [ForeignKey("T44t1Id")]
        public virtual ICollection <TestListClass2> T44t2List { get; set; }
    }

    [Table("testTable2")]
    public class TestListClass : EntityBase<TestListClass>
    {
        //[ForeignKey("T44t1")]
        //public int T44t1Id { get; set; }
        //[ForeignKey("T44t1Id")]
        //public virtual T44t1 T44t1 { get; set; }

        [MaxLength(100)]
        public string DatetimeLastUpdate { get; set; }

        [Autoincrement]
        [Key]
        [NotNull]
        public int T44t2Id { get; set; }

        [NotNull]
        [Column]
        public bool IsDeleted { get; set; }

        [Ignore]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Organization { get; set; }

        //public int Id100 { get; set; }
        
    }


    [Table("testTable3")]
    public class TestListClass2 : EntityBase<TestListClass2>
    {
        [ForeignKey("T44t1")]
        public int T44t1Id { get; set; }
        [ForeignKey("T44t1Id")]
        public virtual T44t1 T44t1 { get; set; }

        [MaxLength(100)]
        public string DatetimeLastUpdate { get; set; }

        [Autoincrement]
        [Key]
        [NotNull]
        public int T44t2Id { get; set; }

        [NotNull]
        [Column]
        public bool IsDeleted { get; set; }

        [Ignore]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Organization { get; set; }

        //public int Id100 { get; set; }

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