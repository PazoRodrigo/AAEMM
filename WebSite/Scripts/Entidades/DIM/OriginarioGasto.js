﻿var _Lista_OriginarioGasto;

class OriginarioGasto extends DBE {
    constructor() {
        super();
        this.IdEntidad = 0;
        this.Nombre = '';
        this.Observaciones = '';
    }

    // ABM
    async Alta() {
        await this.ValidarCamposOriginarioGasto();
        this.Nombre = this.Nombre.toUpperCase();
        this.Observaciones = this.Observaciones.toUpperCase();
        try {
            let ObjU = JSON.parse(sessionStorage.getItem("User"));
            this.IdUsuarioAlta = ObjU.IdEntidad;
             let data = {
                'entidad': this
            };
            let id = await ejecutarAsync(urlWsOriginarioGasto + "/Alta", data);
            if (id !== undefined)
                this.IdEntidad = id;
            _Lista_OriginarioGasto.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }
    async Modifica() {
        await this.ValidarCamposOriginarioGasto();
        this.Nombre = this.Nombre.toUpperCase();
        this.Observaciones = this.Observaciones.toUpperCase();
        try {
            let ObjU = JSON.parse(sessionStorage.getItem("User"));
            this.IdUsuarioModifica = ObjU.IdEntidad;
           let data = {
                'entidad': this
            };
            let id = await ejecutarAsync(urlWsOriginarioGasto + "/Modifica", data);
            if (id !== undefined)
                this.IdEntidad = id;
            let buscados = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
                return entidad.IdEntidad !== id;
            });
            _Lista_OriginarioGasto = buscados;
            this.IdEstado = 0;
            _Lista_OriginarioGasto.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }
    async Baja() {
        try {
            let ObjU = JSON.parse(sessionStorage.getItem("User"));
            this.IdUsuarioBaja = ObjU.IdEntidad;
            let data = {
                'entidad': this
            };
            let id = await ejecutarAsync(urlWsOriginarioGasto + "/Baja", data);
            if (id !== undefined)
                this.IdEntidad = id;
            let buscados = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
                return entidad.IdEntidad !== id;
            });
            _Lista_OriginarioGasto = buscados;
            this.IdEstado = 1;
            _Lista_OriginarioGasto.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }

    async ValidarCamposOriginarioGasto() {
        let sError = '';
        if (this.Nombre.length === 0) {
            sError += 'Debe ingresar el Nombre';
        }
        if (sError !== '') {
            throw '<b> Error de grabación </b> <br/><br/>' + sError;
        }
    }

    // Todos
    static async Todos() {
        if (_Lista_OriginarioGasto === undefined) {
            _Lista_OriginarioGasto = await OriginarioGasto.TraerTodas();
        }
        return _Lista_OriginarioGasto;
    }

    // Traer
    static async TraerUno(IdEntidad) {
        _Lista_OriginarioGasto = await OriginarioGasto.TraerTodos();
        let buscado = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
            return entidad.IdEntidad === IdEntidad;
        });
        return buscado[0];
    }
    static async TraerTodos() {
        return await OriginarioGasto.Todos();
    }
    static async TraerTodosActivos() {
        _Lista_OriginarioGasto = await OriginarioGasto.TraerTodos();
        let buscado = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
            return entidad.IdEstado === 0;
        });
        return buscado;
    }
    static async TraerTodas() {
        let lista = await ejecutarAsync(urlWsOriginarioGasto + "/TraerTodos");
        _Lista_OriginarioGasto = [];
        let result = [];
        if (lista.length > 0) {
            $.each(lista, function (key, value) {
                result.push(LlenarEntidadOriginarioGasto(value));
            });
        }
        _Lista_OriginarioGasto = result;
        return _Lista_OriginarioGasto;
    }
    // Otros
    static async Refresh() {
        _Lista_OriginarioGasto = await OriginarioGasto.TraerTodas();
    }
    // Herramientas
    static async ArmarGrilla(lista, div, eventoSeleccion, eventoEliminar, estilo) {
        $('#' + div + '').html('');
        let str = '';
        lista.sort(SortXNombre);
        if (lista.length > 0) {
            str += '<div style="' + estilo + '">';
            str += '    <ul class="ListaGrilla">';
            let estiloItem = '';
            for (let item of lista) {
                estiloItem = 'LinkListaGrillaObjeto';
                if (item.IdEstado === 1) {
                    estiloItem = 'LinkListaGrillaObjetoEliminado';
                }
                let aItem = '<a href="#" class="mibtn-seleccionOriginarioGasto" data-Evento="' + eventoSeleccion + '" data-Id="' + item.IdEntidad + '">' + item.Nombre + '</a>';
                let aEliminar = '<a href="#" class="mibtn-EliminarOriginarioGasto" data-Evento="' + eventoEliminar + '" data-Id="' + item.IdEntidad + '"><span class="icon-bin"></span></a>';
                str += String.format('<li><div class="LinkListaGrilla ' + estiloItem + '">{0}</div><div class="LinkListaGrilla LinkListaGrillaElimina">{1}</div></li>', aItem, aEliminar);
            }
            str += '    </ul>';
            str += '</div>';
        }
        return $('#' + div + '').html(str);
    }
    static async ArmarRadios(lista, div, evento, estilo) {
        $('#' + div + '').html('');
        let str = '';
        await Area.Refresh();
        if (lista.length > 0) {
            str += '<div style="' + estilo + '">';
            str += '    <table class="table table-bordered" style="width: 70%;">';
            str += '        <thead>';
            str += '            <tr>';
            str += '                <th colspan="2" style="text-align: center;">Areas</th>';
            str += '            </tr>';
            str += '        </thead>';
            str += '        <tbody>';
            for (let item of lista) {
                let radioSeleccion = '<input type="radio" class="mibtn-seleccionArea"  name="rblArea" data-Evento="' + evento + '" data-Id="' + item.IdEntidad + '" value="' + item.IdEntidad + '">';
                str += String.format('<tr><td align="center" valign="middle" style="width: 5%;">{0}</td><td align="left">{1}</td></tr>', radioSeleccion, item.Nombre);
            }
            str += '        </tbody>';
            str += '    </table>';
            str += '</div>';
        }
        return $('#' + div + '').html(str);
    }
    static async ArmarCheckBoxs(lista, div, evento, estilo) {
        $('#' + div + '').html('');
        let str = '';
        str += '<div style="' + estilo + '">';
        await Area.Refresh();
        if (lista.length > 0) {
            for (let item of lista) {
                str += '<div class="col-lg-4"><input type="checkbox" class="micbx-Area" name="CkbList_Areas" value="' + item.IdEntidad + '"    id="chk_' + item.IdEntidad + '" /><label for="chk_' + item.IdEntidad + '"> ' + item.Nombre + '</label></div>';
            }
        }
        str += '</div>';
        return $('#' + div + '').html(str);
    }
    static async ArmarCombo(lista, div, selector, evento, ventana, Cbo) {
        let cbo = "";
        cbo += '<div id="' + Cbo + '" class="dropdown">';
        cbo += '    <button id="' + selector + '" class="btn btn-primary dropdown-toggle btn-md btn-block" type="button" data-toggle="dropdown">' + ventana;
        cbo += '        <span class="caret"></span>';
        cbo += '    </button>';
        cbo += '<ul class="dropdown-menu">';
        $(lista).each(function () {
            cbo += '<li><a href="#" class="mibtn-seleccionOriginarioGasto" data-Id="' + this.IdEntidad + '"  data-Evento="' + evento + '" > ' + this.Nombre + '</a></li>';
        });
        cbo += '</ul>';
        cbo += '</div>';
        return $('#' + div + '').html(cbo);
    }
}
function LlenarEntidadOriginarioGasto(entidad) {
    let Res = new OriginarioGasto;
    Res.IdUsuarioAlta = entidad.IdUsuarioAlta;
    Res.IdUsuarioBaja = entidad.IdUsuarioBaja;
    Res.IdUsuarioModifica = entidad.IdUsuarioModifica;
    Res.FechaAlta = entidad.FechaAlta;
    Res.FechaBaja = entidad.FechaBaja;
    Res.IdMotivoBaja = entidad.IdMotivoBaja;
    Res.IdEntidad = entidad.IdEntidad;
    Res.Nombre = entidad.Nombre;
    Res.Observaciones = entidad.Observaciones;
    Res.IdEstado = entidad.IdEstado;
    return Res;
}
$('body').on('click', ".mibtn-seleccionOriginarioGasto", async function () {
    try {
        $this = $(this);
        let buscado = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
            return entidad.IdEntidad === parseInt($this.attr("data-Id"));
        });
        let Seleccionado = buscado[0];
        let evento = $this.attr("data-Evento");
        let event = new CustomEvent(evento, { detail: Seleccionado });
        document.dispatchEvent(event);
    } catch (e) {
        alertAlerta(e);
    }
});
$('body').on('click', ".mibtn-EliminarOriginarioGasto", async function () {
    try {
        $this = $(this);
        let buscado = $.grep(_Lista_OriginarioGasto, function (entidad, index) {
            return entidad.IdEntidad === parseInt($this.attr("data-Id"));
        });
        let Seleccionado = buscado[0];
        let evento = $this.attr("data-Evento");
        let event = new CustomEvent(evento, { detail: Seleccionado });
        document.dispatchEvent(event);
    } catch (e) {
        alertAlerta(e);
    }
});
