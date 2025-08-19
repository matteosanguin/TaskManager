namespace TaskManager.Application.Commands.Projects.CreateProject;

using FastEndpoints;
using FluentValidation;

public class CreateProjectCommandValidator : Validator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Il nome del progetto è obbligatorio.")
            .MaximumLength(100)
            .WithMessage("Il nome del progetto non può superare i 100 caratteri.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("La descrizione del progetto non può superare i 500 caratteri.");
    }
}
