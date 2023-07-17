using CarsApi.Models;

namespace CarsApi.Services.Interfaces
{
	public interface IFakeDataService
	{
		IEnumerable<T> CreateFakeEntities<T>(int quantity)
			where T : CarModel;
	}
}
