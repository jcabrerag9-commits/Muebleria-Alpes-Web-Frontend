using System;
using System.Collections.Generic;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Finanzas
{
    public class HistorialFinancieroViewModel
    {
        public List<MovimientoFinancieroViewModel> Movimientos { get; set; } = new List<MovimientoFinancieroViewModel>();
        
        // Filtros
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string TipoMovimiento { get; set; }
        public int? FacturaId { get; set; }
        public int? ClienteId { get; set; }
    }

    public class MovimientoFinancieroViewModel
    {
        public int MovimientoId { get; set; }
        public int OrdenId { get; set; }
        public int? PagoId { get; set; }
        public int FacturaId { get; set; }
        public string FacturaNumero { get; set; }
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public string TipoMovimiento { get; set; }
        public string EstadoFactura { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoNuevo { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
    }
}
