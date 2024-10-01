using Agrigate.Core.Services.DeviceService.Models;
using FluentValidation;

namespace Agrigate.Api.Validators;

public class CreateDeviceValidator : AbstractValidator<DeviceBase>
{
    public CreateDeviceValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Location).NotNull().NotEmpty().WithMessage("Location is required");
    }
}