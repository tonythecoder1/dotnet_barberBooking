using dotidentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public async Task<IActionResult> Reservar()
    {
        var model = new ReservaFormModel
        {
            Barbeiros = await _context.Barbeiros
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Nome })
                .ToListAsync(),

            Servicos = await _context.Servicos
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nome })
                .ToListAsync(),

            HorasDisponiveis = new List<SelectListItem>()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ReservaFormModel model)
    {
        _logger.LogDebug("🚀 Entrou no método Criar");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);
            model.Barbeiros = _context.Barbeiros.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Nome }).ToList();
            model.Servicos = _context.Servicos.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nome }).ToList();
            model.HorasDisponiveis = new List<SelectListItem>();
            return View("Reservar", model);
        }

        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            _logger.LogError("Utilizador não autenticado ao tentar criar reserva.");
            ModelState.AddModelError("", "Utilizador não autenticado.");
            model.Barbeiros = _context.Barbeiros.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Nome }).ToList();
            model.Servicos = _context.Servicos.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nome }).ToList();
            model.HorasDisponiveis = new List<SelectListItem>();
            return View("Reservar", model);
        }

        var dataHoraFinal = model.DataHora.Date.Add(TimeSpan.Parse(model.HoraSelecionada));

        var existe = await _context.Reservas.AnyAsync(r =>
            r.DataHora == dataHoraFinal && r.BarbeiroId == model.BarbeiroId);

        if (existe)
        {
            _logger.LogWarning("Horário já reservado para o barbeiro selecionado.");
            ModelState.AddModelError("", "Esse horário já está reservado para o barbeiro selecionado.");
            model.Barbeiros = _context.Barbeiros.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Nome }).ToList();
            model.Servicos = _context.Servicos.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nome }).ToList();
            model.HorasDisponiveis = new List<SelectListItem>();
            return View("Reservar", model);
        }

        var reserva = new Reserva
        {
            DataHora = dataHoraFinal,
            Observacoes = model.Observacoes,
            UserId = userId,
            BarbeiroId = model.BarbeiroId,
            ServicoId = model.ServicoId
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
            .Include(r => r.Barbeiro)
            .Include(r => r.servico)
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

    [HttpGet]
    public async Task<IActionResult> HorariosDisponiveis(DateTime data, int barbeiroId)
    {
        var horariosFixos = new List<string>
        {
            "10:00", "10:30", "11:00", "11:30",
            "14:00", "14:30", "15:00", "15:30",
            "16:00", "16:30", "17:00", "17:30",
            "18:00", "18:30"
        };

        var reservasDoDia = await _context.Reservas
            .Where(r => r.DataHora.Date == data.Date && r.BarbeiroId == barbeiroId)
            .ToListAsync();

        var horariosIndisponiveis = reservasDoDia
            .Select(r => r.DataHora.ToString("HH:mm"))
            .ToList();

        var horariosDisponiveis = horariosFixos
            .Where(h => !horariosIndisponiveis.Contains(h))
            .ToList();

        return Json(horariosDisponiveis);
    }
}
