Imports MySql.Data.MySqlClient

Public Class Home
    ' List to store booking information
    Private bookingList As New List(Of BookingInfo)

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowBookingInfo()
    End Sub

    Private Sub ShowBookingInfo()
        ConnectDatabase()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "SELECT booking_id, customer_name, customer_phone, Status FROM bookings"
            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        ' Access the data using reader.GetString or other appropriate methods
                        Dim bookingId As Integer = reader.GetInt32(reader.GetOrdinal("booking_id"))
                        Dim customerName As String = reader.GetString(reader.GetOrdinal("customer_name"))
                        Dim customerPhone As String = reader.GetString(reader.GetOrdinal("customer_phone"))
                        Dim status As String = reader.GetString(reader.GetOrdinal("Status"))

                        ' Display the data in labels
                        Label4.Text = bookingId.ToString()
                        Label6.Text = customerName
                        Label5.Text = customerPhone
                        Label7.Text = status

                        ' Optionally, you might want to add a delay or process the data in some way
                        ' System.Threading.Thread.Sleep(1000) ' 1000 milliseconds delay
                    End While
                End Using
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Update the status in the database
        UpdateStatusToPending()

        ' Refresh the label with the updated status
        RefreshStatusLabel()
    End Sub

    Private Sub UpdateStatusToPending()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "UPDATE bookings SET Status = 'pending' WHERE booking_id = @bookingId"
            Using command As New MySqlCommand(query, conn)
                ' If booking_id uniquely identifies the record, assume bookingId is an integer
                command.Parameters.AddWithValue("@bookingId", Label4.Text) ' Replace 123 with the actual booking_id value
                command.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RefreshStatusLabel()
        Try
            ' Use the existing connection from Module1
            Dim query As String = "SELECT Status FROM bookings WHERE booking_id = @bookingId"
            Using command As New MySqlCommand(query, conn)
                ' If booking_id uniquely identifies the record, assume bookingId is an integer
                command.Parameters.AddWithValue("@bookingId", 123) ' Replace 123 with the actual booking_id value

                ' Read the status from the database
                Dim updatedStatus As String = Convert.ToString(command.ExecuteScalar())

                ' Display the updated status in the label
                Label7.Text = updatedStatus
            End Using
        Catch ex As Exception
            ' Handle exceptions, e.g., display an error message
            MessageBox.Show($"Error retrieving updated status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

End Class