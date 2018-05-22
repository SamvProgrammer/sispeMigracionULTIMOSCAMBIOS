﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISPE_MIGRACION.formularios.PRESTACIONES_ECON.OTORGAMIENTO_PH
{
    public partial class hipotecarios : Form
    {
        public hipotecarios()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "select * from datos.hip";
            List<Dictionary<string, object>> r = globales.consulta(query);
            object[] arreglo = new object[r.Count];
            barra.Maximum = r.Count;
            int barrax = 0;
            int contador = 0;
            int porcentaje = 0;
            int checarPorcentaje = r.Count / 100;
            int verificador = 0;
            foreach (Dictionary<string, object> item in r) {
                Application.DoEvents();
                barrax++;
                barra.Value = barrax;

                if (checarPorcentaje == verificador) {
                    porcentaje++;
                    label1.Text = porcentaje.ToString() + " %";
                    verificador = 0;
                }
                verificador++;
                
                query = string.Format("select * from datos.edoctahip where folio = {0} order by f_desc desc limit 1;", item["folio"]);
                List<Dictionary<string, object>> auxItem = globales.consulta(query);
                int numDescPendiente = Convert.ToInt32(auxItem[0]["totdes"]) - Convert.ToInt32(auxItem[0]["numdesc"]);
                double importe_unitario_diego = Convert.ToDouble(item["imp_unit"]);
                double impor_unitario_santi = Convert.ToDouble(auxItem[0]["imp_unit"]);
                double importe_bueno = importe_unitario_diego;
                if (Convert.ToInt32(item["folio"])== 4780) {
                    int n = 43;
                }
                if (importe_unitario_diego != impor_unitario_santi) {
                    int aux1 = Convert.ToInt32(impor_unitario_santi / 2);
                    int aux2 = Convert.ToInt32(importe_unitario_diego);
                    if (aux1 == aux2)
                    {
                        string fechaauxs = Convert.ToString(auxItem[0]["f_desc"]).Replace(" 12:00:00 a. m.", "").Split('/')[1];
                        if (fechaauxs != "12" && fechaauxs != "5" ) {
                            importe_bueno = impor_unitario_santi;
                        }
                        
                    }
                    else {
                    
                    }
                    
                }

                double pagoPendiente = importe_bueno * numDescPendiente;
                double importe = Convert.ToDouble(item["importe"]);
                double saldo = Convert.ToDouble(item["saldo"]);

                double diferencia = saldo - pagoPendiente;
                if (saldo == pagoPendiente) continue;

                object[] objeto = { item["folio"], item["rfc"], item["saldo"], pagoPendiente, diferencia,"","","","","",item["nombre"] };
                arreglo[contador] = objeto;
                contador++;
            }

            object[] reporteador = new object[contador];

            for (int x = 0; x < arreglo.Length; x++) {
                if (arreglo[x] == null) break;
                reporteador[x] = arreglo[x];
                 
            }

          globales.reportes("reportePaty", "datosPaty",reporteador);
        }
    }
}
