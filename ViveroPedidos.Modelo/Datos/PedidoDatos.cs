using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Modelo.Datos
{
    public class PedidoDatos
    {
        public List<Pedido> Listar()
        {
            using var context = new ViveroContext();
            return context.Pedido.OrderByDescending(p => p.Fecha).ToList();
        }

        public Pedido? ObtenerConDetalle(int id)
        {
            using var context = new ViveroContext();
            // Carga eagerly los detalles
            return context.Pedido
                .Include(p => p.Detalles)
                .FirstOrDefault(p => p.Id == id);
        }

        public void GuardarPedidoTransaccional(Pedido pedido)
        {
            using var context = new ViveroContext();
            // Inicia transacción atómica para cabecera y detalle
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // Asignar número correlativo (máximo actual + 1)
                var maxNumero = context.Pedido.Max(p => (int?)p.Numero) ?? 0;
                pedido.Numero = maxNumero + 1;

                context.Pedido.Add(pedido);
                context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw; // Relanza la excepción tras hacer rollback
            }
        }
    }
}
