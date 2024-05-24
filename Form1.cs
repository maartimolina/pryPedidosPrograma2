using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryPedidosPrograma2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {

            clsBaseDatos db = new clsBaseDatos();
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            db.ValidarUsuario(usuario, contraseña);

            MessageBox.Show(db.estadoConexion);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
