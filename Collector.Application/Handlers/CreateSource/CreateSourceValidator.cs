using FluentValidation;

namespace Collector.Application.Handlers.CreateSource;

public class CreateSourceValidator : AbstractValidator<CreateSourceCommand>
{
    public CreateSourceValidator()
    {
        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.WebPage)
            .Must(e => e.IsAbsoluteUri)
            .WithErrorCode("WebPage.AbsoluteUri");

        RuleFor(e => e.WebPage)
            .Must(e => e.Scheme == Uri.UriSchemeHttp || e.Scheme == Uri.UriSchemeHttps)
            .When(e => e.WebPage.IsAbsoluteUri)
            .WithErrorCode("WebPage.NonHttpScheme");
    }
}