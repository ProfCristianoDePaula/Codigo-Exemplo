using System.ComponentModel.DataAnnotations;
namespace ClinicaVet.ViewModels;
public class TutorFormViewModel { public int Id { get; set; } [Required, StringLength(100)] public string Nome { get; set; } = ""; [Required, EmailAddress, StringLength(160)] public string Email { get; set; } = ""; [Required, StringLength(14), Display(Name="CPF"), RegularExpression(@"^(?:\d{11}|\d{3}\.\d{3}\.\d{3}-\d{2})$", ErrorMessage="Informe um CPF com 11 dígitos.")] public string Cpf { get; set; } = ""; [StringLength(20)] public string? Telefone { get; set; } }
