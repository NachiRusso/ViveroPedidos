using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ViveroPedidos.Modelo.Datos;
using ViveroPedidos.Modelo.Entidades;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Controladora
{
    public class PlantaControladora
    {
        private readonly PlantaDatos _datos = new PlantaDatos();

        // MÉTODO FALTANTE Y PUBLIC
        public List<Planta> Listar(string? filtro = null)
        {
            var plantas = _datos.ListarTodas();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();
                plantas = plantas.Where(p =>
                    p.Codigo.ToLower().Contains(filtro) ||
                    p.NombreComun.ToLower().Contains(filtro) ||
                    p.Especie.ToLower().Contains(filtro)).ToList();
            }

            return plantas;
        }

        // AGREGADO PUBLIC
        public List<Planta> ListarDisponibles()
        {
            return _datos.ListarTodas()
                           .Where(p => p.Disponible)
                           .OrderBy(p => p.NombreComun)
                           .ToList();
        }

        // AGREGADO PUBLIC
        public void GuardarNuevo(string codigo, string nombreComun, string especie)
        {
            //ValidarDatosPlanta(codigo, nombreComun, especie);

            if (_datos.ExisteCodigo(codigo.Trim()))
                throw new InvalidOperationException("Ya existe una planta con ese código.");

            var nuevaPlanta = new Planta
            {
                Codigo = codigo.Trim(),
                NombreComun = nombreComun.Trim(),
                Especie = especie.Trim(),
                Disponible = true
            };

            _datos.Agregar(nuevaPlanta);
        }

        // AGREGADO PUBLIC
        public void Modificar(int id, string codigo, string nombreComun, string especie, bool disponible)
        {
            var planta = _datos.ObtenerPorId(id) ?? throw new InvalidOperationException("La planta no existe.");

            //ValidarDatosPlanta(codigo, nombreComun, especie);

            if (_datos.ExisteCodigo(codigo.Trim(), id))
                throw new InvalidOperationException("El código ingresado ya pertenece a otra planta.");

            planta.Codigo = codigo.Trim();
            planta.NombreComun = nombreComun.Trim();
            planta.Especie = especie.Trim();
            planta.Disponible = disponible;

            _datos.Modificar(planta);
        }

        // AGREGADO PUBLIC
        public void MarcarNoDisponible(int id)
        {
            var planta = _datos.ObtenerPorId(id) ?? throw new InvalidOperationException("La planta no existe.");

            if (!planta.Disponible)
                throw new InvalidOperationException("La planta ya se encuentra marcada como no disponible.");

            planta.Disponible = false;
            _datos.Modificar(planta);
        }

        // AGREGADO PUBLIC
        public void Eliminar(int id)
        {
            if (_datos.ObtenerPorId(id) == null)
                throw new InvalidOperationException("La planta no existe.");

            if (_datos.TienePedidosAsociados(id))
                throw new InvalidOperationException("No se puede eliminar la planta porque ya está incluida en uno o más pedidos.");

            _datos.Eliminar(id);
        }
    }
}