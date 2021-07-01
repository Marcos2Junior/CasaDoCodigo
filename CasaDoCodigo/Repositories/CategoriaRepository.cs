using CasaDoCodigo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> GravaCategoriaAsync(string nome);
    }
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public async Task<Categoria> GravaCategoriaAsync(string nome)
        {
            Categoria categoria = dbSet.FirstOrDefault(x => x.Nome == nome);
            if (categoria == null)
            {
                categoria = new Categoria(nome);
                contexto.Add(categoria);
                await contexto.SaveChangesAsync();
            }

            return categoria;
        }
    }
}
