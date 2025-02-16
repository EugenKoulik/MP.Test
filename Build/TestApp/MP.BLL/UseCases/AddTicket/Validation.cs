using FluentValidation;
using MP.BLL.Helpers;

namespace MP.BLL.UseCases.AddTicket;

public static partial class AddTicketUseCase
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Title)
                .NotEmpty().WithMessage("Название билета обязательно.")
                .Must(title => !string.IsNullOrWhiteSpace(title))
                .WithMessage("Название билета не может быть пустым.");
            
            RuleFor(x => x.TimeZoneId)
                .NotEmpty().WithMessage("Часовой пояс обязателен.")
                .Must(DateTimeHelper.BeAValidTimeZone).WithMessage("Неверный часовой пояс.");

            RuleFor(x => x.Model.VisitDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Дата посещения не может быть в прошлом.");

            RuleFor(x => x.Model.VisitorsNumber)
                .InclusiveBetween(1, 10)
                .WithMessage("Количество посетителей должно быть в диапазоне от 1 до 10.");
        }
    }
}