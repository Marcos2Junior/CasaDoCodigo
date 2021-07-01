using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public ProdutoRepository(ApplicationContext contexto, ICategoriaRepository categoriaRepository) : base(contexto)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IList<Produto> GetProdutos()
        {
            return GetIncludeProduto().ToList();
        }

        public async Task<IList<Produto>> GetProdutosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
            {
                return GetProdutos();
            }

            string filtroFormat = filtro.Trim().ToLower();
            return await GetIncludeProduto().Where(x =>
            x.Categoria.Nome.ToLower().Contains(filtroFormat) ||
            x.Nome.ToLower().Contains(filtroFormat)).ToListAsync();
        }

        private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Produto, Categoria> GetIncludeProduto()
        {
            return dbSet.Include(x => x.Categoria);
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    Categoria categoria = await _categoriaRepository.GravaCategoriaAsync(livro.Categoria);
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
