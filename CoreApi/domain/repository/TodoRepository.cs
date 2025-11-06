using CoreApi.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.domain.repository
{
    public class TodoRepository: DbContext
    {
        public TodoRepository(DbContextOptions<TodoRepository> options): base(options) { }

        public DbSet<TodoEntity> Todos => Set<TodoEntity>();
    }
}
