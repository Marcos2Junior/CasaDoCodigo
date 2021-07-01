using System.Collections.Generic;

namespace CasaDoCodigo.Models.ViewModels
{
    public class BuscaProdutoViewModel
    {
        public string Filtro { get; set; }
        public IList<Produto> Produtos { get; set; }
    }
}
