using FluentValidation;
using System;
using System.Linq;

namespace KariyerNet.Recruitment.JobAdverts;

public class CreateUpdateJobAdvertDtoValidator : AbstractValidator<CreateUpdateJobAdvertDto>
{
	public CreateUpdateJobAdvertDtoValidator()
	{
		RuleFor(x => x.PositionId)
			.NotNull()
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Description)
			.NotNull();

		RuleFor(x => x.Description)
			.MaximumLength(JobAdvertConsts.DescriptionMaxLength);

		RuleFor(x => x.Salary)
			.GreaterThanOrEqualTo(0)
			.When(x => x.Salary.HasValue);

		RuleFor(X => X.WorkType)
			.IsInEnum();

		RuleFor(x => x.BenefitIds)
			.Must(x => x.All(y => y != Guid.Empty))
			.When(x => x.BenefitIds is not null);
	}
}
