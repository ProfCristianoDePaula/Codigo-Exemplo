using System.ComponentModel.DataAnnotations;
namespace ClinicaVet.Models;
public class Usuario { public int Id { get; set; } [Required, StringLength(80)] public string Nome { get; set; } = ""; [Required, EmailAddress, StringLength(160)] public string Email { get; set; } = ""; [Required] public string SenhaHash { get; set; } = ""; }
