using FluentValidation;

namespace KariyerNet.Recruitment.Benefits;

public class CreateUpdateBenefitDtoValidator : AbstractValidator<CreateUpdateBenefitDto>
{
	public CreateUpdateBenefitDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Name)
			.MaximumLength(BenefitConsts.NameMaxLength);

		RuleFor(x => x.Description)
			.MaximumLength(BenefitConsts.DescriptionMaxLength);
	}
}
