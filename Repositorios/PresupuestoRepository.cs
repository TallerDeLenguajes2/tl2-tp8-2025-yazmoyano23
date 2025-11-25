using Microsoft.Data.Sqlite;
using System.Timers;

    public class PresupuestoRepository
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "nuevo.db");

        string cadenaConexion;

        public PresupuestoRepository()
        {
            // La conexión debe usar la ruta absoluta
            cadenaConexion = $"Data Source={dbPath};";
        }

        //Crear un nuevo Presupuesto. (recibe un objeto Presupuesto)

        public void InsertarPresupuesto(Presupuestos nuevoPresupuesto)
        {
            using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string consulta = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@Destinatario,@Fecha)"; //consulta
                using var insertCmd = new SqliteCommand(consulta, conexion);

                insertCmd.Parameters.Add(new SqliteParameter("@Destinatario", nuevoPresupuesto.NombreDestinatario));
                insertCmd.Parameters.Add(new SqliteParameter("@Fecha", nuevoPresupuesto.FechaCreacion));
                insertCmd.ExecuteNonQuery();

            }
        }


        public List<Presupuestos> GetAll()
        {
            //Consultar a la bd con el select construir la lista en el bucle y retornar
            var presupuestos = new List<Presupuestos>();

            using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string consulta = @"SELECT 
                        P.idPresupuesto,
                        P.NombreDestinatario,
                        P.FechaCreacion,
                        PR.idProducto,
                        PR.Descripcion AS Producto,
                        PR.Precio,
                        PD.Cantidad
                    FROM 
                        Presupuestos P
                    LEFT JOIN 
                        PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                    LEFT JOIN 
                        Productos PR ON PD.idProducto = PR.idProducto";
                using (SqliteCommand selectCmd = new SqliteCommand(consulta, conexion))
                using (SqliteDataReader reader = selectCmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var id = Convert.ToInt32(reader["idPresupuesto"]);
                        var destinatario = reader["NombreDestinatario"].ToString();
                        var fecha = Convert.ToDateTime(reader["FechaCreacion"]);

                        var nuevoPresupuesto = new Presupuestos(id, destinatario, fecha);
                        if (!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                        {
                            var idProdu = Convert.ToInt32(reader["idProducto"]);
                            var descrip = reader["Producto"].ToString();
                            var precio = Convert.ToInt32(reader["Precio"]);

                            var producto = new Productos(idProdu, descrip, precio);
                            var detalle = new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));

                            nuevoPresupuesto.AgregarDetalle(detalle);

                        }

                        presupuestos.Add(nuevoPresupuesto);
                    }
                    conexion.Close(); //MODIFICAR PARA QUE CONSTRUYA EL DETALLE SI ES QUE EXISTE 

                }

                return presupuestos;
            }
        }

        //Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve un
        //Presupuesto con sus productos y cantidades)

        public Presupuestos GetById(int idPresupuesto)
        {
            Presupuestos presupuesto = null;
            int presupuestoEncontrado = 0;
            string consulta = @"SELECT 
                        P.idPresupuesto,
                        P.NombreDestinatario,
                        P.FechaCreacion,
                        PR.idProducto,
                        PR.Descripcion AS Producto,
                        PR.Precio,
                        PD.Cantidad
                    FROM 
                        Presupuestos P
                    LEFT JOIN 
                        PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                    LEFT JOIN 
                        Productos PR ON PD.idProducto = PR.idProducto
                    WHERE 
                        P.idPresupuesto = @idPresupuesto;";

            using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                SqliteCommand selectCmd = new SqliteCommand(consulta, conexion);
                selectCmd.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Si encuentro un presupuesto
                        if (presupuestoEncontrado == 0)
                        {
                            var id = Convert.ToInt32(reader["idPresupuesto"]);
                            var destinatario = reader["NombreDestinatario"].ToString();
                            var fecha = Convert.ToDateTime(reader["FechaCreacion"]);

                            presupuesto = new Presupuestos(id, destinatario, fecha);
                            presupuestoEncontrado++;
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                        {
                            var idProdu = Convert.ToInt32(reader["idProducto"]);
                            var descrip = reader["Producto"].ToString();
                            var precio = Convert.ToInt32(reader["Precio"]);

                            var producto = new Productos(idProdu, descrip, precio);
                            var detalle = new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));

                            presupuesto.AgregarDetalle(detalle);

                        }
                    }
                }
                conexion.Close();
            }
            return presupuesto;
        }

        public bool ExistePresupuesto(int id)
        {
            using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string consulta = "SELECT COUNT(*) FROM Presupuestos WHERE idPresupuesto = @id";
                using var selectCmd = new SqliteCommand(consulta, conexion);
                selectCmd.Parameters.Add(new SqliteParameter("@id", id));
                long cantidad = (long)selectCmd.ExecuteScalar();
                return cantidad > 0;
            }
        }

        public void AgregarDetalleProducto(int idPres, int idProd, int Cant)
        {

            using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string consulta = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cantidad)";
                using var insertCmd = new SqliteCommand(consulta, conexion);

                insertCmd.Parameters.Add(new SqliteParameter("@idPres", idPres));
                insertCmd.Parameters.Add(new SqliteParameter("@idProd", idProd));
                insertCmd.Parameters.Add(new SqliteParameter("@cantidad", Cant));
                insertCmd.ExecuteNonQuery();
                
            }
        }

    public void Eliminar(Presupuestos presupuesto)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";

            using var deleteCmd = new SqliteCommand(consulta, conexion);
            deleteCmd.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
            deleteCmd.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void ModificarPresupuesto(Presupuestos nuevo)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = @"UPDATE Presupuestos
                                SET NombreDestinatario = @Nombre, FechaCreacion = @Fecha
                                WHERE idPresupuesto = @id";
            using var updateCmd = new SqliteCommand(consulta, conexion);

            updateCmd.Parameters.Add(new SqliteParameter("@Nombre", nuevo.NombreDestinatario));
            updateCmd.Parameters.Add(new SqliteParameter("@Fecha", nuevo.FechaCreacion));
            updateCmd.Parameters.Add(new SqliteParameter("@id", nuevo.IdPresupuesto));
            updateCmd.ExecuteNonQuery();
            conexion.Close();                    
        }
    }
}
