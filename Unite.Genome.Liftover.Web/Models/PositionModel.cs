using System.ComponentModel.DataAnnotations;

namespace Unite.Genome.Liftover.Web.Models;

public class PositionModel
{
    [Required]
    [RegularExpression(@"^(([1-9]|1[0-9]|2[0-2])|X|Y|MT)$")]
    public string Chromosome { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Start { get; set; }

    [Range(0, int.MaxValue)]
    public int? End { get; set; }
}
