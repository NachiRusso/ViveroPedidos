using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViveroPedidos.Modelo.Datos;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Controladora
{
    public class PedidoControladora
    {
        private readonly PedidoDatos _pedidoDatos = new PedidoDatos();
        private readonly PlantaDatos _plantaDatos = new PlantaDatos();

        public List<Pedido> Listar(DateTime? desde = null, DateTime? hasta = null, string? solicitante = null)
        {
            var pedidos = _pedidoDatos.Listar();

            if (desde.HasValue)
                pedidos = pedidos.Where(p => p.Fecha.Date >= desde.Value.Date).ToList();

            if (hasta.HasValue)
                pedidos = pedidos.Where(p => p.Fecha.Date <= hasta.Value.Date).ToList();

            if (!string.IsNullOrWhiteSpace(solicitante))
                pedidos = pedidos.Where(p => p.SolicitanteNombre.Contains(solicitante, StringComparison.OrdinalIgnoreCase)).ToList();

            return pedidos;
        }

        public Pedido? ObtenerConDetalle(int id)
        {
            return _pedidoDatos.ObtenerConDetalle(id); // Retorna null si no existe, como pide el enunciado
        }

        public Pedido Registrar(DateTime fecha, string solicitanteNombre, string solicitanteDocumento, DateTime fechaRetiro, List<(int PlantaId, int Cantidad)> lineas)
        {
            // Validaciones de cabecera
            if (string.IsNullOrWhiteSpace(solicitanteNombre) || string.IsNullOrWhiteSpace(solicitanteDocumento))
                throw new InvalidOperationException("El nombre y documento del solicitante son obligatorios.");

            if (fechaRetiro.Date < fecha.Date)
                throw new InvalidOperationException("La fecha de retiro no puede ser anterior a la fecha del pedido.");

            if (lineas == null || !lineas.Any())
                throw new InvalidOperationException("Debe agregar al menos una planta al pedido.");

            // Validación de líneas repetidas en la colección que llega
            var duplicados = lineas.GroupBy(l => l.PlantaId).Any(g => g.Count() > 1);
            if (duplicados)
                throw new InvalidOperationException("El pedido contiene plantas duplicadas. Agrupe las cantidades en una sola línea.");

            var pedido = new Pedido
            {
                Fecha = fecha.Date,
                SolicitanteNombre = solicitanteNombre.Trim(),
                SolicitanteDocumento = solicitanteDocumento.Trim(),
                FechaRetiro = fechaRetiro.Date,
                Detalles = new List<PedidoDetalle>()
            };

            int cantidadTotalItems = 0;

            // Procesar y validar cada línea
            foreach (var linea in lineas)
            {
                if (linea.Cantidad <= 0)
                    throw new InvalidOperationException("La cantidad pedida debe ser mayor a cero.");

                var planta = _plantaDatos.ObtenerPorId(linea.PlantaId)
                    ?? throw new InvalidOperationException($"La planta con ID {linea.PlantaId} no existe en el catálogo.");

                if (!planta.Disponible)
                    throw new InvalidOperationException($"La planta '{planta.NombreComun}' no está disponible para pedidos.");

                var detalle = new PedidoDetalle
                {
                    PlantaId = planta.Id,
                    Cantidad = linea.Cantidad,
                    NombreHistorico = planta.NombreComun // Copia del nombre actual según regla de negocio
                };

                pedido.Detalles.Add(detalle);
                cantidadTotalItems += linea.Cantidad;
            }

            // Asignar suma total de artículos a la cabecera
            pedido.CantidadItems = cantidadTotalItems;

            // Delegar la persistencia y transacción a la capa de datos
            _pedidoDatos.GuardarPedidoTransaccional(pedido);

            return pedido;
        }
    }
}
