Imports System.Data
Imports System.Data.SqlClient
Public Class SQLControl
    Public lServer As String = "SDN-ENVY-2020\SDN_HPENVY"
    Public lDatabase As String = "TestDB"
    Public SqlConnection As New SqlClient.SqlConnection("Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;")
    Public sqlcmd As SqlCommand
    Public sqlda As SqlDataAdapter
    Public sqlds As DataSet


    Public Params As New List(Of SqlParameter)
    Public RecordCount As Integer
    Public Exception As String

    Public Sub ExecQuery(pQuery As String)
        Try
            SqlConnection.Open()

            sqlcmd = New SqlCommand(pQuery, SqlConnection)
            Params.ForEach(Sub(x) sqlcmd.Parameters.Add(x))
            Params.Clear()

            sqlds = New DataSet
            sqlda = New SqlDataAdapter(sqlcmd)
            RecordCount = sqlda.Fill(sqlds)
            SqlConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error in ExecQuery.")
        End Try
        If SqlConnection.State = ConnectionState.Open Then
            SqlConnection.Close()
        End If
    End Sub
    Public Sub ExecCmd(pQuery As String)
        Try
            SqlConnection.Open()
            sqlcmd = New SqlCommand(pQuery, SqlConnection)
            Params.ForEach(Sub(x) sqlcmd.Parameters.Add(x))
            Params.Clear()

            sqlcmd.ExecuteNonQuery()
            SqlConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error in ExecCmd.")
        End Try
        If SqlConnection.State = ConnectionState.Open Then
            SqlConnection.Close()
        End If
    End Sub
End Class
