using ClinicaVet.Data; using ClinicaVet.Models; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.AspNetCore.Mvc.Rendering; using Microsoft.EntityFrameworkCore;
namespace ClinicaVet.Controllers;
[Authorize] public class PetsController(ClinicaContext db):Controller
{
 public async Task<IActionResult> Index()=>View(await db.Pets.AsNoTracking().Include(x=>x.Tutor).OrderBy(x=>x.Nome).ToListAsync());
 public async Task<IActionResult> Create(){await Tutores();return View(new Pet());}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Pet m){if(!ModelState.IsValid){await Tutores(m.TutorId);return View(m);}db.Pets.Add(m);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Edit(int id){var x=await db.Pets.FindAsync(id);if(x is null)return NotFound();await Tutores(x.TutorId);return View(x);}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Pet m){if(id!=m.Id)return NotFound();if(!ModelState.IsValid){await Tutores(m.TutorId);return View(m);}db.Entry(m).Property(x=>x.Nome).IsModified=true;db.Entry(m).Property(x=>x.Especie).IsModified=true;db.Entry(m).Property(x=>x.DataNascimento).IsModified=true;db.Entry(m).Property(x=>x.TutorId).IsModified=true;await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Delete(int id){var x=await db.Pets.AsNoTracking().Include(x=>x.Tutor).FirstOrDefaultAsync(x=>x.Id==id);return x is null?NotFound():View(x);}
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await db.Pets.FindAsync(id);if(x is null)return NotFound();try{db.Pets.Remove(x);await db.SaveChangesAsync();}catch(DbUpdateException){TempData["Erro"]="Este pet possui agendamentos e não pode ser excluído.";}return RedirectToAction(nameof(Index));}
 private async Task Tutores(int? id=null)=>ViewData["TutorId"]=new SelectList(await db.Tutores.OrderBy(x=>x.Nome).ToListAsync(),"Id","Nome",id);
}
