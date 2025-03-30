using dotidentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class ReservaViewController : Controller
{
    private readonly MyUserDbContext _context;
    private readonly UserManager<MyUser> _userManager;
    private readonly ILogger<ReservaViewController> _logger;

    public ReservaViewController(MyUserDbContext context, UserManager<MyUser> userManager, ILogger<ReservaViewController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Reservar()
    {
        return View(new ReservaFormModel());
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ReservaFormModel model)
    {
        _logger.LogDebug("🚀 Entrou no método Criar");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);
            return View("Reservar", model);
        }

        var userId = _userManager.GetUserId(User);
        if(userId == null)
        {
            _logger.LogError("Utilizador não autenticado ao tentar criar reserva.");
            ModelState.AddModelError("", "Utilizador não autenticado.");
            return View("Reservar", model);
        }

        var reserva = new Reserva
        {
            DataHora = model.DataHora,
            Observacoes = model.Observacoes,
            UserId = userId
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensagem"] = "Reserva criada com sucesso!";
        _logger.LogInformation("Reserva criada com sucesso para o UserId {UserId}", userId);

        return RedirectToAction("Reservar");
    }

    [HttpGet]   
    public async Task<IActionResult> VerReservas()
    {
        var userId = _userManager.GetUserId(User);
    if (userId == null)
    {
        _logger.LogError("Utilizador não autenticado ao tentar ver reservas.");
        return Unauthorized();
    }

    var reservas = await _context.Reservas
        .Where(r => r.UserId == userId)
        .OrderByDescending(r => r.DataHora)
        .ToListAsync();

    return View(reservas);
    }

    [HttpPost]
    public async Task<IActionResult> CancelarReserva(int id)
    {
        var userId = _userManager.GetUserId(User);
        var reserva = await _context.Reservas
        .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (reserva == null)
        {
            _logger.LogWarning("Reserva não encontrada ou acesso negado. ID: {Id}", id);
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensagem"] = "Reserva cancelada com sucesso!";
        _logger.LogInformation("Reserva cancelada com sucesso para o UserId {UserId}", userId);

            return RedirectToAction("VerReservas"); 
    }


}
