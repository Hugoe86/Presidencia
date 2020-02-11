<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Cierre_Mensual.aspx.cs" Inherits="paginas_Contabilidad_Frm_Cat_Con_Cierre_Mensual" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
//        $(document).ready(function() {
//           Configuracion_Inicial();
//        });
//        function Configuracion_Inicial(){
//            jQuery.ajaxSetup({
//                async: false,
//                cache: false,
//                timeout: (2 * 1000),
//                beforeSend: function() {
//                    MostrarProgress();
//                },
//                complete: function() {
//                    OcultarProgress();
//                },
//                error: function() {
//                    OcultarProgress();
//                }
//            });
//    }
 // Cerrar el mes seleccionado
        function Cerrar_Mes(Control) {
            //limpiar controles
            var Mes = $(Control).parent().attr('class');
            var anio = $('select[id$=Cmb_Anio_Contable] :selected').text();
            if ($(Control).is(':checked')) 
            {
                x1 = confirm('Estas seguro que deseas Cerrar el Mes?');
                if (x1 == true) {
                    MostrarProgress();
                    var cadena = "Accion=Cerrar_Mes&id=" + Mes + "&x=" + anio + "&";
                    $.ajax({
                        url: "Frm_Ope_Con_Cierre_Mensual.aspx?" + cadena,
                        type: 'POST',
                        async: false,
                        cache: false,
                        success: function(data) {
                            alert("El Mes fue Cerrado Satisfactoriamente");
                            location.reload();
                        }
                    });  
                } else {
                $(Control).attr('checked', false);
                }
            }
        }
        function Abrir_Mes(Control) {
            //limpiar controles
            var Mes = $(Control).parent().attr('class');
            var anio = $('select[id$=Cmb_Anio_Contable] :selected').text();
            if ($(Control).is(':checked')) 
            {
                x1 = confirm('Estas seguro que deseas Abrir el Mes?');
                if (x1 == true) {
                    MostrarProgress();
                    var cadena = "Accion=Abrir_Mes&id=" + Mes + "&x=" + anio + "&";
                    $.ajax({
                    url: "Frm_Ope_Con_Cierre_Mensual.aspx?" + cadena,
                        type: 'POST',
                        async: false,
                       cache: false,
                       success: function(data) {
                       alert("El Mes fue Abierto Satisfactoriamente");
                            location.reload();
                        }
                   }); 
                } else {
                $(Control).attr('checked', false);
                }
            }
        }
        //Modal progress, uso de animación de preogresso, indicador de actividad -------------------------------------------------
        function MostrarProgress() {
            $('[id$=UpCierre]').show();
        }
        function OcultarProgress() {
            $('[id$=UpCierre]').delay(10000).hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager_Parametros_Contabilidad" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpCierre" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Parametros_Contabilidad" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cierre Mensual</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">
                        </td> 
                    </tr>          
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td width="10%">*Año</td>
                        <td width="10%">
                            <asp:DropDownList ID="Cmb_Anio_Contable" runat="server" Width="150px" AutoPostBack="true"  onselectedindexchanged="Cmb_Anio_Contable_SelectedIndexChanged">
                                <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                <asp:ListItem>2010</asp:ListItem>
                                <asp:ListItem>2011</asp:ListItem>
                                <asp:ListItem>2012</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                                <asp:ListItem>2018</asp:ListItem>
                                <asp:ListItem>2019</asp:ListItem>
                                <asp:ListItem>2020</asp:ListItem>
                                <asp:ListItem>2021</asp:ListItem>
                                <asp:ListItem>2022</asp:ListItem>
                                <asp:ListItem>2023</asp:ListItem>
                                <asp:ListItem>2024</asp:ListItem>
                                <asp:ListItem>2025</asp:ListItem>
                                <asp:ListItem>2026</asp:ListItem>
                                <asp:ListItem>2027</asp:ListItem>
                                <asp:ListItem>2028</asp:ListItem>
                                <asp:ListItem>2029</asp:ListItem>
                                <asp:ListItem>2030</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td width="5%"></td>
                        <td width="45%"></td>
                    </tr>
                </table>
                <table width="98%" class="estilo_fuente">
                    <tr>
                    <td>
                    &nbsp;
                    </td>
                    </tr>
                    <tr>
                        <td style="width:100%;text-align:center;vertical-align:top;"> 
                            <center>
                                <div style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                    <asp:GridView ID="Grid_Cierres_Mensuales" runat="server" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                        OnRowDataBound="Grid_Cierres_Mensuales_RowDataBound" 
                                        onselectedindexchanged="Grid_Cierres_Mensuales_SelectedIndexChanged" Width="100%">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>                       
                                            <asp:BoundField DataField="Anio" HeaderText="Año">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle HorizontalAlign="Left" Width="7%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Mes" HeaderText="Mes">
                                                <HeaderStyle HorizontalAlign="Left" Width="13%" />
                                                <ItemStyle HorizontalAlign="Left" Width="13%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:TemplateField  HeaderText= "Cerrar">
                                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                <ItemTemplate >
                                                    <asp:CheckBox ID="Chk_Cerrar" runat="server"  onclick="Cerrar_Mes(this);"  CssClass='<%# Eval("Mes") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText= "Abrir">
                                                <HeaderStyle HorizontalAlign="center" Width="10%" />                                                
                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                <ItemTemplate >
                                                 <asp:CheckBox ID="Chk_Abrir"  runat="server" onclick="Abrir_Mes(this);" CssClass='<%# Eval("Mes") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="USUARIO_CREO" HeaderText="Empleado Creo">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha Creo">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>                                            
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </center>                                       
                        </td>
                    </tr>
                </table>
                
                <asp:GridView ID="Grid_Bitacora" runat="server" AllowPaging="True" CssClass="GridView_1" 
                    AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="98%"
                    AllowSorting="True" HeaderStyle-CssClass="tblHead">
                    <Columns>                       
                        <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" >
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:BoundField>
                           <asp:BoundField DataField="Descripcion" HeaderText="Tipo">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Usuario_Creo" HeaderText=" Creo">
                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" 
                             DataFormatString="{0:dd/MMM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Debe" HeaderText="Debe" 
                             DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" Width="10%" />
                        </asp:BoundField>    
                         <asp:BoundField DataField="haber" HeaderText="Haber" 
                             DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" Width="10%" />
                        </asp:BoundField>                                
                    </Columns>
                    <SelectedRowStyle CssClass="GridSelected" />
                    <PagerStyle CssClass="GridHeader" />
                    <HeaderStyle CssClass="tblHead" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                </asp:GridView>                           
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

