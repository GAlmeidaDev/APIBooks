using System;
using System.Collections.Generic;
using System.Linq;
using ApiBooks.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApiBooks.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BookContext _context;

        public Repository(BookContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().FromSqlRaw($"EXEC sp_GetAll{typeof(TEntity).Name}s").ToList();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>()
                .FromSqlRaw($"EXEC sp_GetBookById @Id", new SqlParameter("@Id", id))
                .AsEnumerable()
                .FirstOrDefault()!;
        }

        public void Add(TEntity entity)
        {
            _context.Database.ExecuteSqlRaw(
                $"EXEC sp_AddDigitalBook @Title, @Author, @PublicationYear",
                new SqlParameter("@Title", typeof(TEntity).GetProperty("Title").GetValue(entity)),
                new SqlParameter("@Author", typeof(TEntity).GetProperty("Author").GetValue(entity)),
                new SqlParameter("@PublicationYear", typeof(TEntity).GetProperty("PublicationYear").GetValue(entity))
            );
        }


        public void Update(TEntity entity)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null)
            {
                throw new ArgumentException("Entity does not have an 'Id' property.");
            }

            var idValue = idProperty.GetValue(entity);
            if (idValue == null || !(idValue is int id))
            {
                throw new ArgumentException("Entity 'Id' property must be a valid integer.");
            }

            _context.Database.ExecuteSqlRaw($"EXEC sp_UpdateDigitalBook @Id, @Title, @Author, @PublicationYear",
                new SqlParameter("@Id", id),
                new SqlParameter("@Title", typeof(TEntity).GetProperty("Title").GetValue(entity)),
                new SqlParameter("@Author", typeof(TEntity).GetProperty("Author").GetValue(entity)),
                new SqlParameter("@PublicationYear", typeof(TEntity).GetProperty("PublicationYear").GetValue(entity)));
        }

        public void Delete(int id)
        {
            _context.Database.ExecuteSqlRaw($"EXEC sp_DeleteDigitalBook @Id", new SqlParameter("@Id", id));
        }
    }
}