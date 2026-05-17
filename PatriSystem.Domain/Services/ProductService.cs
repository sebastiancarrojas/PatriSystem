using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Response<object>> CreateAsync(Product product)
        {
            try
            {
                if (!string.IsNullOrEmpty(product.Barcode))
                {
                    var exists = await _productRepository.ExistsWithBarcodeAsync(product.Barcode);
                    if (exists)
                        return Response<object>.Failure("Ya existe un producto con ese código de barras");
                }

                await _productRepository.CreateAsync(product);
                return Response<object>.Success("Producto creado correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al crear el producto");
            }
        }
    }
}