Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient
Imports Mysqlx.XDevAPI.Relational

Public Class CheckBillForm
    Dim WithEvents Pn As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Private billIdToPrint As Integer
    Private totalAmountToPrint As Decimal
    Private staffNameToPrint As String
    Private Service As String
    Private timeInToPrint As DateTime

    Private Sub CheckBillForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBillsData()
    End Sub
    Private Sub LoadBillsData()
        ConnectDatabase() ' Call the method to open the database connection

        ' Check if the connection is open
        If conn.State = ConnectionState.Open Then
            Dim query As String = "SELECT
    b.bill_id,
    b.total_amount,
    b.payment_status,
    bo.booking_date,
    bo.customer_name,
    s.service_name,
    s.price,
    st.Name AS staff_name
FROM
    bills b
JOIN
    bookings bo ON b.booking_id = bo.booking_id
JOIN
    services s ON bo.service_id = s.service_id
JOIN
    staff st ON bo.staff_id = st.staff_id where payment_status = 'unpaid'"
            Dim dt As New DataTable()

            Try
                ' Use the MySqlCommand and MySqlDataAdapter for MySQL
                Using command As New MySqlCommand(query, conn)
                    Using adapter As New MySqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using

                ' Bind the DataTable to the DataGridView
                Guna2DataGridView1.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("Error while fetching data: " & ex.Message)
            End Try
        End If

        ' Close the database connection when done
        conn.Close()
    End Sub

    Private Sub Guna2DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            ' Assuming the required columns are at specific indices, modify as per your actual DataGridView setup
            billIdToPrint = CInt(Guna2DataGridView1.Rows(e.RowIndex).Cells("bill_id").Value)
            totalAmountToPrint = CDec(Guna2DataGridView1.Rows(e.RowIndex).Cells("total_amount").Value)
            staffNameToPrint = Guna2DataGridView1.Rows(e.RowIndex).Cells("staff_name").Value.ToString()
            Service = Guna2DataGridView1.Rows(e.RowIndex).Cells("service_name").Value.ToString()
            timeInToPrint = DateTime.Now
            ' Call the PrintBill method to print the bill using the selected data
            UpdatePaymentStatusToPaid(billIdToPrint)
            PrintBill()
        End If
    End Sub
    Private Sub PrintBill()
        PPD.Document = Pn
        DirectCast(PPD, Form).WindowState = FormWindowState.Maximized
        PPD.ShowDialog()
    End Sub
    Private Sub UpdatePaymentStatusToPaid(billId As Integer)
        ConnectDatabase()

        If conn.State = ConnectionState.Open Then
            Try
                ' Construct the update query
                Dim updateQuery As String = "UPDATE bills SET payment_status = 'paid' WHERE bill_id = @billId"

                ' Create a MySqlCommand object
                Using command As New MySqlCommand(updateQuery, conn)
                    ' Add parameters to the query
                    command.Parameters.AddWithValue("@billId", billId)

                    ' Execute the update query
                    command.ExecuteNonQuery()

                    ' Optionally, you can display a message or perform other actions after the update
                    MessageBox.Show("Payment status updated to 'paid' successfully.")
                End Using
            Catch ex As Exception
                MessageBox.Show("Error updating payment status: " & ex.Message)
            End Try
        End If

        conn.Close()
    End Sub

    Private Sub Pn_BeginPrint(sender As Object, e As PrintEventArgs) Handles Pn.BeginPrint
        Dim Pnsetup As New PageSettings
        'Pnsetup.PaperSize = New PaperSize("Customr", 250, 500)
        Pnsetup.PaperSize = New PaperSize("Customr", 250, 300)
        Pn.DefaultPageSettings = Pnsetup
    End Sub

    Private Sub Pn_PrintPage(sender As Object, e As PrintPageEventArgs) Handles Pn.PrintPage
        Dim f16b As New Font("Saysettha OT", 16, FontStyle.Bold)
        Dim f12 As New Font("Saysettha OT", 12, FontStyle.Regular)
        Dim f10b As New Font("Saysettha OT", 10, FontStyle.Bold)
        Dim f10 As New Font("Saysettha OT", 10, FontStyle.Regular)
        Dim f8 As New Font("Saysettha OT", 8, FontStyle.Regular)
        Dim f8b As New Font("Saysettha OT", 8, FontStyle.Bold)
        Dim marginL As Integer = Pn.DefaultPageSettings.Margins.Left
        Dim marginC As Integer = Pn.DefaultPageSettings.PaperSize.Width / 2
        Dim marginR As Integer = Pn.DefaultPageSettings.PaperSize.Width

        Dim Right As New StringFormat
        Dim Center As New StringFormat
        Right.Alignment = StringAlignment.Far
        Center.Alignment = StringAlignment.Center

        Dim Line As String = "------------------------------"

        ' Your existing code for drawing the document
        e.Graphics.DrawString("ຮ້ານນວດ", f16b, Brushes.Black, marginC, 8, Center)
        e.Graphics.DrawString("Tel 089-999-2221", f10, Brushes.Black, marginC, 50, Center)
        e.Graphics.DrawString("ໃບບິນ", f10b, Brushes.Black, marginC, 70, Center)
        e.Graphics.DrawString(Line, f10, Brushes.Black, marginC, 85, Center)

        e.Graphics.DrawString("Staff :", f10b, Brushes.Black, 8, 100)
        If Not String.IsNullOrEmpty(staffNameToPrint) Then
            e.Graphics.DrawString(staffNameToPrint, f10, Brushes.Black, marginR - 8, 100, Right)
        End If

        e.Graphics.DrawString("Date :", f10b, Brushes.Black, 8, 115)
        e.Graphics.DrawString(timeInToPrint.ToString("dd/MM/yyyy"), f10, Brushes.Black, marginR - 8, 115, Right)

        e.Graphics.DrawString(Line, f10, Brushes.Black, marginC, 130, Center)

        e.Graphics.DrawString("Servicename", f10b, Brushes.Black, 8, 145)
        e.Graphics.DrawString("Price", f10b, Brushes.Black, marginR - 8, 145, Right)

        If Not String.IsNullOrEmpty(staffNameToPrint) Then
            e.Graphics.DrawString(Service, f10, Brushes.Black, marginL + 63, 170, Right)
        End If

        If Not String.IsNullOrEmpty(staffNameToPrint) Then
            e.Graphics.DrawString(totalAmountToPrint.ToString("#,##0.00"), f10, Brushes.Black, marginR - 8, 170, Right)
        End If

        e.Graphics.DrawString(Line, f10, Brushes.Black, marginC, 190, Center)
        e.Graphics.DrawString("Thank you", f10b, Brushes.Black, marginC, 220, Center)

    End Sub
End Class
