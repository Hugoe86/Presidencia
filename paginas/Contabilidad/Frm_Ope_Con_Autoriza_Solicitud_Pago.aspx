<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Autoriza_Solicitud_Pago.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Autoriza_Solicitud_Pago" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
 // Cerrar el mes seleccionado
        function Autorizar_Solicitud(Control) {
            //limpiar controles
            var No_Solicitud = $(Control).parent().attr('class');
            var comentario;
            if ($(Control).is(':checked')) 
            {
                x1 = confirm('Estas seguro que deseas Autorizar la Solicitud de Pago ?');
                if (x1 == true) {
                    comentario = prompt('Ingresa Algun Comentario Sobre la Autorizacion de la Solicitud', 'NINGUNO'); 
                    
                    MostrarProgress();
                    var cadena = "Accion=Autorizar_Solicitud&id=" + No_Solicitud + "&x=" + comentario+ "&";
                    $.ajax({
                        url: "Frm_Ope_Con_Autoriza_Solicitud_Pago.aspx?" + cadena,
                        type: 'POST',
                        async: false,
                        cache: false,
                        success: function(data) {
                            alert("La Solicitud de Pago se Autorizo Correctamente ");
                            location.reload();
                        }
                    });  
                } else {
                $(Control).attr('checked', false);
                }
            }
        }
        function Rechazar_Solicitud(Control) {
            //limpiar controles
            var No_Solicitud = $(Control).parent().attr('class');
            if ($(Control).is(':checked')) 
            {
                x1 = confirm('Estas seguro que deseas Rechazar la Solicitud de Pago?');
                if (x1 == true) {
                    comentario = prompt('Ingresa el Motivo del Rechazo de la Solicitud', 'NINGUNO'); 
                    MostrarProgress();
                    var cadena = "Accion=Rechazar_Solicitud&id=" + No_Solicitud + "&x=" + comentario + "&";
                    $.ajax({
                    url: "Frm_Ope_Con_Autoriza_Solicitud_Pago.aspx?" + cadena,
                        type: 'POST',
                        async: false,
                       cache: false,
                       success: function(data) {
                       alert("La Solicitud de Pago se Rechazo Correctamente");
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
            $('[id$=Up_Autorizacion]').show();
        }
        function OcultarProgress() {
            $('[id$=Up_Autorizacion]').delay(10000).hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager_Parametros_Contabilidad" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Up_Autorizacion" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Parametros_Contabilidad" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Autorizacion de Solicitud de Pagos</td>
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
                <table width="98%" class="estilo_fuente">
                    <tr>
                    <td>
                    &nbsp;
                    <asp:HiddenField ID="Txt_Monto_Solicitud" runat="server" />
                    <asp:HiddenField ID="Txt_Cuenta_Contable_ID_Proveedor" runat="server" />
                    <asp:HiddenField ID="Txt_Cuenta_Contable_reserva" runat="server" />
                    <asp:HiddenField ID="Txt_No_Reserva" runat="server" />
                    </td>
                    </tr>
                    <tr>
                        <td style="width:100%;text-align:center;vertical-align:top;"> 
                            <center>
                                <div style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                    <asp:GridView ID="Grid_Solicitud_Pagos" runat="server" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                        onpageindexchanging="Grid_Solicitud_Pagos_PageIndexChanging" Width="100%">
                                        <Columns>                       
                                            <asp:BoundField DataField="No_Solicitud_Pago" HeaderText="Solicitud Pago">
                                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                <ItemStyle HorizontalAlign="Left" Width="7%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="No_Reserva" HeaderText="No Reserva">
                                                <HeaderStyle HorizontalAlign="center" Width="13%" />
                                                <ItemStyle HorizontalAlign="center" Width="13%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tipo_Solicitud_Pago_ID" HeaderText="tipo_Pago_ID">
                                                <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="Tipo_Pago" HeaderText="Tipo Pago">
                                                <HeaderStyle HorizontalAlign="Left" Width="13%" />
                                                <ItemStyle HorizontalAlign="Left" Width="13%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Concepto" HeaderText="Concepto">
                                                <HeaderStyle HorizontalAlign="Left" Width="32%" />
                                                <ItemStyle HorizontalAlign="Left" Width="32%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                                <ItemStyle HorizontalAlign="Left" Width="8%" />
                                            </asp:BoundField>
                                            <asp:TemplateField  HeaderText= "Autorizar">
                                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                <ItemTemplate >
                                                    <asp:CheckBox ID="Chk_Autorizado" runat="server"  onclick="Autorizar_Solicitud(this);"  CssClass='<%# Eval("No_Solicitud_Pago") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText= "Rechazado">
                                                <HeaderStyle HorizontalAlign="center" Width="10%" />                                                
                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                <ItemTemplate >
                                                 <asp:CheckBox ID="Chk_Rechazado"  runat="server" onclick="Rechazar_Solicitud(this);" CssClass='<%# Eval("No_Solicitud_Pago") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                          
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

