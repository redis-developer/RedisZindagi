using FluentValidation;
using Zindagi.Domain.RequestsAggregate.Queries;

namespace Zindagi.Domain.RequestsAggregate.Validators
{
    public class SearchBloodRequestValidator : AbstractValidator<SearchBloodRequest>
    {
        public SearchBloodRequestValidator() =>
            RuleFor(prop => prop.SearchQuery)
                .MaximumLength(100)
                .WithName("Search");
    }
}
