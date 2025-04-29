using OpenTelemetry;
using System.Diagnostics;

namespace ThreadLike.Common.Infrastructure.OpenTelemetryExtend
{
	public class ExternalServiceProcessor : BaseProcessor<Activity>
	{
		public override void OnEnd(Activity activity)
		{
			// For PostgreSQL
			if (activity.Tags.Any(tag => tag.Key == "db.system" && tag.Value == "postgresql"))
			{
				activity.SetTag("service.name", "PostgreSQL");
			}

			// For Redis
			if (activity.Tags.Any(tag => tag.Key == "db.system" && tag.Value == "redis"))
			{
				activity.SetTag("service.name", "Redis");
			}
			if(activity.Tags.Any(tag => tag.Key == "job"))
			{
				// This is a job, so we don't want to process it traces, it will dilute the traces
				return;
			}
			base.OnEnd(activity);
		}
	}
}
