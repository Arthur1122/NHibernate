using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace NhibernateTutorial
{
    public class Student
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
    }

    public class StudentMap : ClassMapping<Student>
    {
        public StudentMap()
        {
            Id(p => p.ID,
              map =>
              {
                  map.Generator(Generators.Identity);
              });
            Property(c => c.Age);
            Property(c => c.Name, x => x.Length(100));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var conn  = "Server=localhost;Port=3307;Database=Student;Uid=*******;Pwd=******";
            var mapper = new ModelMapper();
            mapper.AddMapping<StudentMap>();
            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var Configuration = new Configuration();
            Configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = conn;
                db.Dialect<MySQL5InnoDBDialect>();
                db.Driver<MySqlDataDriver>();
            })
                .AddMapping(domainMapping);
            Configuration.SessionFactory().GenerateStatistics();

            var SessionFactory = Configuration.BuildSessionFactory();
            SessionFactory.OpenSession();
        }
    }
}
