using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task AddCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.AnyAsync(c => c.Name == category.Name);
            if (existingCategory)
            {
                throw new Exception("Category already exists");
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            //check
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }
            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }
            // Update only necessary fields
            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
        }
    }
}
