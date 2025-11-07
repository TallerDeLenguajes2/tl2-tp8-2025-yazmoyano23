 public class Presupuestos
    {
        private int idPresupuesto;

        string nombreDestinatario;
        DateTime fechaCreacion;
        List<PresupuestosDetalle> detalle;

        public Presupuestos()
        {
            
        }
        public Presupuestos(int id,string destinatario, DateTime fecha)
        {
            this.idPresupuesto = id;
            this.nombreDestinatario = destinatario;
            this.fechaCreacion = fecha;
            this.detalle = new List<PresupuestosDetalle>();
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

        public double MontoPresupuesto()
        {
            double monto = detalle.Sum(d => d.Cantidad * d.Producto.Precio);
            return monto;
        }
        
        public double MontoPresupuestoConIva()
        {
            return MontoPresupuesto() * 1.21;
        }
        public int CantidadProductos()
        {
            return detalle.Sum(d => d.Cantidad);
        }

        public void AgregarDetalle(PresupuestosDetalle detalle)
        {
            this.detalle.Add(detalle);
        }
    }