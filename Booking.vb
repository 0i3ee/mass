Imports MySql.Data.MySqlClient

Public Class Booking

    Dim selectedDay As String


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub Booking_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        LoadTime()
        PopulateServicesComboBox()
        LoadTime()

        DateTimePicker1.Value = DateTime.Now
        ' Disable ComboBoxes when DateTimePicker has no value
        Guna2ComboBox1.Enabled = False
        Guna2ComboBox3.Enabled = False
        Guna2ComboBox4.Enabled = False



    End Sub
    Private Sub LoadTime()
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Query to select all records from the Time table
            Dim query As String = "SELECT * FROM Time"

            Using cmd As New MySqlCommand(query, conn)
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

    Private Sub PopulateServicesComboBox()
        Guna2ComboBox2.Items.Clear()
        Try

            ' Use the existing connection from the module
            ConnectDatabase()
            Dim query As String = "SELECT service_id, service_name, price FROM services"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()

                    Guna2ComboBox3.Items.Clear()
                    While reader.Read()
                        Guna2ComboBox2.Items.Add(reader("service_name").ToString())
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

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Dim selectedDateTime As DateTime? = DateTimePicker1.Value

        If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) Then
            MessageBox.Show("ກະລຸນາປ້ອນຊື່.")
            Return
        End If

        If String.IsNullOrWhiteSpace(Guna2TextBox3.Text) Then
            MessageBox.Show("ກະລຸນາປ້ອນເບີໂທ.")
            Return
        End If

        ' Assuming you have ComboBox named 'Guna2ComboBox1' for time slot selection
        If Guna2ComboBox2.SelectedIndex = -1 Then
            MessageBox.Show("ກະລຸນາເລືອກບໍລິການ.")
            Return
        End If
        If selectedDateTime = DateTime.MinValue Then
            ' If no date and time are selected, disable ComboBoxes
            Guna2ComboBox1.Enabled = False
            Guna2ComboBox2.Enabled = False
            Guna2ComboBox4.Enabled = False
        Else
            ' If a date and time are selected, enable ComboBoxes
            Guna2ComboBox1.Enabled = True
            Guna2ComboBox2.Enabled = True
            Guna2ComboBox4.Enabled = True
        End If
        If selectedDateTime IsNot Nothing AndAlso selectedDateTime.Value.Date < DateTime.Now.Date Then
            MessageBox.Show("ກະລຸນາເລືອກວັນທີໃຫ້ຖືກຕ້ອງ")
            Return
        End If
        If Guna2ComboBox3.SelectedIndex = -1 Then
            MessageBox.Show("ກະລຸນາເລືອກກະ.")
            Return
        End If
        If Guna2ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("ກະລຸນາເລືອກພະນັກງານນວດ.")
            Return
        End If
        If Guna2ComboBox4.SelectedIndex = -1 Then
            MessageBox.Show("ກະລຸນາເລືອກໂມງນວດ.")
            Return
        End If
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


                        Dim bookingQuery As String = "INSERT INTO bookings (staff_id, service_id, booking_date, customer_name, customer_phone,Status, time_slot_id,Datemassage) " &
                                     "VALUES (@StaffId, @ServiceId, NOW(), @CustomerName, @CustomerPhone,'book', @TimeSlotId,@Datemassage)"
                        Using bookingCommand As MySqlCommand = New MySqlCommand(bookingQuery, conn)
                            bookingCommand.Parameters.AddWithValue("@StaffId", selectedStaffId)
                            bookingCommand.Parameters.AddWithValue("@ServiceId", selectedServiceID)

                            bookingCommand.Parameters.AddWithValue("@CustomerName", Guna2TextBox1.Text)
                            bookingCommand.Parameters.AddWithValue("@CustomerPhone", Guna2TextBox3.Text)
                            bookingCommand.Parameters.AddWithValue("@TimeSlotId", slectedtimeslotID)
                            Dim daytime As DateTime = DateTimePicker1.Value
                            bookingCommand.Parameters.AddWithValue("@Datemassage", daytime)

                            bookingCommand.ExecuteNonQuery()
                            ClearComboBoxes()
                            ClearTextBoxes()
                            Guna2ComboBox1.Enabled = False
                            Guna2ComboBox3.Enabled = False
                            Guna2ComboBox4.Enabled = False
                            DateTimePicker1.Value = Date.Now()
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
            Guna2ComboBox1.SelectedIndex = -1 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox1.SelectedItem = Nothing
        End If

        If Guna2ComboBox2.Items.Count > 0 Then
            Guna2ComboBox2.SelectedIndex = -1 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox2.SelectedItem = Nothing
        End If

        If Guna2ComboBox4.SelectedItem > 0 Then
            Guna2ComboBox4.SelectedIndex = -1 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox4.SelectedItem = Nothing
        End If

        If Guna2ComboBox3.Items.Count > 0 Then
            Guna2ComboBox3.SelectedIndex = -1 ' Set to the first item or -1 if you want no selection
        Else
            Guna2ComboBox3.SelectedItem = Nothing
        End If
    End Sub

    Private Sub ClearTextBoxes()
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
        ' Add more TextBoxes as needed
    End Sub



    Private Sub Guna2ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox2.SelectedIndexChanged
        ' Use the existing connection from the module
        ConnectDatabase()

        Try
            Dim selectedService As String = If(Guna2ComboBox2.SelectedItem IsNot Nothing, Guna2ComboBox2.SelectedItem.ToString(), "")

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
    Private Sub LoadTimeslot(selectedTime As String, selectedstaff As String)
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            Dim selectedstaffid As Integer = 0
            Dim daytime As DateTime = DateTimePicker1.Value
            Dim formattedDateTime As String = daytime.ToString("yyyy-MM-dd")

            ' Retrieve staff_id based on staff name
            Dim selectedstaffidQuery As String = "SELECT staff_id FROM staff WHERE Name = @SelectedStaffName"
            Using staffidcommand As MySqlCommand = New MySqlCommand(selectedstaffidQuery, conn)
                staffidcommand.Parameters.AddWithValue("@SelectedStaffName", selectedstaff)
                selectedstaffid = Convert.ToInt32(staffidcommand.ExecuteScalar())
            End Using

            ' Check the number of existing bookings for the selected staff, date, and time slot
            Dim existingBookingQuery As String = "SELECT COUNT(*) FROM bookings 
                                              WHERE staff_id = @SelectedStaffId 
                                              AND time_slot_id = @SelectedTimeSlot
                                              AND (booking_date = @SelectedDate OR Datemassage = @SelectedDate)"
            Using existingBookingCommand As MySqlCommand = New MySqlCommand(existingBookingQuery, conn)
                existingBookingCommand.Parameters.AddWithValue("@SelectedStaffId", selectedstaffid)
                existingBookingCommand.Parameters.AddWithValue("@SelectedTimeSlot", selectedTime)
                existingBookingCommand.Parameters.AddWithValue("@SelectedDate", formattedDateTime)
                Dim existingBookingCount As Integer = Convert.ToInt32(existingBookingCommand.ExecuteScalar())

                ' If the staff has already booked the selected time slot for the selected date, display a message and exit
                'If existingBookingCount > 0 Then
                '    MessageBox.Show("This staff member has already booked the selected time slot for the selected date.")
                '    Return
                'End If
            End Using

            ' Retrieve available time slots for the selected time and staff on the selected date
            Dim query As String = "SELECT time_slots.time_slot_id, time_slots.time_slot 
                               FROM time_slots 
                               LEFT JOIN bookings ON time_slots.time_slot_id = bookings.time_slot_id 
                               LEFT JOIN staff ON staff.time_id = time_slots.time_id 
                               LEFT JOIN time ON staff.time_id = time.time_id 
                               WHERE (time_slots.time_id = @SelectedTime OR staff.staff_id = @SelectedStaffId)
                               AND (time_slots.time_slot_id IS NULL OR time_slots.time_slot_id NOT IN 
                                   (SELECT time_slot_id FROM bookings 
                                    WHERE (booking_date = @SelectedDate OR Datemassage = @SelectedDate)
                                    AND (staff_id = @SelectedStaffId OR staff_id IS NULL))
                               AND time_slots.time_slot_id NOT IN 
                                   (SELECT time_slot_id FROM bookings 
                                    WHERE staff_id = @SelectedStaffId AND Status = 'End'))"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SelectedTime", selectedTime)
                cmd.Parameters.AddWithValue("@SelectedStaffId", selectedstaffid)
                cmd.Parameters.AddWithValue("@SelectedDate", formattedDateTime)

                ' Using a MySqlDataReader to read the results of the query
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing items in the combo box
                    Guna2ComboBox4.Items.Clear()
                    ' Loop through the records and add each available time slot to the combo box
                    While reader.Read()
                        Dim time_slot_id As Integer = Convert.ToInt32(reader("time_slot_id"))
                        Dim time_slot As String = reader("time_slot").ToString()

                        ' Check if the time_slot is already in the combo box
                        If Not Guna2ComboBox4.Items.Contains(time_slot) Then
                            ' Add the time_slot to the combo box
                            Guna2ComboBox4.Items.Add(time_slot)
                        End If
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







    Public Sub Guna2ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox3.SelectedIndexChanged

        Dim selectedDay As String = DateTimePicker1.Value.ToString("dddd")
        ConnectDatabase()

        Try
            ' Check if a time is selected in Guna2ComboBox3
            If Guna2ComboBox3.SelectedItem IsNot Nothing Then
                Dim selectedTimeID As String = Guna2ComboBox3.SelectedItem.ToString()

                Dim timeslotquery As String = "SELECT time_id FROM time WHERE Time_name = @SelectedStaffName"
                Using timeslotcommand As MySqlCommand = New MySqlCommand(timeslotquery, conn)
                    timeslotcommand.Parameters.AddWithValue("@SelectedStaffName", selectedTimeID)
                    Dim selectedTime As Integer = Convert.ToInt32(timeslotcommand.ExecuteScalar())

                    ' Enable Guna2ComboBox1 since a time is selected
                    Guna2ComboBox1.Enabled = True

                    ' Populate Guna2ComboBox1 based on the selected time and day
                    PopulateStaffComboBox(selectedTime, selectedDay)
                End Using
            Else
                ' If no time is selected, disable Guna2ComboBox1
                Guna2ComboBox1.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try




    End Sub

    Private Sub PopulateStaffComboBox(selectedTime As String, selectedDay As String)
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff with the specified availability
            ' Check if the selected day matches any entry in the 'Day' column
            Dim query As String = "SELECT Name FROM staff WHERE time_id = @Time AND Status = 'available' AND WorkDays LIKE @Day"
            Guna2ComboBox1.Items.Clear()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Time", selectedTime)
                cmd.Parameters.AddWithValue("@Day", "%" & selectedDay & "%")

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim staff_name As String = reader("Name").ToString()
                        Guna2ComboBox1.Items.Add(staff_name)
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


    Public Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        ' Declare selectedDateTime as nullable DateTime
        Dim selectedDateTime As DateTime? = DateTimePicker1.Value

        ' Update selectedDay when the DateTimePicker value changes
        selectedDay = If(selectedDateTime.HasValue, selectedDateTime.Value.ToString("dddd"), "")

        ' Enable ComboBoxes when DateTimePicker has a selected date
        If selectedDateTime Is Nothing OrElse selectedDateTime = DateTime.MinValue Then
            Guna2ComboBox1.Enabled = False

            Guna2ComboBox3.Enabled = False  ' Enable Guna2ComboBox3
            Guna2ComboBox4.Enabled = False

        Else


            Guna2ComboBox3.Enabled = True  ' Enable Guna2ComboBox3

        End If
    End Sub


    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged

        Dim selectedstaff As String = If(Guna2ComboBox1.SelectedItem IsNot Nothing, Guna2ComboBox1.SelectedItem.ToString(), "")
        ConnectDatabase()

        Try

            ' Check if a time is selected in Guna2ComboBox3
            Dim selectedTimeID As String = If(Guna2ComboBox3.SelectedItem IsNot Nothing, Guna2ComboBox3.SelectedItem.ToString(), "")

            If Not String.IsNullOrWhiteSpace(selectedTimeID) Then
                Dim timeslotquery As String = "SELECT time_id FROM time WHERE Time_name = @SelectedStaffName"
                Using timeslotcommand As MySqlCommand = New MySqlCommand(timeslotquery, conn)
                    timeslotcommand.Parameters.AddWithValue("@SelectedStaffName", selectedTimeID)
                    Dim selectedTime As Integer = Convert.ToInt32(timeslotcommand.ExecuteScalar())

                    ' Enable Guna2ComboBox4 since a time is selected
                    Guna2ComboBox4.Enabled = True

                    ' Load data into Guna2ComboBox4 based on the selected time and staff
                    LoadTimeslot(selectedTime, selectedstaff)
                End Using
            Else
                ' If no time is selected, disable Guna2ComboBox4
                Guna2ComboBox4.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try



    End Sub

    Private Sub Guna2ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox4.SelectedIndexChanged

    End Sub
End Class
