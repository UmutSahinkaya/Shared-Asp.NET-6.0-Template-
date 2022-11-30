using AutoMapper;
using Shared.Entities;
using Shared.Models;

namespace Shared
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
            CreateMap<User, EditUserModel>().ReverseMap();

            CreateMap<Book, BookViewModel>().ReverseMap();
            CreateMap<Book, CreateBookModel>().ReverseMap();
            CreateMap<Book, EditBookModel>().ReverseMap();
        }

    }
}
