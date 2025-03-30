using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotidentity.Models;
using dotidentity;

public class AdminController : Controller
{
    private readonly MyUserDbContext _context;

    public AdminController(MyUserDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var model = new AdminViewModel
        {
            Barbeiros = _context.Barbeiros.ToList(),
            Servicos = _context.Servicos.ToList()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddBarbeiro(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome))
        {
            _context.Barbeiros.Add(new Barbeiro { Nome = nome });
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBarbeiro(int id)
    {
        var barbeiro = await _context.Barbeiros.FindAsync(id);
        if (barbeiro != null)
        {
            _context.Barbeiros.Remove(barbeiro);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddTipoDeCorte(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome))
        {
            _context.Servicos.Add(new Servico { Nome = nome });
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTipoDeCorte(int id)
    {
        var servico = await _context.Servicos.FindAsync(id);
        if (servico != null)
        {
            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
