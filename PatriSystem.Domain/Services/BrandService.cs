using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Response<Brand>> CreateAsync(Brand brand)
        {
            try
            {
                var exists = await _brandRepository.ExistsWithNameAsync(brand.BrandName);
                if (exists)
                    return Response<Brand>.Failure("Ya existe una marca con ese nombre");

                await _brandRepository.CreateAsync(brand);

                var created = await _brandRepository.GetByNameAsync(brand.BrandName);
                return Response<Brand>.Success(created!, "Marca creada correctamente");
            }
            catch (Exception ex)
            {
                return Response<Brand>.Failure(ex, "Error al crear la marca");
            }
        }

        public async Task<Response<List<Brand>>> GetAllAsync()
        {
            try
            {
                var brands = await _brandRepository.GetAllAsync();
                return Response<List<Brand>>.Success(brands);
            }
            catch (Exception ex)
            {
                return Response<List<Brand>>.Failure(ex, "Error al obtener las marcas");
            }
        }

        public async Task<Response<object>> UpdateAsync(Guid id, Brand brand)
        {
            try
            {
                var existing = await _brandRepository.GetByIdAsync(id);
                if (existing == null)
                    return Response<object>.Failure("No se encontró la marca");

                if (!string.IsNullOrEmpty(brand.BrandName) && existing.BrandName != brand.BrandName)
                {
                    var exists = await _brandRepository.ExistsWithNameAsync(brand.BrandName);
                    if (exists)
                        return Response<object>.Failure("Ya existe una marca con ese nombre");
                }

                existing.BrandName = brand.BrandName;
                existing.BrandDescription = brand.BrandDescription;

                await _brandRepository.UpdateAsync(existing);
                return Response<object>.Success("marca actualizada correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al actualizar la marca");
            }
        }

        public async Task<Response<PaginationResponse<Brand>>> GetPaginatedAsync(BrandPaginationRequest request)
        {
            try
            {
                var result = await _brandRepository.GetPaginatedAsync(request);
                return Response<PaginationResponse<Brand>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<PaginationResponse<Brand>>.Failure(ex, "Error al obtener las marcas");
            }
        }
    }
}