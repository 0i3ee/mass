Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Public Class Register
    Private Sub InsertUser()
        Try
            ' Check if txtUname, txtpwd, and Guna2ComboBox1 are not null or empty
            If Not String.IsNullOrEmpty(txtUname.Text) AndAlso Not String.IsNullOrEmpty(txtpwd.Text) AndAlso Guna2ComboBox1.SelectedItem IsNot Nothing Then
                ' Use the ConnectDatabase method from your module
                ConnectDatabase()

                ' Insert data into the users table
                Dim query As String = "INSERT INTO users (username, password, role) VALUES (@Username, @Password, @Role)"
                Using command As New MySqlCommand(query, conn)
                    ' Set parameters
                    command.Parameters.AddWithValue("@Username", txtUname.Text)
                    command.Parameters.AddWithValue("@Password", txtpwd.Text) ' Remember to hash or encrypt the password in a real-world scenario
                    command.Parameters.AddWithValue("@Role", Guna2ComboBox1.SelectedItem.ToString())

                    ' Execute the query
                    command.ExecuteNonQuery()

                    MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    moveb()
                    clear()
                End Using
            Else
                MessageBox.Show("ກະລຸນາປ້ອນຂໍ້ມູນໃຫ້ຄົບ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error while adding user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Close the database connection in the finally block
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        InsertUser()
    End Sub
    Private Sub clear()
        txtUname.Clear()
        txtpwd.Clear()
        Guna2ComboBox1.SelectedIndex = -1
    End Sub
    Private Sub Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtpwd.PasswordChar = "*"c
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Application.ExitThread()
    End Sub
    Private Sub moveb()
        Dim form1Instance As New Form1()

        ' Show Form1
        form1Instance.Show()


        Me.Hide()
    End Sub
    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        moveb()


    End Sub
End Class