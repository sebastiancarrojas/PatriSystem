using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<Response<Guid>> CreateAsync(Sale sale)
        {
            try
            {
                if (sale.SaleDetails == null || !sale.SaleDetails.Any())
                    return Response<Guid>.Failure("La venta no tiene productos");

                var productIds = sale.SaleDetails.Select(d => d.ProductId).ToList();
                var products = await _productRepository.GetAllAsync();
                var relevantProducts = products.Where(p => productIds.Contains(p.Id)).ToList();

                if (relevantProducts.Count != productIds.Count)
                    return Response<Guid>.Failure("Uno o más productos no existen");

                decimal total = 0;

                foreach (var detail in sale.SaleDetails)
                {
                    if (detail.Quantity <= 0)
                        return Response<Guid>.Failure("Cantidad inválida");

                    var product = relevantProducts.First(p => p.Id == detail.ProductId);

                    var subTotal = product.UnitPrice * detail.Quantity;
                    total += subTotal;

                    detail.UnitPrice = product.UnitPrice;
                    detail.SubTotal = subTotal;
                    detail.SaleId = sale.Id;
                }

                sale.TotalAmount = total;
                sale.SaleDate = DateTime.UtcNow;

                await _saleRepository.CreateAsync(sale, relevantProducts);
                return Response<Guid>.Success(sale.Id, "Venta registrada correctamente");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Failure(ex, "Error inesperado al registrar la venta");
            }
        }

        public async Task<Response<List<Sale>>> GetAllAsync()
        {
            try
            {
                var sales = await _saleRepository.GetAllAsync();
                return Response<List<Sale>>.Success(sales);
            }
            catch (Exception ex)
            {
                return Response<List<Sale>>.Failure(ex, "Error al obtener las ventas");
            }
        }

        public async Task<Response<Sale>> GetByIdAsync(Guid id)
        {
            try
            {
                var sale = await _saleRepository.GetByIdAsync(id);
                if (sale == null)
                    return Response<Sale>.Failure("Venta no encontrada");

                return Response<Sale>.Success(sale);
            }
            catch (Exception ex)
            {
                return Response<Sale>.Failure(ex, "Error al obtener la venta");
            }
        }
    }
}