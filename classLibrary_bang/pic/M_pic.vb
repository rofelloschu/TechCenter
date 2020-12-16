
Imports System.Drawing
Module M_pic
    'https://www.cnblogs.com/bomo/archive/2013/02/25/2932700.html
    '2020109 增加字顏色
    Function TextToBitmap(ByVal text As String, ByVal font As Font, ByVal rect As Rectangle, ByVal fontcolor As Color, ByVal backColor As Color) As Bitmap
        Dim g As Graphics
        Dim bmp As Bitmap
        Dim format As StringFormat = New StringFormat(StringFormatFlags.NoClip)

        If rect = Rectangle.Empty Then
            bmp = New Bitmap(1, 1)
            g = Graphics.FromImage(bmp)
            Dim sizef As SizeF = g.MeasureString(text, font, PointF.Empty, format)
            Dim width As Integer = CInt((sizef.Width + 1))
            Dim height As Integer = CInt((sizef.Height + 1))
            rect = New Rectangle(0, 0, width, height)
            bmp.Dispose()
            bmp = New Bitmap(width, height)
        Else
            bmp = New Bitmap(rect.Width, rect.Height)
        End If

        g = Graphics.FromImage(bmp)
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
        g.FillRectangle(New SolidBrush(backColor), rect)
        Dim font_Brushes As New SolidBrush(fontcolor)
        g.DrawString(text, font, font_Brushes, rect, format)
        Return bmp
    End Function
    'https://dotblogs.com.tw/ricochen/2009/12/07/12324
    '水平合併
    Function HorizontalMergeImages(ByVal img1 As Image, ByVal img2 As Image) As Image
        Dim MergedImage As Image = Nothing
        Dim Wide As Int32 = 0
        Dim High As Int32 = 0
        Wide = img1.Width + img2.Width

        If img1.Height >= img2.Height Then
            High = img1.Height
        Else
            High = img2.Height
        End If

        Dim mybmp As Bitmap = New Bitmap(Wide, High)
        Dim gr As Graphics = Graphics.FromImage(mybmp)
        gr.DrawImage(img1, 0, 0)
        gr.DrawImage(img2, img1.Width, 0)
        MergedImage = mybmp
        gr.Dispose()
        Return MergedImage
    End Function
    '垂直合併
    Function VerticalMergeImages(ByVal img1 As Image, ByVal img2 As Image) As Image
        Dim MergedImage As Image = Nothing
        Dim Wide As Int32 = 0
        Dim High As Int32 = 0
        High = img1.Height + img2.Height

        If img1.Width >= img2.Width Then
            Wide = img1.Width
        Else
            Wide = img2.Width
        End If

        Dim mybmp As Bitmap = New Bitmap(Wide, High)
        Dim gr As Graphics = Graphics.FromImage(mybmp)
        gr.DrawImage(img1, 0, 0)
        gr.DrawImage(img2, 0, img1.Height)
        MergedImage = mybmp
        gr.Dispose()
        Return MergedImage
    End Function
    '浮水印
    Function MarkImage(ByVal img1 As Image, ByVal img2 As Image) As Image
        Dim MergedImage As Image = Nothing
        Dim gr As Graphics = System.Drawing.Graphics.FromImage(img1)
        Dim Logo As Bitmap = New Bitmap(img2.Width, img2.Height)
        Dim tgr As Graphics = Graphics.FromImage(Logo)
        Dim cmatrix As System.Drawing.Imaging.ColorMatrix = New System.Drawing.Imaging.ColorMatrix()
        cmatrix.Matrix33 = 0.5F
        Dim imgattributes As System.Drawing.Imaging.ImageAttributes = New System.Drawing.Imaging.ImageAttributes()
        imgattributes.SetColorMatrix(cmatrix, System.Drawing.Imaging.ColorMatrixFlag.[Default], System.Drawing.Imaging.ColorAdjustType.Bitmap)
        tgr.DrawImage(img2, New Rectangle(0, 0, Logo.Width, Logo.Height), 0, 0, Logo.Width, Logo.Height, GraphicsUnit.Pixel, imgattributes)
        tgr.Dispose()
        Dim i1_w As Integer = img1.Width / 3
        gr.DrawImage(Logo, i1_w, 10)

        gr.Dispose()
        MergedImage = img1
        Return MergedImage
    End Function

