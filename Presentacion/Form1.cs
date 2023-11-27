using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;
using Dominio;

namespace Presentacion
{
    public partial class FrmComercio : Form
    {
        LogicaComercio logica = new LogicaComercio();

        private List<Articulo> listaArticulos;

        public FrmComercio()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            listaArticulos = logica.ListarArticulos();
            dgvArticulos.DataSource = listaArticulos;
            CargarImagen(listaArticulos[0].Imagen);
            OcultarColumnas();

        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            // Muestra la imagen del artículo seleccionado en el PictureBox.
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                CargarImagen(articuloSeleccionado.Imagen);

            }
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxArticulo.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            Refrescar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo artSeleccionado;
            // Obtiene el artículo seleccionado en el DataGridView.
            artSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            // Abre el formulario para modificar el artículo seleccionado.
            frmAltaArticulo modificar = new frmAltaArticulo(artSeleccionado);

            modificar.ShowDialog();

            Refrescar();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Articulo artSeleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar este artículo?", "Eliminar artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    artSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    logica.Eliminar(artSeleccionado.Id);
                    Refrescar();

                }     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OcultarColumnas()
        {
            //Escuende la columna "ImagenUrl" del dgv
            dgvArticulos.Columns["Imagen"].Visible = false;
            //Escuende la columna "Id" del dgv
            dgvArticulos.Columns["Id"].Visible = false;

        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.SelectedRows.Count > 0 && dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                // Obtiene el artículo seleccionado en el DataGridView.
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                // Abre el formulario de detalles del artículo seleccionado.
                frmDetallesArticulos detalles = new frmDetallesArticulos(seleccionado);
                detalles.ShowDialog();
                Refrescar();
                txtFiltro.Text = "";
            }
            else
            {
                MessageBox.Show("Seleccione un artículo");
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
            {
                // Filtra la lista de artículos según el texto ingresado en el filtro.
                listaFiltrada = listaArticulos.FindAll(a => a.Nombre.ToUpper().Contains(filtro.ToUpper()) || a.CodigoArticulo.ToUpper().Contains(filtro.ToUpper()) || a.Descripcion.ToUpper().Contains(filtro.ToUpper()) || a.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || a.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                // Si el filtro es corto o se borra lo escrito, muestra la lista completa de artículos.
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            OcultarColumnas();
        }
    }
}
