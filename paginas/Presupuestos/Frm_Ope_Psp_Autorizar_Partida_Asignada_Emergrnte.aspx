<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Psp_Autorizar_Partida_Asignada_Emergrnte.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Psp_Autorizar_Partida_Asignada_Emergrnte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Autorización Partida Asignada</title>
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" >
        function Grid_Anidado(Control, Fila)
        {
            var div = document.getElementById(Control); 
            var img = document.getElementById('img' + Control);
            
            if (div.style.display == "none") 
            {
                div.style.display = "inline";
                if (Fila == 'alt') {
                    img.src = "../imagenes/paginas/stocks_indicator_down.png";
                }
                else {
                    img.src = "../imagenes/paginas/stocks_indicator_down.png";
                }
                img.alt = "Contraer Registros";
            }
            else 
            {
                div.style.display = "none";
                if (Fila == 'alt') {
                    img.src = "../imagenes/paginas/add_up.png";
                }
                else {
                    img.src = "../imagenes/paginas/add_up.png";
                }
                img.alt = "Expandir Registros";
            }
        }
    </script>
</head>
<body>
    <form id="Frm_Ope_Psp_Autorizar_Partida_Asignada_Emergente" runat="server">
        <div style="min-height:580px; max-height:800px; width:99%;vertical-align:top;border-style:outset;border-color: Silver; background-color:White;">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
                EnableScriptLocalization="true">
            </asp:ScriptManager>    
            <div>
                <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
                    <ContentTemplate>
                         <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                            DisplayAfter="0">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                                </div>
                                <div class="processMessage" id="div_progress">
                                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                                </div>
                            </ProgressTemplate>                    
                        </asp:UpdateProgress>
                        <center>
                          <div id="Div_Encabezado" runat="server">
                            <table style="width:99%;" border="0" cellspacing="0">
                                <tr align="center">
                                    <td colspan="4" class="label_titulo">
                                        Autorización de Partida Asignada
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td colspan="4">
                                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                            Visible="false" />
                                        <asp:Label ID="Lbl_Encanezado_Error" runat="server" Text="Favor de:" ForeColor="#990000" Visible="false"></asp:Label><br />
                                        <asp:Label ID="Lbl_Error" runat="server" ForeColor="#990000" Visible="false" ></asp:Label>
                                    </td>
                                </tr>
                                <tr class="barra_busqueda" align="right">
                                    <td align="left" valign="middle" colspan="2">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                            CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                            ToolTip="Salir" OnClick="Btn_Salir_Click" />
                                    </td>
                                    <td colspan="2"> &nbsp; </td>
                                </tr>
                            </table>
                        </div>
                         <div id="Div1" style="clear:both;">&nbsp;</div>
                          <div id="Div_Dependencias_Presupuestadas" runat="server">
                           <asp:Panel ID="Pnl_Dependencias" runat="server" GroupingText="Unidades Responsables Presupuestadas" Width="99%">
                            <asp:GridView ID="Grid_Dependencias_Presupuestadas" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" Width="90%"
                                GridLines= "None" EmptyDataText="No se encontraron Unidades Responsables con presupuestos"
                                onselectedindexchanged="Grid_Dependencias_Presupuestadas_SelectedIndexChanged">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="DEPENDENCIA_ID"/>
                                    <asp:BoundField DataField="ESTATUS"/>
                                    <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia">
                                        <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" Width="50%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO">
                                         <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL" HeaderText="Total Presupuestado" DataFormatString="{0:c}" >
                                         <HeaderStyle HorizontalAlign="Right"/>
                                        <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />       
                            </asp:GridView>
                           </asp:Panel>
                        </div>
                          <div id="Div_Partidas_Asignadas" runat="server">
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales" Width="99%">
                            <table style="width: 100%; text-align: center;">
                                <tr>
                                    <td colspan="13" style="text-align: left;">
                                       <asp:HiddenField id="Hf_Producto_ID" runat="server" />
                                       <asp:HiddenField id="Hf_Precio" runat="server" />
                                       <asp:HiddenField id="Hf_Programa" runat="server" />
                                       <asp:HiddenField id="Hf_Fte_Financiamiento" runat="server" />
                                       <asp:HiddenField id="Hf_Anio" runat="server" />
                                       <asp:HiddenField id="Hf_Dependencia_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Unidad Responsable</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" Style="width: 99%;" TabIndex="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Programa</td>
                                    <td colspan="5" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" Style="width: 99%;" TabIndex="3"/>
                                    </td>
                                     <td colspan="2" style="text-align: left;">Fuente Financiamiento</td>
                                    <td colspan="4" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Style="width: 97%;" TabIndex="2"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Capítulo</td>
                                    <td colspan="8" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Capitulos" runat="server" Style="width: 99%;" TabIndex="2"/>
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" Width="45%"></asp:Label>
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Style="width: 50%; font-size:x-small;"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Partida</td>
                                    <td colspan="8" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Style="width: 99%;" TabIndex="3" />
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label ID="Lbl_Stock" runat="server" Text="Stock" Width=" 45%"></asp:Label>
                                        <asp:DropDownList ID="Cmb_Stock" runat="server" Style="width: 50%;" Enabled="false" >
                                            <asp:ListItem Value="SI">SI</asp:ListItem>
                                            <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr  id="Tr_Productos" runat="server">
                                    <td colspan="2" style="text-align: left;">Producto</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Producto" runat="server" Style="width: 99%;" TabIndex="4"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Justificación</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Justificacion" runat="server" Style="width: 98.5%; font-size: x-small;" TabIndex="6" MaxLength="2000"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Comentarios</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Comentarios" runat="server" Style="width: 98.5%; font-size: x-small;" TextMode="MultiLine" 
                                         MaxLength="2000"></asp:TextBox> <%--Onkeyup="javascript:Validar_Caracteres();"--%>
                                    </td>
                                </tr>
                                <tr >
                                    <td colspan="13" style="text-align: left;">
                                        <asp:Label runat="server" ID="Lbl_Coment" Style="color:Red; text-align:left;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Enero</td>
                                    <td>Febrero</td>
                                    <td>Marzo</td>
                                    <td>Abril</td>
                                    <td>Mayo</td>
                                    <td>Junio</td>
                                    <td>Julio</td>
                                    <td>Agosto</td>
                                    <td>Septiembre</td>
                                    <td>Octubre</td>
                                    <td>Noviembre</td>
                                    <td>Diciembre</td>
                                    <td>Total</td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <asp:TextBox ID="Txt_Enero" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="7" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Febrero" runat="server" Style="width:75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="8" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Marzo" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Abril" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Mayo" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="11"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Junio" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="12"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Julio" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="13"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Agosto" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="14"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Septiembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="15"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Octubre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="16"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Noviembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="17"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Diciembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="18"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Total" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="19" ReadOnly="true" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="text-align:right">
                                    <td>
                                       <asp:Label ID="Lbl_Cantidad" runat="server" style="width: 20px;text-align:left; font-size:xx-small"></asp:Label>&nbsp;
                                       <asp:Label ID="Lbl_Txt_Enero" runat="server" style="width: 55px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Febrero" runat="server" style="width:105px;  font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Marzo" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Abril" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Mayo" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Junio" runat="server" style="width:75px;font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Julio" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Agosto" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Septiembre" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Octubre" runat="server" style="width: 75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Noviembre" runat="server" style="width:75px;  font-size:x-small"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:Label ID="Lbl_Txt_Diciembre" runat="server" style="width:75px; font-size:x-small"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <table style="width:100%;">
                            <tr>
                                <td class="barra_busqueda" style="height:1px;">                            
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Pnl_Partidas_Asignadas" runat="server" GroupingText="Partidas Asignadas" Width="99%">
                            <div style="width:100%; height:auto; max-height: 500px; overflow:auto; vertical-align:top;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:100%;">
                                          <asp:GridView ID="Grid_Partida_Asignada" runat="server" style="white-space:normal;"
                                            AutoGenerateColumns="False" GridLines="None" 
                                            Width="100%"  EmptyDataText="No se encontraron partidas aignadas" 
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                            DataKeyNames="PARTIDA_ID"   OnRowDataBound="Grid_Partida_Asignada_RowDataBound"
                                            OnRowCreated="Grid_Partidas_Asignadas_Detalle_RowCreated">
                                            <Columns>
                                                <asp:TemplateField> 
                                                  <ItemTemplate> 
                                                        <a href="javascript:Grid_Anidado('div<%# Eval("PARTIDA_ID") %>', 'one');"> 
                                                             <img id="imgdiv<%# Eval("PARTIDA_ID") %>" alt="Click expander/contraer registros" border="0" src="../imagenes/paginas/add_up.png" /> 
                                                        </a> 
                                                  </ItemTemplate> 
                                                  <AlternatingItemTemplate> 
                                                       <a href="javascript:Grid_Anidado('div<%# Eval("PARTIDA_ID") %>', 'alt');"> 
                                                            <img id="imgdiv<%# Eval("PARTIDA_ID") %>" alt="Click expander/contraer registros" border="0" src="../imagenes/paginas/add_up.png" /> 
                                                       </a> 
                                                  </AlternatingItemTemplate> 
                                                  <ItemStyle HorizontalAlign ="Center" Font-Size="X-Small" Width="3%" />
                                                </asp:TemplateField>      
                                                <asp:BoundField DataField="PARTIDA_ID" />
                                                <asp:BoundField DataField="CLAVE" HeaderText="Partida" >
                                                    <HeaderStyle HorizontalAlign="Left"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="6%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_ENE" HeaderText="Enero" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_FEB" HeaderText="Febrero" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_MAR" HeaderText="Marzo" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_ABR" HeaderText="Abril" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_MAY" HeaderText="Mayo" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_JUN" HeaderText="Junio" DataFormatString="{0:#,###,##0.00}">
                                                   <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_JUL" HeaderText="Julio" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_AGO" HeaderText="Agosto" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_SEP" HeaderText="Septiembre" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_OCT" HeaderText="Octubre" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>                 
                                                <asp:BoundField DataField="TOTAL_NOV" HeaderText="Noviembre" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>                   
                                                <asp:BoundField DataField="TOTAL_DIC" HeaderText="Diciembre" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                </asp:BoundField>                
                                                <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:#,###,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center"/>
                                                    <ItemStyle HorizontalAlign="Right"  Width="8%" Font-Size="XX-Small"/>
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                   <ItemTemplate>
                                                     </td>
                                                     </tr> 
                                                     <tr>
                                                      <td colspan="100%">
                                                       <div id="div<%# Eval("PARTIDA_ID") %>" style="display:none;position:relative;left:20px;" >                                                     
                                                           <asp:GridView ID="Grid_Partidas_Asignadas_Detalle" runat="server" style="white-space:normal;"
                                                               CssClass="GridView_Nested" HeaderStyle-CssClass="tblHead"
                                                               AutoGenerateColumns="false" GridLines="None" Width="98%"
                                                               OnSelectedIndexChanged="Grid_Partidas_Asignadas_Detalle_SelectedIndexChanged"
                                                               OnRowCreated="Grid_Partidas_Asignadas_Detalle_RowCreated">
                                                               <Columns>
                                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                        ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:ButtonField> 
                                                                    <asp:BoundField DataField="DEPENDENCIA_ID"  />
                                                                    <asp:BoundField DataField="PROYECTO_ID"  />
                                                                    <asp:BoundField DataField="CAPITULO_ID"  />
                                                                    <asp:BoundField DataField="PARTIDA_ID"  />
                                                                    <asp:BoundField DataField="PRODUCTO_ID"  />
                                                                    <asp:BoundField DataField="PRECIO"  />
                                                                    <asp:BoundField DataField="JUSTIFICACION"  />
                                                                    <asp:BoundField DataField="CLAVE_PARTIDA" HeaderText="Partida" >
                                                                        <HeaderStyle HorizontalAlign="Left" Font-Size="XX-Small"/>
                                                                        <ItemStyle HorizontalAlign="Left" Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CLAVE_PRODUCTO" HeaderText="Clave Producto" >
                                                                        <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>                                                                          
                                                                    <asp:BoundField DataField="ENERO" HeaderText="Enero" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FEBRERO" HeaderText="Febrero" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MARZO" HeaderText="Marzo" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ABRIL" HeaderText="Abril" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MAYO" HeaderText="Mayo" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="JUNIO" HeaderText="Junio" DataFormatString="{0:#,###,##0.00}">
                                                                       <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="JULIO" HeaderText="Julio" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AGOSTO" HeaderText="Agosto" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SEPTIEMBRE" HeaderText="Septiembre" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OCTUBRE" HeaderText="Octubre" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="6%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>                 
                                                                    <asp:BoundField DataField="NOVIEMBRE" HeaderText="Noviembre" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>                   
                                                                    <asp:BoundField DataField="DICIEMBRE" HeaderText="Diciembre" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="7%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>                
                                                                    <asp:BoundField DataField="IMPORTE_TOTAL" HeaderText="Total" DataFormatString="{0:#,###,##0.00}">
                                                                        <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                        <ItemStyle HorizontalAlign="Right"  Width="8%" Font-Size="XX-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ID" />
                                                                    <asp:BoundField DataField="FUENTE_FINANCIAMIENTO_ID" />
                                                               </Columns>
                                                               <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                               <PagerStyle CssClass="GridHeader_Nested" />
                                                               <HeaderStyle CssClass="GridHeader_Nested" />
                                                               <AlternatingRowStyle CssClass="GridAltItem_Nested" /> 
                                                           </asp:GridView>
                                                       </div>
                                                      </td>
                                                     </tr>
                                                   </ItemTemplate>
                                                </asp:TemplateField>
                                             </Columns>
                                             <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                          </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right;">
                                            <asp:Label ID="Lbl_Total_Ajuste" runat="server" Text="Total" style="font-size:small; font-weight:bold;"></asp:Label>
                                            &nbsp;<asp:TextBox ID="Txt_Total_Ajuste" runat="server" style="text-align:right; border-color:Navy;" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        </div>
                       </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
