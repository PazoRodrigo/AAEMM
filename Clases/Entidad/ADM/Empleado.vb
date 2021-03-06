﻿Option Explicit On
Option Strict On

Imports Clases.DataAccessLibrary
Imports Clases.Entidad
Imports LUM
Imports Connection


Namespace Entidad
    Public Class Empleado
        Inherits DBE

        Public Structure StrBusquedaEmpleado
            Public IncluirAfiliados As Integer
            Public IncluirNoAfiliados As Integer
            Public Nombre As String
            Public CUIL As Long
            Public DNI As Integer
            Public CUIT As Long
            Public RazonSocial As String
        End Structure

#Region " Atributos / Propiedades "
        Public Property IdEntidad() As Integer = 0
        Public Property Nombre() As String = ""
        Public Property NroDocumento() As Long = 0
        Public Property CUIL() As Long = 0
        Public Property NroSindical() As Long = 0
        Public Property FechaNacimiento() As Date? = Nothing
        Public Property IdSexo() As Integer = 0
        Public Property IdEstadoCivil() As Integer = 0
        Public Property ObjDomicilio() As Domicilio = Nothing
        Public Property IdEstado() As Enumeradores.EstadoEmpleado = Nothing
#End Region
#Region " Lazy Load "
        Public ReadOnly Property LngFechaNacimiento() As Long
            Get
                Dim result As Long = 0
                If FechaNacimiento.HasValue Then
                    result = CLng(Year(FechaNacimiento.Value).ToString & Right("00" & Month(FechaNacimiento.Value).ToString, 2) & Right("00" & Day(FechaNacimiento.Value).ToString, 2))
                End If
                Return result
            End Get
        End Property
#End Region
#Region " Constructores "
        Sub New()

        End Sub
        Sub New(ByVal id As Integer)
            Dim objImportar As Empleado = TraerUno(id)
            ' DBE
            IdUsuarioAlta = objImportar.IdUsuarioAlta
            IdUsuarioBaja = objImportar.IdUsuarioBaja
            IdMotivoBaja = objImportar.IdMotivoBaja
            FechaAlta = objImportar.FechaAlta
            FechaBaja = objImportar.FechaBaja
            ' Entidad
            IdEntidad = objImportar.IdEntidad
            Nombre = objImportar.Nombre
            NroDocumento = objImportar.NroDocumento
            CUIL = objImportar.CUIL
            NroSindical = objImportar.NroSindical
            FechaNacimiento = objImportar.FechaNacimiento
            IdSexo = objImportar.IdSexo
            IdEstadoCivil = objImportar.IdEstadoCivil
            IdEstado = objImportar.IdEstado
            ' Domicilio
            ObjDomicilio.Direccion = objImportar.ObjDomicilio.Direccion
            ObjDomicilio.CodigoPostal = objImportar.ObjDomicilio.CodigoPostal
            ObjDomicilio.IdLocalidad = objImportar.ObjDomicilio.IdLocalidad
        End Sub
        Sub New(ByVal DtODesde As DTO.DTO_Empleado)
            ' DBE
            IdUsuarioAlta = DtODesde.IdUsuarioAlta
            IdUsuarioBaja = DtODesde.IdUsuarioBaja
            IdMotivoBaja = DtODesde.IdMotivoBaja
            If DtODesde.FechaAlta > 0 Then
                Dim TempFecha As String = Right(DtODesde.FechaAlta.ToString, 2) + "/" + Left(Right(DtODesde.FechaAlta.ToString, 4), 2) + "/" + Left(DtODesde.FechaAlta.ToString, 4)
                FechaAlta = CDate(TempFecha)
            End If
            If DtODesde.FechaBaja > 0 Then
                Dim TempFecha As String = Right(DtODesde.FechaBaja.ToString, 2) + "/" + Left(Right(DtODesde.FechaBaja.ToString, 4), 2) + "/" + Left(DtODesde.FechaBaja.ToString, 4)
                FechaBaja = CDate(TempFecha)
            End If
            ' Entidad
            IdEntidad = DtODesde.IdEntidad
            Nombre = DtODesde.Nombre
            NroDocumento = DtODesde.NroDocumento
            CUIL = DtODesde.CUIL
            NroSindical = DtODesde.NroSindical
            If DtODesde.FechaNacimiento > 0 Then
                Dim TempFecha As String = Right(DtODesde.FechaNacimiento.ToString, 2) + "/" + Left(Right(DtODesde.FechaNacimiento.ToString, 4), 2) + "/" + Left(DtODesde.FechaNacimiento.ToString, 4)
                FechaNacimiento = CDate(TempFecha)
            End If
            IdSexo = DtODesde.IdSexo
            IdEstadoCivil = DtODesde.IdEstadoCivil
            IdEstado = CType(DtODesde.IdEstado, Enumeradores.EstadoEmpleado)
            ' Domicilio
            ObjDomicilio.Direccion = DtODesde.Domicilio.Direccion
            ObjDomicilio.CodigoPostal = DtODesde.Domicilio.CodigoPostal
            ObjDomicilio.IdLocalidad = DtODesde.Domicilio.IdLocalidad
        End Sub
