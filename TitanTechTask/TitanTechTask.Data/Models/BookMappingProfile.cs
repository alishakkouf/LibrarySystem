using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TitanTechTask.Domain.Books;

namespace TitanTechTask.Data.Models
{
    internal class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDomain>().ReverseMap();
        }
    }
}
