Imports MySql.Data.MySqlClient
Public Class bill_History
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
    staff st ON bo.staff_id = st.staff_id "
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

    Private Sub bill_History_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBillsData()

    End Sub
End Class