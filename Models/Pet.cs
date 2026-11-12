using System.ComponentModel.DataAnnotations;
namespace ClinicaVet.Models;
public class Pet { public int Id { get; set; } [Required, StringLength(80)] public string Nome { get; set; } = ""; [StringLength(50)] public string? Especie { get; set; } public DateOnly? DataNascimento { get; set; } [Display(Name="Tutor")] public int TutorId { get; set; } public Tutor? Tutor { get; set; } public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>(); }
