Imports MySql.Data.MySqlClient
Public Class Delete
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

    Private Sub DeleteSelectedRow()
        ' Check if any row is selected in the DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)

            ' Get the primary key value (e.g., staff_id)
            Dim staffId As Integer = Convert.ToInt32(selectedRow.Cells("staff_id").Value)

            ' Confirm with the user before deleting
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ' Execute the delete operation
                Try
                    ' Use the ConnectDatabase method from your module
                    ConnectDatabase()

                    ' Query to delete the staff record
                    Dim query As String = "DELETE FROM staff WHERE staff_id = @StaffId"

                    ' Create a command with parameters
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@StaffId", staffId)

                        ' Execute the delete query
                        cmd.ExecuteNonQuery()

                        MessageBox.Show("Record deleted successfully.")
                        ShowStaffData()
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try

                ' Refresh the DataGridView to reflect the changes
                LoadDataIntoDataGridView()
            End If
        Else
            MessageBox.Show("Please select a row to delete.")
        End If
    End Sub

    ' Call this function to load data into DataGridView when needed
    Private Sub LoadDataIntoDataGridView()
        ' Code to load data into the DataGridView, replace with your actual code
    End Sub


    Private Sub Delete_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowStaffData()
        Guna2DataGridView1.ClearSelection()
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        SearchByName()
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox3.TextChanged
        SearchByPhone()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        DeleteSelectedRow()

    End Sub
End Class