using CarsApi.Models;
using CarsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
	[Route("entities")]
	[ApiController]
	public class EntitiesController : UmbracoApiController
	{

		private readonly IContentService _contentService;
		private readonly IPublishedContentQuery _publishedContentQuery;
		private readonly IFakeDataService _fakeDataService;

		public EntitiesController(IContentService contentTypeService, IPublishedContentQuery publishedContentQuery, IFakeDataService fakeDataService)
		{
			_contentService = contentTypeService;
			_publishedContentQuery = publishedContentQuery;
			_fakeDataService = fakeDataService;
		}

		[HttpPost("fake/{quantity}")]
		public IActionResult CreateFakeData(int quantity)
		{
			var cars = _fakeDataService.CreateFakeEntities<CarModel>(quantity);

			foreach (var car in cars)
			{
				CreateEntity(car);
			}

			return Ok(cars);
		}

		[HttpGet]
		public IActionResult GetEntities()
		{

			var root = _publishedContentQuery.Content(1064);

			if (root is null)
				return NotFound();

			var entities = root.Children().Select(i => new
			{
				Brand = i.Name,
				Icon = i.Value<IPublishedContent>("icon")!.Url(),
				FoundationDate = i.Value<DateTime>("foundationDate"),
				OriginCountry = i.Value<string>("originCountry"),
				Description = i.Value<string>("Description"),
				Models = i.Children().Select(i => new CarModel
				{
					Name = i.Name,
					Image = i.Value<IPublishedContent>("image")!.Url(),
					BrandName = i.Ancestor()!.Name,
					LaunchDate = i.Value<DateTime>("launchDate"),
					Url = i.Url(),
					Description = i.Value<string>("description")!,
					Price = i.Value<decimal>("price")
				})

			});

			return Ok(entities);
		}

		[HttpGet("{id}")]
		public IActionResult GetEntity(int id)
		{
			var entity = _publishedContentQuery.Content(id);

			if (entity is null)
				return NotFound();

			CarModel result = new CarModel
			{
				Name = entity.Name,
				BrandName = entity.Ancestor()!.Name,
				Image = entity.Value<IPublishedContent>("image")!.Url(),
				LaunchDate = entity.Value<DateTime>("launchDate"),
				Description = entity.Value<string>("description")!,
				Price = entity.Value<decimal>("price")
			};

			return Ok(result);
		}

		[HttpPost]
		public IActionResult CreateEntity(CarModel model)
		{
			var parentId = Guid.Parse("4533f42c-b5ef-4451-8084-ac130b209b1e");

			IContent content = _contentService.Create(model.Name, parentId, "carModel");

			content.SetValue("image", model.Image);
			content.SetValue("description", model.Description);
			content.SetValue("launchDate", model.LaunchDate);
			content.SetValue("price", model.Price);

			_contentService.SaveAndPublish(content);

			return Ok(content.Id);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateEntity(int id, CarModel model)
		{
			var entity = _contentService.GetById(id);

			if (entity is null)
				return NotFound();

			entity.Name = model.Name;
			entity.SetValue("image", model.Image);
			entity.SetValue("description", model.Description);
			entity.SetValue("launchDate", model.LaunchDate);
			entity.SetValue("price", model.Price);

			_contentService.SaveAndPublish(entity);

			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteEntity(int id)
		{
			var entity = _contentService.GetById(id);

			if (entity is null)
				return NotFound();

			_contentService.Delete(entity);

			return Ok();
		}
	} 
}
