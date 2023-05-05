using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Cast")]
public class ImportCastDto
{
    [XmlElement("FullName")]
    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string FullName { get; set; }

    [XmlElement("IsMainCharacter")]
    [Required]
    public string IsMainCharacter { get; set; }

    [XmlElement("PhoneNumber")]
    [Required]
    [RegularExpression(@"^\+44-\d{2}-\d{3}-\d{4}$")]
    public string PhoneNumber { get; set; }

    [XmlElement("PlayId")]
    [Required]
    [ForeignKey(nameof(Play))]
    public int PlayId { get; set; }
}
