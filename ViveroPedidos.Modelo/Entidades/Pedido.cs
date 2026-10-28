using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViveroPedidos.Modelo.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string SolicitanteNombre { get; set; }
        public string SolicitanteDocumento { get; set; }
        public DateTime FechaRetiro { get; set; }
        public int CantidadItems { get; set; }

        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
