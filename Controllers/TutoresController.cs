using ClinicaVet.Data; using ClinicaVet.Models; using ClinicaVet.Services; using ClinicaVet.ViewModels; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace ClinicaVet.Controllers;
[Authorize] public class TutoresController(ClinicaContext db, ICpfCripto cpf) : Controller
{
 public async Task<IActionResult> Index(string? termo) { var q=db.Tutores.AsNoTracking().AsQueryable(); if(!string.IsNullOrWhiteSpace(termo)) q=q.Where(x=>x.Nome.Contains(termo)||x.Email.Contains(termo)); ViewData["Termo"]=termo; return View(await q.OrderBy(x=>x.Nome).ToListAsync()); }
 public IActionResult Create()=>View(new TutorFormViewModel());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(TutorFormViewModel m) { if(!ModelState.IsValid)return View(m); db.Tutores.Add(new Tutor{Nome=m.Nome.Trim(),Email=m.Email.Trim(),Telefone=m.Telefone,CpfCriptografado=cpf.Proteger(m.Cpf)}); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
 public async Task<IActionResult> Edit(int id) { var x=await db.Tutores.FindAsync(id); if(x is null)return NotFound(); return View(new TutorFormViewModel{Id=x.Id,Nome=x.Nome,Email=x.Email,Telefone=x.Telefone}); }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,TutorFormViewModel m) { if(id!=m.Id)return NotFound(); if(!ModelState.IsValid)return View(m); var x=await db.Tutores.FindAsync(id); if(x is null)return NotFound(); x.Nome=m.Nome.Trim();x.Email=m.Email.Trim();x.Telefone=m.Telefone;if(!string.IsNullOrWhiteSpace(m.Cpf))x.CpfCriptografado=cpf.Proteger(m.Cpf); await db.SaveChangesAsync();return RedirectToAction(nameof(Index)); }
 public async Task<IActionResult> Delete(int id) { var x=await db.Tutores.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);return x is null?NotFound():View(x); }
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id) { var x=await db.Tutores.FindAsync(id);if(x is null)return NotFound();try{db.Tutores.Remove(x);await db.SaveChangesAsync();}catch(DbUpdateException){TempData["Erro"]="Este tutor possui pets e não pode ser excluído.";}return RedirectToAction(nameof(Index)); }
}
