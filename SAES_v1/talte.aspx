﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="talte.aspx.cs" Inherits="SAES_v1.talte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>
         function save() {
             swal({
                 allowEscapeKey: false,
                 allowOutsideClick: false,
                 type: 'success',
                 html: '<h2 class="swal2-title" id="swal2-title">Se guardaron los datos exitosamente</h2>Favor de validar en el listado.'
             })
         }

         function update() {
             swal({
                 allowEscapeKey: false,
                 allowOutsideClick: false,
                 type: 'success',
                 html: '<h2 class="swal2-title" id="swal2-title">Se guardaron los datos exitosamente</h2>Favor de validar en el listado.'
             })
         }

         //-----  Función de agregar error  ------//
         function errorForm(idElementForm, textoAlerta) {
             let elemento = idElementForm;
             let textoInterno = textoAlerta;
             let elementoId = document.getElementById(elemento);
             elementoId.classList.add('error');
             elementoId.classList.remove('validado');
             elementoId.classList.remove('sinvalidar');
             errorId = 'error-' + String(elemento);
             let tieneError = document.getElementById(errorId);
             if (!tieneError) {
                 const parrafo = document.createElement("p");
                 const contParrafo = document.createTextNode(textoInterno);
                 parrafo.appendChild(contParrafo);
                 parrafo.classList.add('textoError');
                 parrafo.id = 'error-' + String(elemento)
                 //Depende de estructura de HTML
                 elementoId.parentNode.appendChild(parrafo);
             }
         }

         //----- Función de remover error ------//
         function validadoForm(idElementForm) {
             let elemento = idElementForm;
             let elementoId = document.getElementById(elemento);
             elementoId.classList.remove('error');
             elementoId.classList.add('validado');
             elementoId.classList.remove('sinvalidar');
             errorId = 'error-' + String(elemento);
             let tieneError = document.getElementById(errorId);
             if (tieneError) {
                 tieneError.remove();
             }
         }

         //---- Matricula ----//
         function validarMatricula(idEl) {
             const idElemento = idEl;
             let nombre = document.getElementById(idElemento).value;
             if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                 errorForm(idElemento, 'Favor de ingresar una matrícula valida');
                 return false;
             } else {
                 validadoForm(idElemento);
             }
         }

         //---- Lada ----//
         function validarlada(idEl) {
             const idElemento = idEl;
             let nombre = document.getElementById(idElemento).value;
             if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                 errorForm(idElemento, 'Favor de ingresar una clave lada valida');
                 return false;
             } else {
                 validadoForm(idElemento);
             }
         }

         //---- Telefono ----//
         function validarTelefono(idEl) {
             const idElemento = idEl;
             let nombre = document.getElementById(idElemento).value;
             if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                 errorForm(idElemento, 'Favor de ingresar un teléfono valido');
                 return false;
             } else {
                 validadoForm(idElemento);
             }
         }

         //----Validación de Tipo Direccion----//
         function validar_t_telefono(idEl) {
             const idElemento = idEl;
             valor = $("#" + idElemento).val();

             if (valor == 0) {
                 errorForm(idElemento, 'Debes seleccionar un tipo de teléfono válido');
                 return false;
             } else {
                 validadoForm(idElemento);
             }
         }

         //----Validación de Estatus----//
         function validar_estatus(idEl) {
             const idElemento = idEl;
             valor = $("#" + idElemento).val();

             if (valor == 0) {
                 errorForm(idElemento, 'Debes seleccionar un estatus válido');
                 return false;
             } else {
                 validadoForm(idElemento);
             }
         }

         //---- Valida Campos Solicitud ----//
         function validar_campos_telefono(e) {
             event.preventDefault(e);
             validarMatricula('ContentPlaceHolder1_txt_matricula');
             validarlada('ContentPlaceHolder1_txt_lada');
             validarTelefono('ContentPlaceHolder1_txt_telefono');            
             validar_t_telefono('ContentPlaceHolder1_ddl_tipo_telefono');
             validar_estatus('ContentPlaceHolder1_ddl_estatus');
             return false;
         }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="x_title">
        <h2>
            <img src="Images/Admisiones/telefono.png" style="width: 30px;" /><small>Teléfono</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end">
            <li class="nav-item">
                <a class="nav-link " href="tpers.aspx">Datos Generales</a>
            </li>
            <li class="nav-item">
                <a class="nav-link " href="taldi.aspx">Dirección</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="talte.aspx">Teléfono</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talco.aspx">Correo</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tadmi.aspx">Solicitud</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tredo.aspx">Documentos</a>
            </li>
        </ul>
        <asp:UpdatePanel ID="upd_Campus" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_talte" runat="server">
                    <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                        <div class="col-md-2">
                            <label for="ContentPlaceHolder1_txt_matricula" class="form-label">Matrícula</label>
                            <asp:TextBox ID="txt_matricula"  runat="server" CssClass="form-control" OnTextChanged="txt_matricula_TextChanged" AutoPostBack="true"></asp:TextBox><!--Configurar BackEnd la longitud de la BD-->
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_txt_nombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txt_nombre"  runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_tipo_telefono" class="form-label">Tipo de Teléfono</label>
                            <asp:DropDownList ID="ddl_tipo_telefono" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="w-100"></div>
                        <div class="col-md-2">
                            <label for="ContentPlaceHolder1_txt_lada" class="form-label">Lada</label>
                            <asp:TextBox ID="txt_lada" MaxLength="2" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_telefono" class="form-label">Teléfono</label>
                            <asp:TextBox ID="txt_telefono" MaxLength="8" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label for="ContentPlaceHolder1_txt_extension" class="form-label">Extensión</label>
                            <asp:TextBox ID="txt_extension" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_estatus" class="form-label">Estatus</label>
                            <asp:DropDownList ID="ddl_estatus" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>  
                        <asp:Label ID="lbl_id_pers" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_consecutivo" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_talte" runat="server">
                    <div class="col-md-4" style="text-align: center; margin-top: 15px;">
                        <asp:Button ID="btn_cancel" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" OnClick="btn_cancel_Click"/>
                        <asp:Button ID="btn_save" runat="server" CssClass="btn btn-round btn-success" Text="Agregar" OnClick="btn_save_Click"/>
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" OnClick="btn_update_Click"/>
                    </div>
                </div>
                <div id="table_talte">
                    <asp:GridView ID="GridTelefono" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small" OnSelectedIndexChanged="GridTelefono_SelectedIndexChanged">
                        <Columns>
                            <asp:ButtonField ButtonType="image" ImageUrl="~/Images/Generales/hacer-clic.png" ControlStyle-Height="24px" ControlStyle-Width="24px" CommandName="select" HeaderText="Seleccionar" ItemStyle-CssClass="button_select" />
                            <asp:BoundField DataField="ID_NUM" HeaderText="Id_Num">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE" HeaderText="Matrícula" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" />
                             <asp:BoundField DataField="TIPO_TEL" HeaderText="T_telefono">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Tipo Teléfono" />
                            <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LADA" HeaderText="Lada" />
                            <asp:BoundField DataField="TELEFONO" HeaderText="Teléfono" />
                            <asp:BoundField DataField="EXTENSION" HeaderText="Extensión" />
                            <asp:BoundField DataField="C_ESTATUS" HeaderText="Estatus_code">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha Registro" />
                        </Columns>
                        <SelectedRowStyle CssClass="selected_table" />
                        <HeaderStyle BackColor="#2a3f54" ForeColor="white" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <script>
        function load_datatable() {
            let table_solicitudes = $("#ContentPlaceHolder1_GridTelefono").DataTable({
                language: {
                    bProcessing: 'Procesando...',
                    sLengthMenu: 'Mostrar _MENU_ registros',
                    sZeroRecords: 'No se encontraron resultados',
                    sEmptyTable: 'Ningún dato disponible en esta tabla',
                    sInfo: 'Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros',
                    sInfoEmpty: 'Mostrando registros del 0 al 0 de un total de 0 registros',
                    sInfoFiltered: '(filtrado de un total de _MAX_ registros)',
                    sInfoPostFix: '',
                    sSearch: 'Buscar:',
                    sUrl: '',
                    sInfoThousands: '',
                    sLoadingRecords: 'Cargando...',
                    oPaginate: {
                        sFirst: 'Primero',
                        sLast: 'Último',
                        sNext: 'Siguiente',
                        sPrevious: 'Anterior'
                    }
                },
                scrollResize: true,
                scrollY: '500px',
                scrollCollapse: true,
                order: [
                    [0, "asc"]
                ],
                lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
                "autoWidth": true,
                dom: '<"top"if>rt<"bottom"lBp><"clear">',
                buttons: [
                    {
                        title: 'SAES_Catálogo de Campus',
                        className: 'btn-dark',
                        extend: 'excel',
                        text: 'Exportar Excel',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 7, 8]
                        }
                    },
                    {
                        title: 'SAES_Catálogo de Campus',
                        className: 'btn-dark',
                        extend: 'pdfHtml5',
                        text: 'Exportar PDF',
                        orientation: 'landscape',
                        pageSize: 'LEGAL',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 7, 8]
                        }
                    }
                ],
                stateSave: true
            });
        }

        function remove_class() {
            $('.selected_table').removeClass("selected_table")
        }
        </script>
</asp:Content>