#End Region
#Region " Métodos Estáticos"
        ' Traer
        Public Shared Function TraerTodosXBusqueda(busqueda As StrBusquedaEmpleado) As List(Of Empleado)
            Dim sqlQuery As String = "SELECT TOP 200 * FROM  OSEMM.dbo.basa001"
            Dim existeParametro As Boolean = False
            If busqueda.Nombre.Length > 0 Then
                If Not existeParametro Then
                    existeParametro = True
                    sqlQuery += " WHERE "
                Else
                    sqlQuery += " AND "
                End If
                sqlQuery += "ape_nom LIKE '%" + busqueda.Nombre + "%'"
            End If
            If busqueda.CUIL > 0 Then
                If Not existeParametro Then
                    existeParametro = True
                    sqlQuery += " WHERE "
                Else
                    sqlQuery += " AND "
                End If
                sqlQuery += " CAST(REPLACE(CUIL,'-','') as Bigint)  = '" + busqueda.CUIL.ToString + "'"
            End If
            If busqueda.DNI > 0 Then
                If Not existeParametro Then
                    existeParametro = True
                    sqlQuery += " WHERE "
                Else
                    sqlQuery += " AND "
                End If
                sqlQuery += " CAST(REPLACE(NRO_DOC,'-','') as Bigint)  = '" + busqueda.DNI.ToString + "'"
            End If
            'If busqueda.CUIT > 0 Then
            '    If Not existeParametro Then
            '        existeParametro = True
            '        sqlQuery += " WHERE "
            '    Else
            '        sqlQuery += " AND "
            '    End If
            '    sqlQuery += " CAST(REPLACE(CUIT,'-','') as Bigint)  = '" + busqueda.CUIT.ToString + "'"
            'End If
            'If busqueda.RazonSocial.Length > 0 Then
            '    If Not existeParametro Then
            '        existeParametro = True
            '        sqlQuery += " WHERE "
            '    Else
            '        sqlQuery += " AND "
            '    End If
            '    sqlQuery += "denomina LIKE '%" + busqueda.RazonSocial + "%'"
            'End If
            ' Afiliado
            If Not existeParametro Then
                existeParametro = True
                sqlQuery += " WHERE "
            Else
                sqlQuery += " AND "
            End If
            If busqueda.IncluirNoAfiliados = 1 AndAlso busqueda.IncluirAfiliados = 0 Then
            Else
                sqlQuery += " Fec_Baja IS NULL"
            End If
            ' Fecha Baja
            Dim result As List(Of Empleado) = DAL_Empleado.TraerTodosXBusqueda(sqlQuery)
            Return result
        End Function
        Public Shared Function TraerUno(ByVal Id As Integer) As Empleado
            Dim result As Empleado = DAL_Empleado.TraerUno(Id)
            If result Is Nothing Then
                Throw New Exception("No existen Empleados para la búsqueda")
            End If
            Return result
        End Function
        'Public Shared Function TraerTodos() As List(Of Empleado)
        '    Dim result As List(Of Empleado) = DAL_Empleado.TraerTodos()
        '    If result Is Nothing Then
        '        Throw New Exception("No existen Empleados para la búsqueda")
        '    End If
        '    Return result
        'End Function
        Public Shared Function TraerTodosXCUIL(CUIL As Long) As List(Of Empleado)
            Dim result As List(Of Empleado) = DAL_Empleado.TraerTodosXCUIL(CUIL)
            If result.Count = 0 Then
                Throw New Exception("No existen Empleados para la búsqueda")
            End If
            Return result
        End Function
        Public Shared Function TraerTodosXNombre(Nombre As String) As List(Of Empleado)
            Dim result As List(Of Empleado) = DAL_Empleado.TraerTodosXNombre(Nombre.Trim)
            If result.Count = 0 Then
                Throw New Exception("No existen Empleados para la búsqueda")
            End If
            Return result
        End Function
        Public Shared Function TraerTodosXNroDocumento(NroDocumento As Long) As List(Of Empleado)
            Dim result As List(Of Empleado) = DAL_Empleado.TraerTodosXNroDocumento(NroDocumento)
            If result.Count = 0 Then
                Throw New Exception("No existen Empleados para la búsqueda")
            End If
            Return result
        End Function
        ' Nuevos
