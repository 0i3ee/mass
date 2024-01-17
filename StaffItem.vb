Public Class StaffItem
    Public Property StaffId As Integer
    Public Property Name As String

    Public Overrides Function ToString() As String
        ' Override ToString to display Name in ComboBox
        Return Name
    End Function
End Class
