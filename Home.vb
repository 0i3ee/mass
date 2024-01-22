Imports MySql.Data.MySqlClient

Public Class Home
    Private conn As MySqlConnection
    Private verticalPosition As Integer = 12
    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectDatabase()
        LoadButtonsFromDatabase()

    End Sub

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

        ' ... (Similar code for other labels)

        ' Increment the counter for the next set of labels
        labelCounter += 4 ' Incremented by 4 to accommodate the new column
        End While
        Else
        ' No bookings found
        MessageBox.Show("No bookings found with status 'book'.")
        End If

        ' Close the reader
        reader.Close()
        End Using
        Catch ex As Exception
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
        ' Close the reader
        reader.Close()
        End Using
    Catch ex As Exception
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Check if there is at least one booking with status 'pending'
            If HasBookingsWithStatusa(conn, "pending") Then
                ' Show the form to check the bill
                ' Pass the selected booking information to the CheckBillForm
                Dim selectedBooking As BookingInfo = bookingList.FirstOrDefault(Function(x) x.Status = "pending")
                If selectedBooking IsNot Nothing Then
                    Dim checkBillForm As New CheckBillForm(selectedBooking)
                    checkBillForm.ShowDialog()
                Else
                    MessageBox.Show("No pending bookings found.")
                End If
            Else
                ' No bookings found with status 'pending'
                MessageBox.Show("No bookings found with status 'pending'.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bill: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub
                ' Close the reader
                reader.Close()
            End Using
    Catch ex As Exception
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Check if there is at least one booking with status 'pending'
            If HasBookingsWithStatusa(conn, "pending") Then
                ' Show the form to check the bill
                ' Pass the selected booking information to the CheckBillForm
                Dim selectedBooking As BookingInfo = bookingList.FirstOrDefault(Function(x) x.Status = "pending")
                If selectedBooking IsNot Nothing Then
                    Dim checkBillForm As New CheckBillForm(selectedBooking)
                    checkBillForm.ShowDialog()
                Else
                    MessageBox.Show("No pending bookings found.")
                End If
            Else
                ' No bookings found with status 'pending'
                MessageBox.Show("No bookings found with status 'pending'.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bill: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub
                ' Close the reader
                reader.Close()
            End Using
    Catch ex As Exception
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Check if there is at least one booking with status 'pending'
            If HasBookingsWithStatusa(conn, "pending") Then
                ' Show the form to check the bill
                ' Pass the selected booking information to the CheckBillForm
                Dim selectedBooking As BookingInfo = bookingList.FirstOrDefault(Function(x) x.Status = "pending")
                If selectedBooking IsNot Nothing Then
                    Dim checkBillForm As New CheckBillForm(selectedBooking)
                    checkBillForm.ShowDialog()
                Else
                    MessageBox.Show("No pending bookings found.")
                End If
            Else
                ' No bookings found with status 'pending'
                MessageBox.Show("No bookings found with status 'pending'.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bill: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub
    Dim clickedButton As Button = DirectCast(sender, Button)
        MessageBox.Show("Button Clicked: " & clickedButton.Text, "Button Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Check if there is at least one booking with status 'pending'
            If HasBookingsWithStatusa(conn, "pending") Then
                ' Show the form to check the bill
                ' Pass the selected booking information to the CheckBillForm
                Dim selectedBooking As BookingInfo = bookingList.FirstOrDefault(Function(x) x.Status = "pending")
                If selectedBooking IsNot Nothing Then
                    Dim checkBillForm As New CheckBillForm(selectedBooking)
                    checkBillForm.ShowDialog()
                Else
                    MessageBox.Show("No pending bookings found.")
                End If
            Else
                ' No bookings found with status 'pending'
                MessageBox.Show("No bookings found with status 'pending'.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bill: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

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
End Class
