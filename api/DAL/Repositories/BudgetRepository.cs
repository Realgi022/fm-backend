using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;

        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Budget?> GetMonthlyBudgetAsync(int userId, int year, int month)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId &&
                    b.Year == year &&
                    b.Month == month);
        }
    }
}
