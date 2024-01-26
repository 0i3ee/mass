Public Class Main
    Dim sidebar As String = "Close"
    Dim Employsidebar As String = "Close"
    Dim bilsidebar As String = "Close"
    Private _userRole As String



    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CustomizeUI(userRole)
    End Sub
    Public Sub SetUsername(username As String)
        Label1.Text = username
    End Sub
    Public Property UserRole As String
        Get
            Return _userRole
        End Get
        Set(value As String)
            _userRole = value
            ' Call a method to customize the UI based on the user's role
            CustomizeUI(_userRole)
        End Set
    End Property
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

    Private Sub Timer3_Tick(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2Button9_Click(sender As Object, e As EventArgs) Handles Guna2Button9.Click
        switchPanel(bill_History)
    End Sub

    Public Sub switchPanel(ByVal panel As Form)

        If Edit IsNot Nothing AndAlso Edit.Visible AndAlso Not Edit.IsDisposed Then
            Edit.Close()
        End If
        If Delete IsNot Nothing AndAlso Delete.Visible AndAlso Not Delete.IsDisposed Then
            Delete.Close()
        End If
        If list IsNot Nothing AndAlso list.Visible AndAlso Not list.IsDisposed Then
            list.Close()
        End If
        If Booking IsNot Nothing AndAlso Booking.Visible AndAlso Not Booking.IsDisposed Then
            Booking.Close()
        End If
        If Home IsNot Nothing AndAlso Home.Visible AndAlso Not Home.IsDisposed Then
            Home.Close()
        End If

        Panel1.Controls.Clear()
        Dim newForm As Form = DirectCast(Activator.CreateInstance(panel.GetType()), Form)
        newForm.TopLevel = False
        Panel1.Controls.Add(newForm)
        newForm.Show()


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

    Private Sub CustomizeUI(userRole As String)
        ' For example, show or hide buttons based on the user's role
        If userRole = "admin" Then
            ' Admin-specific functionality
            Guna2Button2.Visible = True
            Guna2Button3.Visible = True
            Guna2Button4.Visible = True
            Guna2Button9.Visible = True
            Guna2Button10.Visible = True
            Guna2Button15.Visible = True
        ElseIf userRole = "staff" Then

            Guna2Button2.Visible = True
            Guna2Button3.Visible = True
            Guna2Button4.Visible = False
            emsidebar.Visible = False

            Guna2Button5.Visible = False
            Guna2Button6.Visible = False
            Guna2Button7.Visible = False
            Guna2Button8.Visible = False

            Guna2Button9.Visible = True
            Guna2Button10.Visible = True
            Guna2Button15.Visible = False
        End If
    End Sub


    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        switchPanel(list)
    End Sub

    Private editForm As Edit
    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        switchPanel(Edit)
    End Sub

    Private Sub Guna2Button8_Click(sender As Object, e As EventArgs) Handles Guna2Button8.Click
        Edit.Close()
        list.Close()

        switchPanel(Delete)
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        switchPanel(Booking)
    End Sub



    Private Sub Guna2Button12_Click(sender As Object, e As EventArgs)
        switchPanel(bill_History)
    End Sub

    Private Sub Guna2Button15_Click(sender As Object, e As EventArgs) Handles Guna2Button15.Click
        switchPanel(Service)
    End Sub

    Private Sub Guna2Button10_Click(sender As Object, e As EventArgs) Handles Guna2Button10.Click
        switchPanel(Overall)
    End Sub

    Private Sub Guna2Button14_Click(sender As Object, e As EventArgs) Handles Guna2Button14.Click
        ' Show a message box with OK and Cancel buttons
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to sign out?", "Sign Out", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        ' Check if the user clicked OK
        If result = DialogResult.OK Then
            ' Perform actions related to signing out

            ' Close the current form
            Me.Close()

            ' Open the login form (replace LoginForm with the actual name of your login form)
            Dim loginForm As New Form1()
            loginForm.Show()
        End If
    End Sub

    Private Sub Guna2Button11_Click(sender As Object, e As EventArgs) Handles Guna2Button11.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Exit?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        ' Check if the user clicked OK
        If result = DialogResult.OK Then
            Application.ExitThread()
        End If

    End Sub
End Class