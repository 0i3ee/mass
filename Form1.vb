Imports MySql.Data.MySqlClient
Public Class Form1
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ConnectDatabase()
        cmd = New MySqlCommand("select * from users where username=@username and password=@password", conn)
        cmd.Parameters.AddWithValue("@username", txtUname.Text)
        cmd.Parameters.AddWithValue("@password", txtpwd.Text)
        dr = cmd.ExecuteReader()

        If (dr.HasRows) Then
            ' Successful login
            MessageBox.Show("OK")

            ' Create an instance of the Main form
            Dim frm As New Main

            ' Set the username in the Main form
            frm.SetUsername(txtUname.Text)

            ' Show the Main form
            frm.Show()


            Me.Hide()
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
        End If
    End Sub

    Private Sub txtpwd_MouseLeave(sender As Object, e As EventArgs) Handles txtpwd.MouseLeave
        If txtpwd.Text = "" Then
            txtpwd.Text = "Type Your Password"
            txtpwd.ForeColor = Color.Gray
        End If
    End Sub
End Class
