Public Class CheckBillForm
    Private ReadOnly selectedBooking As BookingInfo

    ' Constructor that accepts a BookingInfo object
    Public Sub New(ByVal bookingInfo As BookingInfo)
        InitializeComponent()

        ' Store the selected booking information
        selectedBooking = bookingInfo

        ' Call a method to display the information in the form (you need to create this method)
        DisplayBookingInfo()
    End Sub

    ' Method to display the booking information in the form
    Private Sub DisplayBookingInfo()
        ' Example: Display the information in labels or textboxes in the form
        Label4.Text = "Booking ID: " & selectedBooking.BookingId.ToString()
        Label5.Text = "Customer Name: " & selectedBooking.CustomerName
        Label6.Text = "Status: " & selectedBooking.Status
        Label7.Text = "Date Message: " & selectedBooking.DateMessage.ToString("yyyy-MM-dd")
    End Sub
End Class
