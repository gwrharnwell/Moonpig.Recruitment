namespace Moonpig.PostOffice.Data
{
    using Moonpig.PostOffice.Models;
    using System.Linq;

    public interface IDbContext
    {
        IQueryable<Supplier> Suppliers { get; }

        IQueryable<Product> Products { get; }
    }
}
