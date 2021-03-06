﻿Option Explicit On
Option Strict On

Imports Clases.DataAccessLibrary
Imports Clases.DTO
Imports Clases.Entidad

Namespace Entidad
    Public Class Domicilio

#Region " Atributos / Propiedades "
        Public Property Direccion() As String = ""
        Public Property CodigoPostal() As Integer = 0
        Public Property Localidad() As String = ""
        Public Property IdLocalidad() As Integer = 0
#End Region
#Region " Lazy Load "
        Public Property _ObjLocalidad() As Object = Nothing
        Public ReadOnly Property ObjLocalidad() As Object
            Get
                If _ObjLocalidad Is Nothing Then
                    If IdLocalidad > 0 Then
                        _ObjLocalidad = Entidad.Localidad.TraerUno(IdLocalidad)
                    Else
                        _ObjLocalidad = New Localidad
                    End If
                End If
                Return _ObjLocalidad
            End Get
        End Property
#End Region
#Region " Constructores "
        Sub New()

        End Sub
        Sub New(ByVal DtODesde As DTO.DTO_Domicilio)
            ' Entidad
            Direccion = DtODesde.Direccion
            CodigoPostal = DtODesde.CodigoPostal
            IdLocalidad = DtODesde.IdLocalidad
        End Sub
#End Region
#Region " Métodos Públicos "
        Friend Shared Function LlenarDomicilio(dr As DataRow) As Domicilio
            Dim result As New Domicilio
            If Not dr Is Nothing Then
                result = DAL_Domicilio.LlenarEntidad(dr)
            End If
            Return result
        End Function
        Friend Function ToDto() As DTO_Domicilio
            Dim ObjLocalidadTemp As New Localidad
            If IdLocalidad = 0 AndAlso CodigoPostal > 0 Then
                Dim listaLocalTemp As List(Of Localidad) = Entidad.Localidad.TraerTodosXCP(CodigoPostal)
                If listaLocalTemp.Count > 0 Then
                    ObjLocalidadTemp = listaLocalTemp(0)
                    IdLocalidad = ObjLocalidadTemp.IdEntidad
                End If
            End If
            Dim result As New DTO.DTO_Domicilio With {
                .Direccion = Direccion,
                .CodigoPostal = CodigoPostal,
                .IdLocalidad = IdLocalidad,
                .Localidad = ObjLocalidadTemp.ToDTO
            }
            Return result
        End Function
#End Region

    End Class ' Domicilio
End Namespace ' Entidad

Namespace DTO
    Public Class DTO_Domicilio

#Region " Atributos / Propiedades"
        Public Property Direccion() As String = ""
        Public Property CodigoPostal() As Integer = 0
        Public Property IdLocalidad() As Integer = 0
        Public Property Localidad() As DTO_Localidad = Nothing
#End Region
    End Class ' DTO_Domicilio
End Namespace ' DTO

Public Class DAL_Domicilio

#Region " Stored "
    Const storeAlta As String = "ADM.p_Empresa_Alta"
    Const storeBaja As String = "ADM.p_Empresa_Baja"
    Const storeModifica As String = "ADM.p_Empresa_Modifica"
    Const storeTraerUnoXId As String = "ADM.p_Empresa_TraerUnoXId"
    Const storeTraerTodos As String = "ADM.p_Empresa_TraerTodos"
    Const storeTraerTodosXCUIT As String = "ADM.p_Empresa_TraerTodosXCUIT"
    Const storeTraerTodosXRazonSocial As String = "ADM.p_Empresa_TraerTodosXRazonSocial"
    Const storeTraerTodosXCentroCosto As String = "ADM.p_Empresa_TraerTodosXCentroCosto"
#End Region
#Region " Métodos Públicos "
    ' ABM
    ' Traer
    Public Shared Function LlenarEntidad(ByVal dr As DataRow) As Domicilio
        Dim entidad As New Domicilio
        ' Entidad
        If dr.Table.Columns.Contains("Direccion") Then
            If dr.Item("Direccion") IsNot DBNull.Value Then
                entidad.Direccion = dr.Item("Direccion").ToString.ToUpper.Trim
            End If
        End If
        If dr.Table.Columns.Contains("CodigoPostal") Then
            If dr.Item("CodigoPostal") IsNot DBNull.Value Then
                entidad.CodigoPostal = CInt(dr.Item("CodigoPostal"))
            End If
        End If
        If dr.Table.Columns.Contains("Localidad") Then
            If dr.Item("Localidad") IsNot DBNull.Value Then
                entidad.Localidad = dr.Item("Localidad").ToString.ToUpper.Trim
            End If
        End If
        If dr.Table.Columns.Contains("IdLocalidad") Then
            If dr.Item("IdLocalidad") IsNot DBNull.Value Then
                entidad.IdLocalidad = CInt(dr.Item("IdLocalidad"))
            End If
        End If
        Return entidad
    End Function
#End Region
End Class ' DAL_Empresa
