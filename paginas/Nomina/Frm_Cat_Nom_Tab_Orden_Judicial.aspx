<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Tab_Orden_Judicial.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Tab_Orden_Judicial" Title="Catálogo de Parámetros de Orden Judicial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
         function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
 
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Rol.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Contrato.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Areas.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Puesto.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Escolaridad.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Sindicato.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Turno.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Zona.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Trabajador.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Nomina.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";
            return false;
        }  
        
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        } 
        
        function Cerrar_Modal_Popup() {
            $find('Busqueda_Empleados').hide();
            return false;
        } 
        
        function pageLoad(){
            $("select[id$=Cmb_Tipo_Desc_Orden_Judicial_Sueldo]").bind("change keyup", function() {Validar_Cantidad_Porcentaje_Sueldo();});
            $("select[id$=Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo]").bind("change keyup", function() {Validar_Cantidad_Porcentaje_Aguinaldo();});
            $("select[id$=Cmb_Tipo_Desc_Orden_Judicial_PV]").bind("change keyup", function() {Validar_Cantidad_Porcentaje_Prima_Vacacional();});
            $("select[id$=Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion]").bind("change keyup", function() {Validar_Cantidad_Porcentaje_Indemnizacion();});
        }
        
        function Validar_Cantidad_Porcentaje_Sueldo(){
            var Cantidad_Porcentaje = $('select[id$=Cmb_Tipo_Desc_Orden_Judicial_Sueldo] :selected').val();
            var Valor = parseFloat(($('input[id$=Txt_Cantidad_Porcentaje_Sueldo_OJ]').val() == '')? '0': $('input[id$=Txt_Cantidad_Porcentaje_Sueldo_OJ]').val());
            
            if(!(Cantidad_Porcentaje == 'N' )){
                if(Cantidad_Porcentaje == 'CANTIDAD'){
                    $('input[id$=Txt_Cantidad_Porcentaje_Sueldo_OJ]').val(Valor.toString());
                }else if(Cantidad_Porcentaje == 'PORCENTAJE'){
                    if( (Valor < 0) || (Valor > 100)){
                        $('input[id$=Txt_Cantidad_Porcentaje_Sueldo_OJ]').val('0');
                        alert('Los porcentajes deben ser mayores a cero pero menores a 100 [0-100].');
                    }else{
                        $('input[id$=Txt_Cantidad_Porcentaje_Sueldo_OJ]').val(Valor.toString());
                    }
                }
            }else{
                alert('Seleccione un tipo de descuento ya sea por cantidad o porcentaje.');
            }
        }
        
        function Validar_Cantidad_Porcentaje_Aguinaldo(){
            var Cantidad_Porcentaje = $('select[id$=Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo] :selected').val();
            var Valor = parseFloat(($('input[id$=Txt_Cantidad_Porcentaje_Aguinaldo_OJ]').val() == '')? '0': $('input[id$=Txt_Cantidad_Porcentaje_Aguinaldo_OJ]').val());
            
            if(!(Cantidad_Porcentaje == 'N' )){
                if(Cantidad_Porcentaje == 'CANTIDAD'){
                    $('input[id$=Txt_Cantidad_Porcentaje_Aguinaldo_OJ]').val(Valor.toString());
                }else if(Cantidad_Porcentaje == 'PORCENTAJE'){
                    if( (Valor < 0) || (Valor > 100)){
                        $('input[id$=Txt_Cantidad_Porcentaje_Aguinaldo_OJ]').val('0');
                        alert('Los porcentajes deben ser mayores a cero pero menores a 100 [0-100].');
                    }else{
                        $('input[id$=Txt_Cantidad_Porcentaje_Aguinaldo_OJ]').val(Valor.toString());
                    }
                }
            }else{
                alert('Seleccione un tipo de descuento ya sea por cantidad o porcentaje.');
            }
        }
        
        function Validar_Cantidad_Porcentaje_Prima_Vacacional(){
            var Cantidad_Porcentaje = $('select[id$=Cmb_Tipo_Desc_Orden_Judicial_PV] :selected').val();
            var Valor = parseFloat(($('input[id$=Txt_Cantidad_Porcentaje_PV_OJ]').val() == '')? '0': $('input[id$=Txt_Cantidad_Porcentaje_PV_OJ]').val());
            
            if(!(Cantidad_Porcentaje == 'N' )){
                if(Cantidad_Porcentaje == 'CANTIDAD'){
                    $('input[id$=Txt_Cantidad_Porcentaje_PV_OJ]').val(Valor.toString());
                }else if(Cantidad_Porcentaje == 'PORCENTAJE'){
                    if( (Valor < 0) || (Valor > 100)){
                        $('input[id$=Txt_Cantidad_Porcentaje_PV_OJ]').val('0');
                        alert('Los porcentajes deben ser mayores a cero pero menores a 100 [0-100].');
                    }else{
                        $('input[id$=Txt_Cantidad_Porcentaje_PV_OJ]').val(Valor.toString());
                    }
                }
            }else{
                alert('Seleccione un tipo de descuento ya sea por cantidad o porcentaje.');
            }
        }

        function Validar_Cantidad_Porcentaje_Indemnizacion(){
            var Cantidad_Porcentaje = $('select[id$=Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion] :selected').val();
            var Valor = parseFloat(($('input[id$=Txt_Cantidad_Porcentaje_Indemnizacion_OJ]').val() == '')? '0': $('input[id$=Txt_Cantidad_Porcentaje_Indemnizacion_OJ]').val());
            
            if(!(Cantidad_Porcentaje == 'N' )){
                if(Cantidad_Porcentaje == 'CANTIDAD'){
                    $('input[id$=Txt_Cantidad_Porcentaje_Indemnizacion_OJ]').val(Valor.toString());
                }else if(Cantidad_Porcentaje == 'PORCENTAJE'){
                    if( (Valor < 0) || (Valor > 100)){
                        $('input[id$=Txt_Cantidad_Porcentaje_Indemnizacion_OJ]').val('0');
                        alert('Los porcentajes deben ser mayores a cero pero menores a 100 [0-100].');
                    }else{
                        $('input[id$=Txt_Cantidad_Porcentaje_Indemnizacion_OJ]').val(Valor.toString());
                    }
                }
            }else{
                alert('Seleccione un tipo de descuento ya sea por cantidad o porcentaje.');
            }
        }
        
        function Eliminar(texto){
            var Identificador =$('input[id$=Txt_Orden_Judicial_ID]').val();
            
            if(Identificador == ''){
               $('#<%=Lbl_Mensaje_Error.ClientID %>').html('<br/ >&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; + Seleccione el registro a ' + texto + '.');
               $('#<%=Lbl_Mensaje_Error.ClientID %>').show();
               $('img[id$=Img_Error]').show();
               return false;
            }else{
                if(texto == 'Modificar')
                    return true;
                else
                    return confirm('Esta seguro de eliminar el registro seleccionado?');
            }
        }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="Sm_Tabulador_Orden_Judicial" runat="server"/>
    <asp:UpdatePanel ID="UPnl_Tab_Orden_Judicial" runat="server" UpdateMode="Always">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="UPrs_Tab_Orden_Judicial" runat="server" AssociatedUpdatePanelID="UPnl_Tab_Orden_Judicial" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Indicador_Presidencia.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        
            <div id="Div_Contenedor_Tab_Orden_Judicial" style="width:98%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Tabulador Orden Judicial</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" style="display:none;"/>&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" CssClass="estilo_fuente_mensaje_error" style="display:none;" />
                        </td>
                    </tr>
                </table>
                
                <table width="98%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td colspan="2"> 
                             <div align="right" class="barra_busqueda">
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="3" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" 
                                                OnClientClick="javascript:return Eliminar('Modificar');"/>
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="5"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                OnClientClick="javascript:return Eliminar('Eliminar');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                    <asp:ImageButton ID="Btn_Busqueda" runat="server" ToolTip="Búsqueda Avanzada"
                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" Height="24px"
                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" />
                                                </td>
                                            </tr>
                                        </table>
                                       </td>
                                     </tr>
                                  </table>
                                       </td>
                                     </tr>
                                  </table>
                                </div>
                         </td>
                     </tr>
                </table>
                                  
                  <table style="width:97%">
                    <tr>
                        <td colspan="4" style="width:100%;">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; cursor:default;">
                            Orden Judicial ID
                        </td>
                        <td style="width:30%; text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Orden_Judicial_ID" runat="server" Width="98%" TabIndex="1"/>
                        </td>
                        <td style="width:20%; text-align:left; cursor:default;">
                            &nbsp; &nbsp;
                        </td>
                        <td style="width:30%; text-align:left; cursor:default;">
                        
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; cursor:default;">
                            Empleado
                        </td>
                        <td style="width:80%; text-align:left; cursor:default;" colspan="3">
                            <asp:TextBox ID="Txt_Empleado" runat="server" Width="98%" TabIndex="2"/>
                            <asp:HiddenField ID="HTxt_Empleado" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; cursor:default;">
                            Beneficiario
                        </td>
                        <td style="width:80%; text-align:left; cursor:default;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Beneficiario" runat="server" Width="98%" TabIndex="2"
                                onkeyup='this.value = this.value.toUpperCase();'/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        Sueldo
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Tipo_Desc_Orden_Judicial_Sueldo" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem Value='N'>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="PORCENTAJE">PORCENTAJE</asp:ListItem>
                                            <asp:ListItem Value="CANTIDAD">CANTIDAD</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        <asp:TextBox ID="Txt_Cantidad_Porcentaje_Sueldo_OJ" runat="server" Width="97%" TabIndex="2"
                                            style="text-align:right;" onblur="javascript:Validar_Cantidad_Porcentaje_Sueldo();"
                                            onkeyup="this.value = (this.value.match(/^(?:\+|-)?\d+$/))? this.value : '';"/>
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Neto_Bruto_Sueldo_OJ" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="NETO">NETO</asp:ListItem>
                                            <asp:ListItem Value="BRUTO">BRUTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        Aguinaldo
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem Value='N'>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="PORCENTAJE">PORCENTAJE</asp:ListItem>
                                            <asp:ListItem Value="CANTIDAD">CANTIDAD</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        <asp:TextBox ID="Txt_Cantidad_Porcentaje_Aguinaldo_OJ" runat="server" Width="97%" TabIndex="2"
                                            style="text-align:right;"  onblur="javascript:Validar_Cantidad_Porcentaje_Aguinaldo();"
                                            onkeyup="this.value = (this.value.match(/^(?:\+|-)?\d+$/))? this.value : '';"/>
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Neto_Bruto_Aguinaldo_OJ" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="NETO">NETO</asp:ListItem>
                                            <asp:ListItem Value="BRUTO">BRUTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        Prima Vacacional
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Tipo_Desc_Orden_Judicial_PV" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem Value='N'>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="PORCENTAJE">PORCENTAJE</asp:ListItem>
                                            <asp:ListItem Value="CANTIDAD">CANTIDAD</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        <asp:TextBox ID="Txt_Cantidad_Porcentaje_PV_OJ" runat="server" Width="97%" TabIndex="2"
                                            style="text-align:right;"  onblur="javascript:Validar_Cantidad_Porcentaje_Prima_Vacacional();"
                                            onkeyup="this.value = (this.value.match(/^(?:\+|-)?\d+$/))? this.value : '';"/>
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Neto_Bruto_PV_OJ" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem Value='N'>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="NETO">NETO</asp:ListItem>
                                            <asp:ListItem Value="BRUTO">BRUTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        Indemnizaci&oacute;n
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="PORCENTAJE">PORCENTAJE</asp:ListItem>
                                            <asp:ListItem Value="CANTIDAD">CANTIDAD</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:20%; text-align:left; cursor:default;">
                                        <asp:TextBox ID="Txt_Cantidad_Porcentaje_Indemnizacion_OJ" runat="server" Width="97%" TabIndex="2"
                                            style="text-align:right;"  onblur="javascript:Validar_Cantidad_Porcentaje_Indemnizacion();"
                                            onkeyup="this.value = (this.value.match(/^(?:\+|-)?\d+$/))? this.value : '';"/>
                                    </td>
                                    <td style="width:30%; text-align:left; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Neto_Bruto_Indemnizacion_OJ" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                            <asp:ListItem Value="NETO">NETO</asp:ListItem>
                                            <asp:ListItem Value="BRUTO">BRUTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width:100%;">
                            <hr />
                        </td>
                    </tr>
                  </table>
                  <table style="width:98%;">
                    <tr>
                        <td>
                         <asp:GridView ID="Grid_Tabulador_Orden_Judicial" runat="server" CssClass="GridView_1" Width="98%"
                             AutoGenerateColumns="False"  GridLines="None" 
                             OnSelectedIndexChanged="Grid_Tabulador_Orden_Judicial_SelectedIndexChanged"
                             AllowSorting="True" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"  HorizontalAlign="Left"/>
                                        <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ORDEN_JUDICIAL_ID" HeaderText="">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="BENEFICIARIO" HeaderText="Beneficiario">
                                        <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                        <ItemStyle HorizontalAlign="Left" Width="80%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView> 
                        </td>
                    </tr>
                  </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
 <asp:UpdatePanel ID="Upnal_Busqueda_Empleado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
            CancelControlID="Btn_Comodin_Close" DropShadow="True"  DynamicServicePath="" Enabled="True"/>  
        <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
        <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
    </ContentTemplate>
 </asp:UpdatePanel>
 
 <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;color:White;">
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        CssClass="button_autorizar" style="cursor: move; width:98%;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;text-align:left;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:return Cerrar_Modal_Popup();"/>  
                </td>
            </tr>
        </table>
    </asp:Panel>
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Indicador_Presidencia.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 

                                  <table width="100%">
                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           No Empleado 
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server"
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC"
                                                WatermarkCssClass="watermarked"/>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Estatus
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">   
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Rol
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Rol" runat="server" Width="100%" />
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" 
                                                AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Dependencia_SelectedIndexChanged"/>
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Contrato
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Tipo_Contrato" runat="server" Width="100%" />
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Area
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Areas" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Puesto
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Puesto" runat="server" Width="100%" />
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Escolariadad
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Escolaridad" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Sindicato
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Sindicato" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Turno
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Turno" runat="server" Width="100%" />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Zona
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Zona" runat="server" Width="100%" />
                                        </td>                                         
                                    </tr>
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Trabajador
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Tipo_Trabajador" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Nomina
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Tipo_Nomina" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fecha Inicio
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="14"/>
                                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                    ValidChars="/_"/>
                                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                                                    TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                                                    OnClientShown="calendarShown"/>
                                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                    ToolTip="Seleccione la Fecha"/> 
                                                <cc1:MaskedEditExtender 
                                                    ID="Mee_Txt_Busqueda_Fecha_Inicio" 
                                                    Mask="99/LLL/9999" 
                                                    runat="server"
                                                    MaskType="None" 
                                                    UserDateFormat="DayMonthYear" 
                                                    UserTimeFormat="None" Filtered="/"
                                                    TargetControlID="Txt_Busqueda_Fecha_Inicio" 
                                                    Enabled="True" 
                                                    ClearMaskOnLostFocus="false"/>  
                                                <cc1:MaskedEditValidator 
                                                    ID="Mev_Txt_Busqueda_Fecha_Inicio" 
                                                    runat="server" 
                                                    ControlToValidate="Txt_Busqueda_Fecha_Inicio"
                                                    ControlExtender="Mee_Txt_Busqueda_Fecha_Inicio" 
                                                    EmptyValueMessage="Es valido no ingresar fecha inicial"
                                                    InvalidValueMessage="Fecha Inicial Invalida" 
                                                    IsValidEmpty="true" 
                                                    TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fecha Fin
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="15"/>
                                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                    ValidChars="/_"/>
                                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                                                    TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                                                    OnClientShown="calendarShown"/>
                                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                    ToolTip="Seleccione la Fecha"/> 
                                                <cc1:MaskedEditExtender 
                                                    ID="Mee_Txt_Busqueda_Fecha_Fin" 
                                                    Mask="99/LLL/9999" 
                                                    runat="server"
                                                    MaskType="None" 
                                                    UserDateFormat="DayMonthYear" 
                                                    UserTimeFormat="None" Filtered="/"
                                                    TargetControlID="Txt_Busqueda_Fecha_Fin" 
                                                    Enabled="True" 
                                                    ClearMaskOnLostFocus="false"/>  
                                                <cc1:MaskedEditValidator 
                                                    ID="Mev_Mee_Txt_Busqueda_Fecha_Fin" 
                                                    runat="server" 
                                                    ControlToValidate="Txt_Busqueda_Fecha_Fin"
                                                    ControlExtender="Mee_Txt_Busqueda_Fecha_Fin" 
                                                    EmptyValueMessage="Es valido no ingresar fecha final"
                                                    InvalidValueMessage="Fecha Final Invalida" 
                                                    IsValidEmpty="true" 
                                                    TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                        
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button_autorizar"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="100%"/> 
                                            </center>
                                        </td>
                                    </tr>
                                  </table
                                  <table style="width:98%;">
                                    <tr>
                                        <td style="width:100%; text-align:left;">
                                            <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="True" CssClass="GridView_Nested"
                                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="100%"
                                                onpageindexchanging="Grid_Empleados_PageIndexChanging"  
                                                onselectedindexchanged="Grid_Empleados_SelectedIndexChanged"
                                                AllowSorting="True" OnSorting="Grid_Empleados_Sorting">
                                                
                                                 <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                 <PagerStyle CssClass="GridHeader_Nested" />
                                                 <HeaderStyle CssClass="GridHeader_Nested" />
                                                 <AlternatingRowStyle CssClass="GridAltItem_Nested" /> 
                                                 
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID" 
                                                        Visible="True" SortExpression="Empleado_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="No_Empleado" HeaderText="Número" 
                                                        Visible="True" SortExpression="No_Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus">
                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO" HeaderText="Nombre" 
                                                        Visible="True" SortExpression="Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                                        <ItemStyle HorizontalAlign="left" Width="75%" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                  </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
             </table>
           </div>
    </asp:Panel>
</asp:Content>

