using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

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
    }
}