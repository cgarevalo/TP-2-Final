using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;

namespace Logica
{
    public class LogicaComercio
    {
        AccesoDatos datosAcceso = new AccesoDatos();

        public List<Articulo> ListarArticulos()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = comando.CommandText = "Select A.Id idArt, Codigo, Nombre, A.Descripcion ArticuloDesc, C.Descripcion Categoria, M.Descripcion Marca, ImagenUrl, Precio, A.IdMarca idMarc, A.IdCategoria idCate From ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id And A.IdCategoria = C.Id";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)lector["idArt"];
                    art.CodigoArticulo = (string)lector["Codigo"];
                    art.Nombre = (string)lector["Nombre"];
                    art.Descripcion = (string)lector["ArticuloDesc"];
                    art.Precio = (decimal)lector["Precio"];

                    if (!(lector["ImagenUrl"] is DBNull))
                    {
                        art.Imagen = (string)lector["ImagenUrl"];
                    }

                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)lector["idCate"];
                    art.Categoria.Descripcion = (string)lector["Categoria"];

                    art.Marca = new Marca();
                    art.Marca.Id = (int)lector["idMarc"];
                    art.Marca.Descripcion = (string)lector["Marca"];
                    
                    listaArticulos.Add(art);

                }

                conexion.Close();

                return listaArticulos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Marca> ListaMarcas()
        {
            List<Marca> listaMarcas = new List<Marca>();

            try
            {
                datosAcceso.SetearConsulta("Select Id, Descripcion From MARCAS");
                datosAcceso.EjecutarConsulta();

                while (datosAcceso.Lector.Read())
                {
                    Marca marca = new Marca();

                    marca.Id = (int)datosAcceso.Lector["Id"];
                    marca.Descripcion = (string)datosAcceso.Lector["Descripcion"];
                    listaMarcas.Add(marca);

                }

                return listaMarcas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public List<Categoria> ListaCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                datosAcceso.SetearConsulta("Select Id, Descripcion From CATEGORIAS");
                datosAcceso.EjecutarConsulta();

                while (datosAcceso.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)datosAcceso.Lector["Id"];
                    categoria.Descripcion = (string)datosAcceso.Lector["Descripcion"];
                    listaCategorias.Add(categoria);

                }

                return listaCategorias;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public void Agregar(Articulo nuevoArticulo)
        {
            try
            {
                datosAcceso.SetearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) Values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");

                datosAcceso.SetearParametro("@codigo", nuevoArticulo.CodigoArticulo);
                datosAcceso.SetearParametro("@nombre", nuevoArticulo.Nombre);
                datosAcceso.SetearParametro("@descripcion", nuevoArticulo.Descripcion);
                datosAcceso.SetearParametro("@idCategoria", nuevoArticulo.Categoria.Id);
                datosAcceso.SetearParametro("@idMarca", nuevoArticulo.Marca.Id);
                datosAcceso.SetearParametro("@imagenUrl", nuevoArticulo.Imagen);
                datosAcceso.SetearParametro("@precio", nuevoArticulo.Precio);

                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public void Modificar(Articulo modificarArt)
        {
            try
            {
                datosAcceso.SetearConsulta("Update ARTICULOS Set Codigo = @codigo, Nombre = @nombre, Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imag, Precio = @precio Where Id = @id");

                datosAcceso.SetearParametro("@codigo", modificarArt.CodigoArticulo);
                datosAcceso.SetearParametro("@nombre", modificarArt.Nombre);
                datosAcceso.SetearParametro("@desc", modificarArt.Descripcion);
                datosAcceso.SetearParametro("@idMarca", modificarArt.Marca.Id);
                datosAcceso.SetearParametro("@idCategoria", modificarArt.Categoria.Id);
                datosAcceso.SetearParametro("@imag", modificarArt.Imagen);
                datosAcceso.SetearParametro("@precio", modificarArt.Precio);
                datosAcceso.SetearParametro("id", modificarArt.Id);

                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public void AgregarCategoria(Categoria nuevaCategoria)
        {
            try
            {
                datosAcceso.SetearConsulta("INSERT INTO CATEGORIAS (Descripcion) Values (@cateDesc)");
                datosAcceso.SetearParametro("@cateDesc", nuevaCategoria.Descripcion);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public void AgregarMarca(Marca nuevaMarca)
        {
            try
            {
                datosAcceso.SetearConsulta("INSERT INTO MARCAS (Descripcion) Values (@marcaDesc)");
                datosAcceso.SetearParametro("@MarcaDesc", nuevaMarca.Descripcion);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                datosAcceso.SetearConsulta("Delete From ARTICULOS Where Id = @id");
                datosAcceso.SetearParametro("@id", id);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public List<Articulo> Filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                string consulta = "Select A.Id idArt, Codigo, Nombre, A.Descripcion ArticuloDesc, C.Descripcion Categoria, M.Descripcion Marca, ImagenUrl, Precio, A.IdMarca idMarc, A.IdCategoria idCate From ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id And A.IdCategoria = C.Id And ";

                switch (campo)
                {
                    case "Precio":
                        switch (criterio)
                        {
                            case "Menor a":
                                consulta += "Precio < " + filtro;
                                break;

                            case "Mayor a":
                                consulta += "Precio > " + filtro;
                                break;

                            default:
                                consulta += "Precio =  " + filtro;
                                break;
                        }
                        break;

                    case "Marca":
                        consulta += "M.Descripcion Like '%" + filtro + "%'";
                        break;

                    case "Categoría":
                        consulta += "C.Descripcion Like '%" + filtro + "%'";
                        break;
                }

                datosAcceso.SetearConsulta(consulta);
                datosAcceso.EjecutarLectura();

                while (datosAcceso.Lector.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)datosAcceso.Lector["idArt"];
                    art.CodigoArticulo = (string)datosAcceso.Lector["Codigo"];
                    art.Nombre = (string)datosAcceso.Lector["Nombre"];
                    art.Descripcion = (string)datosAcceso.Lector["ArticuloDesc"];
                    art.Precio = (decimal)datosAcceso.Lector["Precio"];

                    if (!(datosAcceso.Lector["ImagenUrl"] is DBNull))
                    {
                        art.Imagen = (string)datosAcceso.Lector["ImagenUrl"];
                    }

                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)datosAcceso.Lector["idCate"];
                    art.Categoria.Descripcion = (string)datosAcceso.Lector["Categoria"];

                    art.Marca = new Marca();
                    art.Marca.Id = (int)datosAcceso.Lector["idMarc"];
                    art.Marca.Descripcion = (string)datosAcceso.Lector["Marca"];

                    articulos.Add(art);

                }

                return articulos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
