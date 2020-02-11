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
using Presidencia.Nomina_Operacion_Proveedores.Datos;
using Presidencia.Proveedores.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;

/// <summary>
/// Summary description for Cls_Ope_Nom_Proveedores
/// </summary>
namespace Presidencia.Nomina_Operacion_Proveedores.Negocio
{
    public class Cls_Ope_Nom_Proveedores_Negocio
    {

        #region Variables Internas

        private Int32 No_Movimiento = 0;
        private String Proveedor_ID = null;
        private String Nomina_ID = null;
        private Int32 No_Nomina = 0;
        private Int32 No_Periodos = 0;
        private DataTable Dt_Datos_Archivo = null;
        private String Usuario = null;

        private Int32 No_Movimiento_Detalle = 0;
        private String Estatus = null;

        private String Empleado_ID = null;
        private String Fecha_Busqueda;
        private String Fecha_Autorizacion;

        #endregion

        #region Variables Publicas

        public Int32 P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public Int32 P_No_Periodos
        {
            get { return No_Periodos; }
            set { No_Periodos = value; }
        }
        public DataTable P_Dt_Datos_Archivo
        {
            get { return Dt_Datos_Archivo; }
            set { Dt_Datos_Archivo = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public Int32 P_No_Movimiento_Detalle
        {
            get { return No_Movimiento_Detalle; }
            set { No_Movimiento_Detalle = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Fecha_Busqueda
        {
            get { return Fecha_Busqueda; }
            set { Fecha_Busqueda = value; }
        }

        public String P_Fecha_Autorizacion {
            get { return Fecha_Autorizacion; }
            set { Fecha_Autorizacion = value; }
        }
        #endregion

        #region Metodos [Conexión con la Base de Datos]

        public void Subir_Informacion()
        {
            //Crear_DataTable_Detallado();
            Cls_Ope_Nom_Proveedores_Datos.Subir_Informacion_Archivo(this);
        }

        public void Modificar_Estatus_Detalle_Proveedores()
        {
            Cls_Ope_Nom_Proveedores_Datos.Modificar_Estatus_Detalle_Proveedores(this);
        }

        public DataTable Consultar_Detalles_Registro_Proveedores()
        {
            return Cls_Ope_Nom_Proveedores_Datos.Consultar_Detalles_Registro_Proveedores(this);
        }

        public DataTable Consultar_Rpt_Detalles_Proveedores()
        {
            return Cls_Ope_Nom_Proveedores_Datos.Consultar_Rpt_Detalles_Proveedores(this);
        }

        public DataTable Consultar_Deduccion() {
            return Cls_Ope_Nom_Proveedores_Datos.Consultar_Deduccion(this);
        }

        public DataTable Identificar_Periodo_Nomina() {
            return Cls_Ope_Nom_Proveedores_Datos.Identificar_Periodo_Nomina(this);
        }
        #endregion

        #region Metodos [Operativos]

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_DataTable_Detallado
        ///DESCRIPCIÓN: Crea El Datatable Detallado que será dado de alta a partir del que
        ///             se leyo del Archivo y de los datos que se calculan.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Abril/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private void Crear_DataTable_Detallado()
        {
            if (Dt_Datos_Archivo != null && Dt_Datos_Archivo.Rows.Count > 0)
            {
                Cls_Cat_Nom_Proveedores_Negocio Proveedores_Negocio = new Cls_Cat_Nom_Proveedores_Negocio();
                Proveedores_Negocio.P_Proveedor_ID = Proveedor_ID;
                DataTable Dt_Deducciones = Proveedores_Negocio.Consultar_Deducciones_Proveedor();
                if (Dt_Deducciones != null && Dt_Deducciones.Rows.Count > 0)
                {
                    DataTable Dt_Datos_Actualizados = new DataTable();
                    DataTable Dt_Contador_Deducciones = new DataTable();

                    //Se cargan las columnas en el DataTable
                    Dt_Datos_Actualizados.Columns.Add("NO_FONACOT", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("RFC", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("NO_CREDITO", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("RETENCION_MENSUAL", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("PLAZO", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("CUOTAS_PAGADAS", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("RETENCION_REAL", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("PERCEPCION_DEDUCCION_ID", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("NOMINA_ID", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("PERIODO", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("CANTIDAD", Type.GetType("System.String"));
                    Dt_Datos_Actualizados.Columns.Add("ESTATUS", Type.GetType("System.String"));

                    Dt_Contador_Deducciones.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Dt_Contador_Deducciones.Columns.Add("CONTADOR", Type.GetType("System.Int32"));

                    for (Int32 Cnt_Registros_Archivo = 0; Cnt_Registros_Archivo < Dt_Datos_Archivo.Rows.Count; Cnt_Registros_Archivo++)
                    {
                        String No_Empleado = Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["CLAVE_EMPLEADO"].ToString();
                        Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                        Empleado_Negocio.P_No_Empleado = No_Empleado;
                        DataTable Dt_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                        if (Dt_Empleado != null && Dt_Empleado.Rows.Count > 0)
                        {
                            Int32 Numero_Fila_Deduccion = Agregar_Contador_Deduccion_Empleado(Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString(), Dt_Contador_Deducciones, 0);
                            if (Numero_Fila_Deduccion < Dt_Deducciones.Rows.Count)
                            {
                                String Deduccion_ID = Dt_Deducciones.Rows[Numero_Fila_Deduccion][Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString();
                                for (Int32 Cnt_ = 0; Cnt_ < No_Periodos; Cnt_++)
                                {
                                    DataRow Fila = Dt_Datos_Actualizados.NewRow();
                                    Fila["NO_FONACOT"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["No_FONACOT"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["No_FONACOT"].ToString() : "";
                                    Fila["RFC"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RFC"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RFC"].ToString() : "";
                                    Fila["NOMBRE"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["NOMBRE"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["NOMBRE"].ToString() : "";
                                    Fila["NO_CREDITO"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["No_CREDITO"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["No_CREDITO"].ToString() : "";
                                    Fila["RETENCION_MENSUAL"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_MENSUAL"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_MENSUAL"].ToString() : "";
                                    Fila["EMPLEADO_ID"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();
                                    Fila["PLAZO"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["PLAZO"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["PLAZO"].ToString() : "";
                                    Fila["CUOTAS_PAGADAS"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["CUOTAS_PAGADAS"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["CUOTAS_PAGADAS"].ToString() : "";
                                    Fila["RETENCION_REAL"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_REAL"] != null) ? Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_REAL"].ToString() : "";
                                    Fila["PERCEPCION_DEDUCCION_ID"] = Deduccion_ID;
                                    Fila["NOMINA_ID"] = Nomina_ID;
                                    Fila["PERIODO"] = (No_Nomina + Cnt_).ToString();
                                    Fila["CANTIDAD"] = (Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_MENSUAL"] != null) ? ((Convert.ToDouble(Dt_Datos_Archivo.Rows[Cnt_Registros_Archivo]["RETENCION_MENSUAL"].ToString())) / No_Periodos).ToString() : "0.0";
                                    Fila["ESTATUS"] = "ACEPTADO";
                                    Dt_Datos_Actualizados.Rows.Add(Fila);
                                }
                            }
                            else
                            {
                                throw new Exception("Las deducciones que tiene asignadas este proveedor no son suficientes para cubrir las del Archivo. El empleado con el número '" + No_Empleado + "' en la Fila '" + (Cnt_Registros_Archivo + 1).ToString() + "'");
                            }
                        }
                        else
                        {
                            throw new Exception("El empleado con el número '" + No_Empleado + "' en la Fila '" + (Cnt_Registros_Archivo + 1).ToString() + "' no fue encontrado.");
                        }
                    }
                    Dt_Datos_Archivo = Dt_Datos_Actualizados;
                }
                else
                {
                    throw new Exception("Este proveedor no tiene deducciones asignadas.");
                }
            }
            else
            {
                throw new Exception("El Archivo no tiene registros.");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Agregar_Contador_Deduccion_Empleado
        ///DESCRIPCIÓN: Agrega una deduccion al empleado consultado para saber en que deduc
        ///             ción del proveedor se asignará.
        ///PARAMETROS:  
        ///             1.  Clave.  Clave que se buscara en el DataTable
        ///             2.  Tabla.  Datatable donde se va a buscar la clave.
        ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Abril/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Int32 Agregar_Contador_Deduccion_Empleado(String Clave, DataTable Tabla, Int32 Columna)
        {
            Int32 Contador_Deducciones = 0;
            if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0)
            {
                if (Tabla.Columns.Count > Columna)
                {
                    for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++)
                    {
                        if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim()))
                        {
                            Contador_Deducciones = Convert.ToInt32(Tabla.Rows[Contador]["CONTADOR"].ToString()) + 1;
                            break;
                        }
                    }
                }
            }
            if (Contador_Deducciones == 0)
            {
                DataRow Fila = Tabla.NewRow();
                Fila["EMPLEADO_ID"] = Clave;
                Fila["CONTADOR"] = 0;
                Tabla.Rows.Add(Fila);
            }
            return Contador_Deducciones;
        }

        #endregion

    }
}
