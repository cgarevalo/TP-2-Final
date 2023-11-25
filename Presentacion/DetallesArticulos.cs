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
    public partial class frmDetallesArticulos : Form
    {
        private Articulo articulo;
        LogicaComercio logica = new LogicaComercio(); 

        public frmDetallesArticulos(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void DetallesArticulos_Load(object sender, EventArgs e)
        {
            dgvDetalles.Rows.Add("Id", articulo.Id);
            dgvDetalles.Rows.Add("Código", articulo.CodigoArticulo);
            dgvDetalles.Rows.Add("Nombre", articulo.Nombre);
            dgvDetalles.Rows.Add("Descripción", articulo.Descripcion);
            dgvDetalles.Rows.Add("Imagen", articulo.Imagen);
            dgvDetalles.Rows.Add("Marca", articulo.Marca.Descripcion);
            dgvDetalles.Rows.Add("Categoría", articulo.Categoria.Descripcion);
            dgvDetalles.Rows.Add("Precio", articulo.Precio);
       
            CargarImagen(articulo.Imagen);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pboImagenArt.Load(imagen);
            }
            catch (Exception)
            {
                pboImagenArt.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }
    }
}
