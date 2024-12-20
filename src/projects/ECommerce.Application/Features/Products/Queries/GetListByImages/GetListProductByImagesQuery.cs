using AutoMapper;
using Core.Application.Pipelines.Caching;
using ECommerce.Application.Features.Products.Queries.GetListByImages;
using ECommerce.Application.Services.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries.GetList;

public class GetListProductByImagesQuery : IRequest<List<GetListProductByProductImagesResponse>>, ICachableRequest
{
    public string CacheKey => "GetAllProductsByImageList";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "Products";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListByImagesQueryHandler : IRequestHandler<GetListProductByImagesQuery, List<GetListProductByProductImagesResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetListByImagesQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<List<GetListProductByProductImagesResponse>> Handle(GetListProductByImagesQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetListAsync(
                enableTracking: false,
                withDeleted: true,
                cancellationToken: cancellationToken
                );

            var response = _mapper.Map<List<GetListProductByProductImagesResponse>>(products);

            return response;
        }
    }



}