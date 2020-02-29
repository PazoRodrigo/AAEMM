﻿var _Lista_ChequePropio;

class ChequePropio extends DBE {
    constructor() {
        super();
        this.IdEntidad = 0;
        this.Nombre = '';
        this.Observaciones = '';
    }

    // ABM
    static async  AltaChequera(Desde, Hasta) {
        //await ValidarChequera();
        try {
            var _Lista_ChequePropio;
            _Lista_ChequePropio = [];
            let data = {
                'Desde': Desde,
                'Hasta': Hasta
            };
            let id = await ejecutarAsync(urlWsChequePropio + "/AltaChequera", data);
            if (id !== undefined)
                this.IdEntidad = id;
            _Lista_ChequePropio.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }
    async Modifica() {
        await this.ValidarCamposChequePropio();
        this.Nombre = this.Nombre.toUpperCase();
        this.Observaciones = this.Observaciones.toUpperCase();
        try {
            let data = {
                'entidad': this
            };
            let id = await ejecutarAsync(urlWsChequePropio + "/Modifica", data);
            if (id !== undefined)
                this.IdEntidad = id;
            let buscados = $.grep(_Lista_ChequePropio, function (entidad, index) {
                return entidad.IdEntidad !== id;
            });
            _Lista_ChequePropio = buscados;
            this.IdEstado = 0;
            _Lista_ChequePropio.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }
    async Baja() {
        try {
            let data = {
                'entidad': this
            };
            let id = await ejecutarAsync(urlWsChequePropio + "/Baja", data);
            if (id !== undefined)
                this.IdEntidad = id;
            let buscados = $.grep(_Lista_ChequePropio, function (entidad, index) {
                return entidad.IdEntidad !== id;
            });
            _Lista_ChequePropio = buscados;
            this.IdEstado = 1;
            _Lista_ChequePropio.push(this);
            return;
        } catch (e) {
            throw e;
        }
    }

    async ValidarCamposChequePropio() {
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
        console.log('_Lista_ChequePropio');
        if (_Lista_ChequePropio === undefined) {
            _Lista_ChequePropio = await ChequePropio.TraerTodas();
        }
        return _Lista_ChequePropio;
    }

    // Traer
    static async TraerUno(IdEntidad) {
        _Lista_ChequePropio = await ChequePropio.TraerTodos();
        let buscado = $.grep(_Lista_ChequePropio, function (entidad, index) {
            return entidad.IdEntidad === IdEntidad;
        });
        let Encontrado = buscado[0];
        return Encontrado;
    }
    static async TraerTodos() {
        return await ChequePropio.Todos();
    }
    static async TraerTodosActivos() {
        _Lista_ChequePropio = await ChequePropio.TraerTodos();
        let buscado = $.grep(_Lista_ChequePropio, function (entidad, index) {
            return entidad.IdEstado === 0;
        });
        return buscado;
    }
    static async TraerTodas() {
        let lista = await ejecutarAsync(urlWsChequePropio + "/TraerTodos");
        _Lista_ChequePropio = [];
        let result = [];
        if (lista.length > 0) {
            $.each(lista, function (key, value) {
                result.push(LlenarEntidadChequePropio(value));
            });
        }
        _Lista_ChequePropio = result;
        return _Lista_ChequePropio;
    }
    // Otros
    static async Refresh() {
        _Lista_ChequePropio = await ChequePropio.TraerTodas();
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
                let aItem = '<a href="#" class="mibtn-seleccionChequePropio" data-Evento="' + eventoSeleccion + '" data-Id="' + item.IdEntidad + '">' + item.Nombre + '</a>';
                let aEliminar = '<a href="#" class="mibtn-EliminarChequePropio" data-Evento="' + eventoEliminar + '" data-Id="' + item.IdEntidad + '"><span class="icon-bin"></span></a>';
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
}
function LlenarEntidadChequePropio(entidad) {
    let Res = new ChequePropio;
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
$('body').on('click', ".mibtn-seleccionChequePropio", async function () {
    try {
        $this = $(this);
        let buscado = $.grep(_Lista_ChequePropio, function (entidad, index) {
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
$('body').on('click', ".mibtn-EliminarChequePropio", async function () {
    try {
        $this = $(this);
        let buscado = $.grep(_Lista_ChequePropio, function (entidad, index) {
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
