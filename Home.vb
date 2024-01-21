Imports MySql.Data.MySqlClient

Public Class Home
    ' List to store booking information
    Private bookingList As New List(Of BookingInfo)

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowBookingInfo()
    End Sub

    Private Sub ShowBookingInfo()
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Replace the following query with your specific criteria
            Dim query As String = "SELECT booking_id, customer_name, status, Datemassage FROM bookings WHERE status = 'book' "

            Using cmd As New MySqlCommand(query, conn)
                ' Execute the query
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                ' Clear the bookingList before populating it
                bookingList.Clear()

                ' Counter for the booking labels
                Dim labelCounter As Integer = 4

                ' Check if there are any results
                If reader.HasRows Then
                    ' Loop through the results
                    While reader.Read() And labelCounter <= 15 ' Incremented the counter for the new column
                        ' Retrieve the values from the query results
                        Dim bookingId As Integer = Convert.ToInt32(reader("booking_id"))
                        Dim customerName As String = reader("customer_name").ToString()
                        Dim status As String = reader("status").ToString()
                        Dim datemessage As DateTime = Convert.ToDateTime(reader("datemassage"))

                        ' Add booking information to the list
                        bookingList.Add(New BookingInfo(bookingId, customerName, status, datemessage))

                        ' Find the label by name
                        Dim label As Label = TryCast(Me.Controls.Find("Label" & labelCounter, True).FirstOrDefault(), Label)

                        ' Check if label is not Nothing and labelCounter is within the expected range
                        If label IsNot Nothing AndAlso labelCounter <= 15 Then ' Updated the condition
                            ' Display the values in the label
                            label.Text = "Booking ID: " & bookingId.ToString()
                        End If

                        ' ... (Similar code for other labels)

                        ' Increment the counter for the next set of labels
                        labelCounter += 4 ' Incremented by 4 to accommodate the new column
                    End While
                Else
                    ' No bookings found
                    MessageBox.Show("No bookings found with status 'book'.")
                End If

                ' Close the reader
                reader.Close()
            End Using
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error retrieving booking info: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Use the existing connection from the module
            ConnectDatabase()

            ' Check if there is at least one booking with status 'pending'
            If HasBookingsWithStatusa(conn, "pending") Then
                ' Show the form to check the bill
                ' Pass the selected booking information to the CheckBillForm
                Dim selectedBooking As BookingInfo = bookingList.FirstOrDefault(Function(x) x.Status = "pending")
                If selectedBooking IsNot Nothing Then
                    Dim checkBillForm As New CheckBillForm(selectedBooking)
                    checkBillForm.ShowDialog()
                Else
                    MessageBox.Show("No pending bookings found.")
                End If
            Else
                ' No bookings found with status 'pending'
                MessageBox.Show("No bookings found with status 'pending'.")
            End If
        Catch ex As Exception
            ' Handle any exceptions that may occur during the database operation
            MessageBox.Show("Error checking bill: " & ex.Message)
        Finally
            ' Close the database connection when done
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
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