using System.ComponentModel.DataAnnotations;
namespace ClinicaVet.ViewModels;
public class PrimeiroAcessoViewModel
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(160, ErrorMessage = "O e-mail deve ter no máximo 160 caracteres.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Informe a senha.")]
    [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = "";

    [Required(ErrorMessage = "Confirme a senha.")]
    [Compare(nameof(Senha), ErrorMessage = "As senhas não coincidem.")]
    [DataType(DataType.Password), Display(Name = "Confirmar senha")]
    public string ConfirmarSenha { get; set; } = "";
}
