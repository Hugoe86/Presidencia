using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Presidencia.Creacion_Plazas.Negocio;
using System.Data;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Codigo_Programatico
{
    public class Cls_Ayudante_Codigo_Programatico
    {
        public static void Configurar_Codigo_Promatico(
            ref DropDownList Cmb_Fuente_Financiamiento,
            ref DropDownList Cmb_Area_Funcional,
            ref DropDownList Cmb_Proyecto_Programa,
            ref DropDownList Cmb_Unidad_Responsable,
            ref DropDownList Cmb_Partida,
            String Clave,
            ref TextBox Txt_SAP_Fuente_Financiamiento,
            ref TextBox Txt_SAP_Area_Responsable,
            ref TextBox Txt_SAP_Programa,
            ref TextBox Txt_SAP_Unidad_Responsable,
            ref TextBox Txt_SAP_Partida
            )
        {
            String Fuente_Financiameinto_ID = String.Empty;
            String Area_Funcional_ID = String.Empty;
            String Proyecto_Programa_ID = String.Empty;
            String Unidad_Responsable_ID = String.Empty;
            String Partida_ID = String.Empty;
            Cls_Cat_Nom_Creacion_Plazas_Negocio INF_PLAZA = null;
            try
            {
                INF_PLAZA = Consultar_Plaza(Clave);

                if (!String.IsNullOrEmpty(INF_PLAZA.P_S_Unidad_Responsable_ID) &&
                    !String.IsNullOrEmpty(INF_PLAZA.P_S_Area_Funcional_ID) &&
                    !String.IsNullOrEmpty(INF_PLAZA.P_S_Fte_Financiamiento_ID) &&
                    !String.IsNullOrEmpty(INF_PLAZA.P_S_Proyecto_Programa_ID) &&
                    !String.IsNullOrEmpty(INF_PLAZA.P_S_Partida_ID))
                {
                    Consultar_SAP_Unidades_Responsables(ref Cmb_Unidad_Responsable, INF_PLAZA.P_S_Unidad_Responsable_ID);
                    Consultar_SAP_Areas_Funcionales(ref Cmb_Area_Funcional, INF_PLAZA.P_S_Area_Funcional_ID);
                    Consultar_SAP_Fuentes_Financiamiento(ref Cmb_Fuente_Financiamiento, INF_PLAZA.P_S_Unidad_Responsable_ID, INF_PLAZA.P_S_Fte_Financiamiento_ID);
                    Consulta_SAP_Programas(ref Cmb_Proyecto_Programa, INF_PLAZA.P_S_Unidad_Responsable_ID, INF_PLAZA.P_S_Proyecto_Programa_ID);
                    Consultar_SAP_Partidas(ref Cmb_Partida, INF_PLAZA.P_S_Proyecto_Programa_ID, INF_PLAZA.P_S_Partida_ID);

                    Txt_SAP_Fuente_Financiamiento.Text = ((Cmb_Fuente_Financiamiento.SelectedItem.Text.Trim().Split('-')[0]) != null) ? Cmb_Fuente_Financiamiento.SelectedItem.Text.Trim().Split('-')[0].Trim() : String.Empty;
                    Txt_SAP_Area_Responsable.Text = ((Cmb_Area_Funcional.SelectedItem.Text.Trim().Split('-')[0]) != null) ? Cmb_Area_Funcional.SelectedItem.Text.Trim().Split('-')[0].Trim() : String.Empty;
                    Txt_SAP_Programa.Text = ((Cmb_Proyecto_Programa.SelectedItem.Text.Trim().Split('-')[0]) != null) ? Cmb_Proyecto_Programa.SelectedItem.Text.Trim().Split('-')[0].Trim() : String.Empty;
                    Txt_SAP_Unidad_Responsable.Text = ((Cmb_Unidad_Responsable.SelectedItem.Text.Trim().Split(')')[0]) != null) ? Cmb_Unidad_Responsable.SelectedItem.Text.Trim().Split(')')[0].Trim() : String.Empty;
                    Txt_SAP_Partida.Text = ((Cmb_Partida.SelectedItem.Text.Trim().Split('-')[0]) != null) ? Cmb_Partida.SelectedItem.Text.Trim().Split('-')[0].Trim() : String.Empty;
                }
                else {
                    Cmb_Fuente_Financiamiento.SelectedIndex = -1;
                    Cmb_Area_Funcional.SelectedIndex = -1;
                    Cmb_Proyecto_Programa.SelectedIndex = -1;
                    Cmb_Unidad_Responsable.SelectedIndex = -1;
                    Cmb_Partida.SelectedIndex = -1;

                    Cmb_Fuente_Financiamiento.DataSource = new DataTable();
                    Cmb_Proyecto_Programa.DataSource = new DataTable();
                    Cmb_Partida.DataSource = new DataTable();

                    Cmb_Fuente_Financiamiento.DataBind();
                    Cmb_Proyecto_Programa.DataBind();
                    Cmb_Partida.DataBind();

                    Txt_SAP_Fuente_Financiamiento.Text = String.Empty;
                    Txt_SAP_Area_Responsable.Text = String.Empty;
                    Txt_SAP_Programa.Text = String.Empty;
                    Txt_SAP_Unidad_Responsable.Text = String.Empty;
                    Txt_SAP_Partida.Text = String.Empty;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al configurar el codigo programatico del empleado. Error: [" + Ex.Message + "]");
            }
        }


        #region (Codigo Programatico)
        private static  DataTable Consultar_SAP_Unidades_Responsables(ref DropDownList Cmb_SAP_Unidad_Responsable, String Unidad_Resposable_ID)
        {
            Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Unidades_Responsables = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Resultado = null;//Variable que lista las unidades responsables registrdas en sistema.

            try
            {
                Dt_Resultado = Obj_Unidades_Responsables.Consulta_Dependencias();

                Cmb_SAP_Unidad_Responsable.DataSource = Dt_Resultado;
                Cmb_SAP_Unidad_Responsable.DataTextField = "CLAVE_NOMBRE";
                Cmb_SAP_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                Cmb_SAP_Unidad_Responsable.DataBind();
                Cmb_SAP_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_SAP_Unidad_Responsable.SelectedIndex = -1;

                Cmb_SAP_Unidad_Responsable.SelectedIndex = Cmb_SAP_Unidad_Responsable.Items.IndexOf(
                    Cmb_SAP_Unidad_Responsable.Items.FindByValue(Unidad_Resposable_ID));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        private static DataTable Consultar_SAP_Fuentes_Financiamiento(ref DropDownList Cmb_SAP_Fuente_Financiamiento, String Dependencia_ID, String Fuente_Financiameinto_ID)
        {
            Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Fte_Financiamiento = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Fte_Financiamiento = null;//Variable que almacenara los resultados de la busqueda realizada.

            try
            {
                Obj_Fte_Financiamiento.P_Unidad_Responsable_ID = Dependencia_ID;
                Dt_Fte_Financiamiento = Obj_Fte_Financiamiento.Consultar_Sap_Det_Fte_Dependencia();

                Cmb_SAP_Fuente_Financiamiento.DataSource = Dt_Fte_Financiamiento;
                Cmb_SAP_Fuente_Financiamiento.DataTextField = "FTE_FINANCIAMIENTO";
                Cmb_SAP_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                Cmb_SAP_Fuente_Financiamiento.DataBind();

                Cmb_SAP_Fuente_Financiamiento.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_SAP_Fuente_Financiamiento.SelectedIndex = -1;

                Cmb_SAP_Fuente_Financiamiento.SelectedIndex = Cmb_SAP_Fuente_Financiamiento.Items.IndexOf(
                    Cmb_SAP_Fuente_Financiamiento.Items.FindByValue(Fuente_Financiameinto_ID));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las fuentes de financiamento registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Fte_Financiamiento;
        }

        private static DataTable Consulta_SAP_Programas(ref DropDownList Cmb_SAP_Programas, String Dependencia_ID, String Proyecto_Programa_ID)
        {
            Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Programas = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios
            DataTable Dt_Programas = null;//Variable que alamacenara los resultados obtenidos de la busqueda realizada.

            try
            {
                Obj_Programas.P_Unidad_Responsable_ID = Dependencia_ID;
                Dt_Programas = Obj_Programas.Consultar_Sap_Det_Prog_Dependencia();

                Cmb_SAP_Programas.DataSource = Dt_Programas;
                Cmb_SAP_Programas.DataTextField = "PROYECTO_PROGRAMA";
                Cmb_SAP_Programas.DataValueField = Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                Cmb_SAP_Programas.DataBind();

                Cmb_SAP_Programas.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_SAP_Programas.SelectedIndex = -1;

                Cmb_SAP_Programas.SelectedIndex = Cmb_SAP_Programas.Items.IndexOf(
                    Cmb_SAP_Programas.Items.FindByValue(Proyecto_Programa_ID));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los programas registrados actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Programas;
        }

        private static DataTable Consultar_SAP_Partidas(ref DropDownList Cmb_SAP_Partida, String Programa_ID, String Partida_ID)
        {
            Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Partidas = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Partidas = null;

            try
            {
                Dt_Partidas = Obj_Partidas.Consultar_Partidas(Programa_ID);

                Cmb_SAP_Partida.DataSource = Dt_Partidas;
                Cmb_SAP_Partida.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Cmb_SAP_Partida.DataTextField = "PARTIDA";
                Cmb_SAP_Partida.DataBind();
                Cmb_SAP_Partida.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_SAP_Partida.SelectedIndex = -1;

                Cmb_SAP_Partida.SelectedIndex = Cmb_SAP_Partida.Items.IndexOf(
                    Cmb_SAP_Partida.Items.FindByValue(Partida_ID));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las partidas registrdas en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas;
        }

        private static DataTable Consultar_SAP_Areas_Funcionales(ref DropDownList Cmb_SAP_Area_Funcional, String Area_Funcional_ID)
        {
            Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Dependencias = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Areas_Funcionales = null;                                             //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

            try
            {
                Dt_Areas_Funcionales = Obj_Dependencias.Consulta_Area_Funcional();

                Cmb_SAP_Area_Funcional.DataSource = Dt_Areas_Funcionales;
                Cmb_SAP_Area_Funcional.DataTextField = "Clave_Nombre";
                Cmb_SAP_Area_Funcional.DataValueField = Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
                Cmb_SAP_Area_Funcional.DataBind();
                // Area_Funcional_ID
                Cmb_SAP_Area_Funcional.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_SAP_Area_Funcional.SelectedIndex = -1;

                Cmb_SAP_Area_Funcional.SelectedIndex = Cmb_SAP_Area_Funcional.Items.IndexOf(
                    Cmb_SAP_Area_Funcional.Items.FindByValue(Area_Funcional_ID));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las áreas funcionales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Areas_Funcionales;
        }
        #endregion

        private static Cls_Cat_Nom_Creacion_Plazas_Negocio Consultar_Plaza(String Clave)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Plazas = null;
            Cls_Cat_Nom_Creacion_Plazas_Negocio INF_PLAZA = new Cls_Cat_Nom_Creacion_Plazas_Negocio();

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + ".* ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Clave + "=" + Clave);

                Dt_Plazas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Plazas is DataTable) {
                    if (Dt_Plazas.Rows.Count > 0) {
                        foreach (DataRow item_plaza in Dt_Plazas.Rows) {
                            if (item_plaza is DataRow) {

                                if (!String.IsNullOrEmpty(item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_FTE_FINANCIAMIENTO_ID].ToString()))
                                    INF_PLAZA.P_S_Fte_Financiamiento_ID = item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_FTE_FINANCIAMIENTO_ID].ToString();

                                if (!String.IsNullOrEmpty(item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_AREA_FUNCIONAL_ID].ToString()))
                                    INF_PLAZA.P_S_Area_Funcional_ID = item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_AREA_FUNCIONAL_ID].ToString();

                                if (!String.IsNullOrEmpty(item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_PROGRAMA_ID].ToString()))
                                    INF_PLAZA.P_S_Proyecto_Programa_ID = item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_PROGRAMA_ID].ToString();

                                if (!String.IsNullOrEmpty(item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_DEPENDENCIA_ID].ToString()))
                                    INF_PLAZA.P_S_Unidad_Responsable_ID = item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_DEPENDENCIA_ID].ToString();

                                if (!String.IsNullOrEmpty(item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_PARTIDA_ID].ToString()))
                                    INF_PLAZA.P_S_Partida_ID = item_plaza[Cat_Nom_Dep_Puestos_Det.Campo_S_PARTIDA_ID].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return INF_PLAZA;
        }

        #region (Plazas)
        public static void Load_Plazas(ref DropDownList Ctrl_Plazas, String Empleado_ID, String Dependencia_ID)
        {
            DataTable Dt_Plazas = null;
            try
            {
                Dt_Plazas = Consultar_Plazas_Empleados(Dependencia_ID, String.Empty, String.Empty);
                Ctrl_Plazas.DataSource = Dt_Plazas;
                Ctrl_Plazas.DataTextField = "PLAZA";
                Ctrl_Plazas.DataValueField = Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID;
                Ctrl_Plazas.DataBind();

                Ctrl_Plazas.Items.Insert(0, new ListItem("<- Seleccione ->", String.Empty));

                foreach (ListItem item in Ctrl_Plazas.Items)
                {
                    if (item.Text.ToString().Contains("OCUPADO"))
                    {
                        item.Enabled = false;
                    }
                }

                if (!String.IsNullOrEmpty(Empleado_ID)) {
                    DataTable Dt_Aux = Consultar_Plazas_Empleados(String.Empty, Empleado_ID, String.Empty);

                    if (Dt_Aux is DataTable) {
                        if (Dt_Aux.Rows.Count > 0) {
                            foreach (DataRow PLAZA in Dt_Aux.Rows) {
                                if (PLAZA is DataRow) {

                                    foreach (ListItem item in Ctrl_Plazas.Items)
                                    {
                                        String clave = item.Text.Split('*')[0];

                                        if (clave.Equals(PLAZA[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString())) {
                                            item.Enabled = true;
                                            //item.Text = item.Text.Split('*')[1];

                                            Ctrl_Plazas.SelectedIndex = Ctrl_Plazas.Items.IndexOf(Ctrl_Plazas.Items.FindByText(item.Text));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar las plazas. Error: [" + Ex.Message + "]");
            }
        }

        private static DataTable Consultar_Plazas_Empleados(String Unidad_Responsable_ID, String Empleado_ID, String Clave)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Plazas = null;

            try
            {
                Mi_SQL.Append("select ");
               
                Mi_SQL.Append("(");
                Mi_SQL.Append("TRIM(" + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + ") || '*' || ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " || '  [' || ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + " || ']'");
                Mi_SQL.Append(") as PLAZA, ");

                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " left outer join ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " on ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                if (!String.IsNullOrEmpty(Unidad_Responsable_ID)) {
                    if (Mi_SQL.ToString().Contains("where"))
                        Mi_SQL.Append(" and " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID + "'");
                    else
                        Mi_SQL.Append(" where " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID + "'");
                }

                if (!String.IsNullOrEmpty(Empleado_ID))
                {
                    if (Mi_SQL.ToString().Contains("where"))
                        Mi_SQL.Append(" and " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                    else
                        Mi_SQL.Append(" where " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Clave))
                {
                    if (Mi_SQL.ToString().Contains("where"))
                        Mi_SQL.Append(" and " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'");
                    else
                        Mi_SQL.Append(" where " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'");
                }

                Dt_Plazas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las plazas. Error: [" + Ex.Message + "]");
            }
            return Dt_Plazas;
        }
        #endregion
    }
}
