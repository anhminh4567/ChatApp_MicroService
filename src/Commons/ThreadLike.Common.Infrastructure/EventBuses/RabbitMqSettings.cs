﻿namespace ThreadLike.Common.Infrastructure.EventBuses;
public class RabbitMqSettings
{
	public string Host { get; set; }
	public string Username { get; set; } = "guest";
	public string Password { get; set; } = "guest";
}
