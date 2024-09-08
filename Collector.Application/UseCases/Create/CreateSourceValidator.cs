using Application.Mediator;
using FluentValidation;

namespace Collector.Application.UseCases.Create;

public class CreateSourceValidator : UseCaseValidator<CreateSourceUseCase>
{
    public CreateSourceValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.WebPage)
            .Must(e => e.IsAbsoluteUri)
            .WithErrorCode("WebPage.AbsoluteUri")
            .Must(e => e.Scheme == Uri.UriSchemeHttp || e.Scheme == Uri.UriSchemeHttps)
            .WithErrorCode("WebPage.HttpScheme");
    }
}