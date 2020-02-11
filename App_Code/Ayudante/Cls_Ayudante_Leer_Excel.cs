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
using System.Data.OleDb;
using System.Text;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Proveedores.Negocios;
using Presidencia.Cap_Masiva_Prov_Fijas.Negocio;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Nomina_Operacion_Proveedores.Negocio;

namespace Presidencia.Ayudante_Excel
{
    public class Cls_Ayudante_Leer_Excel
    {
        /// *****************************************************************************************
        /// Nombre: Leer_Tabla_Excel
        /// 
        /// Descripción: Método que lee el archivo de excel.
        /// 
        /// Parámetros: Path .- Ruta física del archivo.
        ///             Tabla.- Nombre de la tabla que se leera del archivo de excel.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 06/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public static DataTable Leer_Tabla_Excel(String Path, String Tabla)
        {
            OleDbConnection Conexion = new OleDbConnection();
            OleDbCommand Comando = new OleDbCommand();
            OleDbDataAdapter Adaptador = new OleDbDataAdapter();
            DataSet Ds_Informacion = new DataSet();
            String Query = String.Empty;

            try
            {
                Conexion.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Path + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";

                Conexion.Open();
                Comando.CommandText = "Select * From " + Tabla;
                Comando.Connection = Conexion;
                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(Ds_Informacion);
                Conexion.Close();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al leer un archivo de excel. Error: " + Ex.Message + "]");
            }
            return Ds_Informacion.Tables[0];
        }
        /// *****************************************************************************************
        /// Nombre: Crear_Estructura_Carga_Masiva
        /// 
        /// Descripción: Crea la estructura para generar el alta de la incidencia por concepto de carga 
        ///              masiva de perceociones o deducciones.
        /// 
        /// Parámetros: Dt_Empleados_In .- Listado de empleados.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 06/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public static DataTable Crear_Estructura_Carga_Masiva(DataTable Dt_Empleados_In)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacena la información del empleado.
            DataTable Dt_Empleados_Out = new DataTable();//Variable que almacena el listado de empleados.
            String No_Empleado = String.Empty;//Variable que almacena el número del empleado.
            String Empleado_ID = String.Empty;//Variable que almacena el identificador interno del sistema.
            String Nombre = String.Empty;//Variable que almacena el nombre del empleado.
            Double Cantidad = 0.0;//Variable que almacena la cantidad a percibir o descontar al empleado.
            String Referencia = String.Empty;//Variable que almacena la referencia del archivo.

