using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelPlannerAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SmartRequiredAttribute : ValidationAttribute, IBindingSourceMetadata, IPropertyValidationFilter
    {
        private readonly RequiredAttribute _required = new RequiredAttribute();
        private readonly BindRequiredAttribute _bindRequired = new BindRequiredAttribute();

        public BindingSource BindingSource => BindingSource.Custom;

        public override bool IsValid(object? value)
        {
            // Use the logic of [Required]
            return _required.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return _required.FormatErrorMessage(name);
        }

        public bool ShouldValidateEntry(ValidationEntry entry, ValidationEntry parentEntry)
        {
            return true; // Always validate
        }
    }
}
