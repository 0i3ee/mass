Imports System.Runtime.Remoting
Imports MySql.Data.MySqlClient

Public Class Home
    Private conn As MySqlConnection
    Private Const VerticalMargin As Integer = 10
    Private selectedButton As Button
    Private checkBillPanel As Panel

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectDatabase()
        LoadButtonsFromDatabase()
        Panel1.AutoScroll = True
        Me.Guna2Button2.Visible = True
        Me.Guna2Button2.Location = New System.Drawing.Point(803, 62)

        checkBillPanel = New Panel()
        checkBillPanel.Dock = DockStyle.Right
        checkBillPanel.Width = 300
        checkBillPanel.Visible = False
        Me.Controls.Add(checkBillPanel)
    End Sub

    Private Sub ConnectDatabase()
        Dim connectionString As String = "server=localhost; user=root; password=; database=massage; CharSet=utf8;"
        conn = New MySqlConnection(connectionString)
    End Sub

    Private Sub LoadButtonsFromDatabase()
        Panel1.Controls.Clear()
        Try
            Dim query As String = "SELECT booking_id, customer_name, service_id, time_slot_id, Status FROM bookings"
            Using command As New MySqlCommand(query, conn)
                conn.Open()

                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim serviceName As String = GetServiceName(reader("service_id"))
                        Dim timeSlotName As String = GetTimeSlotName(reader("time_slot_id"))

                        Dim newButton As New Button()
                        CustomizeButtonStyle(newButton)
                        newButton.Text = $" {reader("booking_id")} {reader("customer_name")} {serviceName} {timeSlotName} {reader("Status")}"
                        newButton.TextAlign = ContentAlignment.MiddleLeft
                        AddHandler newButton.Click, AddressOf Button_Click
                        newButton.Dock = DockStyle.Top

                        Panel1.Controls.Add(newButton)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading buttons from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub CustomizeButtonStyle(button As Button)
        button.Font = New System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        button.Size = New System.Drawing.Size(287, 198)
        button.UseVisualStyleBackColor = True
        button.ForeColor = Color.Black
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = DirectCast(sender, Button)
        ShowCheckBillDetails(clickedButton)
    End Sub


    Private Sub ShowCheckBillDetails(clickedButton As Button)
        Dim bookingId As Integer = Convert.ToInt32(clickedButton.Text.Split(" "c)(1))
        Dim bookingDetailsForm As New BookingDetailsForm(bookingId)
        bookingDetailsForm.ShowDialog()
    End Sub



    Private Sub AddRowToDataTable(table As DataTable, field As String, value As String)
        Dim newRow As DataRow = table.NewRow()
        newRow("Field") = field
        newRow("Value") = value
        table.Rows.Add(newRow)
    End Sub



    Private Function GetStaffName(staffId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT Name FROM staff WHERE staff_id = @ServiceId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ServiceId", staffId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown staff"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown staff"
        End Try
    End Function

    Private Function GetServiceName(serviceId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT service_name FROM services WHERE service_id = @ServiceId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ServiceId", serviceId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown Service"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown Service"
        End Try
    End Function

    Private Function GetTimeSlotName(timeSlotId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT time_slot FROM time_slots WHERE time_slot_id = @TimeSlotId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@TimeSlotId", timeSlotId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown Time Slot"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown Time Slot"
        End Try
    End Function

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        LoadButtonsFromDatabase()
    End Sub
End Class