            try
            {
                //Hacemos un recorrido del listado de empleado para detectar si hay empleados que estan en el archivo
                //pero que no se encuentran registrados en el sistema.
                Validar_Empleados_Activos_Nomina(Dt_Empleados_In);

                //Creamos la estructura para almacenar la infomración que utilizaremos al dar el alta de
                //una incidencia ya sea una percepción o deducción.
                Dt_Empleados_Out.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                Dt_Empleados_Out.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(String));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad, typeof(Double));

                if (Dt_Empleados_In is DataTable)
                {
                    if (Dt_Empleados_In.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados_In.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                {
                                    //Consultamos la información del empleado.
                                    No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                                    INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(String.Format("{0:000000}", Convert.ToInt32(No_Empleado)));

                                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                                    {
                                        Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
                                        Nombre = "[" + String.Format("{0:000000}", Convert.ToInt32(No_Empleado)) + "] -- " + INF_EMPLEADO.P_Apellido_Paterno + " " + INF_EMPLEADO.P_Apelldo_Materno + " " + INF_EMPLEADO.P_Nombre;

                                        if (!String.IsNullOrEmpty(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                                            Cantidad = Convert.ToDouble(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim());

                                        DataRow RENGLON = Dt_Empleados_Out.NewRow();
                                        RENGLON[Cat_Empleados.Campo_Empleado_ID] = Empleado_ID;
                                        RENGLON[Cat_Empleados.Campo_Nombre] = Nombre;
                                        RENGLON[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad] = Cantidad;
                                        Dt_Empleados_Out.Rows.Add(RENGLON);
                                    }
                                    else
                                    {
                                        StringBuilder Mensaje = new StringBuilder();
                                        Mensaje.Append("El Empleado con número [" + No_Empleado + "], no se encuentra registrado en el sistema. Por lo tanto el archivo no será cargado!!");
                                        throw new Exception(Mensaje.ToString());
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la estructura de la tabla para la carga masiva de informacion. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados_Out;
        }
        /// *****************************************************************************************
        /// Nombre: Crear_Estructura_Carga_Masiva
        /// 
        /// Descripción: Crea la estructura para generar el alta de la incidencia por concepto de carga 
        ///              masiva de deducciones por proveedor.
        /// 
        /// Parámetros: Dt_Empleados_In .- Listado de empleados.
        ///             Proveedor_ID.- Indentificador del proveedor, este campo es para control interno del sistema.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 06/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public static DataTable Crear_Estructura_Carga_Masiva_Proveedor(DataTable Dt_Empleados_In, String Proveedor_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            DataTable Dt_Empleados_Out = new DataTable();//Variable que almacenara un listado de empleados.
            String No_Empleado = String.Empty;//Variables que almacena el numero de empleado.
            String Empleado_ID = String.Empty;//Variable que almacena el identificador interno del empleado.
            String Nombre = String.Empty;//Variable que almacena el nombre del empleado.
            Double Cantidad = 0.0;//Variable que almacena la cantidad que se le retendra al empleado.
            Double Importe = 0.0;//Variable que almacena el importe del prestamo que realizo el proveedor.
            String Referencia = String.Empty;//Variable que almacenara la referencia.
            String Deduccion_ID = String.Empty;//Variable que almacena el identificador de la deduccion.
            String Nombre_Deduccion = String.Empty;//Variable que almacenara el nombre de la deducción.

            try
            {
                Dt_Empleados_Out.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                Dt_Empleados_Out.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(String));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad, typeof(Double));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe, typeof(Double));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo, typeof(Double));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida, typeof(Double));
                Dt_Empleados_Out.Columns.Add("DEDUCCION_ID", typeof(String));
                Dt_Empleados_Out.Columns.Add("NOMBRE_DEDUCCION", typeof(String));


                if (Dt_Empleados_In is DataTable)
                {
                    if (Dt_Empleados_In.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados_In.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                {
                                    No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                                    INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(String.Format("{0:000000}", Convert.ToInt32(No_Empleado)));

                                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                                    {
                                        Deduccion_ID = Consultar_Deduccion_Consecutiva_Corresponde_Empleado(INF_EMPLEADO.P_Empleado_ID, Proveedor_ID, Dt_Empleados_Out);

                                        if (!String.IsNullOrEmpty(Deduccion_ID))
                                            Nombre_Deduccion = Consultar_Nombre_Deduccion(Deduccion_ID);
                                        else Nombre_Deduccion = String.Empty;

                                        Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
                                        Nombre = "[" + String.Format("{0:000000}", Convert.ToInt32(No_Empleado)) + "] -- " + INF_EMPLEADO.P_Apellido_Paterno + " " + INF_EMPLEADO.P_Apelldo_Materno + " " + INF_EMPLEADO.P_Nombre;

                                        if (!String.IsNullOrEmpty(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                                            Cantidad = Convert.ToDouble(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim());

                                        if (!String.IsNullOrEmpty(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString()))
                                            Importe = Convert.ToDouble(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim());

                                        DataRow RENGLON = Dt_Empleados_Out.NewRow();
                                        RENGLON[Cat_Empleados.Campo_Empleado_ID] = Empleado_ID;
                                        RENGLON[Cat_Empleados.Campo_Nombre] = Nombre;
                                        RENGLON[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad] = Cantidad;
                                        RENGLON[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe] = Importe;
                                        RENGLON[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo] = Importe;
                                        RENGLON[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida] = 0;
                                        RENGLON["DEDUCCION_ID"] = Deduccion_ID;
                                        RENGLON["NOMBRE_DEDUCCION"] = Nombre_Deduccion;
                                        Dt_Empleados_Out.Rows.Add(RENGLON);
                                    }
                                    else
                                    {
                                        throw new Exception("Hay Empleados en el archivo seleccionado de los cuales su número de empleado no existe en el sistema, por lo tanto el archivo no será cargado!!");
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la estructura de la tabla para la carga masiva de informacion. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados_Out;
        }
        /// *****************************************************************************************
        /// Nombre: Obtener_Referencia
        /// 
        /// Descripción: Método que lee la referencia del archivo.
        /// 
        /// Parámetros: Dt_Referencia .- Tabla que almacena la referencia del archivo.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 06/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public static String Obtener_Referencia(DataTable Dt_Referencia)
        {
            String Referencia = String.Empty;//Variable que almacenara la referencia del descuento que se le comenzara aplicar al empleado.

            try
            {
                if (Dt_Referencia is DataTable)
                {
                    if (Dt_Referencia.Rows.Count > 0)
                    {
                        foreach (DataRow REFERENCIA in Dt_Referencia.Rows)
                        {
                            if (REFERENCIA is DataRow)
                            {
                                if (!String.IsNullOrEmpty(REFERENCIA[Ope_Nom_Deducciones_Var.Campo_Referencia].ToString()))
                                {
                                    Referencia = REFERENCIA[Ope_Nom_Deducciones_Var.Campo_Referencia].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la referencia. Error: [" + Ex.Message + "]");
            }
            return Referencia;
        }
        /// *****************************************************************************************
        /// Nombre: Consultar_Deduccion_Consecutiva_Corresponde_Empleado
        /// 
        /// Descripción: Consulta el consecitivo de la deduccion que le corresponde al empleado
        ///              de acuerdo al proveedor seleccionado.
        /// 
        /// Parámetros: Empleado_ID .- Identificador del empleado a consultar.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 13/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        private static String Consultar_Deduccion_Consecutiva_Corresponde_Empleado(String Empleado_ID, String Proveedor_ID, DataTable Dt_Claves_Ocupadas)
        {
            Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio Obj_Cap_Masiva_Prov_Fijas = new Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio();
            DataTable Dt_Perc_Dedu_Empl = null;
            String Deduccion_Consecutiva = String.Empty;

            try
            {
                Obj_Cap_Masiva_Prov_Fijas.P_Proveedor_ID = Proveedor_ID;
                Obj_Cap_Masiva_Prov_Fijas.P_Empleado_ID = Empleado_ID;
                Dt_Perc_Dedu_Empl = Obj_Cap_Masiva_Prov_Fijas.Consultar_Claves_Disponibles();

                var claves_ocupadas = from claves in Dt_Claves_Ocupadas.AsEnumerable()
                                      where claves.Field<String>(Cat_Empleados.Campo_Empleado_ID) == Empleado_ID
                                      select new { clave = claves.Field<String>("DEDUCCION_ID") };

                System.Collections.Generic.List<DataRow> Renglones_Eliminar = new System.Collections.Generic.List<DataRow>();

                foreach (var item_clave in claves_ocupadas)
                {
                    if (Dt_Perc_Dedu_Empl is DataTable)
                    {
                        foreach (DataRow FILA in Dt_Perc_Dedu_Empl.Rows)
                        {
                            if (FILA is DataRow)
                            {
                                if (!String.IsNullOrEmpty(FILA[Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    if (FILA[Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString().Trim().Equals((item_clave.clave.ToString())))
                                    {
                                        Renglones_Eliminar.Add(FILA);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DataRow Renglon_Eliminar in Renglones_Eliminar)
                    Dt_Perc_Dedu_Empl.Rows.Remove(Renglon_Eliminar);

                if (Dt_Perc_Dedu_Empl is DataTable)
                {
                    if (Dt_Perc_Dedu_Empl.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Perc_Dedu_Empl.Rows)
                        {
                            if (Dr is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Dr[Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Deduccion_Consecutiva = (Dr[Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                    break; 
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la deducción consecutiva que le corresponde al empleado" +
                    " según el proveedor seleccionado. Error: [" + Ex.Message + "]");
            }
            return Deduccion_Consecutiva;
        }
        /// *****************************************************************************************
        /// Nombre: Consultar_Nombre_Deduccion
        /// 
        /// Descripción: Consulta el nombre y la clave de la deducción por el identificador de la
        ///              la misma.
        /// 
        /// Parámetros: Deduccion_ID .- Identificador de la deducción a consultar.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 13/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        private static String Consultar_Nombre_Deduccion(String Deduccion_ID)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepcion_Deduccion =
                new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.
            DataTable Dt_Percepcion_Deduccion = null;//Variable que gusrada un listado de las percepciones y deducciones que tiene asiganadas el empleado.
            String Nombre_Deduccion = String.Empty;//Variable que almacenara el identificador del concepto.

            try
            {
                Obj_Percepcion_Deduccion.P_PERCEPCION_DEDUCCION_ID = Deduccion_ID;
                Dt_Percepcion_Deduccion = Obj_Percepcion_Deduccion.Consultar_Percepciones_Deducciones_General();

                if (Dt_Percepcion_Deduccion is DataTable)
                {
                    if (Dt_Percepcion_Deduccion.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCION in Dt_Percepcion_Deduccion.Rows)
                        {
                            if (DEDUCCION is DataRow)
                            {
                                Nombre_Deduccion = "[" + DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + "] -- " +
                                    DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el nombre de la deducción. Error: [" + Ex.Message + "]");
            }
            return Nombre_Deduccion;
        }
        /// *****************************************************************************************
        /// Nombre: Validar_Empleados_Activos_Nomina
        /// 
        /// Descripción: Valida que los empleados que se pasaron en el archivo existan en el catálogo de 
        ///              empleados.
        /// 
        /// Parámetros: Dt_Empleados_In .- Listado de empleados.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 06/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        private static void Validar_Empleados_Activos_Nomina(DataTable Dt_Empleados_In)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            String No_Empleado = String.Empty;//Variable que almacenara el número del empleado.
            StringBuilder Mensaje = new StringBuilder();//Variable que almacena el mensaje de error a desplegar.
            Boolean Estatus = false;//Variable que almacena el estatus si todos los empleados exsiten.

            try
            {
                Mensaje.Append("<br />");

                if (Dt_Empleados_In is DataTable)
                {
                    if (Dt_Empleados_In.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados_In.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                {
                                    //Consultamos al empleado.
                                    No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                                    INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(String.Format("{0:000000}", Convert.ToInt32(No_Empleado)));

                                    if (String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                                    {
                                        Mensaje.Append("Empleado:[" + No_Empleado + "]<br />");//Anexamos empleados que no se encuentran dados de alta en el sistema.
                                        Estatus = true;//Indicamos que el empleado no se encuentra registrado.
                                    }
                                }
                            }
                        }
                    }
                }

                if (Estatus)
                    throw new Exception(Mensaje.ToString());//arrojamos una excepción indicando los empleados que no se encontraron en el sistema.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la estructura de la tabla para la carga masiva de informacion. Error: [" + Ex.Message + "]");
            }
        }
        /// *****************************************************************************************
        /// Nombre: Crear_Estructura_Carga_Masiva_Proveedor_Fonacot
        /// 
        /// Descripción: Crea la estructura para generar el alta de la incidencia por concepto de carga 
        ///              masiva de deducciones por FONACOT.
        /// 
        /// Parámetros: Dt_Empleados_In .- Listado de empleados.
        ///             Proveedor_ID.- Indentificador del proveedor, este campo es para control interno del sistema.
        ///             Nomina_ID.- Nomina a partir de cuando se comenzaran a realizar los descuentos.
        ///             No_Nomina.- Periodo a partir de cuando se comenzaran a realizar los descuentos.
        ///             No_Catorcenas_Mes.- Número de catorcenas que tiene el mes.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 23/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public static DataTable Crear_Estructura_Carga_Masiva_Proveedor_Fonacot(DataTable Dt_Empleados_In, String Proveedor_ID, String Nomina_ID, String No_Nomina, Int32 No_Catorcenas_Mes)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Empleados_Out = new DataTable();//Variable que almacenara un listado de empleados.
            DataTable Dt_Calendario_Nomina = null;//Variable que almacenara el calendario de nomina consultado.
            String No_Empleado = String.Empty;//Variables que almacena el numero de empleado.
            String Empleado_ID = String.Empty;//Variable que almacena el identificador interno del empleado.
            String Nombre = String.Empty;//Variable que almacena el nombre del empleado.
            String RFC = String.Empty;//Variable que almacenara el RFC del empleado.
            String Deduccion_ID = String.Empty;//Variable que almacena el identificador de la deduccion.
            String Nombre_Deduccion = String.Empty;//Variable que almacenara el nombre de la deducción.
            String No_Fonacot = String.Empty;//Variable que almacenara el número de fonacot.
            String No_Credito = String.Empty;//Variable que almacenara el número de crédito.
            String Cantidad = String.Empty;//Variable que almacena la cantidad que se le retendra al empleado.
            String Cuotas_Pagadas = String.Empty;//Variable que almacena las cuotas pagadas que lleva el empleado.
            String Plazo_Mensual = String.Empty;//Variable que almacena el plazo mensual.
            String Plazo_Catorcenal = String.Empty;//Variable que almacena el plazo catorcenal.
            String Retencion_Mensual = String.Empty;//Variable que almacena la retención mensual que se le hará el empleado.
            String Retencion_Catorcenal = String.Empty;//Variable que almacena la retención catorcenal que se le hará el empleado.
            String Retencion_Real = String.Empty;//Variable que almacena la retención real que se le hará el empleado.
            String Estatus = String.Empty;//Variable que almacena el estatus que tiene el pago de fonacot.
            String Anio_Nomina = String.Empty;//Variable que almacena el año de la nomina.
            String Aux_Nomina_ID = String.Empty;//Variable que almacenara la nomina_id inicial.
            String Aux_No_Nomina = String.Empty;//Variable que almacenara la no_nomina inicial.

            try
            {
                //Guardamos la Nomina_ID y el No_Nomina.
                Aux_Nomina_ID = Nomina_ID;
                Aux_No_Nomina = No_Nomina;

                //Consultamos los periodos de la nomina actual para determinar a partir de cuando se comenzaran a realizar los descuentos.
                Obj_Calendario_Nomina.P_Nomina_ID = Nomina_ID;
                Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nominas();

                //Obtenemos el año del calendario de la nomina actual.
                if (Dt_Calendario_Nomina is DataTable)
                {
                    if (Dt_Calendario_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow CALENDARIO in Dt_Calendario_Nomina.Rows)
                        {
                            if (CALENDARIO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(CALENDARIO[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                                    Anio_Nomina = CALENDARIO[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString().Trim();
                            }
                        }
                    }
                }

                //Creamos la estructura de la tabla que almacenara la información del archivo de excel.
                Dt_Empleados_Out.Columns.Add("ELIMINAR", typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Nombre, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_RFC, typeof(String));
                Dt_Empleados_Out.Columns.Add("NOMBRE_DEDUCCION", typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_No_Credito, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Cantidad, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Plazo, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Periodo, typeof(String));
                Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Estatus, typeof(String));
                Dt_Empleados_Out.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Anio, typeof(String));

                if (Dt_Empleados_In is DataTable)
                {
                    if (Dt_Empleados_In.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados_In.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                //Regresamos la nomina_id y el no_nomina inicial.
                                Nomina_ID = Aux_Nomina_ID;
                                No_Nomina = Aux_No_Nomina;

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                {
                                    //Consultamos la información del empleado.
                                    No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                                    INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(String.Format("{0:000000}", Convert.ToInt32(No_Empleado)));

                                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                                    {
                                        Deduccion_ID = Obtener_Deduccion(Proveedor_ID, INF_EMPLEADO.P_Empleado_ID, Dt_Empleados_Out, Nomina_ID, No_Nomina);

                                        //Consultamos el nombre y la clave de la deduccion consecutiva.
                                        if (!String.IsNullOrEmpty(Deduccion_ID)) Nombre_Deduccion = Consultar_Nombre_Deduccion(Deduccion_ID);
                                        else Nombre_Deduccion = String.Empty;

                                        //Obtenemos los datos del empleado. [Nombre y Empleado_ID].
                                        Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
                                        Nombre = "[" + String.Format("{0:000000}", Convert.ToInt32(No_Empleado)) + "] -- " + INF_EMPLEADO.P_Apellido_Paterno + " " + INF_EMPLEADO.P_Apelldo_Materno + " " + INF_EMPLEADO.P_Nombre;

                                        //Obtenemos la información del archivo de excel seleccionado.
                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_RFC].ToString()))
                                            RFC = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_RFC].ToString().Trim();

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot].ToString()))
                                            No_Fonacot = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot].ToString().Trim();

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_No_Credito].ToString()))
                                            No_Credito = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_No_Credito].ToString().Trim();

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas].ToString()))
                                            Cuotas_Pagadas = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas].ToString().Trim();

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Plazo].ToString()))
                                        {
                                            Plazo_Mensual = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Plazo].ToString().Trim();
                                        }

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual].ToString()))
                                        {
                                            Retencion_Mensual = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual].ToString().Trim();
                                            Retencion_Catorcenal = (Convert.ToDouble(Retencion_Mensual) / No_Catorcenas_Mes).ToString();
                                        }

                                        if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real].ToString()))
                                            Retencion_Real = EMPLEADO[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real].ToString().Trim();

                                        Int32 Contador_Periodos = 1;//Creamos y establecemos el valor inicial del contador de periodos.

                                        while (Contador_Periodos <= No_Catorcenas_Mes)
                                        {
                                            //Cargamos la tabla que almacenara la información del archivo de excel.
                                            DataRow RENGLON = Dt_Empleados_Out.NewRow();

                                            RENGLON["ELIMINAR"] = Empleado_ID + No_Nomina + Deduccion_ID;

                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID] = Empleado_ID;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Nombre] = Nombre;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_RFC] = RFC;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID] = Deduccion_ID;
                                            RENGLON["NOMBRE_DEDUCCION"] = Nombre_Deduccion;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot] = No_Fonacot;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_No_Credito] = No_Credito;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Cantidad] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Retencion_Catorcenal)) ? Retencion_Catorcenal : "0"));
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas] = Cuotas_Pagadas;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Plazo] = Plazo_Mensual;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Retencion_Mensual)) ? Retencion_Mensual : "0")); ;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Retencion_Real)) ? Retencion_Real : "0")); ; ;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID] = Nomina_ID;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Periodo] = No_Nomina;
                                            RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Estatus] = "ACEPTADO";
                                            RENGLON[Cat_Nom_Calendario_Nominas.Campo_Anio] = Anio_Nomina;

                                            Dt_Empleados_Out.Rows.Add(RENGLON);

                                            String Resultado = Obtener_Nomina_Periodo_Consecutivo(Nomina_ID, No_Nomina);
                                            String[] Nomina_Periodo = null;

                                            if (!String.IsNullOrEmpty(Resultado))
                                            {
                                                Nomina_Periodo = Resultado.Split(new Char[] { ',' });//Obtenemos la Nomina_ID y el No_Nomina consecutivos.

                                                if (Nomina_Periodo.Length > 0)
                                                {
                                                    Nomina_ID = Nomina_Periodo[0];
                                                    No_Nomina = Nomina_Periodo[1];
                                                }
                                            }
                                            ++Contador_Periodos;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Hay Empleados en el archivo seleccionado de los cuales su número de empleado no existe en el sistema, por lo tanto el archivo no será cargado!!");
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la estructura de la tabla para la carga masiva de informacion. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados_Out;
        }
        /// ***********************************************************************************************************************************************
        /// Nombre: OBTENER_NOMINA_PERIODO_CONSECUTIVO
        /// 
        /// Descripción: Método que obtiene la nómina y el periodo consecutivo.
        /// 
        /// Parámetros: Nomina_ID.- Nomina vigente.
        ///             No_Nomina.- Periodo nominal.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************************************************************************
        private static String Obtener_Nomina_Periodo_Consecutivo(String Nomina_ID, String No_Nomina)
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
            DataTable Dt_Calendario_Nomina = null;
            String Resultado = String.Empty;

            try
            {
                Obj_Calendario_Nomina.P_Nomina_ID = Nomina_ID;
                Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consulta_Detalles_Nomina();

                DataRow[] Dr_Periodos = Dt_Calendario_Nomina.Select("NO_NOMINA=" + (Convert.ToInt32(No_Nomina) + 1));

                if (Dr_Periodos.Length > 0)
                {
                    foreach (DataRow Fila in Dr_Periodos)
                    {
                        if (!String.IsNullOrEmpty(Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()))
                        {
                            Resultado = Fila[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID].ToString();
                            Resultado += ",";
                            Resultado += Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la nomina y el periodo en el que aplicaran los pagos de fonacot. Error: [" + Ex.Message + "]");
            }
            return Resultado;
        }
        /// ***********************************************************************************************************************************************
        /// Nombre: OBTENER_DEDUCCION
        /// 
        /// Descripción: Método que obtiene la deducción consecutiva que tiene disponible el empleado por el proveedor FONACOT.
        /// 
        /// Parámetros: Proveedor_ID.- FONACOT.
        ///             Empleado_ID.- Identificador del empleado que es utilizado para el control interno del sistema.
        ///             Deducciones_Asignadas.- Tabla que almacena las deducciones que ya fueron ocupadas en la actual carga masiva. Por lo tanto
        ///             tambien se descartara de la deducciones disponibles.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************************************************************************
        private static String Obtener_Deduccion(String Proveedor_ID, String Empleado_ID, DataTable Deducciones_Asignadas,
            String Nomina_ID, String No_Nomina)
        {
            Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexion con la capa de negocios.
            Cls_Ope_Nom_Proveedores_Negocio Obj_Ope_Proveedores = new Cls_Ope_Nom_Proveedores_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Proveedores = null;//Variable que almacenara la lista de proveedores.
            DataTable Dt_Deducciones_Ocupadas = null;//Variable que almacenara las deducciones que actualmente ya no se encuentran disponibles.
            String Deduccion_Consecutiva = String.Empty;//Variable que almacenara la deducción consecutiva.

            try
            {
                //Consultamos las deducciones que tiene asignadas el proveedor.
                Obj_Proveedores.P_Proveedor_ID = Proveedor_ID;
                Dt_Proveedores = Obj_Proveedores.Consultar_Deducciones_Proveedor();

                //Consultamos las deducciones que actualmente se encuentran ocupadas.
                Obj_Ope_Proveedores.P_Empleado_ID = Empleado_ID;
                Obj_Ope_Proveedores.P_Nomina_ID = Nomina_ID;
                Obj_Ope_Proveedores.P_No_Nomina = Convert.ToInt32((!String.IsNullOrEmpty(No_Nomina)) ? No_Nomina : "0");
                Dt_Deducciones_Ocupadas = Obj_Ope_Proveedores.Consultar_Deduccion();

                //Codigo que descarta las deducciones que actulmente se ecuentran ocupadas.
                if (Dt_Deducciones_Ocupadas is DataTable)
                {
                    if (Dt_Deducciones_Ocupadas.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCIONES in Dt_Deducciones_Ocupadas.Rows)
                        {
                            if (DEDUCCIONES is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEDUCCIONES[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString())) {
                                    DataRow[] Dr = Dt_Proveedores.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=" +
                                        DEDUCCIONES[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString());

                                    if (Dr.Length > 0)
                                        Dt_Proveedores.Rows.Remove(Dr[0]);
                                }
                            }
                        }
                    }
                }

                //Codigo que descarta las deducciones que actulmente se ecuentran comprometidas.
                if (Deducciones_Asignadas is DataTable) {
                    if (Deducciones_Asignadas.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCION in Deducciones_Asignadas.Rows)
                        {
                            if (DEDUCCION is DataRow) {
                                if (!String.IsNullOrEmpty(DEDUCCION[Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID].ToString()))
                                {
                                    if (DEDUCCION[Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID].ToString().Trim().Equals(Empleado_ID))
                                    {
                                        if (!String.IsNullOrEmpty(DEDUCCION[Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                        {

                                            DataRow[] Dr = Dt_Proveedores.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=" +
                                                DEDUCCION[Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString().Trim());

                                            if (Dr.Length > 0)
                                                Dt_Proveedores.Rows.Remove(Dr[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Obtenemos la deduccion consecutiva.
                DataRow []Dr_Min_Value = Dt_Proveedores.Select(Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + " = MIN(" + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ")");
                if (Dr_Min_Value.Length > 0)
                    Deduccion_Consecutiva = Dr_Min_Value[0][Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la deduccion consecutiva. Error: [" + Ex.Message + "'");
            }
            return Deduccion_Consecutiva;
        }

        
    }
}