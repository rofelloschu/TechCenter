'20140327
Public Module M_EncodingDefault
    '文字檔編碼判斷
    Function bom(ByVal header() As Byte) As System.Text.Encoding
        If header.Length < 1 Then
            Return System.Text.Encoding.Default
        End If
        Select Case header(0)
            Case &HEF
                If header.Length >= 3 AndAlso header(1) = &HBB AndAlso header(2) = &HBF Then
                    Return System.Text.Encoding.UTF8
                End If
            Case &HFE
                If header.Length >= 2 AndAlso header(1) = &HFF Then
                    Return System.Text.Encoding.BigEndianUnicode
                End If
            Case &HFF
                If header.Length >= 2 AndAlso header(1) = &HFE Then
                    Return System.Text.Encoding.Unicode
                End If
                If header.Length >= 4 AndAlso header(1) = &HFE AndAlso header(2) = &H0 AndAlso header(3) = &H0 Then
                    Return System.Text.Encoding.UTF32
                End If
            Case &H0
                If header.Length >= 4 AndAlso header(1) = &H0 AndAlso header(2) = &HFE AndAlso header(3) = &HFF Then
                    Return System.Text.Encoding.UTF32
                End If
            Case Else
                Return System.Text.Encoding.Default
                ' Return Nothing
        End Select
        Return System.Text.Encoding.Default
    End Function
    Function ByteToString(ByVal data As Byte()) As String

        Try
            If data.Length < 1 Then
                '  Return System.Text.Encoding.Default.GetString(data)
                Return ""
            End If

            Select Case data(0)
                Case &HEF
                    If data.Length >= 3 AndAlso data(1) = &HBB AndAlso data(2) = &HBF Then
                        Return System.Text.Encoding.UTF8.GetString(data)
                    End If
                Case &HFE
                    If data.Length >= 2 AndAlso data(1) = &HFF Then
                        Return System.Text.Encoding.BigEndianUnicode.GetString(data)
                    End If
                Case &HFF
                    If data.Length >= 2 AndAlso data(1) = &HFE Then
                        Return System.Text.Encoding.Unicode.GetString(data)
                    End If
                    If data.Length >= 4 AndAlso data(1) = &HFE AndAlso data(2) = &H0 AndAlso data(3) = &H0 Then
                        Return System.Text.Encoding.UTF32.GetString(data)
                    End If
                Case &H0
                    If data.Length >= 4 AndAlso data(1) = &H0 AndAlso data(2) = &HFE AndAlso data(3) = &HFF Then
                        Return System.Text.Encoding.UTF32.GetString(data)
                    End If
                Case Else
                    'System.Text.Encoding.ASCII
                    Return System.Text.Encoding.Default.GetString(data)
            End Select
        Catch ex As Exception
            ' M_catchException.exWritte("ByteToString")
            Throw ex
        End Try

        ' Return Nothing
        'Console.WriteLine(Encoding.ToString)
        'Console.WriteLine(Encoding.EncodingName)
        'Console.WriteLine(System.Text.Encoding.Default.ToString)
        'Console.WriteLine(System.Text.Encoding.Default.EncodingName.ToString)
        'Console.WriteLine(System.Text.Encoding.UTF8.ToString)
        'Console.WriteLine(System.Text.Encoding.UTF8.EncodingName.ToString)
        'Select Case Encoding.ToString
        '    Case System.Text.Encoding.Default.ToString
        '        'Return bom(data).GetString(data)
        '    Case System.Text.Encoding.UTF8.ToString
        '        Return bom(data).GetString(data)
        '    Case System.Text.Encoding.BigEndianUnicode.ToString
        '        Return bom(data).GetString(data)
        '    Case System.Text.Encoding.Unicode.ToString
        '        Return bom(data).GetString(data)
        '    Case System.Text.Encoding.UTF32.ToString
        '        Return bom(data).GetString(data)
        '    Case Else
        '        'Return bom(data).GetString(data)
        '        Return ""
        'End Select
        Return ""
    End Function
 
End Module
