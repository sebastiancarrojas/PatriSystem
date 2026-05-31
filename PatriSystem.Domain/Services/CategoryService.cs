using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<Category>> CreateAsync(Category category)
        {
            try
            {
                var exists = await _categoryRepository.ExistsWithNameAsync(category.CategoryName);
                if (exists)
                    return Response<Category>.Failure("Ya existe una categoría con ese nombre");

                await _categoryRepository.CreateAsync(category);

                var created = await _categoryRepository.GetByNameAsync(category.CategoryName);
                return Response<Category>.Success(created!, "Categoría creada correctamente");
            }
            catch (Exception ex)
            {
                return Response<Category>.Failure(ex, "Error al crear la categoría");
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

        public async Task<Response<object>> UpdateAsync (Guid id, Category category)
        {
            try
            {
                var existing = await _categoryRepository.GetByIdAsync(id);
                if (existing == null)
                    return Response<object>.Failure("No se encontró la categoría");

                if (!string.IsNullOrEmpty(category.CategoryName) && existing.CategoryName != category.CategoryName)
                {
                 var exists = await _categoryRepository.ExistsWithNameAsync(category.CategoryName);
                    if (exists)
                        return Response<object>.Failure("Ya existe una categoría con ese nombre");
                }

                existing.CategoryName = category.CategoryName;

                await _categoryRepository.UpdateAsync(existing);
                return Response<object>.Success("Categoría actualizada correctamente");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al actualizar la categoría");
            }
        }

        public async Task<Response<PaginationResponse<Category>>> GetPaginatedAsync(CategoryPaginationRequest request)
        {
            try
            {
                var result = await _categoryRepository.GetPaginatedAsync(request);
                return Response<PaginationResponse<Category>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<PaginationResponse<Category>>.Failure(ex, "Error al obtener las categorias");
            }
        }
    }
}