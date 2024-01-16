Imports MySql.Data.MySqlClient
Module Module1
    Public strcon As String = "server=localhost; user=root; password=; database=massage; CharSet=utf8;"
    Public conn As New MySqlConnection
    Public da As New MySqlDataAdapter
    Public ds As New DataSet
    Public cmd As New MySqlCommand
    Public dr As MySqlDataReader
    Public username As String = ""
    Public userauthor As String = ""

    Public Sub ConnectDatabase()
        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If
        conn.ConnectionString = strcon
        Try
            conn.Open()
        Catch ex As Exception
            MessageBox.Show("Error while opening the database connection: " & ex.Message)
        End Try
    End Sub
End Module
