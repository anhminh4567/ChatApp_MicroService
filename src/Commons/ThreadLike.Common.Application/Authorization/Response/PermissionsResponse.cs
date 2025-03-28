namespace ThreadLike.Common.Application.Authorization.Response;

public sealed record PermissionsResponse(string UserId, HashSet<string> Permissions);
