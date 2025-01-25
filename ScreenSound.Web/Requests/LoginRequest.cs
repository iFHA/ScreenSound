using System;
using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests;

public record LoginRequest([Required] string email, [Required] string password);
