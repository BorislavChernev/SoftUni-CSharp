namespace Footballers
{
    using System.Linq;

    using AutoMapper;

    using Data.Models;
    using DataProcessor.ExportDto;


    // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            this.CreateMap<Footballer, ExportFootballerDto>()
                .ForMember(dto => dto.Name, m => m.MapFrom(f => f.Name))
                .ForMember(dto => dto.Position, m => m.MapFrom(f => f.PositionType.ToString()));
            this.CreateMap<Coach, ExportCoachDto>()
                .ForMember(dto => dto.Name, m => m.MapFrom(c => c.Name))
                .ForMember(dto => dto.FootballersCount, m => m.MapFrom(c => c.Footballers.Count))
                .ForMember(dto => dto.Footballers, m => m.MapFrom(c => c.Footballers.OrderBy(f => f.Name).ToArray()));
        }
    }
}
