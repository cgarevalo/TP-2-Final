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

            cboFiltroCampo.Items.Add("Precio");
            cboFiltroCampo.Items.Add("Marca");
            cboFiltroCampo.Items.Add("Categoría");

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
            artSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
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

        private bool ValidarFiltro()
        {
            if (cboFiltroCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un campo.");
                return true;

            }
            if (cboFiltroCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un criterio.");
                return true;

            }
            if (cboFiltroCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    if (!(SoloNumeros(txtFiltro.Text)))
                    {
                        MessageBox.Show("Solo números.");
                        return true;
                    }
                }
            }

            return false;
        }

        private void cboFiltroCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboFiltroCampo.SelectedItem.ToString();

            if (opcion == "Precio")
            {
                cboFiltroCriterio.Items.Clear();
                cboFiltroCriterio.Items.Add("Menor a");
                cboFiltroCriterio.Items.Add("Mayor a");
                cboFiltroCriterio.Items.Add("Igual a");
            }
            else if (opcion == "Marca")
            {
                cboFiltroCriterio.Items.Clear();
                cboFiltroCriterio.DataSource = logica.ListaMarcas();
            }
            else
            {
                cboFiltroCriterio.DataSource = logica.ListaCategorias();
            }
        }

        private bool SoloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (char.IsNumber(caracter))
                {
                    return false;
                }
            }

            return true;

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                string campo = cboFiltroCampo.SelectedItem?.ToString();
                string criterio = cboFiltroCriterio.SelectedItem?.ToString();
                string filtro = txtFiltro.Text;

                if (string.IsNullOrEmpty(campo) || string.IsNullOrEmpty(criterio) || string.IsNullOrEmpty(filtro))
                {
                    MessageBox.Show("Por favor, seleccione un campo, un criterio y proporcione un valor para filtrar.");
                    return;
                }

                List<Articulo> listaFiltro = logica.Filtrar(campo, criterio, filtro);
                dgvArticulos.DataSource = listaFiltro;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            /*try
            {
                if (!(ValidarFiltro()))
                {
                    return;
                }
                string campo = cboFiltroCampo.SelectedItem.ToString();
                string criterio = cboFiltroCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = logica.Filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }*/

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
            if (dgvArticulos.SelectedRows.Count > 0)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmDetallesArticulos detalles = new frmDetallesArticulos(seleccionado);
                detalles.ShowDialog();
                Refrescar();
            }
        }

        private void btnFiltrar_Click_1(object sender, EventArgs e)
        {   

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaArticulos.FindAll(a => a.Nombre.ToUpper().Contains(filtro.ToUpper()) || a.CodigoArticulo.ToUpper().Contains(filtro.ToUpper()) || a.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            OcultarColumnas();

        }
    }
}
