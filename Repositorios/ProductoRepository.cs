using Microsoft.Data.Sqlite;
using System.ComponentModel.Design;
public class ProductoRepository
{
    string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "nuevo.db");

        // Asigna la cadena de conexión
    string cadenaConexion;
    public ProductoRepository()
    {
            // La conexión debe usar la ruta absoluta
        cadenaConexion = $"Data Source={dbPath};";
    }
    public List<Productos> GetAll()
    {
            //Consultar a la bd con el select construir la lista en el bucle y retornar
        var productos = new List<Productos>();

        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "SELECT * FROM Productos";
            using (SqliteCommand selectCmd = new SqliteCommand(consulta, conexion))
            using (SqliteDataReader reader = selectCmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    var id = Convert.ToInt32(reader["idProducto"]);
                    var descripcion = reader["Descripcion"].ToString();
                    var precio = Convert.ToInt32(reader["Precio"]);

                    var nuevoProducto = new Productos(id, descripcion, precio);
                    productos.Add(nuevoProducto);
                }

                return productos;
                conexion.Close();
            }

            
        }
    }

    public void InsertarProducto(Productos nuevoProducto)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "INSERT INTO Productos(Descripcion,Precio) VALUES (@Descripcion,@Precio)"; //consulta
            using var insertCmd = new SqliteCommand(consulta, conexion);

            insertCmd.Parameters.Add(new SqliteParameter("@Descripcion", nuevoProducto.Descripcion));
            insertCmd.Parameters.Add(new SqliteParameter("@Precio", nuevoProducto.Precio));
            insertCmd.ExecuteNonQuery();

        }
    }

    public void ModificarProducto(Productos nuevo)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = @"UPDATE Productos
                                SET Descripcion = @Desc, Precio = @Precio
                                WHERE idProducto = @id";
            using var updateCmd = new SqliteCommand(consulta, conexion);

            updateCmd.Parameters.Add(new SqliteParameter("@Desc", nuevo.Descripcion));
            updateCmd.Parameters.Add(new SqliteParameter("@Precio", nuevo.Precio));
            updateCmd.Parameters.Add(new SqliteParameter("@id", nuevo.IdProducto));
            updateCmd.ExecuteNonQuery();
            conexion.Close();                    
        }
    }

    public void Eliminar(Productos producto)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "DELETE FROM Productos WHERE idProducto = @id";

            using var deleteCmd = new SqliteCommand(consulta, conexion);
            deleteCmd.Parameters.Add(new SqliteParameter("@id", producto.IdProducto));
            deleteCmd.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public bool ExisteProducto(int id)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "SELECT COUNT(*) FROM Productos WHERE idProducto = @id";
            using var selectCmd = new SqliteCommand(consulta, conexion);
            selectCmd.Parameters.Add(new SqliteParameter("@id", id));
            long cantidad = (long)selectCmd.ExecuteScalar();
            return cantidad > 0;
        }
    }

    public Productos GetByID(int id)
    {
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            string consulta = "SELECT * FROM Productos WHERE idProducto = @id";
            using var selectCmd = new SqliteCommand(consulta, conexion);
            selectCmd.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = selectCmd.ExecuteReader())
            {
                Productos nuevoProducto = null;
                while (reader.Read())
                {
                    var idProd = Convert.ToInt32(reader["idProducto"]);
                    var descripcion = reader["Descripcion"].ToString();
                    var precio = Convert.ToInt32(reader["Precio"]);

                    nuevoProducto = new Productos(id, descripcion, precio);
                }

                return nuevoProducto;
            }
        }
    }   

}