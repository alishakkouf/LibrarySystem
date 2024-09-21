using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TitanTechTask.Domain.Books;
using TitanTechTask.Shared.Enums;

namespace TitanTechTask.Data.Models
{
    internal class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDomain>()
                .ForMember(x=>x.AvailabilityStatus , o => o.MapFrom(x=>x.Available ? AvailabilityStatus.Available
                                                                                   : AvailabilityStatus.CheckedOut));
        }
    }
}
