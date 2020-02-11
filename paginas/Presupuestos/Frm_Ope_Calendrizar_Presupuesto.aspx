<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Calendrizar_Presupuesto.aspx.cs" Inherits="paginas_presupuestos_Frm_Ope_Calendrizar_Presupuesto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calendarización de Presupuesto</title>
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>
    <script type="text/javascript" >
        function Sumar(){
            var Total=parseFloat("0.00");
            if($('input[id$=Txt_Enero]').val() != "" && $('input[id$=Txt_Enero]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Enero]').val().replace(',',''));
            }
            if($('input[id$=Txt_Febrero]').val() != "" && $('input[id$=Txt_Febrero]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Febrero]').val().replace(',',''));
            }
             if($('input[id$=Txt_Marzo]').val() != "" && $('input[id$=Txt_Marzo]').val() != "NaN"){
                 Total = Total + parseFloat($('input[id$=Txt_Marzo]').val().replace(',',''));
            }
             if($('input[id$=Txt_Abril]').val() != "" && $('input[id$=Txt_Abril]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Abril]').val().replace(',',''));
            }
             if($('input[id$=Txt_Mayo]').val() != "" && $('input[id$=Txt_Mayo]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Mayo]').val().replace(',',''));
            }
             if($('input[id$=Txt_Junio]').val() != "" && $('input[id$=Txt_Junio]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Junio]').val().replace(',',''));
            }
             if($('input[id$=Txt_Julio]').val() != "" && $('input[id$=Txt_Julio]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Julio]').val().replace(',',''));
            }   
             if($('input[id$=Txt_Agosto]').val() != "" && $('input[id$=Txt_Agosto]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Agosto]').val().replace(',',''));
            }
             if($('input[id$=Txt_Septiembre]').val() != "" && $('input[id$=Txt_Septiembre]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Septiembre]').val().replace(',',''));
            }
             if($('input[id$=Txt_Octubre]').val() != "" && $('input[id$=Txt_Octubre]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Octubre]').val().replace(',',''));
            }
             if($('input[id$=Txt_Noviembre]').val() != "" && $('input[id$=Txt_Noviembre]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Noviembre]').val().replace(',',''));
            }
             if($('input[id$=Txt_Diciembre]').val() != "" && $('input[id$=Txt_Diciembre]').val() != "NaN"){
                Total = Total + parseFloat($('input[id$=Txt_Diciembre]').val().replace(',',''));
            }
            
            $('input[id$=Txt_Total]').val(Total);
            $('input[id$=Txt_Total]').formatCurrency({colorize:true, region: 'es-MX'});
        }
        
        function Calcular(Control){
            var Precio;
            var Cantidad;
            var Costo;
            if($("#Hf_Producto_ID").val() != ""){
                if($("#Hf_Precio").val() != ""){
                    if($('input[id$='+Control.id +']').val() != "" && $('input[id$='+Control.id +']').val() != "NaN"){
                         Precio = parseFloat($("#Hf_Precio").val());
                        Cantidad = parseFloat($('input[id$='+Control.id +']').val());
                        Costo = Cantidad * Precio;
                        $('input[id$=' + Control.id + ']').val(Costo);
                        $('[id$=Lbl_' + Control.id + ']').text(Cantidad);
                        $('[id$=Lbl_Cantidad]').text("Cantidad");
                    }
                }
            }
            Sumar();
        }
        
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            $("input[id$=Txt_Busqueda_Clave]").val("");
            $("input[id$=Txt_Busqueda_Nombre_Producto]").val("");
            $("[id$=Lbl_Numero_Registros]").text("");
            $('[id$=Lbl_Error_Busqueda]').text("");
            $('[id$=Lbl_Error_Busqueda]').css("display", "none");
            $('[id$=Img_Error_Busqueda]').hide();
            $("#grid").remove();
            return false;
        }  
        function Abrir_Modal_Popup() {
            $find('Busqueda_Productos').show();
            return false;
        }
        function Cerrar_Modal_Popup() {
            $find('Busqueda_Productos').hide();
            Limpiar_Ctlr();
            return false;
        } 
        
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
    <form id="Form" runat="server">
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
                                        Calendarización de Presupuesto
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
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                            CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                            ToolTip="Salir" OnClick="Btn_Salir_Click" />
                                    </td>
                                    <td colspan="2"> &nbsp; </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales" Width="99%">
                            <table style="width: 100%; text-align: center;">
                                <tr>
                                    <td colspan="13" style="text-align: left;">
                                       <asp:HiddenField id="Hf_Producto_ID" runat="server" />
                                       <asp:HiddenField id="Hf_Precio" runat="server" />
                                       <asp:HiddenField id="Hf_Programa" runat="server" />
                                       <asp:HiddenField id="Hf_Fte_Financiamiento" runat="server" />
                                       <asp:HiddenField id="Hf_Anio" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">* Unidad Responsable</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Style="width: 99%;" TabIndex="1"
                                           OnSelectedIndexChanged="Cmb_Unidad_Responsable_SelectedIndexChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Programa</td>
                                    <td colspan="8" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Programa" runat="server" Style="width: 99%;"></asp:TextBox>
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label ID="Lbl_Limite" runat="server" Text="Limite Presupuestal" Width=" 45%"></asp:Label>
                                        <asp:TextBox ID="Txt_Limite_Presupuestal" runat="server" Style="width: 48%; text-align:right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">* Capítulo</td>
                                    <td colspan="8" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Capitulos" runat="server" Style="width: 99%;" TabIndex="2"
                                        AutoPostBack="true" OnSelectedIndexChanged="Cmb_Capitulo_SelectedIndexChanged"/>
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" Width="45%"></asp:Label>
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Style="width: 50%; font-size:x-small;">
                                        
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">* Partida</td>
                                    <td colspan="8" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Style="width: 99%;" TabIndex="3"
                                        AutoPostBack="true" OnSelectedIndexChanged="Cmb_Partidas_SelectedIndexChanged" />
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
                                    <td colspan="10" style="text-align: left;">
                                        <asp:DropDownList ID="Cmb_Producto" runat="server" Style="width: 94%;" TabIndex="4"
                                        AutoPostBack="true" OnSelectedIndexChanged="Cmb_Productos_SelectedIndexChanged"/>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:ImageButton ID="Btn_Buscar_Producto" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                            ToolTip="Buscar Producto" TabIndex="5" OnClientClick="javascript:return Abrir_Modal_Popup();"/>
                                            <cc1:ModalPopupExtender ID="Mpe_Busqueda_Productos" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Productos"
                                                PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                                CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                                            <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">Justificación</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Justificacion" runat="server" Style="width: 98.5%; font-size: x-small;" TabIndex="6" MaxLength="2000"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Tr_Comentarios" runat="server">
                                    <td colspan="2" style="text-align: left;">Comentarios</td>
                                    <td colspan="11" style="text-align: left;">
                                        <asp:TextBox ID="Txt_Comentarios" runat="server" Style="width: 98.5%; font-size: x-small;" Enabled = "false" TextMode="MultiLine" ></asp:TextBox>
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
                                            font-size: x-small;" MaxLength="13" TabIndex="7" class="Cantidad"
                                            onClick="$('input[id$=Txt_Enero]').select();"
                                            onBlur="Calcular(this); $('input[id$=Txt_Enero]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                                         <cc1:FilteredTextBoxExtender ID="FTE_Txt_Enero" runat="server" TargetControlID="Txt_Enero" FilterType="Custom,Numbers"   
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Febrero" runat="server" Style="width:75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="8"
                                            onClick="$('input[id$=Txt_Febrero]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Febrero]').formatCurrency({colorize:true, region: 'es-MX'});" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Febrero" runat="server" TargetControlID="Txt_Febrero" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Marzo" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="9"
                                            onClick="$('input[id$=Txt_Marzo]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Marzo]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Marzo" runat="server" TargetControlID="Txt_Marzo" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Abril" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="10"
                                            onClick="$('input[id$=Txt_Abril]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Abril]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Abril" runat="server" TargetControlID="Txt_Abril" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Mayo" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="11"
                                            onClick="$('input[id$=Txt_Mayo]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Mayo]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Mayo" runat="server" TargetControlID="Txt_Mayo" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Junio" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="12"
                                            onClick="$('input[id$=Txt_Junio]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Junio]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Junio" runat="server" TargetControlID="Txt_Junio" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Julio" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="13"
                                            onClick="$('input[id$=Txt_Julio]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Julio]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Julio" runat="server" TargetControlID="Txt_Julio" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Agosto" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="14"
                                            onClick="$('input[id$=Txt_Agosto]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Agosto]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Agosto" runat="server" TargetControlID="Txt_Agosto" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Septiembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="15"
                                            onClick="$('input[id$=Txt_Septiembre]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Septiembre]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Septiembre" runat="server" TargetControlID="Txt_Septiembre" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Octubre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="16"
                                            onClick="$('input[id$=Txt_Octubre]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Octubre]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Octubre" runat="server" TargetControlID="Txt_Octubre" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Noviembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="17"
                                            onClick="$('input[id$=Txt_Noviembre]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Noviembre]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Noviembre" runat="server" TargetControlID="Txt_Noviembre" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Diciembre" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="18"
                                            onClick="$('input[id$=Txt_Diciembre]').select();"
                                            onblur="Calcular(this); $('input[id$=Txt_Diciembre]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                         <cc1:FilteredTextBoxExtender ID="FTE_Txt_Diciembre" runat="server" TargetControlID="Txt_Diciembre" FilterType="Custom,Numbers" 
                                          ValidChars=",."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Total" runat="server" Style="width: 75px; text-align: right;
                                            font-size: x-small;" MaxLength="13" TabIndex="19" Enabled="false" ></asp:TextBox>
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
                                    <td style="text-align: right;">
                                        <asp:ImageButton ID="Btn_Agregar" runat="server" ImageUrl="~/paginas/imagenes/gridview/add_grid.png"
                                                ToolTip="Agregar" OnClick="Btn_Agregar_Click" TabIndex="19"/>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </div>
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
                                                                        <ItemStyle Width="3%" />
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
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" 
                                                                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                                onclick="Btn_Eliminar_Click" CommandArgument='<%#Eval("ID")%>'
                                                                                OnClientClick="return confirm('¿Esta seguro que desea elimina el registro?');"/>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign ="Center" Font-Size="X-Small" Width="3%" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ID" />
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
                       </center>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger   ControlID="Grid_Productos"/>
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
                    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
                    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
                        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                        <table width="99%">
                            <tr>
                                <td style="color:Black;font-size:12;font-weight:bold;">
                                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                                     B&uacute;squeda: Productos
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
                                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Productos" runat="server">
                                            <ContentTemplate>
                                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Productos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Productos" DisplayAfter="0">
                                                    <ProgressTemplate>
                                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress> 
                                                                             
                                                  <table width="100%">
                                                   <tr>
                                                        <td colspan="2">
                                                            <table style="width:80%;">
                                                              <tr>
                                                                <td align="left">
                                                                  <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                                                    Width="24px" Height="24px" style="display:none" />
                                                                    <asp:Label ID="Lbl_Error_Busqueda" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" style="display:none"/>
                                                                </td>            
                                                              </tr>         
                                                            </table>  
                                                        </td>
                                                        <td style="width:100%" colspan="2" align="right">
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
                                                           Clave Producto
                                                        </td>              
                                                        <td style="width:30%;text-align:left;font-size:11px;">
                                                           <asp:TextBox ID="Txt_Busqueda_Clave" runat="server" Width="98%" />
                                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Clave" runat="server" 
                                                                TargetControlID ="Txt_Busqueda_Clave" WatermarkText="Busqueda por Clave" 
                                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                                        </td> 
                                                        <td style="width:20%;text-align:left;font-size:11px;">                                            
                                                        </td>              
                                                        <td style="width:30%;text-align:left;font-size:11px;">                                         
                                                        </td>                                         
                                                    </tr>                                                                                                   
                                                    <tr>
                                                        <td style="width:20%;text-align:left;font-size:11px;">
                                                            Nombre Producto
                                                        </td>              
                                                        <td style="width:30%;text-align:left;" colspan="3">
                                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Producto" runat="server" Width="99.5%" />
                                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Producto" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                                TargetControlID="Txt_Busqueda_Nombre_Producto" ValidChars="áéíóúÁÉÍÓÚ "/>
                                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Producto" runat="server" 
                                                                TargetControlID ="Txt_Busqueda_Nombre_Producto" WatermarkText="Busqueda por Nombre" 
                                                                WatermarkCssClass="watermarked"/>                                                                                               
                                                        </td>                                         
                                                    </tr>
                                                    <tr><td colspan="4">&nbsp;</td></tr>
                                                    <tr>
                                                        <td colspan="4" id="grid">
                                                             <div id="Div_Grid" style="width:100; max-height:300px; overflow:auto; border-style:outset;border-color: Silver;">
                                                                <asp:GridView ID="Grid_Productos" runat="server"  CssClass="GridView_1" 
                                                                    AutoGenerateColumns="False" GridLines="None" Width="99%"
                                                                     HeaderStyle-CssClass="tblHead"
                                                                     OnSelectedIndexChanged ="Grid_Productos_SelectedIndexChanged"
                                                                     EmptyDataText="No se encontraron productos">
                                                                    <Columns>
                                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                            <ItemStyle Width="4%" />
                                                                        </asp:ButtonField>
                                                                        <asp:BoundField DataField="Producto_ID">
                                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                                                            Visible="True" SortExpression="No_Empleado">
                                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CLAVE_PRODUCTO" HeaderText="Nombre Producto" 
                                                                            Visible="True" SortExpression="Empleado">
                                                                            <HeaderStyle HorizontalAlign="Left" Width="87%" />
                                                                            <ItemStyle HorizontalAlign="left" Width="87%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="COSTO">
                                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                                        </asp:BoundField>                                    
                                                                    </Columns>
                                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                                    <PagerStyle CssClass="GridHeader" />
                                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                                </asp:GridView>
                                                             </div>
                                                            <div style="text-align:center;">
                                                                <asp:Label ID="Lbl_Numero_Registros" runat="server" Text=""/>
                                                            </div>
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
                                                               <asp:Button ID="Btn_Busqueda_Productos" runat="server"  Text="Busqueda de Productos" CssClass="button"  
                                                                CausesValidation="false"  Width="200px" OnClick="Btn_Busqueda_Productos_Click"/> 
                                                            </center>
                                                        </td>                                                     
                                                    </tr>                                                                        
                                                  </table>                                                                                                                                                              
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Productos" EventName="Click"/>
                                            </Triggers>                                                                   
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
            </div>
        </div>
    </form>
</body>
</html>
