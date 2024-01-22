Imports MySql.Data.MySqlClient

Public Class Home
    ' List to store booking information
    Private bookingList As New List(Of BookingInfo)

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowBookingInfo()
    End Sub

    Private Sub ShowBookingInfo()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Function HasBookingsWithStatusa(connection As MySqlConnection, status As String) As Boolean
        Try
            ' Query to check if there are any bookings with the specified status
            Dim query As String = "SELECT COUNT(*) FROM bookings WHERE status = @Status"

            Using cmd As New MySqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@Status", status)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                ' Return true if there is at least one booking with the specified status
                Return count > 0
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bookings: " & ex.Message)
            Return False
        End Try
    End Function
End Class