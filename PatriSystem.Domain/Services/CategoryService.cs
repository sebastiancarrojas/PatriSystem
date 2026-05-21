using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<object>> CreateAsync(Category category)
        {
            try
            {
                var exists = await _categoryRepository.ExistsWithNameAsync(category.CategoryName);
                if (exists)
                    return Response<object>.Failure("Ya existe una categoría con ese nombre");

                await _categoryRepository.CreateAsync(category);
                return Response<object>.Success("Categoría creada correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al crear la categoría");
            }
        }

        public async Task<Response<List<Category>>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return Response<List<Category>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Response<List<Category>>.Failure(ex, "Error al obtener las categorías");
            }
        }
    }
}