namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalEmpleados { get; set; }
        public int TotalOrdenes { get; set; }
        public decimal VentasHoy { get; set; }
        public List<OrdenRecienteViewModel> OrdeneRecientes { get; set; } = new();
        public List<string> AlertasInventario { get; set; } = new();
    }

    public class OrdenRecienteViewModel
    {
        public string? NumeroOrden { get; set; }
        public string? Cliente { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
