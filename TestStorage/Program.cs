using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStorage.Entities;

namespace TestStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EFContext context = new EFContext())
            {
                Console.WriteLine("Begin Seed");
                SeedDB(context);
                Console.WriteLine("End Seed");
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
