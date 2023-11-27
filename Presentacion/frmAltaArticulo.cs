using Dominio;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo artic = null;
        LogicaComercio altaLogica = new LogicaComercio();

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            // Constructor utilizado para modificar un artículo existente, recibe un artículo como parámetro
            InitializeComponent();
            artic = articulo;
            Text = "Modificar artículo"; //Cambia el título de la ventana
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            try
            {
                // Recibe una lista de categorías y le agrega la opción "Nueva categoría" al final
                List<Categoria> categoria = altaLogica.ListaCategorias();
                categoria.Add(new Categoria { Descripcion = "Nueva categoría" });

                // Recibe una lista de marcas y le agrega la opción "Nueva marca" al final
                List<Marca> marca = altaLogica.ListaMarcas();
                marca.Add(new Marca { Descripcion = "Nueva marca" });

                //Asigna la lista a cboMarca
                cboMarca.DataSource = marca;
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                //asigna la lista a cboCategoria
                cboCategoria.DataSource = categoria;
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (artic != null)
                {
                    txtCodigoArticulo.Text = artic.CodigoArticulo;
                    txtNombre.Text = artic.Nombre;
                    txtDescripcion.Text = artic.Descripcion;
                    txtImagenUrl.Text = artic.Imagen;
                    txtPrecio.Text = artic.Precio.ToString();
                    CargarImagen(artic.Imagen);
                    cboCategoria.SelectedValue = artic.Categoria.Id;
                    cboMarca.SelectedValue = artic.Marca.Id;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio;

            try
            {
                // Si artic llega en null significa que se quiere crea un nuevo artículo, si no, significa que se quiere modificar uno existente
                if (artic == null)
                {
                    artic = new Articulo();
                }

                string codigo = txtCodigoArticulo.Text;
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                string imagen = txtImagenUrl.Text;

                // Verifica que los TextBox tengan algo escrito
                if (!string.IsNullOrWhiteSpace(codigo) && !string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(descripcion) && !string.IsNullOrWhiteSpace(imagen))
                {
                    artic.CodigoArticulo = codigo;
                    artic.Nombre = nombre;
                    artic.Descripcion = descripcion;
                    artic.Imagen = imagen;
                }
                else
                {
                    MessageBox.Show("Por favor, rellene todos los campos.");
                    return;
                }

                // 
                // Verifica si txtPrecio está vacío, y si se ingresó un número
                if (decimal.TryParse(txtPrecio.Text, out precio) && txtPrecio.Text.Trim() != "")
                {
                    artic.Precio = precio;                   
                }
                else
                {
                    MessageBox.Show("El campo 'Precio' no puede estár vacío, y solo admite números.");
                    return;
                }

                // Carga como marca y categoría lo que se tenga seleccionado en el cboMarca y cboCategoria, mientras no sea "Nueva Marca" y "Nueva categoría"

                artic.Categoria = (Categoria)cboCategoria.SelectedItem;
                artic.Marca = (Marca)cboMarca.SelectedItem;

                if (artic.Id != 0)
                {
                    altaLogica.Modificar(artic);
                    MessageBox.Show("Artículo modificado exitosamente");
                }
                else
                {
                    altaLogica.Agregar(artic);
                    MessageBox.Show("Artículo agrgado exitosamente");
                }

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Método para cargar la una imagen
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxAltaImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxAltaImagen.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }

        // Método que llama al método CargarImagen, para que cargue la imagen despues de salir del txtImagenUrl
        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagenUrl.Text);
        }

        // Método que detecta si se seleccionó "Nueva marca", y le permite escribir la nueva marca deseada
        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            string marcSelec = cboMarca.SelectedItem.ToString();

            if (marcSelec == "Nueva marca")
            {
                MessageBox.Show("Ingrese la nueva marca.");
                cboMarca.DropDownStyle = ComboBoxStyle.DropDown;
            } 
        }

        // Guarda la nueva marca, llamando al método AgregarMarca, y lo agraga al cboMarca
        private void btnGuardarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcionMarca = cboMarca.Text;

                if (!(string.IsNullOrEmpty(descripcionMarca)))
                {
                    Marca nuavaMarca = new Marca { Descripcion = descripcionMarca };
                    altaLogica.AgregarMarca(nuavaMarca);
                    MessageBox.Show("Marca agregada.");

                    // Actualiza la lista de marcas después de agregar la nueva
                    List<Marca> marcaNueva = altaLogica.ListaMarcas();
                    marcaNueva.Add(new Marca { Descripcion = "Nueva marca" });
                    cboMarca.DataSource = marcaNueva;
                    cboMarca.ValueMember = "Id";
                    cboMarca.DisplayMember = "Descripcion";

                    cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Método que detecta si se seleccionó "Nueva categoría", y le permite escribir la nueva categoría deseada
        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cateSelec = cboCategoria.SelectedItem.ToString();

            if (cateSelec == "Nueva categoría")
            {
                MessageBox.Show("Ingrese la nueva marca.");
                cboCategoria.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        // Guarda la nueva categoría, llamando al método AgregarCategoria, y lo agraga al cboCategoria
        private void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcionCate = cboCategoria.Text;

                if (!(string.IsNullOrEmpty(descripcionCate)))
                {
                    Categoria nuevaCategoria = new Categoria { Descripcion = descripcionCate };
                    altaLogica.AgregarCategoria(nuevaCategoria);
                    MessageBox.Show("Categoría agregada");

                    List<Categoria> cateNueva = altaLogica.ListaCategorias();
                    cateNueva.Add(new Categoria { Descripcion = "Nueva categoría" });
                    cboCategoria.DataSource = cateNueva;
                    cboCategoria.ValueMember = "Id";
                    cboCategoria.DisplayMember = "Descripcion";

                    cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
