using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Modelo.Datos
{
    public class PlantaDatos
    {
        public List<Planta> ListarTodas()
        {
            using var context = new ViveroContext();
            return context.Plantas.ToList();
        }

        public Planta? ObtenerPorId(int id)
        {
            using var context = new ViveroContext();
            return context.Plantas.Find(id);
        }

        public bool ExisteCodigo(string codigo, int? idExcluir = null)
        {
            using var context = new ViveroContext();
            var query = context.Plantas.Where(p => p.Codigo.ToLower() == codigo.ToLower());
            if (idExcluir.HasValue)
            {
                query = query.Where(p => p.Id != idExcluir.Value);
            }
            return query.Any();
        }

        public bool TienePedidosAsociados(int plantaId)
        {
            using var context = new ViveroContext();
            return context.PedidoDetalle.Any(d => d.PlantaId == plantaId);
        }

        public void Agregar(Planta planta)
        {
            using var context = new ViveroContext();
            context.Plantas.Add(planta);
            context.SaveChanges();
        }

        public void Modificar(Planta plantaModificada)
        {
            using var context = new ViveroContext();
            context.Plantas.Update(plantaModificada);
            context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            using var context = new ViveroContext();
            var planta = context.Plantas.Find(id);
            if (planta != null)
            {
                context.Plantas.Remove(planta);
                context.SaveChanges();
            }
        }
    }
}