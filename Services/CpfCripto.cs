using Microsoft.AspNetCore.DataProtection;
namespace ClinicaVet.Services;
public class CpfCripto(IDataProtectionProvider provider) : ICpfCripto
{
    private readonly IDataProtector _protector = provider.CreateProtector("ClinicaVet.Cpf.v1");
    public string Proteger(string cpf) => _protector.Protect(new string(cpf.Where(char.IsDigit).ToArray()));
}
