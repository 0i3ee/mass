Public Class BookingInfo
    Public Property BookingId As Integer
    Public Property CustomerName As String
    Public Property Status As String
    Public Property DateMessage As DateTime

    Public Sub New(bookingId As Integer, customerName As String, status As String, dateMessage As DateTime)
        Me.BookingId = bookingId
        Me.CustomerName = customerName
        Me.Status = status
        Me.DateMessage = dateMessage
    End Sub
End Class