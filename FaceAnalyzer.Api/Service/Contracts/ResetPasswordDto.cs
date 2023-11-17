namespace FaceAnalyzer.Api.Service.Contracts;

public record ResetMyPasswordDto(string NewPassword);

public record ResetUserPasswordDto(int UserId, string NewPassword);