using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Control_Patrimonial_Reporte_Completo_Vehiculos.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Completo_Vehiculos_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Completo_Vehiculos.Negocio { 

    public class Cls_Rpt_Pat_Completo_Vehiculos_Negocio {

        #region "Variables Internas"

            private String Vehiculo_ID = null;

        #endregion

        #region "Variables Publicas"

            public String P_Vehiculo_ID {
                get { return Vehiculo_ID; }
                set { Vehiculo_ID = value; }
            }

        #endregion

        #region "Metodos"

            public DataTable Obtener_Datos_Generales() {
                return Cls_Rpt_Pat_Completo_Vehiculos_Datos.Obtener_Datos_Generales(this);
            }

            public DataTable Obtener_Datos_Adquisicion() {
                return Cls_Rpt_Pat_Completo_Vehiculos_Datos.Obtener_Datos_Adquisicion(this);
            }

            public DataTable Obtener_Datos_Estado_Detalles_Vehiculo() {
                DataTable Dt_Datos_Procesados = new DataTable();
                Dt_Datos_Procesados.Columns.Add("DESCRIPCION_1", Type.GetType("System.String"));
                Dt_Datos_Procesados.Columns.Add("ESTADO_1", Type.GetType("System.String"));
                Dt_Datos_Procesados.Columns.Add("DESCRIPCION_2", Type.GetType("System.String"));
                Dt_Datos_Procesados.Columns.Add("ESTADO_2", Type.GetType("System.String"));
                Dt_Datos_Procesados.Columns.Add("DESCRIPCION_3", Type.GetType("System.String"));
                Dt_Datos_Procesados.Columns.Add("ESTADO_3", Type.GetType("System.String"));

                DataTable Dt_Datos_Estado_Detalles_Vehiculo = Cls_Rpt_Pat_Completo_Vehiculos_Datos.Obtener_Datos_Estado_Detalles_Vehiculo(this);


                for (Int32 Contador = 0; Contador < Dt_Datos_Estado_Detalles_Vehiculo.Rows.Count; Contador = Contador + 3 ) {
                    Int32 Entrar_A = 1;
                    DataRow Fila = Dt_Datos_Procesados.NewRow();
                    for (Int32 Contador_Interno = 0; Contador_Interno < 3; Contador_Interno++) {
                        if ((Contador + Contador_Interno) < Dt_Datos_Estado_Detalles_Vehiculo.Rows.Count) { 
                            if (Entrar_A == 1) {
                                Fila["DESCRIPCION_1"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["DESCRIPCION"].ToString();
                                Fila["ESTADO_1"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["ESTADO"].ToString();
                                Entrar_A++;
                            } else if (Entrar_A == 2) {
                                Fila["DESCRIPCION_2"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["DESCRIPCION"].ToString();
                                Fila["ESTADO_2"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["ESTADO"].ToString();
                                Entrar_A++;
                            } else if (Entrar_A == 3) {
                                Fila["DESCRIPCION_3"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["DESCRIPCION"].ToString();
                                Fila["ESTADO_3"] = Dt_Datos_Estado_Detalles_Vehiculo.Rows[(Contador + Contador_Interno)]["ESTADO"].ToString();
                            }
                        }
                    }
                    Dt_Datos_Procesados.Rows.Add(Fila);
                }

                return Dt_Datos_Procesados;
            }
            
        #endregion

    }                                              

}

