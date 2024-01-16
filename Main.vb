Public Class Main
    Dim sidebar As String = "Close"
    Dim Employsidebar As String = "Close"
    Dim bilsidebar As String = "Close"

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        emsidebar.Height = 0
        billsidebar.Height = 0

    End Sub
    Public Sub SetUsername(username As String)
        Label1.Text = username
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        If sidebar = "open" Then
            leftside.Width += 10
            If leftside.Width >= 240 Then
                sidebar = "Close"
                Timer1.Stop()
            End If
        Else
            leftside.Width -= 10
            If leftside.Width <= 60 Then
                sidebar = "open"
                Timer1.Stop()
            End If
        End If



    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Timer1.Start()
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Timer2.Start()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Employsidebar = "open" Then
            emsidebar.Height += 10
            If emsidebar.Height >= 140 Then
                Employsidebar = "Close"
                Timer2.Stop()
            End If
        Else
            emsidebar.Height -= 10
            If emsidebar.Height <= 0 Then
                Employsidebar = "open"
                Timer2.Stop()
            End If
        End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        If bilsidebar = "open" Then
            billsidebar.Height += 10
            If billsidebar.Height >= 70 Then
                bilsidebar = "Close"
                Timer3.Stop()
            End If
        Else
            billsidebar.Height -= 10
            If billsidebar.Height <= 0 Then
                bilsidebar = "open"
                Timer3.Stop()
            End If
        End If
    End Sub

    Private Sub Guna2Button9_Click(sender As Object, e As EventArgs) Handles Guna2Button9.Click
        Timer3.Start()
    End Sub
    Sub switchPanel(ByVal panel As Form)
        Panel1.Controls.Clear()
        panel.TopLevel = False
        Panel1.Controls.Add(panel)
        panel.Show()
    End Sub
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        switchPanel(Home)
    End Sub

    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        Dim employeeForm As New Employee

        ' Call a method or set a property in the Employee form to send data
        employeeForm.SetEmployeeName(Label1.Text)

        ' Switch to the Employee form
        switchPanel(employeeForm)
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        switchPanel(list)
    End Sub

    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        switchPanel(Edit)
    End Sub

    Private Sub Guna2Button8_Click(sender As Object, e As EventArgs) Handles Guna2Button8.Click
        switchPanel(Delete)
    End Sub
End Class