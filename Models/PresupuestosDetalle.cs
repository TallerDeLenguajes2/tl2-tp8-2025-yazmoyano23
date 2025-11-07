    public class PresupuestosDetalle
    {
        Productos producto;
        int cantidad;

        public Productos Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }

        public PresupuestosDetalle(Productos producto, int cantidad)
        {
            Producto = producto;
            Cantidad = cantidad;
        }
    }