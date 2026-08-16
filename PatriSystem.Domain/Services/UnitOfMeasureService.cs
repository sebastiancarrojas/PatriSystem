using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.Domain.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;

        public UnitOfMeasureService(IUnitOfMeasureRepository unitOfMeasureRepository)
        {
            _unitOfMeasureRepository = unitOfMeasureRepository;
        }

        public async Task<Response<UnitOfMeasure>> CreateAsync(UnitOfMeasure unitOfMeasure)
        {
            try
            {
                var exists = await _unitOfMeasureRepository.ExistsWithNameAsync(unitOfMeasure.Name);
                if (exists)
                    return Response<UnitOfMeasure>.Failure("Ya existe una unidad de medida con ese nombre");

                await _unitOfMeasureRepository.CreateAsync(unitOfMeasure);

                var created = await _unitOfMeasureRepository.GetByNameAsync(unitOfMeasure.Name);
                return Response<UnitOfMeasure>.Success(created!, "Unidad de medida creada correctamente");
            }
            catch (Exception ex)
            {
                return Response<UnitOfMeasure>.Failure(ex, "Error al crear la unidad de medida");
            }
        }

        public async Task<Response<List<UnitOfMeasure>>> GetAllAsync()
        {
            try
            {
                var unitsOfMeasure = await _unitOfMeasureRepository.GetAllAsync();
                return Response<List<UnitOfMeasure>>.Success(unitsOfMeasure);
            }
            catch (Exception ex)
            {
                return Response<List<UnitOfMeasure>>.Failure(ex, "Error al obtener las unidades de medida");
            }
        }
    }
}