using Microsoft.AspNetCore.Mvc;

namespace ThreadLike.Chat.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		[HttpGet(Name = "GetDateTimeNow")]
		public Task<IResult> GetDateTimeNow()
		{
			return Task.FromResult(Results.Ok(DateTime.UtcNow));
		}
	}
}
