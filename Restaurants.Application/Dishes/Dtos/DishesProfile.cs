using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Domain.Entites;

namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<UpdateDishCommand, Dish>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore the id since it's already set
            .ForMember(dest => dest.RestaurantId, opt => opt.Ignore()); // Ignore RestaurantId if it shouldn't be updated
        CreateMap<Dish, DishDto>();
    }
}