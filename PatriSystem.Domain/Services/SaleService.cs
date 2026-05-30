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

                var realProductIds = sale.SaleDetails

                    .Where(d => !d.IsTemporary && d.ProductId.HasValue && d.ProductId.Value != Guid.Empty)

                    .Select(d => d.ProductId!.Value)

                    .ToList();

                var products = await _productRepository.GetAllAsync();

                var relevantProducts = products.Where(p => realProductIds.Contains(p.Id)).ToList();

                if (relevantProducts.Count != realProductIds.Count)

                    return Response<Guid>.Failure("Uno o más productos no existen");

                decimal total = 0;

                foreach (var detail in sale.SaleDetails)

                {

                    if (detail.Quantity <= 0)

                        return Response<Guid>.Failure("Cantidad inválida");

                    if (detail.IsTemporary)

                    {

                        if (string.IsNullOrWhiteSpace(detail.ProductName))

                            return Response<Guid>.Failure("El nombre del producto temporal es requerido");

                        if (detail.UnitPrice <= 0)

                            return Response<Guid>.Failure("El precio del producto temporal es inválido");

                        detail.ProductId = null;

                        detail.SubTotal = detail.UnitPrice * detail.Quantity;

                        detail.SaleId = sale.Id;

                        total += detail.SubTotal;

                    }

                    else

                    {

                        if (!detail.ProductId.HasValue || detail.ProductId.Value == Guid.Empty)

                            return Response<Guid>.Failure("El ProductId es requerido para productos no temporales");

                        var product = relevantProducts.FirstOrDefault(p => p.Id == detail.ProductId);

                        if (product == null)

                            return Response<Guid>.Failure($"El producto {detail.ProductId} no existe");

                        detail.UnitPrice = detail.UnitPrice > 0 ? detail.UnitPrice : product.UnitPrice;

                        detail.SubTotal = detail.UnitPrice * detail.Quantity;

                        detail.SaleId = sale.Id;

                        total += detail.SubTotal;

                    }

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