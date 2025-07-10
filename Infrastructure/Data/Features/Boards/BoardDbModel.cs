using System.ComponentModel.DataAnnotations;
using Infrastructure.Data.Features.Defects;

namespace Infrastructure.Data.Features.Boards;

public class BoardDbModel
{
    public int Id { get; set; }
    public int Index { get; set; }
    [MaxLength(255)] public string SerialNumber { get; set; } = "";
    public List<DefectDbModel> Defects { get; set; } = [];
}