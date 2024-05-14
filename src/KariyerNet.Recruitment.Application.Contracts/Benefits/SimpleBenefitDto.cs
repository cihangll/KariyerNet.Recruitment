using System;

namespace KariyerNet.Recruitment.Benefits;

public class SimpleBenefitDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
}