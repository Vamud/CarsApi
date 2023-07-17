using Bogus;
using CarsApi.Models;
using CarsApi.Services.Interfaces;

namespace CarsApi.Services
{
	public class FakeDataService : IFakeDataService
	{
		public IEnumerable<T> CreateFakeEntities<T>(int quantity)
			where T : CarModel
		{
			var faker = new Faker<T>();

			faker.RuleFor(c => c.Name, f => f.Vehicle.Model());
			faker.RuleFor(c => c.Image, f => f.Image.PicsumUrl());
			faker.RuleFor(c => c.LaunchDate, f => f.Date.Past(5));
			faker.RuleFor(c => c.Description, f => f.Lorem.Paragraph(5));
			faker.RuleFor(c => c.Price, f => f.Random.Decimal(10000, 50000));

			var entities = faker.Generate(quantity);

			return entities;
		}
	}
}
