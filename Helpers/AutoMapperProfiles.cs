using System;
using AutoMapper;
using Savaglow.Dtos;
using Savaglow.Models;
using Savaglow.Models.Ledger;
using NomoBucket.API.Dtos;

namespace Savaglow.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegistrationDto, User>();
            CreateMap<User, UserDetailsDto>();
            CreateMap<LedgerItemCreationDto, LedgerItem>();
            CreateMap<LedgerItemCreationDto, RecurringLedgerItem>()
            .ForMember(i => i.RecurringFrequency, opt => opt.MapFrom(i => i.Recurring.RecurringFrequency))
            .ForMember(i => i.RecurringStartDate, opt => opt.MapFrom(i => i.Recurring.RecurringStartDate))
            .ForMember(i => i.RecurringLastModified, opt => opt.MapFrom(i => i.Recurring.RecurringLastModified));
            CreateMap<LedgerItem, LedgerItemDto>()
            .ForMember(i => i.TransactionType, opt => opt.MapFrom(i => Enum.GetName(typeof(TransactionType), i.TransactionType)));
            CreateMap<RecurringLedgerItem, RecurringLedgerItemDto>()
            .ForMember(i => i.TransactionType, opt => opt.MapFrom(i => Enum.GetName(typeof(TransactionType), i.TransactionType)))
            .ForPath(i => i.Recurring.RecurringFrequency, opt => opt.MapFrom(i => i.RecurringFrequency))
            .ForPath(i => i.Recurring.RecurringLastModified, opt => opt.MapFrom(i => i.RecurringLastModified))
            .ForPath(i => i.Recurring.RecurringStartDate, opt => opt.MapFrom(i => i.RecurringStartDate));



        }
    }
}