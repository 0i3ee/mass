Public Class timeslotitem
    Public Property timeslotid As Integer
    Public Property timeslotName As String

    Public Overrides Function ToString() As String
        ' Override ToString to display Name in ComboBox
        Return timeslotName
    End Function
End Class
