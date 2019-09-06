using FluentValidation;
using TodoApi.Models;

namespace TodoApi.Common
{
    public class ItemValidator : AbstractValidator<TodoItem>
    {
        public ItemValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Name is mandatory.");

            RuleFor(x => x.IsComplete)
            .NotEmpty()
            .WithMessage("IsComplete is mandatory.");
        }
    }
}