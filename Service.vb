Imports Guna.UI2.WinForms
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Public Class Service

    Private Sub ShowData()
        Try
            ' Use the ConnectDatabase method from your module
            ConnectDatabase()

            ' Query to select all columns from the services table
            Dim query As String = "SELECT * FROM services"

            ' Create a data adapter and a data table to store the results
            Using da As New MySqlDataAdapter(query, conn)
                Dim dt As New DataTable()

                ' Fill the data table with the results of the query
                da.Fill(dt)

                ' Assuming you have a Guna2DataGridView named 'Guna2DataGridView1' to display the data
                Guna2DataGridView1.DataSource = dt

                ' Customize column names
                Guna2DataGridView1.Columns("service_id").HeaderText = "ID"
                Guna2DataGridView1.Columns("service_name").HeaderText = "Service Name"
                Guna2DataGridView1.Columns("duration_minutes").HeaderText = "Duration (Minutes)"
                Guna2DataGridView1.Columns("price").HeaderText = "Price"

                ' Set the header height
                Guna2DataGridView1.ColumnHeadersHeight = 40
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub






    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Check if the textboxes have valid data
        If String.IsNullOrEmpty(Guna2TextBox1.Text) OrElse
       String.IsNullOrEmpty(Guna2TextBox3.Text) OrElse
       String.IsNullOrEmpty(Guna2TextBox2.Text) Then
            MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Insert data using the module method
        InsertService(Guna2TextBox1.Text, Convert.ToInt32(Guna2TextBox3.Text), Convert.ToDecimal(Guna2TextBox2.Text))
    End Sub

    Public Sub InsertService(serviceName As String, duration As Integer, price As Decimal)
        ' Insert data into the services table
        Dim query As String = "INSERT INTO services (service_name, duration_minutes, price) VALUES (@ServiceName, @Duration, @Price)"

        Try
            ConnectDatabase()
            Using command As New MySqlCommand(query, conn)
                ' Set parameters
                command.Parameters.AddWithValue("@ServiceName", serviceName)
                command.Parameters.AddWithValue("@Duration", duration)
                command.Parameters.AddWithValue("@Price", price)

                ' Execute the query
                command.ExecuteNonQuery()
            End Using

            MessageBox.Show("ເພີ່ມການບໍລິການສຳເລັດ.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ShowData()
            Clear()

        Catch ex As Exception
            MessageBox.Show("Error while adding service: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Close the database connection
            conn.Close()
        End Try
    End Sub
    Private Sub Service_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowData()
    End Sub
    Private Sub Clear()
        Guna2TextBox1.Clear()

        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Try
            ' Check if there is at least one selected row
            If Guna2DataGridView1.SelectedRows.Count > 0 Then
                ' Assuming you have a unique identifier like service_id for updating
                Dim serviceId As Integer = Convert.ToInt32(Guna2DataGridView1.SelectedRows(0).Cells("service_id").Value)

                ' Update data in the services table
                ConnectDatabase()
                Dim query As String = "UPDATE services SET service_name = @ServiceName, duration_minutes = @Duration, price = @Price WHERE service_id = @ServiceId"
                Using command As New MySqlCommand(query, conn)
                    ' Set parameters
                    command.Parameters.AddWithValue("@ServiceName", Guna2TextBox1.Text)
                    command.Parameters.AddWithValue("@Duration", Convert.ToInt32(Guna2TextBox3.Text))
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(Guna2TextBox2.Text))
                    command.Parameters.AddWithValue("@ServiceId", serviceId)

                    ' Execute the query
                    command.ExecuteNonQuery()
                End Using

                ' Refresh the data in Guna2DataGridView1
                ShowData()
            Else
                MessageBox.Show("Please select a row to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Close the database connection in the finally block
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub



    Private Sub Guna2DataGridView1_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Guna2DataGridView1.CellMouseUp
        ' Check if there is at least one selected row
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Assuming you have textboxes named Guna2TextBoxID, Guna2TextBoxServiceName, Guna2TextBoxDuration, and Guna2TextBoxPrice

            Guna2TextBox1.Text = Guna2DataGridView1.SelectedRows(0).Cells("service_name").Value.ToString()
            Guna2TextBox3.Text = Guna2DataGridView1.SelectedRows(0).Cells("duration_minutes").Value.ToString()
            Guna2TextBox2.Text = Guna2DataGridView1.SelectedRows(0).Cells("price").Value.ToString()
        End If
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Try
            ' Check if there is at least one selected row
            If Guna2DataGridView1.SelectedRows.Count > 0 Then
                ' Assuming you have a unique identifier like service_id for deletion
                Dim serviceId As Integer = Convert.ToInt32(Guna2DataGridView1.SelectedRows(0).Cells("service_id").Value)

                ' Delete the row from the services table
                ConnectDatabase()
                Dim query As String = "DELETE FROM services WHERE service_id = @ServiceId"
                Using command As New MySqlCommand(query, conn)
                    ' Set parameter
                    command.Parameters.AddWithValue("@ServiceId", serviceId)

                    ' Execute the query
                    command.ExecuteNonQuery()
                End Using

                ' Refresh the data in Guna2DataGridView1
                ShowData()
            Else
                MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Close the database connection in the finally block
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

End Class