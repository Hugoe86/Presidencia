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
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Presidencia.Ayudante_JQuery
{
    public class Ayudante_JQuery
    {
        /// *********************************************************************************************************
        /// Nombre: Crear_Tabla_Formato_JSON
        /// 
        /// Descripción: Método que devuelve la estructura de una cadena en formato JSON
        ///              con la infomración contenida en la tabla que es pasada como parámetro
        ///              al método.
        /// 
        /// Parámetros: Dt_Datos.- Tabla que almacena la información que será procesada.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Diciembre 2011
        /// Usuario Modifico: Leslie Gonzalez Vazquez
        /// Fecha Modifico: Diciembre 2011
        /// Causa Modificación: Cuando la tabla que se pasaba como parámetro no traia ningun registro el formato de
        ///                     la cadena no era generado de forma correcta.
        /// **********************************************************************************************************
        public static String Crear_Tabla_Formato_JSON(DataTable Dt_Datos){
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonTextWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                Escribir_Formato_JSON.Formatting = Formatting.None;
                Escribir_Formato_JSON.WriteStartObject();
                Escribir_Formato_JSON.WritePropertyName("total");
                Escribir_Formato_JSON.WriteValue((Dt_Datos != null) ? Dt_Datos.Rows.Count.ToString() : "0");
                Escribir_Formato_JSON.WritePropertyName(Dt_Datos.TableName);
                Escribir_Formato_JSON.WriteStartArray();

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteStartObject();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                if (!String.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()))
                                {
                                    Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);
                                    Escribir_Formato_JSON.WriteValue(FILA[COLUMNA.ColumnName].ToString());
                                }
                            }
                            Escribir_Formato_JSON.WriteEndObject();
                        }
                    }
                }

                Escribir_Formato_JSON.WriteEndArray();
                Escribir_Formato_JSON.WriteEndObject();
                Cadena_Resultado = Buffer.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error al crear la cadena json para cargar un combo.");
            }
            return Cadena_Resultado;
        }
        /// *********************************************************************************************************
        /// Nombre: Crear_Tabla_Formato_JSON_DataGrid
        /// 
        /// Descripción: Método que devuelve la estructura de una cadena en formato JSON
        ///              con la infomración contenida en la tabla que es pasada como parámetro
        ///              al método. Este método se utilizara cuando se tenga que cargar un grid.
        /// 
        /// Parámetros: Dt_Datos.- Tabla que almacena la información que será procesada.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Diciembre 2011
        /// Usuario Modifico: Leslie Gonzalez Vazquez
        /// Fecha Modifico: Diciembre 2011
        /// Causa Modificación: Cuando la tabla que se pasaba como parámetro no traia ningun registro el formato de
        ///                     la cadena no era generado de forma correcta.
        /// **********************************************************************************************************
        public static String Crear_Tabla_Formato_JSON_DataGrid(DataTable Dt_Datos, Int32 Total_Registros)
        {
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonTextWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                Escribir_Formato_JSON.Formatting = Formatting.None;
                Escribir_Formato_JSON.WriteStartObject();
                Escribir_Formato_JSON.WritePropertyName("total");
                Escribir_Formato_JSON.WriteValue(Total_Registros.ToString());
                Escribir_Formato_JSON.WritePropertyName((Dt_Datos.TableName != null) ? Dt_Datos.TableName.ToString() : "0");
                Escribir_Formato_JSON.WriteStartArray();

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteStartObject();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                if (!String.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()))
                                {
                                    Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);
                                    Escribir_Formato_JSON.WriteValue(FILA[COLUMNA.ColumnName].ToString());
                                }
                            }
                            Escribir_Formato_JSON.WriteEndObject();
                        }
                    }
                }

                Escribir_Formato_JSON.WriteEndArray();
                Escribir_Formato_JSON.WriteEndObject();
                Cadena_Resultado = Buffer.ToString();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la cadena json para cargar un grid. Error: [" + Ex.Message + "]");
            }
            return Cadena_Resultado;
        }

        /// *********************************************************************************************************
        /// Nombre: Crear_Tabla_Formato_JSON
        /// 
        /// Descripción: Método que devuelve la estructura de una cadena en formato JSON
        ///              con la infomración contenida en la tabla que es pasada como parámetro
        ///              al método.
        /// 
        /// Parámetros: Dt_Datos.- Tabla que almacena la información que será procesada.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Diciembre 2011
        /// Usuario Modifico: Leslie Gonzalez Vazquez
        /// Fecha Modifico: Diciembre 2011
        /// Causa Modificación: Cuando la tabla que se pasaba como parámetro no traia ningun registro el formato de
        ///                     la cadena no era generado de forma correcta.
        /// **********************************************************************************************************
        public static String Crear_Tabla_Formato_JSON_ComboBox(DataTable Dt_Datos)
        {
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonTextWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                Escribir_Formato_JSON.Formatting = Formatting.None;
                Escribir_Formato_JSON.WriteStartArray();

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteStartObject();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                if (!String.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()))
                                {
                                    Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);
                                    Escribir_Formato_JSON.WriteValue(FILA[COLUMNA.ColumnName].ToString());
                                }
                            }
                            Escribir_Formato_JSON.WriteEndObject();
                        }
                    }
                }

                Escribir_Formato_JSON.WriteEndArray();
                Cadena_Resultado = Buffer.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error al crear la cadena json para cargar un combo.");
            }
            return Cadena_Resultado;
        }

    }
}
