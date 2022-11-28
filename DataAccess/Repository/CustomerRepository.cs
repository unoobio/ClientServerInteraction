using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Repository.Interfaces;

namespace DataAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext db) : base(db)
        {
        }
    }
}
