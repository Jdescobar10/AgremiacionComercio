using AgremiacionComercio.Application.DTOs.Request;
using FluentValidation;

namespace AgremiacionComercio.Application.Validators;

public class CreateComercianteValidator : AbstractValidator<CreateComercianteRequest>
{
    public CreateComercianteValidator()
    {
        RuleFor(x => x.NombreRazonSocial)
            .NotEmpty().WithMessage("El nombre o razón social es obligatorio.")
            .MaximumLength(255).WithMessage("El nombre no puede superar 255 caracteres.");

        RuleFor(x => x.MunicipioId)
            .GreaterThan(0).WithMessage("Debe seleccionar un municipio válido.");

        RuleFor(x => x.EstadoId)
            .GreaterThan(0).WithMessage("Debe seleccionar un estado válido.");

        RuleFor(x => x.CorreoElectronico)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(x => !string.IsNullOrWhiteSpace(x.CorreoElectronico));

        RuleFor(x => x.Telefono)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Telefono));

        RuleFor(x => x.FechaRegistro)
            .NotEmpty().WithMessage("La fecha de registro es obligatoria.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de registro no puede ser futura.");
    }
}
