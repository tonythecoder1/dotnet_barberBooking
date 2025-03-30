namespace dotidentity.Models;

public class AdminViewModel
{
    public List<Barbeiro> Barbeiros { get; set; } = new();
    public List<Servico> Servicos { get; set; } = new();
}