#End Region
#Region " Métodos Públicos"
        ' ABM
        Public Sub Alta()
            ValidarAlta()
            DAL_Empleado.Alta(Me)
        End Sub
        Public Sub Baja()
            ValidarBaja()
            DAL_Empleado.Baja(Me)
        End Sub
        Public Sub Modifica()
            ValidarModifica()
            DAL_Empleado.Modifica(Me)
        End Sub
        ' Otros
        Public Function ToDTO() As DTO.DTO_Empleado
            Dim result As New DTO.DTO_Empleado With {
                .IdEntidad = IdEntidad,
                .Nombre = Nombre,
                .NroDocumento = NroDocumento,
                .CUIL = CUIL,
                .NroSindical = NroSindical,
                .IdEstado = IdEstado,
                .IdSexo = IdSexo,
                .IdEstadoCivil = IdEstadoCivil,
                .FechaNacimiento = LngFechaNacimiento,
                .Domicilio = ObjDomicilio.ToDto
            }
            Return result
        End Function
        Public Function ToDTOCabecera() As DTO.DTO_Empleado
            Dim result As New DTO.DTO_Empleado With {
                .IdEntidad = IdEntidad,
                .Nombre = Nombre,
                .NroDocumento = NroDocumento,
                .CUIL = CUIL,
                .NroSindical = NroSindical,
                .IdEstado = IdEstado,
                .IdSexo = IdSexo,
                .IdEstadoCivil = IdEstadoCivil,
                .FechaNacimiento = LngFechaNacimiento
            }
            Return result
        End Function
        ' Nuevos
#End Region
#Region " Métodos Privados "
        ' ABM
        Private Sub ValidarAlta()
            ValidarUsuario(Me.IdUsuarioAlta)
            ValidarCampos()
            ValidarNoDuplicados()
        End Sub
        Private Sub ValidarBaja()
            ValidarUsuario(Me.IdUsuarioBaja)
        End Sub
        Private Sub ValidarModifica()
            ValidarUsuario(Me.IdUsuarioModifica)
            ValidarCampos()
            ValidarNoDuplicados()
        End Sub
        ' Validaciones
        Private Sub ValidarUsuario(ByVal idUsuario As Integer)
            If idUsuario = 0 Then
                Throw New Exception("Debe ingresar al sistema")
            End If
        End Sub
        Private Sub ValidarCampos()
            Dim sError As String = ""
            ValidarCaracteres()
            ' Campo Integer/Decimal
            '	If Me.VariableNumero.toString = "" Then
            '   	sError &= "<b>VariableNumero</b> Debe ingresar VariableNumero. <br />"
            '	ElseIf Not isnumeric(Me.VariableNumero) Then
            '   	sError &= "<b>VariableNumero</b> Debe ser numérico. <br />"
            '	End If

            ' Campo String
            '	If Me.VariableString = "" Then
            '		sError &= "<b>VariableString</b> Debe ingresar VariableString. <br />"
            '	ElseIf Me.apellido.Length > 50 Then
            '		sError &= "<b>VariableString</b>Debe tener como máximo 50 caracteres. <br />"
            '	End If

            ' Campo Date
            '	If Not Me.VariableFecha.has value Then
            '		sError &= "<b>VariableFecha</b> Debe ingresar VariableFecha. <br />"
            '	End If
            If sError <> "" Then
                sError = "<b>Debe corregir los siguientes errores</b> <br /> <br />" & sError
                Throw New Exception(sError)
            End If
        End Sub
        Private Sub ValidarCaracteres()
            Dim sError As String = ""
            If sError <> "" Then
                sError = "<b>Debe corregir los siguientes errores</b> <br /> <br />" & sError
                Throw New Exception(sError)
            End If
        End Sub
        Private Sub ValidarNoDuplicados()
            'Empleado.Refresh()
            'Dim result As Empleado = Todos.Find(Function(x) x.CUIT = CUIT)
            'If Not result Is Nothing Then
            '    Throw New Exception("El Nombre a ingresar ya existe")
            'End If
        End Sub
