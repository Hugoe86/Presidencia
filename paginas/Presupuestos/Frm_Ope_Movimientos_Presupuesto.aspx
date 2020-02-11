<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Movimientos_Presupuesto.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Movimientos_Presupuesto" Title="Movimientos de Presupuesto" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style1
        {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div id="div_progress" class="processMessage" >
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
             
                
             <div id="Div_Fuentes_Financiamiento" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                           Movimiento de Presupuesto
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="100%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div  align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Inactivar" CssClass="Img_Button" TabIndex="3"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('Desea eliminar este Elemento. ¿Confirma que desea proceder?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                
                                            </table>
                                           </td>
                                         </tr>
                                      </table>
                                    </div>
                             </td>
                         </tr>
            </table> 
            
             <div id="Div_Grid_Movimientos" runat="server" style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;">
                    <table width="100%">
                       <tr>
                            <td style="width: 15%;" align="left"> 
                                <asp:Label ID="Lbl_Unidad_Responsable" runat="server" Text="Unidad Responsable"   Width="100%"></asp:Label>
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="95%"  >
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%;"></td>
                            <td style="width: 35%;"></td>
                            
                         <%---  <td style="width: 15%;">
                                <asp:Label ID="Lbl_Buscar_Estatus" runat="server" Text="Buscar por Estatus"   Width="100%"></asp:Label>
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Buscar_Estatus" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Buscar_Estatus_SelectedIndexChanged" >
                                    <asp:ListItem>&lt; SELECCIONE ESTATUS &gt;</asp:ListItem>
                                    <asp:ListItem>TODOS</asp:ListItem>
                                    <asp:ListItem>AUTORIZADA</asp:ListItem>
                                    <asp:ListItem>RECHAZADA</asp:ListItem>
                                    <asp:ListItem>GENERADA</asp:ListItem>
                                    <asp:ListItem>CANCELADA</asp:ListItem>
                                </asp:DropDownList>
                            </td>---%>  
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha"   Width="100%"></asp:Label>
                            </td>
                             <td >
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                            </td>
                            
                               
                            <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                            <td style="width:55%;">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Nombre" Width="180px"/>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                onclick="Btn_Buscar_Click" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Folio>" TargetControlID="Txt_Busqueda" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                    runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                            </td>
                            
                               
                           
    
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Movimiento" runat="server"  CssClass="GridView_1" Width="100%" 
                                        AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" 
                                        AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                        EmptyDataText="No se encuentra ningun Movimiento"
                                        OnSelectedIndexChanged="Grid_Movimiento_SelectedIndexChanged"
                                        OnSorting="Grid_Movimiento_Sorting">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="7%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="NO_SOLICITUD" HeaderText="Solicitud" Visible="True" SortExpression="NO_SOLICITUD">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CODIGO1" HeaderText="Partida Origen" Visible="True" SortExpression="CODIGO1">
                                                <HeaderStyle HorizontalAlign="Left" Width="26%" />
                                                <ItemStyle HorizontalAlign="Left" Width="26%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CODIGO2" HeaderText="Partida Destino" Visible="True" SortExpression="CODIGO2">
                                                <HeaderStyle HorizontalAlign="Left" Width="26%" />
                                                <ItemStyle HorizontalAlign="Left" Width="26%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IMPORTE" HeaderText="Importe" Visible="True" SortExpression="IMPORTE" DataFormatString="{0:n}">
                                                <HeaderStyle HorizontalAlign="Right" Width="14%" />
                                                <ItemStyle HorizontalAlign="Right" Width="14%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS" >
                                                <HeaderStyle HorizontalAlign="Center" Width="17%" />
                                                <ItemStyle HorizontalAlign="Center" Width="17%" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                        </tr>
                    </table >
                </div>
            
            <div id="Div_Datos_Generales" runat="server" style="overflow:auto;height:100px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;" >
                <table id="Table_Datos_Genrerales" width="100%"   border="0" cellspacing="0" class="estilo_fuente">
                        
                        <tr>
                            <td width="20%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="5%"> </td>
                        </tr>        
                        <tr>
                            <td colspan="7"></td>
                        </tr>
                        <tr>
                            <td colspan="7" align="left"> 
                                <asp:Label ID="Lbl_Datos_Generales" runat="server" Text="Datos Generales" Width="98%"  ></asp:Label>
                            </td>
                        </tr>
                        
                         <tr>
                            <td colspan="7">
                            </td>
                            
                        </tr>
                        <tr>
                            <td > 
                                <asp:Label ID="Lbl_Operacion" runat="server" Text="Operación" Width="98%"  ></asp:Label>
                            </td>
                            
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Operacion" runat="server" Width="95%" >
                                             <asp:ListItem>&lt; SELECCIONE OPERACION &gt;</asp:ListItem>
                                    <asp:ListItem>TRASPASO</asp:ListItem>
                                    <asp:ListItem>AMPLIAR</asp:ListItem>
                                    <asp:ListItem>REDUCIR</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                   
                        <tr>
                            <td>
                                 <asp:Label ID="Lbl_Importe" runat="server" Text="Importe" Width="98%"  ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Importe" runat="server"  AutoPostBack="true" OnTextChanged="Txt_Importe_OnTextChanged"  Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Importe_FilteredTextBoxExtender1" 
                                    runat="server" FilterType="Custom ,Numbers" 
                                    ValidChars=","
                                    TargetControlID="Txt_Importe" Enabled="True" >
                                </cc1:FilteredTextBoxExtender>
                            </td>
                             <td align="right">
                                 <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus " Width="98%"  ></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="95%" >
                                    <asp:ListItem>&lt; SELECCIONE ESTATUS&gt;</asp:ListItem>
                                    <asp:ListItem>GENERADA</asp:ListItem>
                                    <asp:ListItem>RECHAZADA</asp:ListItem>
                                 
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr> 
                            <td>
                                <asp:Label ID="Lbl_Numero_solicitud" runat="server" Text="Nùmero de solicitud" Width="98%"  ></asp:Label>
                            </td>
                             <td>
                                <asp:TextBox ID="Txt_No_Solicitud" runat="server" ReadOnly="True"  Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="7"></td>
                        </tr>
                        
                         
                    </table>
                </div>
                 
                <table>
                    <tr>
                        <td></td>
                    </tr>
                </table>
                <%---   ---%>
                <div ID="Div_Partida_Origen" runat="server"  style="overflow:auto;height:170px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;" >
                     <table width="100%"   border="0" cellspacing="0" class="estilo_fuente" border=1>
                                
                        <tr>
                            <td width="20%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="5%"> </td>
                        </tr>        
                        <tr>
                            <td colspan="7" align="center"> 
                                <asp:Label ID="Lbl_Partida_Origen_Encabezado" runat="server" Text="Partida Origen"   Width="100%"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="7">
                            </td>
                            
                        </tr>
                        <tr>
                            <td> 
                                <asp:Label ID="Lbl_Codigo_Origen" runat="server" Text="Codigo Programatico"   Width="95%"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Codigo1" runat="server" ReadOnly="true"
                                         Width="94%"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td > 
                                <asp:Label ID="lbl_Unidad_Responsable_Origen" runat="server" Text="Unidad Responsable" Width="98%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable_Origen" runat="server" Width="95%" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Fuente_Financiamiento_Origen" runat="server" Text="Fuente de Financiamiento"   Width="100%"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Fuente_Financiamiento_Origen" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Fuente_Financiamiento_Origen_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Programa_Origen" runat="server" Text="Programa" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Programa_Origen" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Programa_Origen_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td> 
                                <asp:Label ID="Lbl_Capitulo_Origen" runat="server" Text="Capitulo" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Capitulo_Origen" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Capitulo_Origen_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Partida_Origen" runat="server" Text="Paritda" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Partida_Origen" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Partida_Origen_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Area_Origen" runat="server"  Visible="false"
                                                ReadOnly="True"  Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                        
                        
                        <tr>
                           <td colspan="4"></td> 
                        </tr>
                        
                        <tr>
                    </table>
                </div>   
                <%---   ---%>
                <table>
                    <tr>
                        <td></td>
                    </tr>
                </table>
                <div id="Div_Partida_Destino" runat="server" style="height:230px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;" >
                    <table width="100%"   border="0" cellspacing="0" class="estilo_fuente">
                                
                        <tr>
                            <td width="20%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="15%"> </td>
                            <td width="5%"> </td>
                        </tr>        
                           <td colspan="7"></td> 
                        </tr>
                        
                        <tr>
                            <td colspan="7" align="center"> 
                                <asp:Label ID="Lbl_Partida_Destino_Encabezado" runat="server" Text="Partida Destino"   Width="100%"></asp:Label>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td colspan="7">
                            </td>
                            
                        </tr>
                        <tr>
                            <td> 
                                <asp:Label ID="Lbl_Codigo_Pragramatico_Destino" runat="server" Text="Código Programatico"   Width="100%"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Codigo2" runat="server" ReadOnly="true"
                                         Width="94%"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td > 
                                <asp:Label ID="Lbl_Unidad_Responsable_Destino" runat="server" Text="Unidad Responsable" Width="98%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable_Destino" runat="server" Width="95%" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Fuente_Financiamiento_Destino" runat="server" Text="Fuente de Financiamiento"   Width="100%"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Fuente_Financiamiento_Destino" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Fuente_Financiamiento_Destino_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Programa_Destino" runat="server" Text="Programa" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Programa_Destino" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Programa_Destino_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td> 
                                <asp:Label ID="Lbl_Capitulo_Destino" runat="server" Text="Capitulo" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Capitulo_Destino" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Capitulo_Destino_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                         <tr>
                            <td> 
                                <asp:Label ID="Lbl_Partida_Destino" runat="server" Text="Paritda" Width="100%"  ></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Partida_Destino" runat="server" Width="95%" AutoPostBack="True" 
                                 OnSelectedIndexChanged="Cmb_Partida_Destino_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Area_Destino" runat="server"  Visible="false"
                                                ReadOnly="True"  Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="7">
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td> 
                               <asp:Label ID="Lbl_Justificacion" runat="server" Text="Justificacion" Width="95%"  ></asp:Label>
                            </td>
                            <td colspan="6" >
                                 <asp:TextBox ID="Txt_Justificacion" runat="server" 
                                    TextMode="MultiLine" Width="95%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Justificacion" 
                                     runat="server" WatermarkCssClass="watermarked"
                                    TargetControlID ="Txt_Justificacion" 
                                     WatermarkText="Límite de Caractes 250" Enabled="True">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Justificacion" runat="server" 
                                    TargetControlID="Txt_Justificacion" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                </cc1:FilteredTextBoxExtender> 
                            </td>
                        </tr>
                </table>  
            </div> 
            <%---   ---%>
            
              <div id="Div_Grid_Comentarios" runat="server" style="overflow:auto;height:100px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;">
                    <table width="100%">
                       <tr width="100%">
                            <asp:GridView ID="Grid_Comentarios" runat="server"  CssClass="GridView_1" Width="100%" 
                                AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" 
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                EmptyDataText="No se encuentra ningun comentario">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    
                                    <asp:BoundField DataField="Comentario" HeaderText="Comentario" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Right" Width="25%" />
                                    </asp:BoundField>
                                   
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                       </tr>
                    </table>
                </div>
         </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>