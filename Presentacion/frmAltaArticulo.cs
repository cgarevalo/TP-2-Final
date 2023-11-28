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
                if (!string.IsNullOrWhiteSpace(codigo) && !string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(descripcion) && !string.IsNullOrWhiteSpace(imagen) && !string.IsNullOrWhiteSpace(cboCategoria.Text) && !string.IsNullOrWhiteSpace(cboMarca.Text))
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
            if (!string.IsNullOrWhiteSpace(cboMarca.Text))
            {
                string marcSelec = cboMarca.SelectedItem.ToString();

                if (marcSelec == "Nueva marca")
                {
                    MessageBox.Show("Ingrese la nueva marca.");

                    // Modifica el cboMarca para que se pueda escribir
                    cboMarca.DropDownStyle = ComboBoxStyle.DropDown;

                    // Activa el botón Guardar marca
                    btnGuardarMarca.Enabled = true;
                }
                else
                {
                    // Modifica el cboMarca para que ya no se pueda escribir
                    cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;

                    // Desactiva el bóton Guardar marca
                    btnGuardarMarca.Enabled = false;
                }
            }
        }

        // Guarda la nueva marca, llamando al método AgregarMarca, y lo agrega al cboMarca
        private void btnGuardarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcionMarca = cboMarca.Text;

                // Verifica si el campo CboMarca está vacío
                if (!string.IsNullOrEmpty(descripcionMarca))
                {
                    // Lista que contiene todas las marcas para comprobar si existe en la base de datos. También contiene un objeto nuevo 'Marca' con la descripción 'Nueva marca' para que no permita guardar una marca llamada 'Nueva marca' en la base de datos.
                    List<Marca> listaConMarcas = altaLogica.ListaMarcas();
                    listaConMarcas.Add(new Marca { Descripcion = "Nueva marca" });

                    // Verifica si la marca ya existe
                    if (!listaConMarcas.Exists(m => m.Descripcion.ToLower() == descripcionMarca.ToLower()))
                    {
                        Marca nuavaMarca = new Marca { Descripcion = descripcionMarca };
                        altaLogica.AgregarMarca(nuavaMarca);
                        MessageBox.Show("Marca agregada.");

                        // Actualiza la lista de marcas después de agregar la nueva
                        List<Marca> mostrarMarcas = altaLogica.ListaMarcas();
                        mostrarMarcas.Add(new Marca { Descripcion = "Nueva marca" });
                        cboMarca.DataSource = null;
                        cboMarca.DataSource = mostrarMarcas;
                        cboMarca.ValueMember = "Id";
                        cboMarca.DisplayMember = "Descripcion";

                        // Modifica el cboMarca para que ya no se pueda escribir
                        cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;

                        // Desactiva el botón Guardar marca
                        btnGuardarMarca.Enabled = false;

                        // Seleccionar automáticamente la nueva marca
                        cboMarca.SelectedIndex = mostrarMarcas.FindIndex(m => m.Descripcion == descripcionMarca);

                    }
                    else
                    {
                        MessageBox.Show("La marca ya existe o no ha modificado el campo.");
                    }
                }
                else
                {
                    MessageBox.Show("No puede dejar este campo vacío.");
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
            if (!string.IsNullOrWhiteSpace(cboCategoria.Text))
            {
                string cateSelec = cboCategoria.SelectedItem.ToString();

                if (cateSelec == "Nueva categoría")
                {
                    MessageBox.Show("Ingrese la nueva categoría.");

                    // Modifica el cboCategoría para que se pueda escribir
                    cboCategoria.DropDownStyle = ComboBoxStyle.DropDown;

                    // Activa el botón Guardar categoría
                    btnGuardarCategoria.Enabled = true;
                }
                else
                {
                    // Modifica el cboCategoría para que ya no se pueda escribir
                    cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

                    // Desavtiva el botón Guardar categoría
                    btnGuardarCategoria.Enabled = false;
                }
            }
            
        }

        // Guarda la nueva categoría, llamando al método AgregarCategoria, y lo agraga al cboCategoria
        private void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcionCate = cboCategoria.Text;

                // Verifica si el campo Categoría está vacío
                if (!string.IsNullOrEmpty(descripcionCate))
                {
                    // Lista que contiene todas las categorías para comprobar si existe en la base de datos. También contiene un objeto nuevo 'Categoria' con la descripción 'Nueva categoría' para que no permita guardar una categoría llamada 'Nueva categoría' en la base de datos.
                    List<Categoria> listaConCategorias = altaLogica.ListaCategorias();
                    listaConCategorias.Add(new Categoria { Descripcion = "Nueva categoría" });

                    // Verifica si la categoría ya existe
                    if (!listaConCategorias.Exists(c => c.Descripcion.ToLower() == descripcionCate.ToLower()))
                    {
                        Categoria nuevaCategoria = new Categoria { Descripcion = descripcionCate };
                        altaLogica.AgregarCategoria(nuevaCategoria);
                        MessageBox.Show("Categoría agregada");

                        // Actualiza la lista de categorías después de agregar la nueva
                        List<Categoria> mostrarCategorias = altaLogica.ListaCategorias();
                        mostrarCategorias.Add(new Categoria { Descripcion = "Nueva categoría" });
                        cboCategoria.DataSource = null;
                        cboCategoria.DataSource = mostrarCategorias;
                        cboCategoria.ValueMember = "Id";
                        cboCategoria.DisplayMember = "Descripcion";

                        // Modifica el cboCategoría para que ya no se pueda escribir
                        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

                        // Desactiva el botón Guardar categoría
                        btnGuardarCategoria.Enabled = false;

                        // Seleccionar automáticamente la nueva categoría
                        cboCategoria.SelectedIndex = mostrarCategorias.FindIndex(c => c.Descripcion == descripcionCate);

                    }
                    else
                    {
                        MessageBox.Show("La categoría ya existe o no ha modificado el campo.");
                    }
                }
                else
                {
                    MessageBox.Show("No puede dejar este campo vacío.");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