#End Region
    End Class ' Empleado
End Namespace ' Entidad

Namespace DTO
    Public Class DTO_Empleado
        Inherits DTO_Dimensional


#Region " Atributos / Propiedades"
        Public Property IdEntidad() As Integer = 0
        Public Property Nombre() As String = ""
        Public Property NroDocumento() As Long = 0
        Public Property CUIL() As Long = 0
        Public Property NroSindical() As Long = 0
        Public Property FechaNacimiento() As Long = 0
        Public Property IdSexo() As Integer = 0
        Public Property IdEstadoCivil() As Integer = 0
        Public Property Domicilio() As DTO.DTO_Domicilio = Nothing
#End Region
    End Class ' DTO_Empleado
End Namespace ' DTO

Namespace DataAccessLibrary
    Public Class DAL_Empleado

#Region " Stored "
        Const storeAlta As String = "ADM.p_Empleado_Alta"
        Const storeBaja As String = "ADM.p_Empleado_Baja"
        Const storeModifica As String = "ADM.p_Empleado_Modifica"
        Const storeTraerUnoXId As String = "ADM.p_Empleado_TraerUnoXId"
        Const storeTraerTodosXCUIL As String = "ADM.p_Empleado_TraerTodosXCUIL"
        Const storeTraerTodosXNombre As String = "ADM.p_Empleado_TraerTodosXNombre"
        Const storeTraerTodosXNroDocumento As String = "ADM.p_Empleado_TraerTodosXNroDocumento"
        Const storeTraerTodosXBusqueda As String = "ADM.p_Empleado_TraerXBusquedaLibre"

