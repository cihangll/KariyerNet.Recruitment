using FluentValidation;

namespace KariyerNet.Recruitment.DisabledWords;

public class CreateUpdateDisabledWordDtoValidator : AbstractValidator<CreateUpdateDisabledWordDto>
{
	public CreateUpdateDisabledWordDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Name)
			.MaximumLength(DisabledWordConsts.NameMaxLength);
	}
}