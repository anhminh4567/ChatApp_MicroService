using Microsoft.EntityFrameworkCore;
using ThreadLike.Chat.Domain.Reactions;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Domain.Ultils;

namespace ThreadLike.Chat.Api.Extensions
{
	public static class Seeding
	{
		public static void SeedIcons(this IApplicationBuilder app)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();
			ChatDbContext dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

			string folderPath = "./Medias/";
			if (Directory.Exists(folderPath))
			{
				string[] files = Directory.GetFiles(folderPath);
				foreach (string file in files)
				{
					string content = File.ReadAllText(file);
					string extension = Path.GetExtension(file);
					string? mimeType = FileUtils.GetMimeTypeFromExtension(extension);
					string fileName = Path.GetFileNameWithoutExtension(file);
					if (mimeType == null)
					{
						continue;
					}
					var reaction = Reaction.Create(content,fileName,mimeType);
					reaction.SetId(fileName);

					bool exist = dbContext.Reactions.AnyAsync(x => x.Id == reaction.Id).Result;
					if(!exist)
					{
						dbContext.Reactions.Add(reaction);
					}

				}
			}
			dbContext.SaveChanges();

		}
	}
}
