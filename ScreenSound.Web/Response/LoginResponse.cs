using System;

namespace ScreenSound.Web.Response;

public record LoginResponse(bool Sucesso, string[]? Erros);