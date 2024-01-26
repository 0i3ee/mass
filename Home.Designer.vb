<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Home
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Guna2CustomGradientPanel1 = New Guna.UI2.WinForms.Guna2CustomGradientPanel()
        Me.Guna2Button2 = New Guna.UI2.WinForms.Guna2Button()
        Me.Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Guna2CustomGradientPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(64, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Booking Queue"
        '
        'Guna2CustomGradientPanel1
        '
        Me.Guna2CustomGradientPanel1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom
        Me.Guna2CustomGradientPanel1.Controls.Add(Me.DateTimePicker1)
        Me.Guna2CustomGradientPanel1.Controls.Add(Me.Guna2Button2)
        Me.Guna2CustomGradientPanel1.Controls.Add(Me.Panel1)
        Me.Guna2CustomGradientPanel1.Controls.Add(Me.Label3)
        Me.Guna2CustomGradientPanel1.FillColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Guna2CustomGradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Guna2CustomGradientPanel1.FillColor3 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Guna2CustomGradientPanel1.Location = New System.Drawing.Point(-2, -3)
        Me.Guna2CustomGradientPanel1.Name = "Guna2CustomGradientPanel1"
        Me.Guna2CustomGradientPanel1.Size = New System.Drawing.Size(933, 1046)
        Me.Guna2CustomGradientPanel1.TabIndex = 1
        '
        'Guna2Button2
        '
        Me.Guna2Button2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.Guna2Button2.FillColor = System.Drawing.Color.Transparent
        Me.Guna2Button2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Guna2Button2.ForeColor = System.Drawing.Color.White
        Me.Guna2Button2.Image = Global.WindowsApp1.My.Resources.Resources.refresh2
        Me.Guna2Button2.ImageAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Guna2Button2.ImageSize = New System.Drawing.Size(50, 50)
        Me.Guna2Button2.Location = New System.Drawing.Point(803, 46)
        Me.Guna2Button2.Name = "Guna2Button2"
        Me.Guna2Button2.Size = New System.Drawing.Size(70, 62)
        Me.Guna2Button2.TabIndex = 45
        Me.Guna2Button2.UseTransparentBackground = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Location = New System.Drawing.Point(71, 131)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(802, 833)
        Me.Panel1.TabIndex = 4
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(540, 62)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 31)
        Me.DateTimePicker1.TabIndex = 46
        '
        'Home
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(930, 1041)
        Me.Controls.Add(Me.Guna2CustomGradientPanel1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Home"
        Me.Text = "Home"
        Me.Guna2CustomGradientPanel1.ResumeLayout(False)
        Me.Guna2CustomGradientPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Guna2CustomGradientPanel1 As Guna.UI2.WinForms.Guna2CustomGradientPanel
    Friend WithEvents Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Button2 As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents DateTimePicker1 As DateTimePicker
End Class