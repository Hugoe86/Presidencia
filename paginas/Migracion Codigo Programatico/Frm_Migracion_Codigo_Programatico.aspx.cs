using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.OleDb;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using System.Text;

public partial class paginas_Migracion_Codigo_Programatico_Frm_Migracion_Codigo_Programatico : System.Web.UI.Page
{
    #region (PAGE LOAD)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Cls_Sessiones.Mostrar_Menu = true;
                Cls_Sessiones.Nombre_Empleado = "CARGA INICIAL";
            }
        }
    #endregion

    #region (METODOS GENERALES)
        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Leer_Excel
        ///**********************************************************************************************************************************
        public DataSet Leer_Excel(String sqlExcel, String Path)
        {
            //Para empezar definimos la conexión OleDb a nuestro fichero Excel.
            //String Rta = @MapPath("../../Archivos/PRESUPUESTO_IRAPUATO.xls");
            String Rta = @MapPath(Path);
            string sConnectionString = "";// @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Rta + ";Extended Properties=Excel 8.0;";

            if (Rta.Contains(".xlsx"))       // Formar la cadena de conexion si el archivo es Exceml xml
            {
                sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                        "Data Source=" + Rta + ";" +
                        "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
            }
            else if (Rta.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Excel binario
            {
                sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + Rta + ";" +
                        "Extended Properties=Excel 8.0;";
            }

            //Definimos el DataSet donde insertaremos los datos que leemos del excel
            DataSet DS = new DataSet();

            //Definimos la conexión OleDb al fichero Excel y la abrimos
            OleDbConnection oledbConn = new OleDbConnection(sConnectionString);
            oledbConn.Open();

            //Creamos un comand para ejecutar la sentencia SELECT.
            OleDbCommand oledbCmd = new OleDbCommand(sqlExcel, oledbConn);

            //Creamos un dataAdapter para leer los datos y asocialor al DataSet.
            OleDbDataAdapter da = new OleDbDataAdapter(oledbCmd);
            da.Fill(DS);
            return DS;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
        ///**********************************************************************************************************************************
        internal static String Consecutivo_ID(String Campo_Id, String Tabla, String Tamaño)
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Id; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '0')");
            Mi_SQL.Append(" FROM " + Tabla);

            Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Tamaño.Equals("5"))
            {
                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "00001";
                }
                else
                {
                    Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Id) + 1).Trim();
                }
            }
            else if (Tamaño.Equals("10"))
            {
                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "0000000001";
                }
                else
                {
                    Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Id) + 1).Trim();
                }
            }
            else
            {
                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "0";
                }
                else
                {
                    Consecutivo = (Convert.ToInt32(Id) + 1).ToString().Trim();
                }
            }

            return Consecutivo;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dt_Fuentes_Financiamiento
        ///**********************************************************************************************************************************
        private DataTable Obtener_Dt_Fuentes_Financiamiento()
        {
            DataSet Ds = new DataSet();
            DataTable Dt = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();


            Mi_SQL.Append("SELECT TRIM(" + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ") AS CLAVE, ");
            Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
            Mi_SQL.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);

            Ds = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Ds != null)
            {
                Dt = Ds.Tables[0];
            }

            return Dt;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Fuente_Financiamiento
        ///**********************************************************************************************************************************
        private String Obtener_Fuente_Financiamiento(String Fuente, DataTable Dt_Fuentes)
        {
            String Dato_Id = String.Empty;
            DataTable Dt_Registros = new DataTable();

            if (Dt_Fuentes != null)
            {
                if (Dt_Fuentes.Rows.Count > 0)
                {
                    Dt_Registros = (from Fila in Dt_Fuentes.AsEnumerable()
                                    where Fila.Field<String>(Cat_SAP_Fuente_Financiamiento.Campo_Clave) == Fuente
                                    select Fila).AsDataView().ToTable();

                    if (Dt_Registros != null)
                    {
                        if (Dt_Registros.Rows.Count > 0)
                        {
                            Dato_Id = Dt_Registros.Rows[0][Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString().Trim();
                        }
                    }
                }
            }

            return Dato_Id;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dt_Gpo_Dependencia
        ///**********************************************************************************************************************************
        private DataTable Obtener_Dt_Gpo_Dependencia()
        {
            DataSet Ds = new DataSet();
            DataTable Dt = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();


            Mi_SQL.Append("SELECT TRIM(" + Cat_Grupos_Dependencias.Campo_Clave + ") AS CLAVE, ");
            Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID);
            Mi_SQL.Append(" FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);

            Ds = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Ds != null)
            {
                Dt = Ds.Tables[0];
            }

            return Dt;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Gpo_Dependencia
        ///**********************************************************************************************************************************
        private String Obtener_Gpo_Dependencia(String Gpo_Dependencia, DataTable Dt_Gpo_Dependencia)
        {
            String Dato_Id = String.Empty;
            DataTable Dt_Registros = new DataTable();

            if (Dt_Gpo_Dependencia != null)
            {
                if (Dt_Gpo_Dependencia.Rows.Count > 0)
                {
                    Dt_Registros = (from Fila in Dt_Gpo_Dependencia.AsEnumerable()
                                    where Fila.Field<String>(Cat_Grupos_Dependencias.Campo_Clave) == Gpo_Dependencia.Trim()
                                    select Fila).AsDataView().ToTable();

                    if (Dt_Registros != null)
                    {
                        if (Dt_Registros.Rows.Count > 0)
                        {
                            Dato_Id = Dt_Registros.Rows[0][Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID].ToString().Trim();
                        }
                    }
                }
            }

            return Dato_Id;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dt_Dependencias
        ///**********************************************************************************************************************************
        private DataTable Obtener_Dt_Dependencias()
        {
            DataSet Ds = new DataSet();
            DataTable Dt = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();


            Mi_SQL.Append("SELECT TRIM(" + Cat_Dependencias.Campo_Clave + ") AS CLAVE, ");
            Mi_SQL.Append(Cat_Dependencias.Campo_Dependencia_ID);
            Mi_SQL.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);

            Ds = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Ds != null)
            {
                Dt = Ds.Tables[0];
            }

            return Dt;
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dependencia
        ///**********************************************************************************************************************************
        private String Obtener_Dependencia(String Dependencia, DataTable Dt_Dependencia)
        {
            String Dato_Id = String.Empty;
            DataTable Dt_Registros = new DataTable();

            if (Dt_Dependencia != null)
            {
                if (Dt_Dependencia.Rows.Count > 0)
                {
                    Dt_Registros = (from Fila in Dt_Dependencia.AsEnumerable()
                                    where Fila.Field<String>(Cat_Dependencias.Campo_Clave) == Dependencia.Trim()
                                    select Fila).AsDataView().ToTable();

                    if (Dt_Registros != null)
                    {
                        if (Dt_Registros.Rows.Count > 0)
                        {
                            Dato_Id = Dt_Registros.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString().Trim();
                        }
                    }
                }
            }

            return Dato_Id;
        }

    #endregion


    #region (EVENTOS GENERALES)
        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Cargar_Fuentes_Click
        ///**********************************************************************************************************************************
        protected void Btn_Cargar_Fuentes_Click(object sender, EventArgs e)
        {
            StringBuilder Mi_SQL = new StringBuilder(); ;
            String Mensaje = String.Empty;
            String FF_ID = String.Empty;
            DataTable Dt_Fuentes = new DataTable();

            String SqlExcel = "Select * From [Fte_Financiamiento$]";
            DataSet Ds = Leer_Excel(SqlExcel, "./Archivos/Layaut_Codigo_Programatico.xlsx");
            DataTable Dt = Ds.Tables[0];

            Dt_Fuentes = Obtener_Dt_Fuentes_Financiamiento();//obtenemos las fuentes de financiamiento de la base de datos

            foreach (DataRow Renglon in Dt.Rows)
            {
                if (!String.IsNullOrEmpty(Renglon["Clave"].ToString().Trim()))
                {
                    //obtener si existe o no la fuente
                    FF_ID = Obtener_Fuente_Financiamiento(Renglon["Clave"].ToString().Trim(), Dt_Fuentes);

                    if (String.IsNullOrEmpty(FF_ID))//creamos la fuente si no existe
                    {
                        FF_ID = Consecutivo_ID(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID, Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento, "5");

                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("INSERT INTO " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "(");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Clave + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Usuario_Creo + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Fecha_Creo + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Estatus + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Anio + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + ") VALUES( ");
                        Mi_SQL.Append("'" + FF_ID + "', ");
                        Mi_SQL.Append("'" + Renglon["Clave"].ToString().Trim() + "', ");
                        Mi_SQL.Append("'" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "', ");
                        Mi_SQL.Append("'" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                        Mi_SQL.Append("SYSDATE, ");
                        Mi_SQL.Append("'ACTIVO', ");
                        Mi_SQL.Append(Renglon["Anio"].ToString().Trim() + ", ");
                        Mi_SQL.Append("'" + Renglon["Ramo_33"].ToString().Trim() + "')");

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }
                    else
                    {
                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("UPDATE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                        Mi_SQL.Append(" SET " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " = '" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "', ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Fecha_Modifico + " = SYSDATE, ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + HttpUtility.HtmlDecode(Renglon["Clave"].ToString().Trim()) + "', ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Anio + " = " + HttpUtility.HtmlDecode(Renglon["Anio"].ToString().Trim()) + ", ");
                        Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = '" + HttpUtility.HtmlDecode(Renglon["Ramo_33"].ToString().Trim()) + "' ");
                        Mi_SQL.Append(" WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = '" + FF_ID.Trim() + "'");

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PSP_2012", "alert('ACTUALIZADO');", true);
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Cargar_Gpo_Dependencia_Click
        ///**********************************************************************************************************************************
        protected void Btn_Cargar_Gpo_Dependencia_Click(object sender, EventArgs e)
        {
            StringBuilder Mi_SQL = new StringBuilder(); ;
            String Mensaje = String.Empty;
            String Gpo_Dep_ID = String.Empty;
            DataTable Dt_Gpo_Dependencias = new DataTable();

            String SqlExcel = "Select * From [Grupo_Dependencia$]";
            DataSet Ds = Leer_Excel(SqlExcel, "./Archivos/Layaut_Codigo_Programatico.xlsx");
            DataTable Dt = Ds.Tables[0];

            Dt_Gpo_Dependencias = Obtener_Dt_Gpo_Dependencia();//obtenemos los grupo dependencias de la base de datos

            foreach (DataRow Renglon in Dt.Rows)
            {
                if (!String.IsNullOrEmpty(Renglon["Clave"].ToString().Trim()))
                {
                    //obtener si existe el gpo dependencia
                   Gpo_Dep_ID = Obtener_Gpo_Dependencia(Renglon["Clave"].ToString().Trim().Replace("-",""), Dt_Gpo_Dependencias);

                    if (String.IsNullOrEmpty(Gpo_Dep_ID))//creamos el gpo dependencia si no existe
                    {
                        Gpo_Dep_ID = Consecutivo_ID(Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID, Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias, "5");

                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("INSERT INTO " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "(");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Clave + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Nombre + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Comentarios + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Usuario_Creo + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Fecha_Creo + ", ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Estatus + ") VALUES( ");
                        Mi_SQL.Append("'" + Gpo_Dep_ID + "', ");
                        Mi_SQL.Append("'" + Renglon["Clave"].ToString().Trim() + "', ");
                        Mi_SQL.Append("'" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "', ");
                        Mi_SQL.Append("'" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "', ");
                        Mi_SQL.Append("'" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                        Mi_SQL.Append("SYSDATE, ");
                        Mi_SQL.Append("'ACTIVO')");

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }
                    else
                    {
                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("UPDATE " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);
                        Mi_SQL.Append(" SET " + Cat_Grupos_Dependencias.Campo_Nombre + " = '" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "', ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Fecha_Modifico + " = SYSDATE, ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Clave + " = '" + HttpUtility.HtmlDecode(Renglon["Clave"].ToString().Trim()) + "', ");
                        Mi_SQL.Append(Cat_Grupos_Dependencias.Campo_Comentarios + " = '" + HttpUtility.HtmlDecode(Renglon["Descripcion"].ToString().Trim()) + "' ");
                        Mi_SQL.Append(" WHERE " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + " = '" + Gpo_Dep_ID.Trim() + "'");

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PSP_2012", "alert('ACTUALIZADO');", true);
        }

        ///**********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Cargar_Dependencia_Click
        ///**********************************************************************************************************************************
        protected void Btn_Cargar_Dependencia_Click(object sender, EventArgs e)
        {
            StringBuilder Mi_SQL = new StringBuilder(); ;
            String Mensaje = String.Empty;
            String Gpo_Dep_ID = String.Empty;
            DataTable Dt_Gpo_Dependencias = new DataTable();
            String Dep_ID = String.Empty;
            DataTable Dt_Dependencias = new DataTable();

            String SqlExcel = "Select * From [Dependencias$]";
            DataSet Ds = Leer_Excel(SqlExcel, "./Archivos/Layaut_Codigo_Programatico.xlsx");
            DataTable Dt = Ds.Tables[0];

            Dt_Gpo_Dependencias = Obtener_Dt_Gpo_Dependencia();//obtenemos los grupo dependencias de la base de datos
            Dt_Dependencias = Obtener_Dt_Dependencias();//obtenemos las dependencias de la base de datos

            foreach (DataRow Renglon in Dt.Rows)
            {
                if (!String.IsNullOrEmpty(Renglon["Clave"].ToString().Trim()))
                {
                    //obtener si existe el gpo dependencia
                    Gpo_Dep_ID = Obtener_Gpo_Dependencia(Renglon["Gpo_Dependencia"].ToString().Trim(), Dt_Gpo_Dependencias);
                    //obtener si existe la dependencia
                    Dep_ID = Obtener_Dependencia(Renglon["Clave_Anterior"].ToString().Trim(), Dt_Dependencias);

                    if (!String.IsNullOrEmpty(Gpo_Dep_ID.Trim()))
                    {
                        if (String.IsNullOrEmpty(Dep_ID))//creamos la dependencia si no existe
                        {
                            Dep_ID = Consecutivo_ID(Cat_Dependencias.Campo_Dependencia_ID, Cat_Dependencias.Tabla_Cat_Dependencias, "5");

                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("INSERT INTO " + Cat_Dependencias.Tabla_Cat_Dependencias + "(");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Grupo_Dependencia_ID + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Dependencia_ID + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Clave + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Nombre + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Usuario_Creo + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Fecha_Creo + ", ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Estatus + ") VALUES( ");
                            Mi_SQL.Append("'" + Gpo_Dep_ID + "', ");
                            Mi_SQL.Append("'" + Dep_ID + "', ");
                            Mi_SQL.Append("'" + Renglon["Clave"].ToString().Trim() + "', ");
                            Mi_SQL.Append("'" + HttpUtility.HtmlDecode(Renglon["Nombre"].ToString().Trim()) + "', ");
                            Mi_SQL.Append("'" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                            Mi_SQL.Append("SYSDATE, ");
                            Mi_SQL.Append("'ACTIVO')");

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                        }
                        else
                        {
                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("UPDATE " + Cat_Dependencias.Tabla_Cat_Dependencias);
                            Mi_SQL.Append(" SET " + Cat_Dependencias.Campo_Nombre + " = '" + HttpUtility.HtmlDecode(Renglon["Nombre"].ToString().Trim()) + "', ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.Trim() + "', ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Fecha_Modifico + " = SYSDATE, ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Clave + " = '" + HttpUtility.HtmlDecode(Renglon["Clave"].ToString().Trim()) + "', ");
                            Mi_SQL.Append(Cat_Dependencias.Campo_Grupo_Dependencia_ID + " = '" + Gpo_Dep_ID.Trim() + "' ");
                            Mi_SQL.Append(" WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Dep_ID.Trim() + "'");

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PSP_2012", "alert('ACTUALIZADO');", true);
        }
    #endregion
}
