using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViveroPedidos.Modelo.Entidades
{
    public class PedidoDetalle
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int PlantaId { get; set; }
        public string NombreHistorico { get; set; } = string.Empty;
        public int Cantidad { get; set; }

        public Pedido Pedido { get; set; } = null!;
        public Planta Planta { get; set; } = null!;
    }
}
