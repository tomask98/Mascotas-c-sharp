using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//CURSO – LEGAJO – APELLIDO – NOMBRE

namespace ABMMascotas
{
    public partial class frmMascota : Form
    {
        AccesoDatos oBD;
        List<Mascota> LMascota;
        public frmMascota()
        {
            InitializeComponent();
            oBD= new AccesoDatos();
            LMascota = new List<Mascota>();
        }

        private void frmMascota_Load(object sender, EventArgs e)
        {
            CargarCombo();
            CargarLista();
            Habilitar(false);
        }

        private void Habilitar(bool v)
        {

            btnGrabar.Enabled = v;
            btnSalir.Enabled = !v;
            btnNuevo.Enabled = !v;
            cboEspecie.Enabled = v;
            lstMascotas.Enabled = v;
            txtCodigo.Enabled = v;
            txtNombre.Enabled = v;
            dtpFechaNacimiento.Enabled = v;
            rbtHembra.Enabled = v;
            rbtMacho.Enabled= v;
            btnEditar.Enabled = !v;
        }

        private void CargarLista()
        {
            LMascota.Clear();
            lstMascotas.Items.Clear();
            DataTable tabla = oBD.consultaBD("SELECT * FROM Mascotas");
            foreach (DataRow fila in tabla.Rows)
            {
                Mascota M = new Mascota();
                M.pCodigo = Convert.ToInt32(fila["codigo"]);
                M.pNombre = Convert.ToString(fila["Nombre"]);
                M.pSexo = Convert.ToInt32(fila["Sexo"]);
                M.pFechaNacimiento = Convert.ToDateTime(fila["fechaNacimiento"]);


                LMascota.Add(M);
                lstMascotas.Items.Add(M);
            }
        }

        private void CargarCombo()
        {
            DataTable Tabla = oBD.consultaBD("SELECT * FROM Especies");
            cboEspecie.DataSource = Tabla;
            cboEspecie.DisplayMember = "nombreEspecie";
            cboEspecie.ValueMember = "idEspecie";
            cboEspecie.DropDownStyle= ComboBoxStyle.DropDownList;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quiere salir ?", "Saliendo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
                Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            Limpiar();
            

        }

        private void Limpiar()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            cboEspecie.SelectedIndex = -1;
            rbtHembra.Checked = false;
            rbtMacho.Checked = false;
            dtpFechaNacimiento.Value= DateTime.Now;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                 //Creacion del Objeto Mascota

                Mascota M = new Mascota();

                M.pCodigo = Convert.ToInt32(txtCodigo.Text);
                M.pNombre = txtNombre.Text;
                M.pEspecie = Convert.ToInt32(cboEspecie.SelectedValue);
                if(rbtHembra.Checked)
                {
                    M.pSexo = 2;
                }
                else
                {
                    M.pSexo = 1;
                }
                M.pFechaNacimiento = dtpFechaNacimiento.Value;

                if (!Existe(M))
                {
                   string insertSQL = "INSERT INTO Mascotas VALUES (@codigo,@nombre,@especie,@sexo,@fechaNacimiento)";

                    List<Parametro> lParametros = new List<Parametro>();
                    lParametros.Add(new Parametro("@codigo", M.pCodigo));
                    lParametros.Add(new Parametro("@nombre", M.pNombre));
                    lParametros.Add(new Parametro("@especie", M.pEspecie));
                    lParametros.Add(new Parametro("@sexo", M.pSexo));
                    lParametros.Add(new Parametro("@fechaNacimiento", M.pFechaNacimiento));

                    if (oBD.actualizarBD(insertSQL, lParametros) > 0)
                    {
                        MessageBox.Show("Se insertó con éxito una nueva mascota!!!");
                        CargarLista();
                    }

                }
                    else
                        MessageBox.Show("La mascota ya existe!!!");
                    if (Existe(M))
                {
                    string UpdateSQL = "UPDATE Mascotas SET nombre=@nombre, especie=@especie, sexo=@sexo, fechaNacimiento=@fechaNacimiento WHERE codigo=@Codigo";
                    List<Parametro> lParametros = new List<Parametro>();
                    lParametros.Add(new Parametro("@codigo", M.pCodigo));
                    lParametros.Add(new Parametro("@nombre", M.pNombre));
                    lParametros.Add(new Parametro("@especie", M.pEspecie));
                    lParametros.Add(new Parametro("@sexo", M.pSexo));
                    lParametros.Add(new Parametro("@fechaNacimiento", M.pFechaNacimiento));
                    if (oBD.actualizarBD(UpdateSQL, lParametros) > 0)
                    {
                        MessageBox.Show("Se Modifico con exito");
                        CargarLista();
                    }

                }



                Habilitar(false);
            }
        }

        private bool Existe(Mascota nueva)
        {
            for (int i = 0; i < LMascota.Count; i++)
            {
                if(LMascota[i].pCodigo == nueva.pCodigo)
                return true;
            }
            return false;


        }

        private bool Validar()
        {
            bool Valido = true;
            if (txtCodigo.Text == "")
            {
                MessageBox.Show("El campo no puede quedar vacio");
                txtCodigo.Focus();
                Valido = false;
            }
            else
                try
                {
                    int.Parse(txtCodigo.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Se debe ingresar valores Numericos");
                    txtCodigo.Focus();
                    Valido = false;

                }
           if(txtNombre.Text == "")
            {
                MessageBox.Show("El campo no puede quedar vacio");
                txtNombre.Focus();
             Valido= false;
            }
           else
           if(cboEspecie.SelectedIndex == -1)
            {
                MessageBox.Show("El campo no puede quedar vacio");
                cboEspecie.Focus();
                Valido = false;
            }
           else
                if(!rbtHembra.Checked && !rbtMacho.Checked)
            {
                MessageBox.Show("Tiene que seleccionar un sexo");
                rbtHembra.Focus();
                Valido = false;
            }

           else 
                if( DateTime.Today.Year-dtpFechaNacimiento.Value.Year  > 10)
            {
                MessageBox.Show("La Mascota no puede tener mas de 10 años");
                dtpFechaNacimiento.Focus();
                Valido = false;
            }

            return Valido;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            txtCodigo.Enabled = false;
        }

        private void lstMascotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCampo(lstMascotas.SelectedIndex);
        }

        private void cargarCampo(int posicion)
        {
            txtCodigo.Text = LMascota[posicion].pCodigo.ToString();
            txtNombre.Text = LMascota[posicion].pNombre;
            if(LMascota[posicion].pSexo==1)
            {
                rbtHembra.Checked = true;
            }
            else
            {
                rbtMacho.Checked = true;
            }
            dtpFechaNacimiento.Value = LMascota[posicion].pFechaNacimiento;

        }
    }
}
