using System.ComponentModel.DataAnnotations;
namespace ClinicaVet.Models;
public class Agendamento { public int Id { get; set; } [Required, Display(Name="Data e hora"), DataType(DataType.DateTime)] public DateTime DataHora { get; set; } [Required, StringLength(300)] public string Servico { get; set; } = ""; [Display(Name="Pet")] public int PetId { get; set; } public Pet? Pet { get; set; } }
