using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViveroPedidos.Modelo.Entidades
{
    public class Planta
    {
        
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string NombreComun {  get; set; } = string.Empty ;
        public string Especie { get; set; } = string .Empty ;
        public bool Disponible { get; set; } = true;

        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
