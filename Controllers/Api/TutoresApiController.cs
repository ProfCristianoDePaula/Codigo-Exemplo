using ClinicaVet.Data; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace ClinicaVet.Controllers.Api;
[ApiController,Route("api/tutores"),Authorize] public class TutoresApiController(ClinicaContext db):ControllerBase
{
 [HttpGet] public async Task<IActionResult> Get()=>Ok(await db.Tutores.AsNoTracking().OrderBy(x=>x.Nome).Select(x=>new{x.Id,x.Nome,x.Email,x.Telefone}).ToListAsync());
 [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id){var x=await db.Tutores.AsNoTracking().Where(x=>x.Id==id).Select(x=>new{x.Id,x.Nome,x.Email,x.Telefone}).FirstOrDefaultAsync();return x is null?NotFound():Ok(x);}
}
