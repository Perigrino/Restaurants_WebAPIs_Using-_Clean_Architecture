using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateDishCommand, Dish>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RestaurantId, opt => opt.Ignore());
        CreateMap<Dish, DishDto>();
    }
}
