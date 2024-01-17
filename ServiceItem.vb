Public Class ServiceItem
    Public Property ServiceId As Integer
    Public Property ServiceName As String
    Public Property Price As Decimal

    Public Overrides Function ToString() As String
        ' Override ToString to display service_name in ComboBox
        Return ServiceName
    End Function
End Class
