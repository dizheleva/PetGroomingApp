﻿namespace PetGroomingApp.Web.ViewModels.Service
{
    using System.ComponentModel.DataAnnotations;

    using static PetGroomingApp.Services.Common.EntityConstants.Service;

    public class ServiceFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = ImageUrlRequiredMessage)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public required string ImageUrl { get; set; }

        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = DurationRequiredMessage)]        
        public required string Duration { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(typeof(Decimal), PriceMin, PriceMax, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }
    }
}
