namespace WebApiRPG.DTOs;

public class AttackResultDTO
{
    public int AttackerId { get; set; }
    public int TargetId { get; set; }
    public double Damage { get; set; }
    public double TargetHealth { get; set; }
}
