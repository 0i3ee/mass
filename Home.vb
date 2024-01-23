Imports MySql.Data.MySqlClient

Public Class Home
    Private conn As MySqlConnection
    Private verticalPosition As Integer = 12
    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
<<<<<<< HEAD
        ShowBookingInfo()
    End Sub

    Private Sub ShowBookingInfo()
        ConnectDatabase()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "SELECT booking_id, customer_name, customer_phone, Status FROM bookings"
            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        ' Access the data using reader.GetString or other appropriate methods
                        Dim bookingId As Integer = reader.GetInt32(reader.GetOrdinal("booking_id"))
                        Dim customerName As String = reader.GetString(reader.GetOrdinal("customer_name"))
                        Dim customerPhone As String = reader.GetString(reader.GetOrdinal("customer_phone"))
                        Dim status As String = reader.GetString(reader.GetOrdinal("Status"))
=======
        ConnectDatabase()
        LoadButtonsFromDatabase()
>>>>>>> 4efe99dc53f69d80009f99d7305abba2ef52a4c7

                        ' Display the data in labels
                        Label4.Text = bookingId.ToString()
                        Label6.Text = customerName
                        Label5.Text = customerPhone
                        Label7.Text = status

                        ' Optionally, you might want to add a delay or process the data in some way
                        ' System.Threading.Thread.Sleep(1000) ' 1000 milliseconds delay
                    End While
                End Using
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

<<<<<<< HEAD

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Update the status in the database
        UpdateStatusToPending()

        ' Refresh the label with the updated status
        RefreshStatusLabel()
    End Sub

    Private Sub UpdateStatusToPending()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "UPDATE bookings SET Status = 'pending' WHERE booking_id = @bookingId"
            Using command As New MySqlCommand(query, conn)
                ' If booking_id uniquely identifies the record, assume bookingId is an integer
                command.Parameters.AddWithValue("@bookingId", Label4.Text) ' Replace 123 with the actual booking_id value
                command.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RefreshStatusLabel()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "SELECT Status FROM bookings WHERE booking_id = @bookingId"
            Using command As New MySqlCommand(query, conn)
                ' If booking_id uniquely identifies the record, assume bookingId is an integer
                command.Parameters.AddWithValue("@bookingId", 123) ' Replace 123 with the actual booking_id value

                ' Read the status from the database
                Dim updatedStatus As String = Convert.ToString(command.ExecuteScalar())

                ' Display the updated status in the label
                Label7.Text = updatedStatus
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error retrieving updated status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

=======
    Private Sub ConnectDatabase()
        Dim connectionString As String = "server=localhost; user=root; password=; database=massage; CharSet=utf8;"
        conn = New MySqlConnection(connectionString)
    End Sub

    Private Sub LoadButtonsFromDatabase()
        Try
            Dim query As String = "SELECT customer_name, service_id, time_slot_id, Status FROM bookings"
            Using command As New MySqlCommand(query, conn)
                conn.Open()

                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim serviceName As String = GetServiceName(reader("service_id"))
                        Dim timeSlotName As String = GetTimeSlotName(reader("time_slot_id"))

                        Dim newButton As New Button()
                        CustomizeButtonStyle(newButton)
                        newButton.Text = $" {reader("customer_name")}                                 {serviceName}    {timeSlotName}               {reader("Status")}"
                        newButton.TextAlign = ContentAlignment.MiddleLeft
                        AddHandler newButton.Click, AddressOf Button_Click
                        newButton.Location = New Point(verticalPosition, 12)

                        ' Increment the vertical position for the next button
                        verticalPosition += 300

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
        button.Location = New System.Drawing.Point(12, 12)
        button.UseVisualStyleBackColor = True
        button.ForeColor = Color.Black
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = DirectCast(sender, Button)
        MessageBox.Show("Button Clicked: " & clickedButton.Text, "Button Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

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
>>>>>>> 4efe99dc53f69d80009f99d7305abba2ef52a4c7
End Class