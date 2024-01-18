Imports System.Web.Security
Imports MySql.Data.MySqlClient
Public Class Form1
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ConnectDatabase()
        cmd = New MySqlCommand("SELECT * FROM users WHERE username=@username AND password=@password", conn)
        cmd.Parameters.AddWithValue("@username", txtUname.Text)
        cmd.Parameters.AddWithValue("@password", txtpwd.Text)
        dr = cmd.ExecuteReader()

        If dr.HasRows Then
            ' Valid credentials
            If dr.Read() Then
                ' Get the role from the database
                Dim role As String = dr("role").ToString()

                ' Create an instance of the Main form
                Dim frm As New Main

                ' Set the username in the Main form
                frm.SetUsername(txtUname.Text)

                ' Set the UserRole property in the Main form
                frm.UserRole = role

                ' Show the Main form
                frm.Show()

                ' Hide the current form
                Me.Hide()
            End If
        Else
            ' Invalid credentials
            MessageBox.Show("ຊື່ແລະລະຫັດບໍ່ຖືກຕ້ອງ")
        End If

        dr.Close()
    End Sub


    Private Sub txtUname_MouseEnter(sender As Object, e As EventArgs) Handles txtUname.MouseEnter
        If txtUname.Text = "Type Your Username" Then
            txtUname.Text = ""
            txtUname.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtUname_MouseLeave(sender As Object, e As EventArgs) Handles txtUname.MouseLeave
        If txtUname.Text = "" Then
            txtUname.Text = "Type Your Username"
            txtUname.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtpwd_MouseEnter(sender As Object, e As EventArgs) Handles txtpwd.MouseEnter
        If txtpwd.Text = "Type Your Password" Then
            txtpwd.Text = ""
            txtpwd.ForeColor = Color.Black
            txtpwd.UseSystemPasswordChar = Not CheckBox1.Checked ' Set initial state
        End If
    End Sub

    Private Sub txtpwd_MouseLeave(sender As Object, e As EventArgs) Handles txtpwd.MouseLeave
        If txtpwd.Text = "" Then
            txtpwd.Text = "Type Your Password"
            txtpwd.ForeColor = Color.Gray
            txtpwd.UseSystemPasswordChar = False ' Reset UseSystemPasswordChar to false
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        txtpwd.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Application.ExitThread()
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Dim frmre As New Register

        ' Set the username in the Main form


        ' Show the Main form
        frmre.Show()


        Me.Hide()
    End Sub
End Class
