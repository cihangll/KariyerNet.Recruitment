using AutoMapper;
using KariyerNet.Recruitment.Benefits;
using KariyerNet.Recruitment.DisabledWords;
using KariyerNet.Recruitment.JobAdverts;
using KariyerNet.Recruitment.Positions;

namespace KariyerNet.Recruitment;

public class RecruitmentApplicationAutoMapperProfile : Profile
{
	public RecruitmentApplicationAutoMapperProfile()
	{
		/* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

		CreateMap<Position, PositionDto>();
		CreateMap<CreateUpdatePositionDto, Position>();

		CreateMap<Benefit, BenefitDto>();
		CreateMap<CreateUpdateBenefitDto, Benefit>();

		CreateMap<DisabledWord, DisabledWordDto>();
		CreateMap<CreateUpdateDisabledWordDto, DisabledWord>();

		CreateMap<JobAdvert, JobAdvertDto>();
		CreateMap<JobAdvert, JobAdvertDetailDto>();
		CreateMap<JobAdvertBenefit, JobAdvertBenefitDto>();

		CreateMap<JobAdvert, JobAdvertCreatedOrUpdatedEto>();
	}
}
