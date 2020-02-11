<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generar_Faltas_Retardos_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Nom_Ope_Generar_Faltas_Retardos_Empleados" Title="Generar Faltas y Retardos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">  
    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        } 
    </script>
    
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
   </script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Reloj_Checador" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Generacion de Faltas y Retardos de Empleados</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btn_Generar_Faltas_Retardos" runat="server"
                                                        ImageUrl="~/paginas/imagenes/paginas/Listado.png" 
                                                        TabIndex="1" ToolTip="Generar" onclick="Btn_Generar_Faltas_Retardos_Click" />
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="2" onclick="Btn_Salir_Click"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" /> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td align="right" style="width:41%;"></td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>           
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td width="100%">
                            <asp:Panel id="Pnl_General_Calendario_Reloj" runat="server" GroupingText="Datos Calendario">
                                <table width="100%" class="estilo_fuente">
                                    <tr>  
                                        <td style="width:11%;text-align:left;">*Nomina</td>
                                        <td style="width:14%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="95%" AutoPostBack="true" 
                                                TabIndex="3" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                        </td>
                                        <td style="width:11%;text-align:left;">*Periodo</td>
                                        <td style="width:14%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="95%" TabIndex="4" 
                                                AutoPostBack="true" onselectedindexchanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged" />
                                        </td>
                                        <td style="width:11%;text-align:left;">Fecha Inicio</td>
                                        <td style="width:14%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Inicio_Falta_Retardo" runat="server" Width="95%" ReadOnly="true" 
                                                BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                        </td>
                                        <td style="width:11%;text-align:left;">Fecha Fin</td>
                                        <td style="width:14%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Termino_Falta_Retardo" runat="server" Width="95%" ReadOnly="true" 
                                                BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                                      
                    <tr align="center">
                        <td>
                            <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" > 
                                <asp:GridView ID="Grid_Lista_Faltas_Retardos" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" GridLines="None" Width="100%"
                                    AllowSorting="True" HeaderStyle-CssClass="tblHead">
                                    <Columns>
                                        <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Dependencia_ID" HeaderText="Dependencia_ID" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="No_Empleado" HeaderText="No Empleado" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Empleado" HeaderText="Empleado" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                            <ItemStyle HorizontalAlign="Left" Width="35%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True"
                                            DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Retardo" HeaderText="Retardo" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo_Falta" HeaderText="Tipo Falta" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>          
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

