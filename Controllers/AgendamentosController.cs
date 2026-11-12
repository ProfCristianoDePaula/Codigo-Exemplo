using ClinicaVet.Data; using ClinicaVet.Models; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.AspNetCore.Mvc.Rendering; using Microsoft.EntityFrameworkCore;
namespace ClinicaVet.Controllers;
[Authorize] public class AgendamentosController(ClinicaContext db):Controller
{
 public async Task<IActionResult> Index()=>View(await db.Agendamentos.AsNoTracking().Include(x=>x.Pet).ThenInclude(x=>x!.Tutor).OrderBy(x=>x.DataHora).ToListAsync());
 public async Task<IActionResult> Create(){await Pets();return View(new Agendamento{DataHora=DateTime.Now});}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Agendamento m){if(!ModelState.IsValid){await Pets(m.PetId);return View(m);}db.Agendamentos.Add(m);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Edit(int id){var x=await db.Agendamentos.FindAsync(id);if(x is null)return NotFound();await Pets(x.PetId);return View(x);}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Agendamento m){if(id!=m.Id)return NotFound();if(!ModelState.IsValid){await Pets(m.PetId);return View(m);}db.Entry(m).Property(x=>x.DataHora).IsModified=true;db.Entry(m).Property(x=>x.Servico).IsModified=true;db.Entry(m).Property(x=>x.PetId).IsModified=true;await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Delete(int id){var x=await db.Agendamentos.AsNoTracking().Include(x=>x.Pet).FirstOrDefaultAsync(x=>x.Id==id);return x is null?NotFound():View(x);}
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await db.Agendamentos.FindAsync(id);if(x is not null){db.Remove(x);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
 private async Task Pets(int? id=null)=>ViewData["PetId"]=new SelectList(await db.Pets.OrderBy(x=>x.Nome).ToListAsync(),"Id","Nome",id);
}
