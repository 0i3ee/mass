Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class BookingDetailsForm
    Private _bookingId As Integer

    Public Sub New(bookingId As Integer)
        InitializeComponent()
        _bookingId = bookingId
    End Sub

    Private Sub BookingDetailsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ConnectDatabase()
            LoadLabel()
        Catch ex As Exception
            MessageBox.Show("Error loading from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Public Sub LoadLabel()
        Try
            Dim Nquery As String = "SELECT customer_name, customer_phone, time_slot_id, booking_date, Datemassage, service_id, staff_id, Status FROM bookings WHERE booking_id = @BookingId"

            Using command As New MySqlCommand(Nquery, conn)
                command.Parameters.AddWithValue("@BookingId", _bookingId.ToString())

                Using reader As MySqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim staffName As String = GetStaffName(Convert.ToInt32(reader("staff_id")))
                        Dim serviceName As String = GetServiceName(Convert.ToInt32(reader("service_id")))
                        Dim timeSlotName As String = GetTimeSlotName(Convert.ToInt32(reader("time_slot_id")))

                        Label1.Text = reader("customer_name").ToString()
                        Label2.Text = reader("customer_phone").ToString()
                        Label3.Text = (timeSlotName).ToString()
                        Label4.Text = reader("booking_date").ToString()
                        Label5.Text = reader("Datemassage").ToString()
                        Label6.Text = (serviceName).ToString()
                        Label7.Text = (staffName).ToString()
                        Label8.Text = reader("Status").ToString()
                    Else
                        MessageBox.Show("No data found for the given booking ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Function GetStaffName(staffId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT Name FROM staff WHERE staff_id = @ServiceId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ServiceId", staffId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown staff"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown staff"
        End Try
    End Function

    Private Function GetServiceName(serviceId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT service_name FROM services WHERE service_id = @ServiceId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ServiceId", serviceId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown Service"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown Service"
        End Try
    End Function

    Private Function GetTimeSlotName(timeSlotId As Integer) As String
        Try
            Using connection As New MySqlConnection(conn.ConnectionString)
                connection.Open()
                Dim query As String = "SELECT time_slot FROM time_slots WHERE time_slot_id = @TimeSlotId"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@TimeSlotId", timeSlotId)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Return result.ToString()
                    Else
                        Return "Unknown Time Slot"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Unknown Time Slot"
        End Try
    End Function


    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Dim bookingIdToDelete As Integer = _bookingId

        Try
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to Check bill", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ' Insert into the bill table
                Dim bookingInfo As New Dictionary(Of String, Object)()

                Using connection As New MySqlConnection(conn.ConnectionString)
                    connection.Open()

                    ' Retrieve booking information
                    Dim selectQuery As String = "SELECT * FROM bookings WHERE booking_id = @ID;"
                    Using selectCommand As New MySqlCommand(selectQuery, connection)
                        selectCommand.Parameters.AddWithValue("@ID", bookingIdToDelete)

                        Using reader As MySqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                ' Retrieve information and store it in the dictionary
                                For i As Integer = 0 To reader.FieldCount - 1
                                    bookingInfo.Add(reader.GetName(i), reader.GetValue(i))
                                Next
                            End If
                        End Using
                    End Using

                    ' Check if the dictionary contains the "service_id" key
                    If bookingInfo.ContainsKey("service_id") Then
                        ' Retrieve the service price based on service_id
                        Dim serviceId As Integer = Convert.ToInt32(bookingInfo("service_id"))
                        MessageBox.Show($"Service ID: {serviceId}", "Service Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Retrieve the service price from the services table
                        Dim servicePriceQuery As String = "SELECT price FROM services WHERE service_id = @ServiceId"
                        Using serviceCommand As MySqlCommand = New MySqlCommand(servicePriceQuery, connection)
                            serviceCommand.Parameters.AddWithValue("@ServiceId", serviceId)
                            Dim servicePrice As Decimal = Convert.ToDecimal(serviceCommand.ExecuteScalar())

                            ' Insert into the bills table
                            Dim insertQuery As String = "INSERT INTO bills (booking_id, total_amount, payment_status) VALUES (@BookingID, @TotalAmount, @PaymentStatus);"
                            Using insertCommand As New MySqlCommand(insertQuery, connection)

                                ' Use a transaction to ensure atomicity
                                Using transaction As MySqlTransaction = connection.BeginTransaction()
                                    Try
                                        ' Assign parameters for the insert command
                                        insertCommand.Parameters.AddWithValue("@BookingID", bookingInfo("booking_id"))
                                        insertCommand.Parameters.AddWithValue("@TotalAmount", servicePrice)
                                        insertCommand.Parameters.AddWithValue("@PaymentStatus", "unpaid")

                                        ' Execute the insert command
                                        insertCommand.ExecuteNonQuery()

                                        ' Commit the transaction if everything is successful
                                        transaction.Commit()
                                        switchPanel(CheckBillForm)
                                    Catch ex As Exception
                                        ' Rollback the transaction if an exception occurs
                                        transaction.Rollback()

                                        MessageBox.Show("Error inserting into bills: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End Try
                                End Using
                            End Using
                        End Using
                    Else
                        MessageBox.Show("The 'service_id' key is not present in the dictionary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            Else
                MessageBox.Show("Operation canceled.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error processing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try



    End Sub
    Sub switchPanel(ByVal panel As Form)

        If Edit IsNot Nothing AndAlso Edit.Visible AndAlso Not Edit.IsDisposed Then
            Edit.Close()
        End If
        If Delete IsNot Nothing AndAlso Delete.Visible AndAlso Not Delete.IsDisposed Then
            Delete.Close()
        End If
        If list IsNot Nothing AndAlso list.Visible AndAlso Not list.IsDisposed Then
            list.Close()
        End If
        If Booking IsNot Nothing AndAlso Booking.Visible AndAlso Not Booking.IsDisposed Then
            Booking.Close()
        End If
        If Home IsNot Nothing AndAlso Home.Visible AndAlso Not Home.IsDisposed Then
            Home.Close()
        End If

        Guna2CustomGradientPanel1.Controls.Clear()
        panel.TopLevel = False
        Guna2CustomGradientPanel1.Controls.Add(panel)
        panel.Show()

    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Dim idToDelete As Integer = _bookingId
        Try
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                Using connection As New MySqlConnection(conn.ConnectionString)
                    connection.Open()
                    Dim query As String = "DELETE FROM bookings WHERE booking_id = @ID;"
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@ID", idToDelete)

                        ' Execute the DELETE statement
                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

                        ' Check if any rows were deleted
                        If rowsAffected > 0 Then
                            MessageBox.Show("Row deleted successfully.")
                        Else
                            MessageBox.Show("No matching rows found.")
                        End If

                    End Using
                End Using
            Else
                ' User clicked No, do nothing or handle accordingly
                MessageBox.Show("Deletion canceled.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error deleting from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class
