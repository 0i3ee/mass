Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class Employee

    Public Sub SetEmployeeName(name As String)
        Label5.Text = name
    End Sub
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click

        If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) Then
            MessageBox.Show("Please enter a valid name.")
            Return
        End If

        If String.IsNullOrWhiteSpace(Guna2TextBox3.Text) Then
            MessageBox.Show("Please enter a valid phone number.")
            Return
        End If

        ' Assuming you have ComboBox named 'Guna2ComboBox1' for time slot selection
        If Guna2ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Please select a time slot.")
            Return
        End If

        ' Assuming you have CheckBox controls named chkSunday, chkMonday, ..., chkSaturday
        If Not AnyCheckBoxChecked(Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox4, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7) Then
            MessageBox.Show("Please select at least one workday.")
            Return
        End If

        ConnectDatabase()

        Try
            Dim selectedTimeSlot As String = Guna2ComboBox1.SelectedItem.ToString()

            Dim timeslotquery As String = "SELECT time_id FROM time WHERE Time_name = @SelectedStaffName"
            Using timeslotcommand As MySqlCommand = New MySqlCommand(timeslotquery, conn)
                timeslotcommand.Parameters.AddWithValue("@SelectedStaffName", selectedTimeSlot)
                Dim slectedtimeslotID As Integer = Convert.ToInt32(timeslotcommand.ExecuteScalar())

                Dim selectedWorkDays As String = GetSelectedWorkDays()
                ' Perform the database insertion
                If InsertData(slectedtimeslotID, selectedWorkDays) Then
                    ClearData()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    Private Function GetSelectedWorkDays() As String
        Dim selectedDays As New List(Of String)

        ' Assuming you have CheckBox controls named chkSunday, chkMonday, ..., chkSaturday
        For Each dayCheckBox As CheckBox In {Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox4, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7}
            If dayCheckBox.Checked Then
                selectedDays.Add(dayCheckBox.Text)
            End If
        Next

        ' Convert the list of selected days to a comma-separated string
        Return String.Join(",", selectedDays)
    End Function

    Private Function InsertData(timeSlot As Integer, workDays As String) As Boolean
        ' Use the ConnectDatabase method from your module
        ConnectDatabase()

        Try

            Dim query As String = "INSERT INTO staff (Name, Phone, time_id, WorkDays,Status) VALUES (@Name, @Phone, @TimeSlot, @WorkDays,'Available')"

            Using command As MySqlCommand = New MySqlCommand(query, conn)
                ' Assuming you have TextBox controls named 'txtName' and 'txtPhone' for name and phone
                command.Parameters.AddWithValue("@Name", Guna2TextBox1.Text)
                command.Parameters.AddWithValue("@Phone", Guna2TextBox3.Text)
                command.Parameters.AddWithValue("@TimeSlot", timeSlot)
                command.Parameters.AddWithValue("@WorkDays", workDays)

                command.ExecuteNonQuery()
                MessageBox.Show("Data inserted successfully.")
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Private Function AnyCheckBoxChecked(ParamArray checkBoxes() As CheckBox) As Boolean
        For Each checkBox As CheckBox In checkBoxes
            If checkBox.Checked Then
                Return True
            End If
        Next
        Return False
    End Function




    Private Sub ClearData()
        ' Clear TextBoxes
        Guna2TextBox1.Clear()
        Guna2TextBox1.Clear()
        Guna2TextBox3.Clear()

        ' Clear ComboBox
        Guna2ComboBox1.SelectedIndex = -1

        ' Clear CheckBoxes
        For Each dayCheckBox As CheckBox In {Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox4, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7}
            dayCheckBox.Checked = False
        Next
    End Sub

    Private Sub Guna2CustomGradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel1.Paint

    End Sub

    Private Sub Guna2CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox4.CheckedChanged

    End Sub

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadTime()
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
                    Guna2ComboBox1.Items.Clear()

                    ' Loop through the records and add each Time_name to the combo box
                    While reader.Read()
                        Guna2ComboBox1.Items.Add(reader("Time_name").ToString())
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
End Class
