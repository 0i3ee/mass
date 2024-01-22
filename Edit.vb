Imports MySql.Data.MySqlClient
Public Class Edit
    Private Sub ShowStaffData()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Query to select all columns from the staff table
            Dim query As String = "SELECT staff.staff_id, staff.Name,staff.Phone,staff.WorkDays ,time.Time_name
FROM staff
INNER JOIN time ON staff.time_id = time.time_id;
"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                Dim dt As New DataTable()

                ' Fill the data table with the results of the query
                da.Fill(dt)

                ' Assuming you have a DataGridView named 'Guna2DataGridView1' to display the data
                Guna2DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub SearchByName()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Assuming you have a TextBox named 'Guna2TextBoxSearchName' for searching by name
            Dim searchName As String = Guna2TextBox1.Text.Trim()

            ' Query to select records from the staff table based on the entered name
            Dim query As String = "SELECT staff.staff_id, staff.Name,staff.Phone,staff.WorkDays ,time.Time_name
FROM staff
INNER JOIN time ON staff.time_id = time.time_id WHERE Name LIKE @Name"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                ' Add a parameter for the name using a pattern matching search
                da.SelectCommand.Parameters.AddWithValue("@Name", "%" & searchName & "%")

                Dim dt As New DataTable()

                ' Fill the data table with the results of the query
                da.Fill(dt)

                ' Assuming you have a DataGridView named 'Guna2DataGridView1' to display the search results
                Guna2DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub SearchByPhone()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Assuming you have a TextBox named 'Guna2TextBoxSearchPhone' for searching by phone number
            Dim searchPhone As String = Guna2TextBox3.Text.Trim()

            ' Query to select records from the staff table based on the entered phone number
            Dim query As String = "SELECT staff.staff_id, staff.Name,staff.Phone,staff.WorkDays ,time.Time_name
