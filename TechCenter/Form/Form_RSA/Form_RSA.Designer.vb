<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_RSA
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意:  以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBox_key_path = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button_TypeA = New System.Windows.Forms.Button()
        Me.Button_create = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox_key_name = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox_key_path
        '
        Me.TextBox_key_path.Location = New System.Drawing.Point(93, 24)
        Me.TextBox_key_path.Name = "TextBox_key_path"
        Me.TextBox_key_path.Size = New System.Drawing.Size(322, 22)
        Me.TextBox_key_path.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "設定"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button_TypeA
        '
        Me.Button_TypeA.Location = New System.Drawing.Point(29, 111)
        Me.Button_TypeA.Name = "Button_TypeA"
        Me.Button_TypeA.Size = New System.Drawing.Size(75, 23)
        Me.Button_TypeA.TabIndex = 2
        Me.Button_TypeA.Text = "Type A"
        Me.Button_TypeA.UseVisualStyleBackColor = True
        '
        'Button_create
        '
        Me.Button_create.Location = New System.Drawing.Point(340, 120)
        Me.Button_create.Name = "Button_create"
        Me.Button_create.Size = New System.Drawing.Size(75, 47)
        Me.Button_create.TabIndex = 3
        Me.Button_create.Text = "建立新的key"
        Me.Button_create.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(340, 202)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 47)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "建立新的key"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox_key_name
        '
        Me.TextBox_key_name.Location = New System.Drawing.Point(340, 173)
        Me.TextBox_key_name.Name = "TextBox_key_name"
        Me.TextBox_key_name.Size = New System.Drawing.Size(75, 22)
        Me.TextBox_key_name.TabIndex = 5
        Me.TextBox_key_name.Text = "sunsky"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(233, 214)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "test"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Form_RSA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 261)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox_key_name)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button_create)
        Me.Controls.Add(Me.TextBox_key_path)
        Me.Controls.Add(Me.Button_TypeA)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form_RSA"
        Me.Text = "Form_RSA"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox_key_path As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button_TypeA As System.Windows.Forms.Button
    Friend WithEvents Button_create As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox_key_name As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
