Imports MySql.Data.MySqlClient

Public Class Employee

    Public Sub SetEmployeeName(name As String)
        Label5.Text = name
    End Sub
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Validation: Check if Name, Phone, ComboBox, and at least one CheckBox are not empty or unselected
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
        If Not AnyCheckBoxChecked(Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7) Then
            MessageBox.Show("Please select at least one workday.")
            Return
        End If

        ' If all validations pass, proceed with data insertion
        Dim selectedTimeSlot As String = Guna2ComboBox1.SelectedItem.ToString()
        Dim selectedWorkDays As String = GetSelectedWorkDays()

        ' Perform the database insertion
        If InsertData(selectedTimeSlot, selectedWorkDays) Then
            ' Clear data after successful insertion
            ClearData()
        End If
    End Sub


    Private Function GetSelectedWorkDays() As String
        Dim selectedDays As New List(Of String)

        ' Assuming you have CheckBox controls named chkSunday, chkMonday, ..., chkSaturday
        For Each dayCheckBox As CheckBox In {Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7}
            If dayCheckBox.Checked Then
                selectedDays.Add(dayCheckBox.Text)
            End If
        Next

        ' Convert the list of selected days to a comma-separated string
        Return String.Join(",", selectedDays)
    End Function

    Private Function InsertData(timeSlot As String, workDays As String) As Boolean
        ' Use the ConnectDatabase method from your module
        ConnectDatabase()

        Try

            Dim query As String = "INSERT INTO staff (Name, Phone, TimeSlot, WorkDays) VALUES (@Name, @Phone, @TimeSlot, @WorkDays)"

            Using command As MySqlCommand = New MySqlCommand(query, conn)
                ' Assuming you have TextBox controls named 'txtName' and 'txtPhone' for name and phone
                command.Parameters.AddWithValue("@Name", Guna2TextBox1.Text)
                command.Parameters.AddWithValue("@Phone", Guna2TextBox3.Text)
                command.Parameters.AddWithValue("@TimeSlot", Guna2ComboBox1.SelectedItem.ToString())
                command.Parameters.AddWithValue("@WorkDays", GetSelectedWorkDays())

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
        Guna2TextBox3.Clear()

        ' Clear ComboBox
        Guna2ComboBox1.SelectedIndex = -1

        ' Clear CheckBoxes
        For Each dayCheckBox As CheckBox In {Guna2CheckBox1, Guna2CheckBox2, Guna2CheckBox3, Guna2CheckBox5, Guna2CheckBox6, Guna2CheckBox7}
            dayCheckBox.Checked = False
        Next
    End Sub

    Private Sub Guna2CustomGradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel1.Paint

    End Sub
End Class
