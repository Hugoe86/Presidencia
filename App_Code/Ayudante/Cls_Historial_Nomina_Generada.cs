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
using System.IO;
using System.Text;
using Presidencia.Constantes;

namespace Presidencia.Archivos_Historial_Nomina_Generada
{
    public class Cls_Historial_Nomina_Generada
    {
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Escribir_Archivo_Historial_Nomina_Generada
        ///DESCRIPCIÓN: Hace el registro de los registros que se afectaron al generar la nómina. Esto con el finalidad de tener un log o historial de estas 
        ///             modificaciones y poder hacer un rollback de la información modificada, en caso de regenerar la nómina.
        ///
        ///PARÁMETROS: Ruta.- Sitio donde se ubicara el archivo dentro de la estructura de archivos del Proyecto de Presidencia.
        ///                   Nombre_Archivo.- Nombre que se le definira al archivo que almacenra la informacion historica de la nómina generada.
        ///                   Extencion.- Extencion del archivo.
        ///                   Historial_Nomina_Generada.- Variable que almacenara todos los registros de los movimientos que hubo en la nómina generada
        ///                                               por concepto de Prestamos y Ajustes de ISR. Esto para poder hacer un ROLLBACK de los registros
        ///                                               afectados en caso de realizar una regeneración de la nómina.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static void Escribir_Archivo_Historial_Nomina_Generada(String Ruta, String Nombre_Archivo, String Extencion, StringBuilder Historial_Nomina_Generada)
        {
            StreamWriter Escribir_Archivo = null;//Escritor, variable encargada de escribir el archivo que almacenará el historial de la nómina generada.

            try
            {
                Escribir_Archivo = new StreamWriter(@"" + (Ruta + Nombre_Archivo + Extencion), true, Encoding.UTF8);
                Escribir_Archivo.WriteLine(Historial_Nomina_Generada.ToString());
                Escribir_Archivo.Close();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al escribir el Archivo " + Nombre_Archivo + ". Error: [" + Ex.Message + "]");
            }
        }
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Registro_Insertar_Prestamo
        ///
        ///DESCRIPCIÓN: Hace un barrido de la información que estaba registrada en prestamos antes de generar la nómina al empleado con el prestamos activo,
        ///             y crea una estructura que servira para generar un LOG o Historial para una posible regeneración de la nómina y poder conocer con exactitud 
        ///             que registros en Prestamos fueron afectados.
        ///             
        ///PARÁMETROS:  Dt_Registro .- Tabla que almacena la cantidad de prestamos que el empleado tiene activos actualmente.
        ///             Registro.- Registro que se creara para almacenarlo en el LOG o Archivo Historico de la Generación de la Nómina.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static StringBuilder Crear_Registro_Insertar_Prestamo(DataTable Dt_Registro, ref StringBuilder Registro)
        {
            try
            {
                foreach (DataRow Renglon in Dt_Registro.Rows)
                {
                    Registro.Append("[");

                    Registro.Append("PRESTAMO, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString());
                    else
                        Registro.Append("NULL");

                    Registro.Append("]");
                    Registro.Append("\r\n");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el registro de prestamo que se actualizara, según el monto a descontar del mismo. Error: [" + Ex.Message + "]");
            }
            return Registro;
        }
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Registro_Insertar_Ajuste_ISR
        ///
        ///DESCRIPCIÓN: Hace un barrido de la información que estaba registrada en prestamos antes de generar la nómina al empleado con el Ajuste de ISR Activo,
        ///             y crea una estructura que servira para generar un LOG o Historial para una posible regeneración de la nómina y poder conocer con exactitud 
        ///             que registros en Ajustes de ISR fueron afectados.
        ///             
        ///PARÁMETROS:  Dt_Registro .- Tabla que almacena la cantidad de Ajustes de ISR que el empleado tiene activos actualmente.
        ///             Registro.- Registro que se creara para almacenarlo en el LOG o Archivo Historico de la Generación de la Nómina.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static StringBuilder Crear_Registro_Insertar_Ajuste_ISR(DataTable Dt_Registro, ref StringBuilder Registro)
        {
            try
            {
                foreach (DataRow Renglon in Dt_Registro.Rows)
                {
                    Registro.Append("[");

                    Registro.Append("AJUSTE_ISR, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString()))
                        Registro.Append(Renglon[Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString() + "");
                    else
                        Registro.Append("NULL");

                    Registro.Append("]");
                    Registro.Append("\r\n");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el registro de prestamo que se actualizara, según el monto a descontar del mismo. Error: [" + Ex.Message + "]");
            }
            return Registro;
        }
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Registro_Insertar_Deducciones_Fijas
        ///
        ///DESCRIPCIÓN: Hace un barrido de la información que estaba registrada en prestamos antes de generar la nómina al empleado con sus EMPL_PERC_DEDU_DETA,
        ///             y crea una estructura que servira para generar un LOG o Historial para una posible regeneración de la nómina y poder conocer con exactitud 
        ///             que registros de EMPL_PERC_DEDU_DETA fueron afectados.
        ///             
        ///PARÁMETROS:  Dt_Registro .- Tabla que almacena la cantidad de EMPL_PERC_DEDU_DETA que el empleado tiene actualmente.
        ///             Registro.- Registro que se creara para almacenarlo en el LOG o Archivo Historico de la Generación de la Nómina.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static StringBuilder Crear_Registro_Insertar_Deducciones_Fijas(DataTable Dt_Registro, ref StringBuilder Registro)
        {
            try
            {
                foreach (DataRow Renglon in Dt_Registro.Rows)
                {
                    Registro.Append("[");

                    Registro.Append("EMPL_PERC_DEDU_DETA, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString() + ", ");
                    else
                        Registro.Append("NULL, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString() + ", ");
                    else
                        Registro.Append("0, ");

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia].ToString()))
                        Registro.Append(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia].ToString() + "");
                    else
                        Registro.Append("NULL");

                    Registro.Append("]");
                    Registro.Append("\r\n");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el registro de la deduccion aplicada al empleado por algun prestamo externo. Error: [" + Ex.Message + "]");
            }
            return Registro;
        }
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Archivo_Historial_Generacion_Nomina
        ///DESCRIPCIÓN: Elimina el archivo [LOG o Historial de la Nómina], que se genero para solventar la posible regeneración de la nómina.
        ///
        /// PARÁMETROS: Ruta.- URL que ubica al archivo dentro de la estructura del proyecto de presidencia.
        ///                    Nombre_Archivo.- Nombre que se asignara al archivo.
        ///                    Extencion.- El tipo de estención que tendra el archivo.
        ///                    
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static void Eliminar_Archivo_Historial_Generacion_Nomina(String Ruta, String Nombre_Archivo, String Extencion) {
            String Archivo = "";//Variable que almacenará toda la url del archivo a eliminar.

            try
            {
                Archivo = @"" + (Ruta + Nombre_Archivo + Extencion);//Obtenemos el Full Path del  archivo de regeneración de la nómina.

                if (File.Exists(Archivo)) {
                    File.Delete(Archivo);//Eliminamos el archivo [LOG o Historial de la Nomina Generada].
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el Archivo " + Nombre_Archivo + " con el Historial de la Nómina Generada. Error: [" + Ex.Message + "]");
            }
        }
        ///********************************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Leer_Archivo_Obtener_Historial_Nomina_Generada
        ///DESCRIPCIÓN: Hace un barrido de los registros que se afectaron al generar la nómina. Esta información  la obtendracon de el log o historial que se genero,  
        ///             para poder hacer un rollback de la información modificada, en caso de regenerar la nómina.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 30/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*********************************************************************************************************************************************************
        public static DataSet Leer_Archivo_Obtener_Historial_Nomina_Generada(String Ruta, String Nombre_Archivo, String Extencion)
        {
            StreamReader Lector = null;//Variable que leerá el archivo que almacena los registros que fueron afectados al Generar la Nómina que se desea Regenerar.
            String Linea_Leida = "";//Variable que leerá cada línea del archivo. para su tratamiento como cadena.
            String[] Columnas;//Variable que contendrá todos lo campos que se guardaron por cada registro que se almaceno en cada línea del archivo.
            String Tipo_Registro = "";//Variable que almacenará el tipo de tabla afectada durante la Generacoón de la Nómina. [PRESTAMO, AJUSTE ISR, RECIBO NOMINA O TOTALES DE LA NÓMINA].
            DataRow Renglon_Insertar = null;//Variable que almacenrá cada renglon contruido, y que se insertara en una respectiva tabla.
            DataSet Ds_Historial_Nomina_Generada = new DataSet();//Variable de Tipo Estructura qie almacenará una lista de Tablas [PRESTAMO, AJUSTE ISR, RECIBO NOMINA O TOTALES DE LA NÓMINA].
            DataTable Dt_Prestamos = new DataTable("PRESTAMO");//Tabla [PRESTAMOS] con los registros que fueron afectados al Generar la Nómina.
            DataTable Dt_Ajustes_ISR = new DataTable("AJUSTE_ISR");//Tabla [AJUSTE ISR] con los registros que fueron afectados al Generar la Nómina.
            DataTable Dt_Recibos_Generados = new DataTable("RECIBOS");//Tabla [RECIBOS DE  LA NÓMINA] con los registros que fueron afectados al Generar la Nómina.
            DataTable Dt_Totales_Nomina = new DataTable("TOTALES_NOMINA");//Tabla [TOTALES DE LA NÓMINA] con los registros que fueron afectados al Generar la Nómina.
            DataTable Dt_Empl_Perc_Dedu_Deta = new DataTable("EMPL_PERC_DEDU_DETA");//Tabla [EMPL_PERC_DEDU_DETA] con los registros que fueron afectados al Generar la Nómina.

            try
            {
                //Se válida que exista un archivo [LOG o Historial de los Registros Afectados al Generar la Nómina].
                if (File.Exists(@"" + (Ruta + Nombre_Archivo + Extencion)))
                {
                    //Si existe el archivo el siguiente paso, es crear el objeto que nos ayudará a leer el archivo.
                    Lector = new StreamReader(@"" + (Ruta + Nombre_Archivo + Extencion));

                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                    //al generar la nómina en la tabla [PRESTAMOS].
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud, typeof(String));
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado, typeof(String));
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual, typeof(String));
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_No_Abono, typeof(String));
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo, typeof(String));
                    Dt_Prestamos.Columns.Add(Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo, typeof(String));
                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 

                    //al generar la nómina en la tabla [AJUSTES ISR].
                    Dt_Ajustes_ISR.Columns.Add(Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR, typeof(String));
                    Dt_Ajustes_ISR.Columns.Add(Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado, typeof(String));
                    Dt_Ajustes_ISR.Columns.Add(Ope_Nom_Ajuste_ISR.Campo_No_Pago, typeof(String));
                    Dt_Ajustes_ISR.Columns.Add(Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR, typeof(String));

                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                    //al generar la nómina en la tabla [RECIBOS DE LA NÓMINA].
                    Dt_Recibos_Generados.Columns.Add(Ope_Nom_Recibos_Empleados.Campo_No_Recibo, typeof(String));

                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                    //al generar la nómina en la tabla [TOTALES DE LA NÓMINA].
                    Dt_Totales_Nomina.Columns.Add(Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina, typeof(String));

                    //Se crea la estructura para almacenar los registros afectados en la generación de la nómina
                    //de la tabla de EMPL_PERC_DEDU_DETA.
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina, typeof(String));
                    Dt_Empl_Perc_Dedu_Deta.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia, typeof(String));

                    //Leer el archivo, hasta llegar al final del mismo.
                    //Esto para leer todos los registros que fueron afectados al generar la nómina.
                    while ((Linea_Leida = Lector.ReadLine()) != null)
                    {
                        Renglon_Insertar = null;//Limpiamos el renglon, para que este disponible para cuando se realize el siguiente registro.
                        Linea_Leida = Linea_Leida.Replace("[", "");//Eliminamos el carácter [ de la cadena leida.
                        Linea_Leida = Linea_Leida.Replace("]", "");//Eliminamos el carácter ] de la cadena leida.
                        Columnas = Linea_Leida.Split(new Char[] { ',' });//Obtenemos un arreglo con un número de elementos igual, al número de
                                                                         //elementos separados por comas.
                        Tipo_Registro = Columnas[0];//Variable que almacenará el tipo de tabla afectada durante la Generacoón de la Nómina. [PRESTAMO, AJUSTE ISR, RECIBO NOMINA O TOTALES DE LA NÓMINA].

                        switch (Tipo_Registro)
                        {
                            case "PRESTAMO":
                                if (Columnas.Length == 7)
                                {
                                    Renglon_Insertar = Dt_Prestamos.NewRow();
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud] = Columnas[1];
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado] =  Columnas[2];
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual] = Columnas[3];
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono] =  Columnas[4];
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo] = Columnas[5];
                                    Renglon_Insertar[Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo] = Columnas[6];
                                    Dt_Prestamos.Rows.Add(Renglon_Insertar);
                                }
                                break;
                            case "AJUSTE_ISR":
                                if (Columnas.Length == 5)
                                {
                                    Renglon_Insertar = Dt_Ajustes_ISR.NewRow();
                                    Renglon_Insertar[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR] = Columnas[1];
                                    Renglon_Insertar[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado] =  Columnas[2];
                                    Renglon_Insertar[Ope_Nom_Ajuste_ISR.Campo_No_Pago] =  Columnas[3];
                                    Renglon_Insertar[Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR] = Columnas[4];
                                    Dt_Ajustes_ISR.Rows.Add(Renglon_Insertar);
                                }
                                break;
                            case "RECIBOS":
                                if (Columnas.Length == 2)
                                {
                                    Renglon_Insertar = Dt_Recibos_Generados.NewRow();
                                    Renglon_Insertar[Ope_Nom_Recibos_Empleados.Campo_No_Recibo] = Columnas[1];
                                    Dt_Recibos_Generados.Rows.Add(Renglon_Insertar);
                                }
                                break;
                            case "TOTALES_NOMINA":
                                if (Columnas.Length == 2)
                                {
                                    Renglon_Insertar = Dt_Totales_Nomina.NewRow();
                                    Renglon_Insertar[Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina] = Columnas[1];
                                    Dt_Totales_Nomina.Rows.Add(Renglon_Insertar);
                                }
                                break;
                            case "EMPL_PERC_DEDU_DETA":
                                if (Columnas.Length == 10)
                                {
                                    Renglon_Insertar = Dt_Empl_Perc_Dedu_Deta.NewRow();
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID] = Columnas[1];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID] = Columnas[2];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad] = Columnas[3];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe] = Columnas[4];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo] = Columnas[5];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida] = Columnas[6];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID] = Columnas[7];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina] = Columnas[8];
                                    Renglon_Insertar[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia] = Columnas[9];
                                    Dt_Empl_Perc_Dedu_Deta.Rows.Add(Renglon_Insertar);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    Lector.Close();//Cerramos el lector del archivo.

                    //Agregamos las tablas con los registros que fueron afectados al generar la nómina.
                    //Tablas [PRESTAMOS, AJUSTE DE ISR, RECIBO DE NOMINA, TOTALES DE LA NÓMINA].
                    Ds_Historial_Nomina_Generada.Tables.Add(Dt_Prestamos);
                    Ds_Historial_Nomina_Generada.Tables.Add(Dt_Ajustes_ISR);
                    Ds_Historial_Nomina_Generada.Tables.Add(Dt_Recibos_Generados);
                    Ds_Historial_Nomina_Generada.Tables.Add(Dt_Totales_Nomina);
                    Ds_Historial_Nomina_Generada.Tables.Add(Dt_Empl_Perc_Dedu_Deta);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al escribir el Archivo " + Nombre_Archivo + ". Error: [" + Ex.Message + "]");
            }
            return Ds_Historial_Nomina_Generada;
        }
    }
}
