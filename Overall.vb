
Imports LiveCharts
Imports LiveCharts.Wpf
Imports MySql.Data.MySqlClient
Imports System.Windows.Forms.DataVisualization.Charting
Imports LiveCharts.Defaults
Imports System.Diagnostics.Eventing

Public Class Overall
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ComboBox1.SelectedIndex = 0 Then
            ' Connect to the MySQL database using your module
            Try
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

                    cmd.Connection = conn
                    cmd.CommandText = query

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            ' Clear existing data in CartesianChart1
                            For Each series In CartesianChart1.Series
                                CartesianChart1.Series.Remove(series)
                            Next

                            ' Customize chart appearance
                            CartesianChart1.LegendLocation = LegendLocation.Right

                            CartesianChart1.AxisX.Clear()
                            CartesianChart1.AxisX.Add(New Wpf.Axis With {.Title = "Services", .LabelsRotation = 15})
                            CartesianChart1.AxisY.Clear()
                            CartesianChart1.AxisY.Add(New Wpf.Axis With {.Title = "Booking Count"})

                            ' Create the chart data
                            Dim seriesCollection As New LiveCharts.SeriesCollection()

                            While reader.Read()
                                Dim service_name As String = reader("service_name").ToString()
                                Dim booking_count As Integer = Convert.ToInt32(reader("booking_count"))

                                ' Create a new ColumnSeries for each data point
                                Dim series As New ColumnSeries With {
                                    .Title = service_name,
                                    .Values = New ChartValues(Of Integer) From {booking_count},
                                    .DataLabels = True, ' Show data labels on the bars
                                    .ColumnPadding = 0.5,
                                    .MaxColumnWidth = 100 ' Set the ColumnPadding to add gaps between bars
                                }

                                ' Add the series to CartesianChart1
                                seriesCollection.Add(series)
                            End While

                            ' Add the entire series collection to the chart
                            CartesianChart1.Series = seriesCollection

                        Else
                            MessageBox.Show("No data returned from the query.")
                        End If
                    End Using
                Else
                    MessageBox.Show("Error: Database connection is not open.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error while connecting to the database: " & ex.Message)
            Finally
                ' Always close the connection after usage
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try


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

                    ' Execute the query and create a reader
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        ' Clear existing data in CartesianChart1
                        CartesianChart1.Series.Clear()

                        ' Customize chart appearance
                        CartesianChart1.LegendLocation = LegendLocation.Bottom
                        CartesianChart1.AxisX.Clear() ' Clear existing axes
                        CartesianChart1.AxisX.Add(New Wpf.Axis With {
                            .Title = "Days of the Week",
                            .LabelsRotation = 15,
                            .Separator = New Separator With {.Step = 1}
                        })
                        CartesianChart1.AxisY.Clear() ' Clear existing axes
                        CartesianChart1.AxisY.Add(New Wpf.Axis With {.Title = "Booking Count"})

                        ' Create the chart data
                        Dim seriesCollection As New LiveCharts.SeriesCollection()

                        While reader.Read()

                            Dim booking_day As String = reader("booking_day").ToString()
                            Dim booking_count As Integer = Convert.ToInt32(reader("booking_count"))

                            ' Create a new ColumnSeries for each data point
                            Dim series As New ColumnSeries With {
                                .Title = " (" & booking_day & ")",
                                .Values = New ChartValues(Of Integer) From {booking_count},
                                .ColumnPadding = 50,
                                .MaxColumnWidth = 100' Set ColumnPadding to adjust the gap
                            }

                            ' Add the series to CartesianChart1
                            seriesCollection.Add(series)
                        End While

                        ' Add the entire series collection to the chart
                        CartesianChart1.Series = seriesCollection
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error while fetching data from the database: " & ex.Message)
                End Try
            Else
                MessageBox.Show("Error: Unable to open the database connection.")
            End If
        ElseIf ComboBox1.SelectedIndex = 2 Then
            ' Connect to the MySQL database using your module
            Try
                ConnectDatabase()

                If conn.State = ConnectionState.Open Then
                    ' Query to get the income for each day of the week
                    Dim query As String = "
SELECT DAYNAME(bookings.Datemassage) as booking_day, SUM(services.price) as total_income
FROM bookings
JOIN services ON bookings.service_id = services.service_id
GROUP BY booking_day
ORDER BY FIELD(booking_day, 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday');
"

                    cmd.Connection = conn
                    cmd.CommandText = query

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        CartesianChart1.Series.Clear()
                        If reader.HasRows Then
                            ' Clear existing data in CartesianChart1
                            For Each series In CartesianChart1.Series
                                CartesianChart1.Series.Remove(series)
                            Next

                            ' Customize chart appearance
                            CartesianChart1.LegendLocation = LegendLocation.Right
                            CartesianChart1.AxisX.Clear()
                            CartesianChart1.AxisX.Add(New Wpf.Axis With {.Title = "Days of the Week", .LabelsRotation = 30})
                            CartesianChart1.AxisY.Clear()
                            CartesianChart1.AxisY.Add(New Wpf.Axis With {.Title = "Total Income"})

                            ' Create the chart data
                            Dim seriesCollection As New LiveCharts.SeriesCollection()

                            While reader.Read()
                                Dim booking_day As String = reader("booking_day").ToString()
                                Dim total_income As Decimal = Convert.ToDecimal(reader("total_income"))

                                ' Create a new ColumnSeries for each data point
                                Dim series As New ColumnSeries With {
                                    .Title = booking_day,
                                    .Values = New ChartValues(Of Decimal) From {total_income},
                                    .DataLabels = True, ' Show data labels on the bars
                                    .ColumnPadding = 50, ' Set the ColumnPadding to add more gaps between bars
                                    .MaxColumnWidth = 100 ' Set the MaxColumnWidth to control the size of the bars
                                }

                                ' Add the series to CartesianChart1
                                seriesCollection.Add(series)
                            End While

                            ' Add the entire series collection to the chart
                            CartesianChart1.Series = seriesCollection

                        Else
                            MessageBox.Show("No data returned from the query.")
                        End If
                    End Using
                Else
                    MessageBox.Show("Error: Database connection is not open.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error while connecting to the database: " & ex.Message)
            Finally
                ' Always close the connection after usage
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End If






    End Sub

End Class