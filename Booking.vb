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

            ' Make sure the connection is open before proceeding
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                ' Get the time_slot_id for the selected time slot
                Dim selectedTimeSlotItem As TimeSlotItem = TryCast(Guna2ComboBox4.SelectedItem, TimeSlotItem)

                ' Check if the cast was successful before proceeding
                If selectedTimeSlotItem IsNot Nothing Then
                    Dim selectedTimeSlotId As Integer = selectedTimeSlotItem.TimeSlotId

                    ' Display the selected time slot ID
                    MessageBox.Show("Selected Time Slot ID: " & selectedTimeSlotId)
                Else
                    MessageBox.Show("Please select a valid time slot.")
                End If
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
            Dim query As String = "SELECT staff_id, Name FROM staff"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ' Create a custom class to store staff_id and Name
                        Dim staffItem As New StaffItem()
                        staffItem.StaffId = Convert.ToInt32(reader("staff_id"))
                        staffItem.Name = reader("Name").ToString()

                        ' Add staffItem to ComboBox
                        Guna2ComboBox1.Items.Add(staffItem)
                    End While
                End Using
            End Using

            ' Set the DisplayMember explicitly
            Guna2ComboBox1.DisplayMember = "Name"
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
            Dim query As String = "SELECT time_slot FROM time_slots WHERE time_id = (SELECT time_id FROM Time WHERE Time_name = @SelectedTime)"

            ' Using a MySqlCommand to execute the query
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SelectedTime", selectedTime)

                ' Using a MySqlDataReader to read the results of the query
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing items in the combo box
                    Guna2ComboBox4.Items.Clear()

                    ' Loop through the records and add each time slot to the combo box
                    While reader.Read()
                        ' Assuming time_slot is a string column in the result
                        Dim timeSlot As String = reader("time_slot").ToString()

                        ' Add the formatted time slot to the combo box
                        Guna2ComboBox4.Items.Add(timeSlot)
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
        Dim selectedTime As String = Guna2ComboBox3.SelectedItem.ToString()


        ' Get the selected day from the DateTimePicker
        Dim selectedDay As String = DateTimePicker1.Value.ToString("dddd")

        ' Populate staff based on the selected time and day
        LoadStaffForTime(selectedTime, selectedDay)

        ' Load time slots for the selected time
        LoadTimeslot(selectedTime)
    End Sub


    Private Sub LoadStaffForTime(selectedTime As String, selectedDay As String)
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff with the specified availability
            ' Check if the selected time matches any entry in the 'Time' column
            ' and if the selected day matches any entry in the 'WorkDays' column
            Dim query As String = "SELECT staff_id, Name FROM staff WHERE Status = 'available' AND Time LIKE @Time AND WorkDays LIKE @Day"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Time", "%" & selectedTime & "%")
                cmd.Parameters.AddWithValue("@Day", "%" & selectedDay & "%")

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    Guna2ComboBox1.Items.Clear()
                    While reader.Read()
                        ' Create a custom class to store staff_id and Name
                        Dim staffItem As New StaffItem()
                        staffItem.StaffId = Convert.ToInt32(reader("staff_id"))
                        staffItem.Name = reader("Name").ToString()

                        ' Add staffItem to ComboBox
                        Guna2ComboBox1.Items.Add(staffItem)
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




    Private Sub PopulateStaffComboBox(selectedTime As String, selectedDay As String)
        Guna2ComboBox1.Items.Clear()

        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Modify the query to select staff with the specified availability
            ' Check if the selected day matches any entry in the 'Day' column
            Dim query As String = "SELECT staff_id, Name FROM staff WHERE Time = @Time AND WorkDays LIKE @Day"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Time", selectedTime)
                cmd.Parameters.AddWithValue("@Day", "%" & selectedDay & "%")

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing items in the combo box
                    Guna2ComboBox1.Items.Clear()
                    While reader.Read()
                        ' Create a custom class to store staff_id and Name
                        Dim staffItem As New StaffItem()
                        staffItem.StaffId = Convert.ToInt32(reader("staff_id"))
                        staffItem.Name = reader("Name").ToString()

                        ' Add staffItem to ComboBox
                        Guna2ComboBox1.Items.Add(staffItem)
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
