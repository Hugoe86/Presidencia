using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Catalogo_Cat_Peritos_Internos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Peritos_Internos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Peritos_Internos.Negocio
{
    public class Cls_Cat_Cat_Peritos_Internos_Negocio
    {
        #region variables privadas

        private String Perito_Interno_Id;
        private String Empleado_Id;
        private String Empleado_Nombre;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Perito_Interno_Id
        {
            get { return Perito_Interno_Id; }
            set { Perito_Interno_Id = value; }
        }

        public String P_Empleado_Id
        {
            get { return Empleado_Id; }
            set { Empleado_Id = value; }
        }

        public String P_Empleado_Nombre
        {
            get { return Empleado_Nombre; }
            set { Empleado_Nombre = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Perito_Interno()
        {
            return Cls_Cat_Cat_Peritos_Internos_Datos.Alta_Perito_Interno(this);
        }

        public Boolean Modificar_Perito_Interno()
        {
            return Cls_Cat_Cat_Peritos_Internos_Datos.Modificar_Perito_Interno(this);
        }

        public DataTable Consultar_Peritos_Internos()
        {
            return Cls_Cat_Cat_Peritos_Internos_Datos.Consultar_Peritos_Internos(this);
        }

        public DataTable Consultar_Empleados()
        {
            return Cls_Cat_Cat_Peritos_Internos_Datos.Consultar_Empleados(this);
        }

        #endregion
    }
}