#Region "字轉圖 待改"
    '圖片橫式' 20200116
    Public Function DrawTextString(ByVal text As List(Of String), ByVal width As Integer, ByVal height As Integer, ByVal fontSize As Integer, ByVal colorIndex As Byte(), Optional ByVal V_Bound As Integer = 0, Optional ByVal H_Bound As Integer = 0, Optional ByVal V_Space As Integer = 0, Optional ByVal H_Space As Integer = 0) As Bitmap
        Dim bmp As Bitmap = New Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555)
        Dim g As Graphics = Graphics.FromImage(bmp)
        g.Clear(Color.Black)
        Dim offsetX, offsetY, charSize As Integer
        offsetY = V_Bound
        Dim ColorArray As List(Of Byte) = New List(Of Byte)()
        Dim no As Integer = 0

        For i As Integer = 0 To text.Count - 1
            Dim str As String = text(i)
            offsetX = H_Bound
            Dim line As Char() = str.ToCharArray()

            For k As Integer = 0 To line.Length - 1
                Dim ch As Char = line(k)
                Dim b As Byte() = System.Text.Encoding.GetEncoding(950).GetBytes(New Char() {ch})

                If b.Length = 1 Then
                    charSize = fontSize / 2
                Else
                    charSize = fontSize
                End If

                no += b.Length
                Dim mCharColorIndex As Byte

                If colorIndex IsNot Nothing AndAlso colorIndex.Length > 0 AndAlso CInt(Math.Ceiling(CDec((no / 2.0)) - 1)) < colorIndex.Length Then
                    mCharColorIndex = colorIndex(CInt(Math.Ceiling(CDec((no / 2.0)))) - 1)
                Else
                    mCharColorIndex = GetDefaultColor()
                End If

                ColorArray.Add(mCharColorIndex)
                Dim CharBmp As Bitmap

                If b.Length = 1 AndAlso b(0) >= CByte(&H21) AndAlso b(0) <= CByte(&H25) Then
                    '畫圖
                    Dim PictureCode As Byte() = New Byte() {b(0), System.Text.Encoding.GetEncoding(950).GetBytes(New Char() {line(k + 1)})(0)}
                    'Dim message As String = ""
                    CharBmp = DrawPicture(PictureCode)
                    'If message <> "" Then
                    '    'CommonClass.WriteErrorLog(message)
                    '    'log紀錄
                    'End If

                    k += 1
                    no += 1

                    For y As Integer = 0 To CharBmp.Height - 1

                        For x As Integer = 0 To CharBmp.Width - 1
                            bmp.SetPixel(offsetX + x, offsetY + y, CharBmp.GetPixel(x, y))
                        Next
                    Next

                    offsetX += fontSize + H_Space
                Else
                    '文字
                    CharBmp = DrawChar(ch, charSize, fontSize, System.IO.Directory.GetCurrentDirectory)
                    CharBmp.Save("testdraw.bmp")
                    Dim foreColor, backColor As Color
                    Dim flashRate As Decimal
                    ConvertColorIndex(CByte(mCharColorIndex), foreColor, backColor, flashRate)
                    For y As Integer = 0 To fontSize - 1

                        For x As Integer = 0 To charSize - 1

                            If CharBmp.GetPixel(x, y).R > 0 OrElse CharBmp.GetPixel(x, y).G > 0 OrElse CharBmp.GetPixel(x, y).B > 0 Then
                                bmp.SetPixel(offsetX + x, offsetY + y, foreColor)
                            Else
                                bmp.SetPixel(offsetX + x, offsetY + y, backColor)
                            End If
                        Next
                    Next


                    offsetX += charSize + H_Space
                End If
            Next

            offsetY += fontSize + V_Space
        Next

        Return bmp
    End Function

    Private Sub ConvertColorIndex(ByVal colorIndex As Byte, ByRef foreColor As Color, ByRef backColor As Color, ByRef flashRate As Decimal)
        Dim foreColorValue As Integer = colorIndex And &H3

        Select Case foreColorValue
            Case 1
                foreColor = Color.Green
            Case 2
                foreColor = Color.Red
            Case 3
                foreColor = Color.Yellow
            Case Else
                foreColor = Color.Black
        End Select

        ' ''' Cannot convert LocalDeclarationStatementSyntax, System.ArgumentOutOfRangeException: Exception of type 'System.ArgumentOutOfRangeException' was thrown.
        ' ''' Parameter name: op
        ' ''' Actual value was RightShiftExpression.
        ' '''    at ICSharpCode.CodeConverter.Util.VBUtil.GetExpressionOperatorTokenKind(SyntaxKind op)
        ' '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertTopLevelExpression(ExpressionSyntax topLevelExpression)
        ' '''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
        ' '''    at ICSharpCode.CodeConverter.VB.MethodBodyVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
        ' ''' 
        ' ''' Input: 
        ' '''             int backColorValue = (colorIndex & 0x0c) >> 2;

        ' ''' 
        Dim backColorValue As Integer = (colorIndex And &HC) >> 2
        Select Case backColorValue
            Case 1
                backColor = Color.Green
            Case 2
                backColor = Color.Red
            Case 3
                backColor = Color.Yellow
            Case Else
                backColor = Color.Black
        End Select

        ' ''' Cannot convert LocalDeclarationStatementSyntax, System.ArgumentOutOfRangeException: Exception of type 'System.ArgumentOutOfRangeException' was thrown.
        ' ''' Parameter name: op
        ' ''' Actual value was LeftShiftExpression.
        ' '''    at ICSharpCode.CodeConverter.Util.VBUtil.GetExpressionOperatorTokenKind(SyntaxKind op)
        ' '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.VisitBinaryExpression(BinaryExpressionSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertTopLevelExpression(ExpressionSyntax topLevelExpression)
        ' '''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
        ' '''    at ICSharpCode.CodeConverter.VB.MethodBodyVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        ' '''    at Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        ' '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
        ' '''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
        ' ''' 
        ' ''' Input: 
        ' '''             int flashRateValue = colorIndex << 2 >> 6;

        ' ''' 
        Dim flashRateValue As Integer = colorIndex >> 4
        Select Case flashRateValue
            Case 1
                flashRate = 1
            Case 2
                flashRate = 0.5D
            Case 3
                flashRate = 2
            Case Else
                flashRate = 0
        End Select
    End Sub
    Private Function GetDefaultColor() As Byte
        Return CByte(2)
    End Function
    '20200116
    Private Function DrawChar(ByVal ch As Char, ByVal charSize As Integer, ByVal fontSize As Integer, font_dir_path As String) As Bitmap
        Dim bmp As Bitmap = New Bitmap(charSize, fontSize)
        Dim g As Graphics = Graphics.FromImage(bmp)
        g.Clear(Color.Black)
        Dim fontRatio As Decimal = 1
        'If System.Text.Encoding.GetEncoding(950).GetByteCount(New Char() {ch}) = 1 Then fontRatio = 0.75D
        If System.Text.Encoding.GetEncoding(950).GetByteCount(New Char() {ch}) = 1 Then fontRatio = 0.6
        If System.Text.Encoding.GetEncoding(950).GetByteCount(New Char() {ch}) = 2 AndAlso System.Text.Encoding.GetEncoding(950).GetBytes(New Char() {ch})(0) >= &HF0 Then
            Dim big5code As String = BitConverter.ToString(System.Text.Encoding.GetEncoding(950).GetBytes(New Char() {ch})).Replace("-", "")
            'Dim fileList As System.IO.FileInfo() = New System.IO.DirectoryInfo(System.IO.Path.Combine(ParameterClass.CurrentPath, "font")).GetFiles(big5code & "*.*")
            Dim fileList As System.IO.FileInfo() = New System.IO.DirectoryInfo(font_dir_path).GetFiles(big5code & "*.*")

            For Each fileInfo As System.IO.FileInfo In fileList
                Dim fileContent As Byte() = System.IO.File.ReadAllBytes(fileInfo.FullName)
                Dim fileAttributes As String() = fileInfo.FullName.Replace(fileInfo.Extension, "").Split("_"c)
                Dim image As Bitmap = New Bitmap(32, 32)

                For row As Integer = 0 To bmp.Height - 1

                    For col As Integer = 0 To (bmp.Width / 8) - 1
                        Dim i As Integer = row * (bmp.Width / 8) + col

                        For j As Integer = 7 To 0
                            image.SetPixel((col * 8) + (7 - j), row, If((fileContent(i) And CByte(Math.Pow(2, j))) > 0, Color.White, Color.Black))
                        Next
                    Next
                Next

                g.DrawImage(image, 0, 0)
                Return bmp
            Next
        End If

        Dim brush = New SolidBrush(Color.White)
        'DrawString(s As String, font As System.Drawing.Font, brush As System.Drawing.Brush, x As Single, y As Single)
        'If fontSize = 16 Then
        '    g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((11 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))
        'ElseIf fontSize = 24 Then
        '    g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((15 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))
        'ElseIf fontSize = 32 Then
        '    'g.DrawString(Convert.ToString(ch), New Font("微軟正黑體", CInt((22 * fontRatio))), brush, -3, If(fontRatio = 1, -5, 5))
        '    g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((22 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))

        '    'Dim format As StringFormat = New StringFormat(StringFormatFlags.NoClip)
        '    'Dim sizef As SizeF = g.MeasureString(ch, New Font("細明體", CInt((22 * fontRatio))), PointF.Empty, format)
        '    'Dim width As Integer = CInt((sizef.Width + 1))
        '    'Dim height As Integer = CInt((sizef.Height + 1))
        '    'Dim rect As Rectangle = New Rectangle(0, 0, width, height)
        '    'g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((22 * fontRatio))), brush, rect, Format)
        'ElseIf fontSize = 64 Then
        '    g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((46 * fontRatio))), brush, -9, If(fontRatio = 1, 2, 10))
        'End If

        If fontRatio = 1 Then
            font_set.set_A_Font_22()
            g.DrawString(Convert.ToString(ch), font_set.Font, brush, font_set.x, font_set.y)

        Else
            font_set.set_A_Font_22_onebyte()
            g.DrawString(Convert.ToString(ch), font_set.Font, brush, font_set.x, font_set.y)
        End If


        Return bmp
    End Function
    '20200116
    '20200226
    Private Function DrawPicture(ByVal PictureCode As Byte(), Optional filepath As String = "") As Bitmap
        'message = ""
        'Dim PictureID As String = BitConverter.ToString(PictureCode).Replace("-", "")
        Dim bmp As Bitmap


        If PictureCode.Length <> 2 Then
            bmp = New Bitmap(32, 32)
            Return bmp
        End If

        Dim hex_str As String = PictureCode(0).ToString("X2") + PictureCode(1).ToString("X2")
        'Dim picpath As String = m_colormap.getbmppath(hex_str)
        Dim picpath As String = filepath

        If Not System.IO.File.Exists(picpath) Then
            bmp = New Bitmap(32, 32)
            'message = "無此圖檔(" & PictureID & ".bmp) !!"
            'CommonClass.WriteErrorLog(message) 'log
        Else
            bmp = New Bitmap(picpath)
        End If
        Return bmp
    End Function
    Private Function CalculateDisplayedText(ByVal text As String, ByVal fontSize As Integer, ByVal blockWidth As Integer, ByVal blockHeight As Integer) As List(Of String)
        Dim list As List(Of String) = New List(Of String)()
        Dim firstRow As String = String.Empty
        Dim secondRow As String = String.Empty
        Dim charCountPerRow As Integer = (blockWidth / fontSize) * 2
        Dim charCountPerColumn As Integer = blockHeight / fontSize

        For Each ch As Char In text
            secondRow += ch
            Dim currStrByte As Byte() = System.Text.Encoding.GetEncoding(950).GetBytes(secondRow)
            Dim charByteCount As Integer = currStrByte.Length

            If charByteCount > charCountPerRow Then
                list.Add(firstRow)
                secondRow = String.Empty
                If list.Count = charCountPerColumn Then Exit For
                secondRow += ch
            End If

            firstRow = secondRow
        Next

        If secondRow <> String.Empty Then list.Add(secondRow)
        Return list
    End Function
    Class font_set
        Public Shared Font As System.Drawing.Font
        Public Shared fontsize As Integer
        Public Shared x As Single
        Public Shared y As Single

        '
        Shared Sub set_A_Font_22()
            fontsize = 22
            Font = New Font("細明體", fontsize)
            x = -1
            y = 1
        End Sub
        Shared Sub set_A_Font_22_onebyte()
            fontsize = 18
            Font = New Font("細明體", fontsize)
            x = -1
            y = 3
        End Sub

        '     If fontSize = 16 Then
        '        g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((11 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))
        '    ElseIf fontSize = 24 Then
        '        g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((15 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))
        '    ElseIf fontSize = 32 Then
        ''g.DrawString(Convert.ToString(ch), New Font("微軟正黑體", CInt((22 * fontRatio))), brush, -3, If(fontRatio = 1, -5, 5))
        '        g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((22 * fontRatio))), brush, -1, If(fontRatio = 1, 1, 3))

        ''Dim format As StringFormat = New StringFormat(StringFormatFlags.NoClip)
        ''Dim sizef As SizeF = g.MeasureString(ch, New Font("細明體", CInt((22 * fontRatio))), PointF.Empty, format)
        ''Dim width As Integer = CInt((sizef.Width + 1))
        ''Dim height As Integer = CInt((sizef.Height + 1))
        ''Dim rect As Rectangle = New Rectangle(0, 0, width, height)
        ''g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((22 * fontRatio))), brush, rect, Format)
        '    ElseIf fontSize = 64 Then
        '        g.DrawString(Convert.ToString(ch), New Font("細明體", CInt((46 * fontRatio))), brush, -9, If(fontRatio = 1, 2, 10))
        '    End If
    End Class
#End Region

End Module
