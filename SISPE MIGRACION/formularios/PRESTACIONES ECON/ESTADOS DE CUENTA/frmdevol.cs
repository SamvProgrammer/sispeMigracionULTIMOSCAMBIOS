﻿using SISPE_MIGRACION.formularios.CATÁLOGOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISPE_MIGRACION.formularios.PRESTACIONES_ECON.ESTADOS_DE_CUENTA
{
    public partial class frmdevol : Form
    {
        public frmdevol()
        {
            InitializeComponent();
        }

        private void frmdevol_Load(object sender, EventArgs e)
        {

            frmEmpleados p_quirog = new frmEmpleados();
            p_quirog.enviar = rellenarConsulta;

            p_quirog.ShowDialog();

        }


        public void rellenarConsulta(Dictionary<string, object> resultado, bool externo = false)
        {
            this.txtrfc.Text = Convert.ToString(resultado["rfc"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rfc1 = txtrfc.Text;
            object[] objetotablaReporte;

            string query = string.Format("select * from datos.p_quirog where rfc = '{0}'", rfc1);
            List<Dictionary<string, object>> resultado = globales.consulta(query);
            objetotablaReporte = new object[resultado.Count];
            int contador = 0;

            foreach (Dictionary<string, object> item in resultado)
            {
                string folio = Convert.ToString(item["folio"]);
                string rfc = Convert.ToString(item["rfc"]);
                string nombre_em = Convert.ToString(item["nombre_em"]);
                string emision_cheque = Convert.ToString(item["f_emischeq"]).Replace(" 12:00:00 a. m.", "");
                string importe = Convert.ToString(item["importe"]);

                query = string.Format("select  max(f_descuento) as fultimopago,sum(importe) as pagado,({0} - sum(importe)) as saldo from datos.descuentos where folio = {1}", importe, folio);
                List<Dictionary<string, object>> tmp = globales.consulta(query);
                string fultimopago = Convert.ToString(tmp[0]["fultimopago"]).Replace(" 12:00:00 a. m.", "");
                string pagado = Convert.ToString(tmp[0]["pagado"]);
                string saldo = Convert.ToString(tmp[0]["saldo"]);
                if (string.IsNullOrWhiteSpace(fultimopago)) continue;

                object[] arregloAux = { rfc, nombre_em, folio, "SUSCRIPTOR", fultimopago, importe, pagado, saldo, emision_cheque };
                objetotablaReporte[contador] = arregloAux;
                contador++;
            }

            object[] objectoenviar = new object[contador];
            for (int x = 0; x < objetotablaReporte.Length; x++)
            {
                if (objetotablaReporte[x] == null) break;
                objectoenviar[x] = objetotablaReporte[x];
            }


            globales.reportes("reporteConsultaPDevolucion", "consultaDevolucion", objectoenviar);
        }

        private void frmdevol_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogo = MessageBox.Show("¿Desea cerrar el módulo?",
          "Cerrar el módulo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogo == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void frmdevol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Close();
            }
        }
    }
}

