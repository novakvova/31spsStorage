using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStorage.Entities;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using TestStorage.ViewModel;

namespace TestStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EFContext context = new EFContext())
            {
                //Console.WriteLine("Begin Seed");
                //SeedDB(context);
                //Console.WriteLine("End Seed");
                //ReadDataEFContext(context);

                Console.WriteLine("---------Read EFContext-----------");
                ReadDataEFContext(context, 0, 50000);
                Console.WriteLine("---------Read Storage-----------");
                GetDataSQLStorage(context,20000,50000);
                Console.WriteLine("---------Read EFContext-----------");
                ReadDataEFContext(context, 60000, 50000);
                Console.WriteLine("---------Read Storage-----------");
                GetDataSQLStorage(context, 0, 50000);
                Console.WriteLine("---------Read EFContext-----------");
                ReadDataEFContext(context, 10000, 50000);
                Console.WriteLine("---------Read Storage-----------");
                GetDataSQLStorage(context, 10000, 50000);
            }
        }
        static void GetDataSQLStorage(EFContext context, int skip, int take)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Begin red");
            var jsonOut = new SqlParameter();
            jsonOut.ParameterName = "@JSONOutput";
            jsonOut.Direction = ParameterDirection.Output;
            jsonOut.SqlDbType = SqlDbType.NVarChar;
            jsonOut.Size = -1;
            string sql = @"EXEC [dbo].[spGetRangeUers]
                                @From,
                        		@Take,
                                @JSONOutput out";
            context.Database.ExecuteSqlCommand(sql,
                new SqlParameter("@From", Convert.ToInt32(0)),
                new SqlParameter("@Take", 100),
                jsonOut);
            var data = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonOut.Value.ToString());
            //Console.WriteLine(jsonOut.Value);
            Console.WriteLine("End read: ");
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds );
            Console.WriteLine("RunTime " + elapsedTime);
        }
        static void ReadDataEFContext(EFContext context, int skip, int take)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Begin red");
            var query = context.Users
                .Include(u => u.UserImages)
                .Select(u => new UserViewModel
                {
                    UserId= u.Id,
                    UserName= u.FirstName,
                    UserSex= u.Sex,
                    UserAge= u.Age,
                    UserEmail= u.Email,
                    Images = u.UserImages.Select(g => new UserImageViewModel
                    {
                        Id=g.Id,
                        Name=g.Name
                    }).ToList()
                });
            query = query.OrderBy(u => u.UserId).Skip(10).Take(20000);
            int i = 1;
            foreach (var user in query)
            {
                var data = user;
                //Console.WriteLine("Count: {0}", i);
                i++;
            }
            Console.WriteLine("End read: {0}", i);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
        }
        static void SeedDB(EFContext context)
        {
            var userGenerator = new Faker<User>()
                                    .StrictMode(false)
                                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                                    .RuleFor(u => u.Email, f => f.Internet.Email())
                                    .RuleFor(u => u.Age, f => f.Random.Number(20, 35))
                                    .RuleFor(u => u.Sex, f => f.Random.Bool());

            context.Users.AddRange(userGenerator.Generate(250000).ToArray());
            context.SaveChanges();

            for (int j=0;j<10;j++)
            {
                var imageGenerator = new Faker<UserImage>()
                                    .StrictMode(false)
                                    .RuleFor(i => i.Name, f => Guid.NewGuid().ToString()+".jpg")
                                    //.RuleFor(i => i.Id, f => imageId++)
                                    .RuleFor(i => i.UserId, f => f.Random.Number(1, 250000));
                context.UserImages.AddRange(imageGenerator.Generate(75000).ToArray());
                context.SaveChanges();
            }
            

        }
    }
}