FROM staff
INNER JOIN time ON staff.time_id = time.time_id WHERE Phone LIKE @Phone"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                ' Add a parameter for the phone number using a pattern matching search
                da.SelectCommand.Parameters.AddWithValue("@Phone", "%" & searchPhone & "%")

                Dim dt As New DataTable()

                ' Fill the data table with the results of the query
                da.Fill(dt)

                ' Assuming you have a DataGridView named 'Guna2DataGridView1' to display the search results
                Guna2DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub SelectRowAndDisplayValues()
        ' Check if any row is selected in the DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)

            ' Assuming you have TextBox controls named 'Guna2TextBox4', 'Guna2TextBox2'
            ' ComboBox named 'Guna2ComboBox1', and CheckBoxes named 'Guna2CheckBox1' through 'Guna2CheckBox7'
            Guna2TextBox4.Text = selectedRow.Cells("Name").Value.ToString()
            Guna2TextBox2.Text = selectedRow.Cells("Phone").Value.ToString()

            ' Set the checkboxes based on the WorkDays column
            Dim workDays As String = selectedRow.Cells("WorkDays").Value.ToString()
            Guna2CheckBox1.Checked = workDays.Contains("Sunday")
            Guna2CheckBox2.Checked = workDays.Contains("Monday")
            Guna2CheckBox3.Checked = workDays.Contains("Tuesday")
            Guna2CheckBox4.Checked = workDays.Contains("Wednesday")
            Guna2CheckBox5.Checked = workDays.Contains("Thursday")
            Guna2CheckBox6.Checked = workDays.Contains("Friday")
            Guna2CheckBox7.Checked = workDays.Contains("Saturday")

            ' Populate the ComboBox with time names before selecting a value
            PopulateTimeComboBox()

            ' Retrieve the Time_name from the selected row
            Dim timeName As String = selectedRow.Cells("Time_name").Value.ToString()

            ' Select the item in the ComboBox based on the retrieved Time_name
            Dim selectedItem As DataRowView = Guna2ComboBox1.Items.OfType(Of DataRowView)().FirstOrDefault(Function(item) item.Row.Field(Of String)("Time_name") = timeName)

            If selectedItem IsNot Nothing Then
                Guna2ComboBox1.SelectedItem = selectedItem
            Else
                MessageBox.Show("Error: Selected time name does not exist in the ComboBox.")
            End If

            ' Enable the controls when a row is selected
            SetControlsEnabled(True)
        Else
            ' If no row is selected, disable the controls
            SetControlsEnabled(False)
        End If
    End Sub



    Private Sub Update()

        ' Check if any row is selected in the DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)

            ' Get the staff_id of the selected row
            Dim staffId As Integer = Convert.ToInt32(selectedRow.Cells("staff_id").Value)

            ' Assuming you have TextBox controls named 'txtName', 'txtPhone'
            ' ComboBox named 'cmbTimeSlot', and CheckBoxes named 'chkSunday' through 'chkSaturday'
            Dim updatedName As String = Guna2TextBox4.Text
            Dim updatedPhone As String = Guna2TextBox2.Text
            Dim updatedTimeSlot As String = Guna2ComboBox1.SelectedItem.ToString()
            Dim selectedTimeRow As DataRowView = DirectCast(Guna2ComboBox1.SelectedItem, DataRowView)
            Dim updatedTimeSlotId As Integer = Convert.ToInt32(selectedTimeRow("time_id"))

            ' Construct the WorkDays value based on the checked checkboxes
            Dim updatedWorkDays As New List(Of String)
            If Guna2CheckBox1.Checked Then updatedWorkDays.Add("Sunday")
            If Guna2CheckBox2.Checked Then updatedWorkDays.Add("Monday")
            If Guna2CheckBox3.Checked Then updatedWorkDays.Add("Tuesday")
            If Guna2CheckBox4.Checked Then updatedWorkDays.Add("Wednesday")
            If Guna2CheckBox5.Checked Then updatedWorkDays.Add("Thursday")
            If Guna2CheckBox6.Checked Then updatedWorkDays.Add("Friday")
            If Guna2CheckBox7.Checked Then updatedWorkDays.Add("Saturday")

            Dim updatedWorkDaysString As String = String.Join(",", updatedWorkDays)

            ' Update the data in the database
            Try
                ' Use the ConnectDatabase method from your module
                ConnectDatabase()

                ' Begin a transaction
                Using transaction As MySqlTransaction = conn.BeginTransaction()
                    Try
                        ' Query to update the staff record
                        Dim updateStaffQuery As String = "UPDATE staff SET Name = @Name, Phone = @Phone, time_id = @TimeId, WorkDays = @WorkDays WHERE staff_id = @StaffId"

                        ' Create a command with parameters for staff update
                        Using cmdUpdateStaff As New MySqlCommand(updateStaffQuery, conn, transaction)
                            cmdUpdateStaff.Parameters.AddWithValue("@Name", updatedName)
                            cmdUpdateStaff.Parameters.AddWithValue("@Phone", updatedPhone)
                            cmdUpdateStaff.Parameters.AddWithValue("@TimeId", updatedTimeSlotId)
                            cmdUpdateStaff.Parameters.AddWithValue("@WorkDays", updatedWorkDaysString)
                            cmdUpdateStaff.Parameters.AddWithValue("@StaffId", staffId)

                            ' Execute the update query
                            cmdUpdateStaff.ExecuteNonQuery()

                            ' Commit the transaction if everything is successful
                            transaction.Commit()

                            MessageBox.Show("ແກ້ໄຂສຳເລັດ.")
                            ShowStaffData()
                        End Using
                    Catch ex As Exception
                        ' Rollback the transaction if an exception occurs
                        transaction.Rollback()
                        MessageBox.Show("Error: " & ex.Message)
                    End Try
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try


        Else
            MessageBox.Show("ກະລຸນາເລືອກຂໍ້ມູນຕ້ອງການແກ້ໄຂ.")
        End If
    End Sub

    Private Sub SetControlsEnabled(enabled As Boolean)
        ' Set the enabled property for your controls
        Label5.Visible = True
        Label2.Visible = True
        Label6.Visible = True
        Guna2TextBox4.Visible = True
        Guna2TextBox2.Visible = True
        Guna2ComboBox1.Visible = True
        Guna2CheckBox1.Visible = True
        Guna2CheckBox2.Visible = True
        Guna2CheckBox3.Visible = True
        Guna2CheckBox4.Visible = True
        Guna2CheckBox5.Visible = True
        Guna2CheckBox6.Visible = True
        Guna2CheckBox7.Visible = True
        Guna2Button1.Visible = True
    End Sub

    Private Sub Edit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the DataGridView when the form is loaded
        ShowStaffData()
        RefreshDataGrid()
        Guna2DataGridView1.ClearSelection()
        Label5.Visible = False
        Label2.Visible = False
        Label6.Visible = False
        Guna2TextBox4.Visible = False
        Guna2TextBox2.Visible = False
        Guna2ComboBox1.Visible = False
        Guna2CheckBox1.Visible = False
        Guna2CheckBox2.Visible = False
        Guna2CheckBox3.Visible = False
        Guna2CheckBox4.Visible = False
        Guna2CheckBox5.Visible = False
        Guna2CheckBox6.Visible = False
        Guna2CheckBox7.Visible = False
        Guna2Button1.Visible = False


    End Sub

    Private Sub PopulateTimeComboBox()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Query to select Time_id and Time_name values from the Time table
            Dim query As String = "SELECT time_id, Time_name FROM Time"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                Dim dt As New DataTable()

                ' Fill the data table with the results of the query
                da.Fill(dt)

                ' Assuming you have a ComboBox named 'Guna2ComboBox1'
                Guna2ComboBox1.DataSource = dt
                Guna2ComboBox1.DisplayMember = "Time_name" ' Set the DisplayMember property
                Guna2ComboBox1.ValueMember = "Time_id" ' Set the ValueMember property
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Public Sub RefreshDataGrid()
        ShowStaffData()
        ' Clear the selection in the DataGridView
        Guna2DataGridView1.ClearSelection()
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        SearchByName()
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox3.TextChanged
        SearchByPhone()
    End Sub

    Private Sub Guna2DataGridView1_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Guna2DataGridView1.CellMouseUp
        SelectRowAndDisplayValues()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Update()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        ShowStaffData()
    End Sub

    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged


    End Sub
End Class