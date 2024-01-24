Imports MySql.Data.MySqlClient
Imports System.Windows.Forms.DataVisualization.Charting
Public Class Overall
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Create and configure the chart
        ConnectDatabase()

        If conn.State = ConnectionState.Open Then
            ' Query to get the count of bookings for each service
            Dim query As String = "
            SELECT services.service_name, COUNT(bookings.booking_id) as booking_count
            FROM bookings
            JOIN services ON bookings.service_id = services.service_id
            GROUP BY services.service_id
            ORDER BY booking_count DESC;
        "

            Try
                cmd.Connection = conn
                cmd.CommandText = query

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        ' Clear existing data in Chart1
                        Chart1.Series.Clear()

                        ' Create a new series for the chart
                        Dim series1 As New Series()
                        series1.ChartArea = "ChartArea1"
                        series1.Name = "Series1"

                        ' Populate the chart data
                        While reader.Read()
                            Dim service_name As String = reader("service_name").ToString()
                            Dim booking_count As Integer = Convert.ToInt32(reader("booking_count"))

                            series1.Points.AddXY(service_name, booking_count)
                        End While

                        ' Add the new series to Chart1
                        Chart1.Series.Add(series1)

                        ' Customize the chart appearance
                        series1.ChartType = SeriesChartType.Bar
                        Chart1.Titles.Add("Most Booked Services")
                        Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Angle = -45
                    Else
                        MessageBox.Show("No data returned from the query.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error while fetching data from the database: " & ex.Message)
            End Try
        Else
            MessageBox.Show("Error: Unable to open the database connection.")
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ' Check if the selected index is 0
        If ComboBox1.SelectedIndex = 0 Then
            ' Connect to the MySQL database using your module
            ConnectDatabase()

            If conn.State = ConnectionState.Open Then
                ' Query to get the count of bookings for each service
                Dim query As String = "
                SELECT services.service_name, COUNT(bookings.booking_id) as booking_count
                FROM bookings
                JOIN services ON bookings.service_id = services.service_id
                GROUP BY services.service_id
                ORDER BY booking_count DESC;
            "

                Try
                    cmd.Connection = conn
                    cmd.CommandText = query

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            ' Clear existing data in Chart1
                            Chart1.Series.Clear()

                            ' Create a new series for the chart
                            Dim series1 As New Series()
                            series1.ChartArea = "ChartArea1"
                            series1.Name = "Series1"

                            ' Populate the chart data
                            While reader.Read()
                                Dim service_name As String = reader("service_name").ToString()
                                Dim booking_count As Integer = Convert.ToInt32(reader("booking_count"))

                                series1.Points.AddXY(service_name, booking_count)
                            End While

                            ' Add the new series to Chart1
                            Chart1.Series.Add(series1)

                            ' Customize the chart appearance
                            series1.ChartType = SeriesChartType.Bar
                            Chart1.Titles.Add("Most Booked Services")
                            Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Angle = -45
                        Else
                            MessageBox.Show("No data returned from the query.")
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error while fetching data from the database: " & ex.Message)
                End Try
            Else
                MessageBox.Show("Error: Unable to open the database connection.")
            End If
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ' Load data for the most booked services for each day of the week

            ' Connect to the MySQL database using your module
            ConnectDatabase()

            If conn.State = ConnectionState.Open Then
                ' Query to get the count of bookings for each service for each day of the week
                Dim query As String = "
                SELECT services.service_name, DAYNAME(bookings.Datemassage) as booking_day, COUNT(bookings.booking_id) as booking_count
                FROM bookings
                JOIN services ON bookings.service_id = services.service_id
                GROUP BY services.service_id, DAYNAME(bookings.Datemassage)
                ORDER BY booking_count DESC;
            "

                Try
                    cmd.Connection = conn
                    cmd.CommandText = query

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            ' Clear existing data in Chart1
                            Chart1.Series.Clear()

                            ' Create a new series for the chart
                            Dim series1 As New Series()
                            series1.ChartArea = "ChartArea1"
                            series1.Name = "Series1"

                            ' Define a color palette for each day
                            Dim colorPalette As Color() = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet}

                            ' Counter to loop through the color palette
                            Dim colorIndex As Integer = 0

                            ' Populate the chart data
                            While reader.Read()
                                Dim service_name As String = reader("service_name").ToString()
                                Dim booking_day As String = reader("booking_day").ToString()
                                Dim booking_count As Integer = Convert.ToInt32(reader("booking_count"))

                                ' Add data points to the series for each day with a different color
                                series1.Points.AddXY(service_name & " (" & booking_day & ")", booking_count)
                                series1.Points(series1.Points.Count - 1).Color = colorPalette(colorIndex)

                                ' Increment the color index, looping back to the beginning if necessary
                                colorIndex = (colorIndex + 1) Mod colorPalette.Length
                            End While

                            ' Add the new series to Chart1
                            Chart1.Series.Add(series1)

                            ' Customize the chart appearance
                            series1.ChartType = SeriesChartType.Bar
                            Chart1.Titles.Add("Most Booked Services by Day")
                            Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Angle = -45
                        Else
                            MessageBox.Show("No data returned from the query.")
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error while fetching data from the database: " & ex.Message)
                End Try
            Else
                MessageBox.Show("Error: Unable to open the database connection.")
            End If
        End If
    End Sub

End Class