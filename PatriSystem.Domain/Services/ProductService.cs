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

        public async Task<Response<object>> UpdateAsync(Guid id, Product product)
        {
            try
            {
                var existing = await _productRepository.GetByIdAsync(id);
                if (existing == null)
                    return Response<object>.Failure("Producto no encontrado");

                if (!string.IsNullOrEmpty(product.Barcode) && existing.Barcode != product.Barcode)
                {
                    var exists = await _productRepository.ExistsWithBarcodeAsync(product.Barcode);
                    if (exists)
                        return Response<object>.Failure("Ya existe un producto con ese código de barras");
                }

                existing.ProductName = product.ProductName;
                existing.Barcode = product.Barcode;
                existing.ProductDescription = product.ProductDescription;
                existing.UnitPrice = product.UnitPrice;
                existing.UnitOfMeasure = product.UnitOfMeasure;
                existing.CategoryId = product.CategoryId;
                existing.BrandId = product.BrandId;

                await _productRepository.UpdateAsync(existing);
                return Response<object>.Success("Producto actualizado correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al actualizar el producto");
            }
        }

        public async Task<Response<object>> DeactivateAsync(Guid id)
        {
            try
            {
                var existing = await _productRepository.GetByIdAsync(id);
                if (existing == null)
                    return Response<object>.Failure("Producto no encontrado");

                await _productRepository.DeactivateAsync(id);
                return Response<object>.Success("Producto desactivado correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al desactivar el producto");
            }
        }

        public async Task<Response<List<Product>>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Response<List<Product>>.Success(products);
            }
            catch (Exception ex)
            {
                return Response<List<Product>>.Failure(ex, "Error al obtener los productos");
            }
        }

        public async Task<Response<Product>> GetByIdAsync(Guid id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                    return Response<Product>.Failure("Producto no encontrado");

                return Response<Product>.Success(product);
            }
            catch (Exception ex)
            {
                return Response<Product>.Failure(ex, "Error al obtener el producto");
            }
        }
    }
}