using System;
using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record GeneroRequest([Required] string Nome, [Required] string Descricao);
