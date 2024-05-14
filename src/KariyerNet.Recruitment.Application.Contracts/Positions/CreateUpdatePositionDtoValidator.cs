using FluentValidation;

namespace KariyerNet.Recruitment.Positions;

public class CreateUpdatePositionDtoValidator : AbstractValidator<CreateUpdatePositionDto>
{
	public CreateUpdatePositionDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Name)
			.MaximumLength(PositionConsts.NameMaxLength);

		RuleFor(x => x.Description)
			.MaximumLength(PositionConsts.DescriptionMaxLength);
	}
}
