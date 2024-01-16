Imports MySql.Data.MySqlClient
Public Class list
    Private Sub ShowStaffData()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Query to select all columns from the staff table
            Dim query As String = "SELECT * FROM staff"

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

    Private Sub SearchByTimeSlot()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Assuming you have a ComboBox named 'Guna2ComboBox1' for searching by TimeSlot
            Dim selectedTimeSlot As String = Guna2ComboBox1.SelectedItem.ToString()

            ' Query to select records from the staff table based on the selected TimeSlot
            Dim query As String = "SELECT * FROM staff WHERE TimeSlot = @TimeSlot"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                ' Add a parameter for the TimeSlot
                da.SelectCommand.Parameters.AddWithValue("@TimeSlot", selectedTimeSlot)

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

    Private Sub SearchByName()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Assuming you have a TextBox named 'Guna2TextBoxSearchName' for searching by name
            Dim searchName As String = Guna2TextBox1.Text.Trim()

            ' Query to select records from the staff table based on the entered name
            Dim query As String = "SELECT * FROM staff WHERE Name LIKE @Name"

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
            Dim query As String = "SELECT * FROM staff WHERE Phone LIKE @Phone"

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


    Private Sub SearchByWorkDays()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Query to select records from the staff table based on selected workdays
            Dim query As String = "SELECT * FROM staff WHERE "

            ' Create a list to store selected workdays
            Dim selectedWorkDays As New List(Of String)

            ' Check each checkbox and add the corresponding day to the list if checked
            If Guna2CheckBox1.Checked Then
                selectedWorkDays.Add("Sunday")
            End If
            If Guna2CheckBox2.Checked Then
                selectedWorkDays.Add("Monday")
            End If
            If Guna2CheckBox3.Checked Then
                selectedWorkDays.Add("Tuesday")
            End If
            If Guna2CheckBox4.Checked Then
                selectedWorkDays.Add("Wednesday")
            End If
            If Guna2CheckBox5.Checked Then
                selectedWorkDays.Add("Thursday")
            End If
            If Guna2CheckBox6.Checked Then
                selectedWorkDays.Add("Friday")
            End If
            If Guna2CheckBox7.Checked Then
                selectedWorkDays.Add("Saturday")
            End If

            ' If no checkboxes are checked, display all records
            If selectedWorkDays.Count = 0 Then
                ShowStaffData()
                Return
            End If

            ' Add conditions for each selected workday to the query
            query &= "("
            For Each workday As String In selectedWorkDays
                query &= $"WorkDays LIKE '%{workday}%' OR "
            Next
            ' Remove the trailing 'OR'
            query = query.TrimEnd(" "c, "O"c, "R"c)
            query &= ")"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
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




    Private Sub list_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the DataGridView when the form is loaded
        ShowStaffData()
    End Sub

    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged
        ' Trigger the search when the selection changes in the ComboBox
        SearchByTimeSlot()
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        SearchByName()
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox3.TextChanged
        SearchByPhone()
    End Sub

    Private Sub Guna2CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox1.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox2.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox3.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox4.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox5.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox6.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox7.CheckedChanged
        SearchByWorkDays()
    End Sub

    Private Sub Guna2DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellContentClick


    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Guna2CustomGradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel1.Paint

    End Sub
End Class
