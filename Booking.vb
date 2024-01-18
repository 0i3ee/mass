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
        LoadTime()



    End Sub
    Private Sub LoadTime()
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Query to select all records from the Time table
            Dim query As String = "SELECT * FROM Time"

            ' Using a MySqlCommand to execute the query
            Using cmd As New MySqlCommand(query, conn)
                ' Using a MySqlDataReader to read the results of the query
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing items in the combo box
                    Guna2ComboBox3.Items.Clear()

                    ' Loop through the records and add each Time_name to the combo box
                    While reader.Read()
                        Guna2ComboBox3.Items.Add(reader("Time_name").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error loading time: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub



    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Try
            ConnectDatabase()

            ' Check if the connection is not null and is closed, then open it
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim selectedStaffName As String = Guna2ComboBox1.SelectedItem.ToString()
            Dim selectedService As String = Guna2ComboBox2.SelectedItem.ToString()
            Dim selectedtimeslot As String = Guna2ComboBox4.SelectedItem.ToString()

            ' Query to find StaffId based on the selected staff name
            Dim staffquery As String = "SELECT staff_Id FROM staff WHERE Name = @SelectedStaffName"

            Using staffcommand As MySqlCommand = New MySqlCommand(staffquery, conn)
                staffcommand.Parameters.AddWithValue("@SelectedStaffName", selectedStaffName)
                Dim selectedStaffId As Integer = Convert.ToInt32(staffcommand.ExecuteScalar())

                Dim Servicequery As String = "SELECT service_id FROM services WHERE service_name = @SelectedStaffName"
                Using Servicecommand As MySqlCommand = New MySqlCommand(Servicequery, conn)
                    Servicecommand.Parameters.AddWithValue("@SelectedStaffName", selectedService)
                    Dim selectedServiceID As Integer = Convert.ToInt32(Servicecommand.ExecuteScalar())


                    Dim timeslotquery As String = "SELECT time_slot_id FROM time_slots WHERE time_slot = @SelectedStaffName"
                    Using timeslotcommand As MySqlCommand = New MySqlCommand(timeslotquery, conn)
                        timeslotcommand.Parameters.AddWithValue("@SelectedStaffName", selectedtimeslot)
                        Dim slectedtimeslotID As Integer = Convert.ToInt32(timeslotcommand.ExecuteScalar())


                        Dim bookingQuery As String = "INSERT INTO bookings (staff_id, service_id, booking_date, customer_name, customer_phone, time_slot_id) " &
                                     "VALUES (@StaffId, @ServiceId, NOW(), @CustomerName, @CustomerPhone, @TimeSlotId)"
                        Using bookingCommand As MySqlCommand = New MySqlCommand(bookingQuery, conn)
                            bookingCommand.Parameters.AddWithValue("@StaffId", selectedStaffId)
                            bookingCommand.Parameters.AddWithValue("@ServiceId", selectedServiceID)

                            bookingCommand.Parameters.AddWithValue("@CustomerName", Guna2TextBox1.Text)
                            bookingCommand.Parameters.AddWithValue("@CustomerPhone", Guna2TextBox3.Text)
                            bookingCommand.Parameters.AddWithValue("@TimeSlotId", slectedtimeslotID)

                            bookingCommand.ExecuteNonQuery()
                            ClearComboBoxes()
                            ClearTextBoxes()
                            PopulateStaffComboBox()
                        End Using
                    End Using
                End Using
            End Using
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
    Private Sub LoadTimeslot(selectedTime As String)
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Query to select time slots based on the provided time
            Dim query As String = "SELECT * FROM time_slots WHERE time_id = (SELECT time_id FROM Time WHERE Time_name = @SelectedTime)"

            ' Using a MySqlCommand to execute the query
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SelectedTime", selectedTime)

                ' Using a MySqlDataReader to read the results of the query
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing items in the combo box
                    Guna2ComboBox4.Items.Clear()
                    ' Loop through the records and add each time slot to the combo box
                    While reader.Read()
                        Dim time_slot As String = reader("time_slot").ToString()
                        Guna2ComboBox4.Items.Add(time_slot)
                    End While
                End Using
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error loading time slots: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Guna2ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox3.SelectedIndexChanged
        ' Assuming the selected time is related to staff availability
        ' You might need to adjust this logic based on how time is associated with staff availability
        Dim selectedTime As String = Guna2ComboBox3.SelectedItem.ToString()

        ' Get the selected day from the DateTimePicker
        Dim selectedDay As String = DateTimePicker1.Value.ToString("dddd")

        ' Populate staff based on the selected time and day
        PopulateStaffComboBox(selectedTime, selectedDay)

        LoadTimeslot(selectedTime)

    End Sub

    Private Sub PopulateStaffComboBox(selectedTime As String, selectedDay As String)
        Guna2ComboBox1.Items.Clear()

        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff with the specified availability
            ' Check if the selected day matches any entry in the 'Day' column
            Dim query As String = "SELECT Name FROM staff WHERE Status = 'available' AND Time = @Time AND WorkDays LIKE @Day"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Time", selectedTime)
                cmd.Parameters.AddWithValue("@Day", "%" & selectedDay & "%")

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


    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        ' You can use the selected day in other parts of your code if needed
        Dim selectedDay As String = DateTimePicker1.Value.ToString("dddd")
    End Sub


End Class
