using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStorage.Entities;
using System.Data.Entity;
using System.Diagnostics;

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
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Console.WriteLine("Begin red");
                var query = context.Users
                    .Include(u => u.UserImages)
                    .Select(u=>new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Sex,
                        u.Email,
                        u.Age,
                        Images = u.UserImages.Select(g=>g)
                    });
                query = query.OrderBy(u => u.Id).Skip(10).Take(20000);
                int i = 1;
                foreach (var user in query)
                {
                    var data = user;
                    //Console.WriteLine("Count: {0}", i);
                    i++;
                }
                Console.WriteLine("End read: {0}",i);
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
            }
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
