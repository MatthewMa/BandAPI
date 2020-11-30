using BandAPI.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Models
{
    [TitleAndDescription(ErrorMessage = "Title must be different from description.")]
    public class AlbumCreateDTO //: IValidatableObject
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(400)]
        public string Description { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return new ValidationResult("The title and description need to be different", new[] { "AlbumCreateDTO" });
            }
        }*/
    }
}