#End Region
#Region " Métodos Públicos "
        ' ABM
        Public Shared Sub Alta(ByVal entidad As Empleado)
            Dim store As String = storeAlta
            Dim pa As New parametrosArray
            pa.add("@idUsuarioAlta", entidad.IdUsuarioAlta)
            pa.add("@Nombre", entidad.Nombre.ToString.ToUpper.Trim)
            pa.add("@NroDocumento", entidad.NroDocumento)
            pa.add("@CUIL", entidad.CUIL)
            pa.add("@NroSindical", entidad.NroSindical)
            pa.add("@FechaNacimiento", LUM.Convertidor.DateToDB(entidad.FechaNacimiento))
            pa.add("@IdSexo", entidad.IdSexo)
            pa.add("@IdEstadoCivil", entidad.IdEstadoCivil)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If Not dt Is Nothing Then
                    If dt.Rows.Count = 1 Then
                        entidad.IdEntidad = CInt(dt.Rows(0)(0))
                    End If
                End If
            End Using
        End Sub
        Public Shared Sub Baja(ByVal entidad As Empleado)
            Dim store As String = storeBaja
            Dim pa As New parametrosArray
            pa.add("@idUsuarioBaja", entidad.IdUsuarioBaja)
            pa.add("@id", entidad.IdEntidad)
            pa.add("@IdMotivoBaja", entidad.IdMotivoBaja)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If Not dt Is Nothing Then
                    If dt.Rows.Count = 1 Then
                        entidad.IdEntidad = CInt(dt.Rows(0)(0))
                    End If
                End If
            End Using
        End Sub
        Public Shared Sub Modifica(ByVal entidad As Empleado)
            Dim store As String = storeModifica
            Dim pa As New parametrosArray
            pa.add("@idUsuarioModifica", entidad.IdUsuarioModifica)
            pa.add("@id", entidad.IdEntidad)
            pa.add("@Nombre", entidad.Nombre.ToString.ToUpper.Trim)
            pa.add("@NroDocumento", entidad.NroDocumento)
            pa.add("@CUIL", entidad.CUIL)
            pa.add("@NroSindical", entidad.NroSindical)
            pa.add("@FechaNacimiento", LUM.Convertidor.DateToDB(entidad.FechaNacimiento))
            pa.add("@IdSexo", entidad.IdSexo)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If Not dt Is Nothing Then
                    If dt.Rows.Count = 1 Then
                        entidad.IdEntidad = CInt(dt.Rows(0)(0))
                    End If
                End If
            End Using
        End Sub
        ' Traer
        Public Shared Function TraerUno(ByVal id As Integer) As Empleado
            Dim store As String = storeTraerUnoXId
            Dim result As New Empleado
            Dim pa As New parametrosArray
            pa.add("@id", id)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If Not dt Is Nothing Then
                    If dt.Rows.Count = 1 Then
                        result = LlenarEntidad(dt.Rows(0))
                    ElseIf dt.Rows.Count = 0 Then
                        result = Nothing
                    End If
                End If
            End Using
            Return result
        End Function
        Public Shared Function TraerTodosXCUIL(ByVal CUIL As Long) As List(Of Empleado)
            Dim store As String = storeTraerTodosXCUIL
            Dim listaResult As New List(Of Empleado)
            Dim pa As New parametrosArray
            pa.add("@CUIL", CUIL)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        listaResult.Add(LlenarEntidad(dr))
                    Next
                End If
            End Using
            Return listaResult
        End Function
        Public Shared Function TraerTodosXNombre(ByVal Nombre As String) As List(Of Empleado)
            Dim store As String = storeTraerTodosXNombre
            Dim listaResult As New List(Of Empleado)
            Dim pa As New parametrosArray
            pa.add("@Nombre", Nombre)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        listaResult.Add(LlenarEntidad(dr))
                    Next
                End If
            End Using
            Return listaResult
        End Function
        Public Shared Function TraerTodosXNroDocumento(ByVal NroDocumento As Long) As List(Of Empleado)
            Dim store As String = storeTraerTodosXNroDocumento
            Dim listaResult As New List(Of Empleado)
            Dim pa As New parametrosArray
            pa.add("@NroDocumento", NroDocumento)
            Using dt As DataTable = Connection.Connection.TraerDt(store, pa)
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        listaResult.Add(LlenarEntidad(dr))
                    Next
                End If
            End Using
            Return listaResult
        End Function
        Friend Shared Function TraerTodosXBusqueda(sqlQuery As String) As List(Of Empleado)
            Dim store As String = storeTraerTodosXBusqueda
            Dim listaResult As New List(Of Empleado)
            Dim pa As New parametrosArray
            pa.add("@sqlQuery", sqlQuery)
            Using dt As DataTable = Connection.Connection.TraerDT(store, pa)
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        listaResult.Add(LlenarEntidad(dr))
                    Next
                End If
            End Using
            Return listaResult
        End Function
