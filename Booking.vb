Imports System.Web.UI
Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class Booking

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub Booking_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()

        ' Populate ComboBoxes
        PopulateServicesComboBox()

    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Try
            ConnectDatabase()

            ' Check if the connection is not null and is closed, then open it
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            ' Make sure the connection is open before proceeding
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                ' Get the staff_id for the selected staff
                Dim selectedStaffItem As StaffItem = DirectCast(Guna2ComboBox1.SelectedItem, StaffItem)
                Dim selectedStaffId As Integer = selectedStaffItem.StaffId

                ' Get the service_id for the selected service
                Dim selectedServiceItem As ServiceItem = DirectCast(Guna2ComboBox2.SelectedItem, ServiceItem)
                Dim selectedServiceId As Integer = selectedServiceItem.ServiceId

                ' Insert into bookings table
                Dim bookingQuery As String = "INSERT INTO bookings (staff_id, service_id, booking_date, customer_name, customer_phone) " &
                                        "VALUES (@StaffId, @ServiceId, NOW(), @CustomerName, @CustomerPhone)"

                ' Insert into bills table


                Using command As MySqlCommand = New MySqlCommand(bookingQuery, conn)
                    ' Insert into bookings table
                    command.Parameters.AddWithValue("@StaffId", selectedStaffId)
                    command.Parameters.AddWithValue("@ServiceId", selectedServiceId)
                    command.Parameters.AddWithValue("@CustomerName", Guna2TextBox1.Text) ' Replace with actual customer name
                    command.Parameters.AddWithValue("@CustomerPhone", Guna2TextBox3.Text) ' Replace with actual customer phone

                    ' Execute the command for booking insertion
                    command.ExecuteNonQuery()

                    ' Retrieve the last inserted booking_id
                    Dim lastBookingId As Integer
                    Using getLastBookingIdCmd As MySqlCommand = New MySqlCommand("SELECT LAST_INSERT_ID()", conn)
                        lastBookingId = Convert.ToInt32(getLastBookingIdCmd.ExecuteScalar())
                    End Using

                    ' Update the status of the selected staff to 'not available'
                    Dim updateStatusQuery As String = "UPDATE staff SET Status = 'not_available' WHERE staff_id = @StaffId"

                    Using updateStatusCommand As MySqlCommand = New MySqlCommand(updateStatusQuery, conn)
                        updateStatusCommand.Parameters.AddWithValue("@StaffId", selectedStaffId)
                        updateStatusCommand.ExecuteNonQuery()
                    End Using

                    Dim billQuery As String = "INSERT INTO bills (booking_id, total_amount, payment_status) " &
                                              "VALUES (@BookingId, @TotalAmount, @PaymentStatus)"

                    ' Insert into bills table
                    Using billCommand As MySqlCommand = New MySqlCommand(billQuery, conn)
                        billCommand.Parameters.AddWithValue("@BookingId", lastBookingId)
                        ' Replace with the actual total amount and payment status
                        billCommand.Parameters.AddWithValue("@TotalAmount", Guna2TextBox2.Text)
                        billCommand.Parameters.AddWithValue("@PaymentStatus", "unpaid")

                        ' Execute the command for bill insertion
                        billCommand.ExecuteNonQuery()
                    End Using

                    MessageBox.Show("Booking and Bill added successfully.")
                    ClearComboBoxes()
                    ClearTextBoxes()
                    PopulateStaffComboBox()



                End Using
            Else
                MessageBox.Show("The database connection is not open.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub
    Private Sub ClearComboBoxes()
        If Guna2ComboBox1.Items.Count > 0 Then
            Guna2ComboBox1.SelectedIndex = 0 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox1.SelectedItem = Nothing
        End If

        If Guna2ComboBox2.Items.Count > 0 Then
            Guna2ComboBox2.SelectedIndex = 0 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox2.SelectedItem = Nothing
        End If
    End Sub

    Private Sub ClearTextBoxes()
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
        ' Add more TextBoxes as needed
    End Sub
    Private Sub PopulateServicesComboBox()
        Guna2ComboBox2.Items.Clear()
        Try
            ' Use the existing connection from the module
            ConnectDatabase()
            Dim query As String = "SELECT service_id, service_name, price FROM services"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ' Create a custom class to store service_id, service_name, and price
                        Dim serviceItem As New ServiceItem()
                        serviceItem.ServiceId = Convert.ToInt32(reader("service_id"))
                        serviceItem.ServiceName = reader("service_name").ToString()
                        serviceItem.Price = Convert.ToDecimal(reader("price"))

                        ' Add serviceItem to ComboBox
                        Guna2ComboBox2.Items.Add(serviceItem)

                        ' Optionally, you can set the display text
                        ' Guna2ComboBox2.Items.Add(serviceItem.ServiceName)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Make sure to close the connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    Private Sub PopulateStaffComboBox()
        Guna2ComboBox1.Items.Clear()

        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff_id along with Name
            Dim query As String = "SELECT staff_id, Name FROM staff WHERE Status = 'available'"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ' Create a custom class to store staff_id and Name
                        Dim staffItem As New StaffItem()
                        staffItem.StaffId = Convert.ToInt32(reader("staff_id"))
                        staffItem.Name = reader("Name").ToString()

                        ' Add staffItem to ComboBox
                        Guna2ComboBox1.Items.Add(staffItem)

                        ' Optionally, you can set the display text
                        ' Guna2ComboBox1.Items.Add(staffItem.Name)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Make sure to close the connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    Private Sub Guna2ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox2.SelectedIndexChanged
        ' Use the existing connection from the module
        ConnectDatabase()

        Try
            Dim selectedService As String = Guna2ComboBox2.SelectedItem.ToString()

            ' Query to retrieve the price of the selected service
            Dim query As String = "SELECT price FROM services WHERE service_name = @ServiceName"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ServiceName", selectedService)
                Dim price As Object = cmd.ExecuteScalar()

                ' Display the price in Guna2TextBox2
                If price IsNot Nothing AndAlso Not IsDBNull(price) Then
                    Guna2TextBox2.Text = price.ToString()
                Else
                    Guna2TextBox2.Text = ""
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Make sure to close the connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Guna2ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox3.SelectedIndexChanged
        ' Assuming the selected time is related to staff availability
        ' You might need to adjust this logic based on how time is associated with staff availability
        Dim selectedTime As String = Guna2ComboBox3.SelectedItem.ToString()

        ' Populate staff based on the selected time
        PopulateStaffComboBox(selectedTime)
    End Sub

    Private Sub PopulateStaffComboBox(selectedTime As String)
        Guna2ComboBox1.Items.Clear()

        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff with the specified availability
            Dim query As String = "SELECT Name FROM staff WHERE Status = 'available' AND Time = @Time"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Time", selectedTime)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ' Add staff Name to ComboBox
                        Guna2ComboBox1.Items.Add(reader("Name").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Make sure to close the connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

End Class
