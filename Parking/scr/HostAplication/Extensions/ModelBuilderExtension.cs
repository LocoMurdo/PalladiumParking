using Microsoft.EntityFrameworkCore;

namespace Parking.API.scr.HostAplication.Extensions
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Adds the entity as a model so that EF Core can generate the database schema (tables, primary keys, foreign keys, among others).
        /// </summary>
        /// <typeparam name="TEntity">The entity type to be added.</typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>

        public static ModelBuilder AddEntity<TEntity>(this ModelBuilder modelBuilder) where TEntity : class
        {
            modelBuilder.Entity<TEntity>();
            return modelBuilder;
        }
    }
}