#End Region
#Region " Métodos Privados "
        Private Shared Function LlenarEntidad(ByVal dr As DataRow) As Empleado
            Dim entidad As New Empleado
            ' DBE
            If dr.Table.Columns.Contains("idUsuarioAlta") Then
                If dr.Item("idUsuarioAlta") IsNot DBNull.Value Then
                    entidad.IdUsuarioAlta = CInt(dr.Item("idUsuarioAlta"))
                End If
            End If
            If dr.Table.Columns.Contains("idUsuarioBaja") Then
                If dr.Item("idUsuarioBaja") IsNot DBNull.Value Then
                    entidad.IdUsuarioBaja = CInt(dr.Item("idUsuarioBaja"))
                End If
            End If
            If dr.Table.Columns.Contains("idUsuarioModifica") Then
                If dr.Item("idUsuarioModifica") IsNot DBNull.Value Then
                    entidad.IdUsuarioModifica = CInt(dr.Item("idUsuarioModifica"))
                End If
            End If
            If dr.Table.Columns.Contains("IdMotivoBaja") Then
                If dr.Item("IdMotivoBaja") IsNot DBNull.Value Then
                    entidad.IdMotivoBaja = CInt(dr.Item("IdMotivoBaja"))
                End If
            End If
            If dr.Table.Columns.Contains("fechaAlta") Then
                If dr.Item("fechaAlta") IsNot DBNull.Value Then
                    entidad.FechaAlta = CDate(dr.Item("fechaAlta"))
                End If
            ElseIf dr.Table.Columns.Contains("fe_Alta") Then
                If dr.Item("fe_Alta") IsNot DBNull.Value Then
                    entidad.FechaAlta = CDate(dr.Item("fe_Alta"))
                End If
            End If
            If dr.Table.Columns.Contains("fechaBaja") Then
                If dr.Item("fechaBaja") IsNot DBNull.Value Then
                    entidad.FechaBaja = CDate(dr.Item("fechaBaja"))
                End If
            End If
            ' Entidad
            Try
                If dr.Table.Columns.Contains("id") Then
                    If dr.Item("id") IsNot DBNull.Value Then
                        entidad.IdEntidad = CInt(dr.Item("id"))
                    End If
                End If
                If dr.Table.Columns.Contains("ApellidoNombre") Then
                    If dr.Item("ApellidoNombre") IsNot DBNull.Value Then
                        entidad.Nombre = dr.Item("ApellidoNombre").ToString.ToUpper.Trim
                    End If
                ElseIf dr.Table.Columns.Contains("APE_NOM") Then
                    If dr.Item("APE_NOM") IsNot DBNull.Value Then
                        entidad.Nombre = dr.Item("APE_NOM").ToString.ToUpper.Trim
                    End If
                End If
                If dr.Table.Columns.Contains("NroDocumento") Then
                    If dr.Item("NroDocumento") IsNot DBNull.Value Then
                        entidad.NroDocumento = CLng(dr.Item("NroDocumento"))
                    End If
                ElseIf dr.Table.Columns.Contains("NRO_DOC") Then
                    If dr.Item("NRO_DOC") IsNot DBNull.Value Then
                        entidad.NroDocumento = CLng(dr.Item("NRO_DOC"))
                    End If
                End If
                If dr.Table.Columns.Contains("CUIL") Then
                    If dr.Item("CUIL") IsNot DBNull.Value Then
                        entidad.CUIL = CLng(Replace(dr.Item("CUIL").ToString, "-", ""))
                    End If
                End If
                If dr.Table.Columns.Contains("NroSindical") Then
                    If dr.Item("NroSindical") IsNot DBNull.Value Then
                        entidad.NroSindical = CLng(dr.Item("NroSindical"))
                    End If
                End If
                If dr.Table.Columns.Contains("FechaNacimiento") Then
                    If dr.Item("FechaNacimiento") IsNot DBNull.Value Then
                        entidad.FechaNacimiento = CDate(dr.Item("FechaNacimiento"))
                    End If
                ElseIf dr.Table.Columns.Contains("fec_nac") Then
                    If dr.Item("fec_nac") IsNot DBNull.Value Then
                        entidad.FechaNacimiento = CDate(dr.Item("fec_nac"))
                    End If
                End If
                If dr.Table.Columns.Contains("IdSexo") Then
                    If dr.Item("IdSexo") IsNot DBNull.Value Then
                        entidad.IdSexo = CInt(dr.Item("IdSexo"))
                    End If
                ElseIf dr.Table.Columns.Contains("sexo") Then
                    If dr.Item("sexo") IsNot DBNull.Value Then
                        Select Case dr.Item("sexo").ToString
                            Case "M"
                                entidad.IdSexo = 1
                            Case "F"
                                entidad.IdSexo = 0
                            Case Else
                        End Select
                    End If
                End If
                If dr.Table.Columns.Contains("IdEstadoCivil") Then
                    If dr.Item("IdEstadoCivil") IsNot DBNull.Value Then
                        If dr.Item("IdEstadoCivil").ToString.Trim <> "" Then
                            entidad.IdEstadoCivil = CInt(dr.Item("IdEstadoCivil"))
                        End If
                    End If
                End If
            Catch ex As Exception
                Dim a As String = entidad.NroDocumento.ToString
                Throw New Exception(a)
            End Try

            ' Domicilio
            entidad.ObjDomicilio = Domicilio.LlenarDomicilio(dr)
            Return entidad
        End Function
#End Region
    End Class ' DAL_Empleado
End Namespace ' DataAccessLibrary