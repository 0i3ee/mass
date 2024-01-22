﻿Imports MySql.Data.MySqlClient

Public Class Home
    Private conn As MySqlConnection
    Private verticalPosition As Integer = 12
    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectDatabase()
        LoadButtonsFromDatabase()

    End Sub

    Private Sub ConnectDatabase()
        Dim connectionString As String = "server=localhost; user=root; password=; database=massage; CharSet=utf8;"
        conn = New MySqlConnection(connectionString)
    End Sub

    Private Sub LoadButtonsFromDatabase()
        Try
            Dim query As String = "SELECT customer_name, service_id, time_slot_id, Status FROM bookings"
            Using command As New MySqlCommand(query, conn)
                conn.Open()

                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim serviceName As String = GetServiceName(reader("service_id"))
                        Dim timeSlotName As String = GetTimeSlotName(reader("time_slot_id"))

                        Dim newButton As New Button()
                        CustomizeButtonStyle(newButton)
                        newButton.Text = $" {reader("customer_name")}                                 {serviceName}    {timeSlotName}               {reader("Status")}"
                        newButton.TextAlign = ContentAlignment.MiddleLeft
                        AddHandler newButton.Click, AddressOf Button_Click
                        newButton.Location = New Point(verticalPosition, 12)

                        ' Increment the vertical position for the next button
                        verticalPosition += 300

                        Panel1.Controls.Add(newButton)
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading buttons from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub CustomizeButtonStyle(button As Button)
        button.Font = New System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        button.Size = New System.Drawing.Size(287, 198)
        button.Location = New System.Drawing.Point(12, 12)
        button.UseVisualStyleBackColor = True
        button.ForeColor = Color.Black
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = DirectCast(sender, Button)
        MessageBox.Show("Button Clicked: " & clickedButton.Text, "Button Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

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
End Class