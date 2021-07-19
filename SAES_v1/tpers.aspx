﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tpers.aspx.cs" Inherits="SAES_v1.tpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/gijgo@1.9.6/js/gijgo.min.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/gijgo@1.9.6/js/messages/messages.es-es.js" type="text/javascript"></script>
    <link href="https://unpkg.com/gijgo@1.9.13/css/gijgo.min.css" rel="stylesheet" type="text/css" />
    <style>
        span button {
            margin-bottom: 0px !important;
        }
    </style>
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

        //---- Nombre Alumno ----//
        function validarNombre(idEl) {
            const idElemento = idEl;
            let nombre = document.getElementById(idElemento).value;
            if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                errorForm(idElemento, 'Favor de ingresar un nombre valido');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        //---- Apellido Paterno Alumno ----//
        function validarApellidoP(idEl) {
            const idElemento = idEl;
            let nombre = document.getElementById(idElemento).value;
            if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                errorForm(idElemento, 'Favor de ingresar un apellido paterno valido');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        //---- Apellido Materno Alumno ----//
        function validarApellidoM(idEl) {
            const idElemento = idEl;
            let nombre = document.getElementById(idElemento).value;
            if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
                errorForm(idElemento, 'Favor de ingresar un apellido materno valido');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        //---- CURP ----//
        function validarCurp(idEl) {
            const idElemento = idEl;
            let valor = document.getElementById(idElemento).value;
            if (valor == null || valor.length == 0 || !(/^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$/.test(valor))) {
                errorForm(idElemento, 'Favor de agregar un CURP válido');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        function validarCurp_format(valor) {
            if (valor == null || valor.length == 0 || !(/^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$/.test(valor))) {
                errorForm('ContentPlaceHolder1_txt_curp', 'Favor de agregar un CURP válido');
                return false;
            } else {
                validadoForm('ContentPlaceHolder1_txt_curp');
            }
        }

        //---- Valida Campos Solicitud ----//
        function validar_campos_solicitud(e) {
            event.preventDefault(e);
            validarNombre('ContentPlaceHolder1_txt_nombre');
            validarApellidoP('ContentPlaceHolder1_txt_apellido_p');
            validarApellidoM('ContentPlaceHolder1_txt_apellido_m');
            validarCurp('ContentPlaceHolder1_txt_curp');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Admisiones/curriculum.png" style="width: 30px;" /><small>Catálogo de Campus</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end">
            <li class="nav-item">
                <a class="nav-link active" href="tpers.aspx">Datos Generales</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="taldi.aspx">Dirección</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talte.aspx">Teléfono</a>
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
        <asp:UpdatePanel ID="upd_tpers" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_solicitud" runat="server">
                    <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_matricula" class="form-label">Matrícula</label>
                            <asp:TextBox ID="txt_matricula" MaxLength="9" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_nombre" class="form-label">Nombre(s)</label>
                            <asp:TextBox ID="txt_nombre" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_apellido_p" class="form-label">Apellido Paterno</label>
                            <asp:TextBox ID="txt_apellido_p" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_apellido_m" class="form-label">Apellido Materno</label>
                            <asp:TextBox ID="txt_apellido_m" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="w-100"></div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_genero" class="form-label">Genero</label>
                            <asp:DropDownList ID="ddl_genero" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_estado_c" class="form-label">Estado Civil</label>
                            <asp:DropDownList ID="ddl_estado_c" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_curp" class="form-label">CURP</label>
                            <asp:TextBox ID="txt_curp" MaxLength="18" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_f_nac" class="form-label">Fecha Nacimiento</label>
                            <asp:TextBox ID="txt_f_nac"  runat="server" CssClass="form-control"></asp:TextBox>
                            <script>
                                function ctrl_f_nac() {
                                    $('#ContentPlaceHolder1_txt_f_nac').datepicker({
                                        uiLibrary: 'bootstrap4',
                                        locale: 'es-es',
                                        format: 'dd/mm/yyyy'
                                    });
                                }
                            </script>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_tpers" runat="server">
                    <div class="col-md-4" style="text-align: center; margin-top: 15px;">
                        <asp:Button ID="btn_cancel" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" OnClick="btn_cancel_Click"/>
                        <asp:Button ID="btn_save" runat="server" CssClass="btn btn-round btn-success" Text="Agregar" OnClick="btn_save_Click"/>
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" OnClick="btn_update_Click" />
                    </div>
                </div>
                <div id="table_campus">
                    <asp:GridView ID="GridSolicitudes" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small" OnSelectedIndexChanged="GridSolicitudes_SelectedIndexChanged">
                        <Columns>
                            <asp:ButtonField ButtonType="image" ImageUrl="~/Images/Generales/hacer-clic.png" ControlStyle-Height="24px" ControlStyle-Width="24px" CommandName="select" HeaderText="Seleccionar" ItemStyle-CssClass="button_select" />
                            <asp:BoundField DataField="CLAVE" HeaderText="Matrícula" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" />
                            <asp:BoundField DataField="PATERNO" HeaderText="Apellido Paterno" />
                            <asp:BoundField DataField="MATERNO" HeaderText="Apellido Materno" />
                            <asp:BoundField DataField="C_GENERO" HeaderText="C_Genero">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GENERO" HeaderText="Genero" />
                            <asp:BoundField DataField="C_CIVIL" HeaderText="C_Civil">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="E_CIVIL" HeaderText="Estado Civil" />
                            <asp:BoundField DataField="CURP" HeaderText="CURP" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha Nacimiento" />
                            <asp:BoundField DataField="USUARIO" HeaderText="Usuario" />
                            <asp:BoundField DataField="FECHA_REG" HeaderText="Fecha Registro" />
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
            let table_solicitudes = $("#ContentPlaceHolder1_GridSolicitudes").DataTable({
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
