﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="testa.aspx.cs" Inherits="SAES_v1.testa" %>

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

        function carga_menu() {
            $("#operacion").addClass("active");
            $("#demograficos").addClass("current-page");
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


        //----Validación de País----//
        function validar_pais_e(idEl) {
            const idElemento = idEl;
            valor = $("#" + idElemento).val();

            if (valor == 0) {
                errorForm(idElemento, 'Debes seleccionar un país válido');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        //---- Clave Estado ----//
        function validarclaveEstado(idEl, ind) {
            const idElemento = idEl;
            if (ind == 0) {
                let documento = document.getElementById(idElemento).value;
                if (documento == null || documento.length == 0 || /^\s+$/.test(documento)) {
                    errorForm(idElemento, 'Por favor ingresa una clave valida');
                    return false;
                } else {
                    validadoForm(idElemento);
                }
            } else {
                errorForm(idElemento, 'La clave ingresada ya existe');
                return false;
            }

        }

        //---- Nombre Estado ----//
        function validarNombreEstado(idEl) {
            const idElemento = idEl;
            let tamin = document.getElementById(idElemento).value;
            if (tamin == null || tamin.length == 0 || /^\s+$/.test(tamin)) {
                errorForm(idElemento, 'Por favor ingresa el nombre del estado');
                return false;
            } else {
                validadoForm(idElemento);
            }
        }

        //---- Valida fomulario completo ----//
        function validar_campos_estado(e) {
            event.preventDefault(e);
            validar_pais_e('ContentPlaceHolder1_cbo_pais');
            validarclaveEstado('ContentPlaceHolder1_c_estado', 0);
            validarNombreEstado('ContentPlaceHolder1_n_estado');
            return false;
        }

    </script>
    <style>
        #operacion ul{
            display:block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Operaciones/globo.png" style="width: 30px;" /><small>Catálogos Demográficos</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end">
            <li class="nav-item">
                <a class="nav-link " href="tpais.aspx">Países</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="testa.aspx">Estados</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tdele.aspx">Delegación-Municipio</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tcopo.aspx">Código Postal</a>
            </li>
        </ul>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_estado" runat="server">
                    <div class="row g-3" style="margin-top: 15px;">
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_cbo_pais" class="form-label">Pais</label>
                            <asp:DropDownList ID="cbo_pais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label for="ContentPlaceHolder1_c_estado" class="form-label">Clave</label>
                            <asp:TextBox ID="c_estado" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_n_estado" class="form-label">Nombre</label>
                            <asp:TextBox ID="n_estado" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_estatus_estado" class="form-label">Estatus</label>
                            <asp:DropDownList ID="estatus_estado" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_estado" runat="server">
                    <div class="col-md-3" style="text-align: center;">
                        <asp:Button ID="cancel_estado" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" OnClick="cancel_estado_Click" />
                        <asp:Button ID="save_estado" runat="server" CssClass="btn btn-round btn-success" Text="Agregar" OnClientClick="destroy_table();" OnClick="save_estado_Click" />
                        <asp:Button ID="update_estado" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" OnClientClick="destroy_table();" OnClick="update_estado_Click" />
                    </div>
                </div>

                <div id="table_estados">
                    <asp:GridView ID="GridEstados" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small" OnSelectedIndexChanged="GridEstados_SelectedIndexChanged">
                        <Columns>
                            <asp:ButtonField ButtonType="image" ImageUrl="~/Images/Generales/hacer-clic.png" ControlStyle-Height="24px" ControlStyle-Width="24px" CommandName="select" HeaderText="Seleccionar" ItemStyle-CssClass="button_select" />
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Estado" />
                            <asp:BoundField DataField="C_PAIS" HeaderText="clave_pais">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PAIS" HeaderText="Pais" />
                            <asp:BoundField DataField="ESTATUS_CODE" HeaderText="Estatus_code">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" />
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
            let table_e = $("#ContentPlaceHolder1_GridEstados").DataTable({
                language: {
                    sProcessing: 'Procesando...',
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
                }, scrollY: '500px',
                scrollCollapse: true,
                order: [
                    [0, "asc"]
                ],
                lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
                "autoWidth": true,
                dom: '<"top"if>rt<"bottom"lBp><"clear">',
                buttons: [
                    {
                        title: 'SAES_Catálogo de Estados',
                        className: 'btn-dark',
                        extend: 'excel',
                        text: 'Exportar Excel',
                        exportOptions: {
                            columns: [1, 2, 4, 6, 7]
                        }
                    },
                    {
                        title: 'SAES_Catálogo de Estados',
                        className: 'btn-dark',
                        extend: 'pdfHtml5',
                        text: 'Exportar PDF',
                        orientation: 'landscape',
                        pageSize: 'LEGAL',
                        exportOptions: {
                            columns: [1, 2, 4, 6, 7]
                        }
                    }
                ]
            });
            table_e.fadeIn("fast");
        }

        function destroy_table() {
            $("#ContentPlaceHolder1_GridEstados").DataTable().destroy();
        }
        function remove_class() {
            $('.selected_table').removeClass("selected_table")
        }
    </script>
</asp:Content>
