using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public  class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger, IMapper mapper, IRestaurantRepository restaurantRepository) : 
    IRequestHandler<GetAllRestaurantsQuery, PageResults<RestaurantDto>>
{
    public async Task<PageResults<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var (restaurant, totalCount) = await restaurantRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageSize, request.PageNumber);
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurant);

        var results = new PageResults<RestaurantDto>(restaurantsDto, totalCount, request.PageSize, request.PageNumber);
        return results!;
    }
}