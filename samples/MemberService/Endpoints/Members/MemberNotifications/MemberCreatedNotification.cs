namespace MemberService.Endpoints.Members.MemberNotifications;

internal record MemberCreatedNotification(string Name, string Email, string? CellPhone) : INotification;
