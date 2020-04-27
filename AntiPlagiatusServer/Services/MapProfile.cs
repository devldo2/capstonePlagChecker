using AntiPlagiatusServer.Data.Entities;
using AntiPlagiatusServer.Models;
using AntiPlagiatusServer.Models.DTO;
using AutoMapper;

namespace AntiPlagiatusServer.Services
{
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<UserModel, User>();
            CreateMap<User, UserModel>();

            //CreateMap<UserModel, UserViewModel>();
            //CreateMap<UserViewModel, UserModel>();

            CreateMap<OperationReportModel, OperationReport>();
            CreateMap<OperationReport, OperationReportModel>();

            CreateMap<ContentModel, Content>();
            CreateMap<Content, ContentModel>();

            CreateMap<IgnoreRuleModel, IgnoreRule>();
            CreateMap<IgnoreRule, IgnoreRuleModel>();

            CreateMap<DomainModel, Domain>();
            CreateMap<Domain, DomainModel>();

            CreateMap<LayerByDomainModel, LayerByDomain>().ForMember(dest=> dest.Words, opt=> opt.MapFrom(source=> source.StringWords));
            CreateMap<LayerByDomain, LayerByDomainModel>().ForMember(dest => dest.StringWords, opt => opt.MapFrom(source => source.Words));
        }
    }
}
