'20150507
'ParameterFile_undone�W�[

'�ݧ�L�ݦs����r�ɭק鷺�e
'Ū�g�ɮ�
'�Ȯɨ��Nmutex
'�M�glog�� logFile class
'�iŪ�i�g�Ѽ��� ParameterFile class
'20140127�W�[logFile_undone
'20150507 �ץ�ĵ�i
Imports System.IO
Imports System.Threading
'Namespace tools.file
<ObsoleteAttribute("DLL�L��", False)> _
 Public Class APFile
    Implements IDisposable
    'Imports System.IO
    Private Path As String
    Private name As String
    Private data() As String
    ' Private output_mutex As Mutex = New Mutex(False)
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
#Region "New"
    '�b�{���P���|�U�إ�Parameter.txt
    Sub New()
        Me.New(Directory.GetCurrentDirectory(), "Parameter.txt")
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), "Parameter.txt")
    End Sub
    '��Jstring���|,string�ɦW�̷ӿ�J�ȫإ�
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
    End Sub
    '��Jstring�ɦW�̷ӿ�J�ȫإ�
    Sub New(ByVal newName As String)
        Me.New(Directory.GetCurrentDirectory(), newName)
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), newName)
    End Sub
#End Region

    'Ū���ɮפ��e��mdata(string)
    Public Function load() As String()
        If System.IO.File.Exists(readPath) Then
            data = System.IO.File.ReadAllLines(readPath)
            'data = read()
            Return data
        Else
            Return Nothing
        End If

    End Function
    '�ɮ׬O�_�s�b
    Function Exists() As Boolean
        Return System.IO.File.Exists(readPath)
    End Function
    '��J���|�ɮ׬O�_�s�b
    Function Exists(ByVal filePath As String) As Boolean
        Return System.IO.File.Exists(filePath)
    End Function

#Region "�s��"
    '�Ndata�x�s���ɮ�
    Public Sub save()
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        isDataNull()
        System.IO.File.WriteAllLines(readPath, data)
        'For lineCount As Integer = 0 To data.Length - 1
        '    write(data(lineCount))
        'Next
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '��Jstring�ɦW,�Ndata�x�s���ɮ�
    Public Sub save(ByVal fileName As String)
        isDataNull()
        System.IO.File.WriteAllLines((Path + "\" + fileName), data)
        'setName(fileName)
        'save()
    End Sub
    '��Jstring()��Ʀ�data,�Ndata�x�s���ɮ�
    Public Sub save(ByVal data As String())
        setData(data)
        save()
    End Sub
    '��Jstring�ɦWsting()��Ʀ�data,�Ndata�x�s���ɮ�
    Public Sub save(ByVal fileName As String, ByVal data As String())
        setData(data)
        save(fileName)
    End Sub
    '��Jstring������|,�Ndata�x�s���ɮ�
    Sub saveAs(ByVal allPath As String)
        setPathAndName(allPath)
        save()
    End Sub
#End Region
#Region "�g�J(���J)"
    '��Jstring,�g�J�ɮ�
    Public Sub write(ByVal text As String)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '��Jbyte(),�ഫ���Q���i��string�g�J�ɮ�
    Public Sub write(ByVal data() As Byte)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        Dim text As String = Nothing
        For i As Integer = 0 To data.Length - 1
            text = text + data(i).ToString("X2")
        Next
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '?
    Public Sub write_Sp(ByVal data() As Byte)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        Dim text As String = Nothing
        For i As Integer = 0 To data.Length - 1
            text = text + data(i).ToString("X2") + " "
        Next
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub

    '��J����rstring�έn�g�J��,�N�g�J���л\����r�᭱����
    Function writeKey(ByVal keyword As String, ByVal writeText As String) As String
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim keyMessage As String = Nothing
        Try

            If keyword Is Nothing OrElse writeText Is Nothing Then
                Exit Try
            End If
            Dim keywordNumber As Integer
            read()
            For index As Integer = 0 To data.Length - 1
                keywordNumber = data(index).IndexOf(keyword)
                If keywordNumber = -1 Then
                Else
                    keyMessage = data(index).Substring(keywordNumber + keyword.Length)
                    data(index) = keyword + writeText
                    Exit For
                End If
            Next
            AutoResetEvent.Set()
            save()
            Return keyMessage
        Catch ex As Exception
            AutoResetEvent.Set()
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Nothing
        Finally
            ' AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try
        Return Nothing
    End Function
#End Region
#Region "Ū�X"
    '�^��Ū�쪺���estring()
    Public Function read() As String()
        Return read(Nothing, Nothing)
    End Function
    '��J�}�Y����rstring,�^�ǥ�������r���᪺��
    Public Function read(ByVal keyword As String) As String()
        Return read(keyword, Nothing)
    End Function
    '��J�}�Y����rstring��������rstring,�^�ǥ���������r��������
    Public Function read(ByVal keyword As String, ByVal endword As String) As String()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim NextData As String
            Do
                'Ū���@��rstring
                NextData = sr.ReadLine()
                If NextData Is Nothing Then
                Else
                    stringLineS.Add(NextData)
                    Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                    If (keyword Is Nothing) OrElse (stringLine.IndexOf(keyword) = -1) Then
                        If (endword Is Nothing) OrElse (stringLine.IndexOf(endword) = -1) Then
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(0)
                            If (keyword Is Nothing) Then
                            Else
                                stringLineS.RemoveAt(stringLineS.Count - 1)
                            End If
                        Else
                            Dim endAddr As Integer = stringLine.IndexOf(endword)
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(0, endAddr)
                        End If
                        'Console.WriteLine(stringLineS.Item(stringLineS.Count - 1))
                    Else
                        Dim startAddr As Integer = stringLine.IndexOf(keyword) + keyword.Length
                        If (endword Is Nothing) OrElse (stringLine.IndexOf(endword) = -1) Then

                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(startAddr)
                        Else
                            Dim endAddr As Integer = stringLine.IndexOf(endword) - startAddr
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(startAddr, endAddr)
                        End If


                        'Console.WriteLine(stringLineS.Item(stringLineS.Count - 1))
                    End If
                End If

            Loop Until NextData Is Nothing
            data = stringLineS.ToArray()
            sr.Close()
            GC.Collect()
            Return Me.data
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Me.data
        Finally
        End Try
    End Function
    '��J����rstring,�^������r���᪺��(��@)
    Function readKey(ByVal keyword As String) As String

        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim keyMessage As String = Nothing
            Do

                stringLineS.Add(sr.ReadLine())
                If stringLineS.Item(stringLineS.Count - 1) Is Nothing Then

                Else
                    If keyword Is Nothing Then

                        ' Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                    Else
                        Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                        If stringLine.IndexOf(keyword) = -1 Then
                            'stringLines.Item(stringLines.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)

                        Else
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)
                            keyMessage = stringLineS.Item(stringLineS.Count - 1)
                            'Ū������r,���Xloop����
                            Exit Do
                        End If
                    End If
                    'Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                End If
            Loop Until stringLineS.Item(stringLineS.Count - 1) Is Nothing
            'data = stringLines.ToArray()
            sr.Close()
            Return keyMessage
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Nothing
        Finally
            AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try

    End Function
    '    Function readArray() As Array

    '   End Function
#End Region
#Region "�R��"
    '�R���̫�@��
    Sub deleteEndLine()
        load()
        Dim tempDate(data.Length - 2) As String
        For count As Integer = 0 To tempDate.Length - 1
            tempDate(count) = data(count)
        Next
        save(tempDate)
    End Sub
    '�R��keyword��
    Function deleteKeyLine(ByVal keywork As String) As Boolean
        Dim isdelete As Boolean = False
        Dim KeywordIndex As Integer
        Try
            KeywordIndex = Me.getKeywordIndex(keywork)
            If KeywordIndex = -1 Then
                isdelete = False
            Else
                load()
                Dim dataList As New List(Of String)
                For count As Integer = 0 To data.Length - 1
                    If Not count = KeywordIndex Then
                        dataList.Add(data(count))
                    Else
                        isdelete = True
                    End If
                Next
                save(dataList.ToArray)
            End If
            Return isdelete
        Catch ex As Exception
            Return False
        End Try
    End Function
    '�R���ɮ�
    Sub delete()
        If Exists() Then
            System.IO.File.Delete(readPath)
        End If

    End Sub
#End Region

    'data�O�_�s�b,���s�b�w�]��"Null"
    Function isDataNull() As Boolean
        Dim isNull As Boolean = False
        If data Is Nothing Then
            Dim temp(0) As String
            temp(0) = "Null"
            data = temp
            isNull = True
        End If
        Return isNull
    End Function
    '���okeyword������
    Function getKeywordIndex(ByVal keyword As String) As Integer
        ' output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim keywordIndex As Integer = -1
            Do

                stringLineS.Add(sr.ReadLine())
                If stringLineS.Item(stringLineS.Count - 1) Is Nothing Then

                Else
                    If keyword Is Nothing Then

                        ' Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                    Else
                        Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                        If stringLine.IndexOf(keyword) = -1 Then
                            'stringLines.Item(stringLines.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)

                        Else
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)
                            keywordIndex = stringLineS.Count - 1
                            'Ū������r,���Xloop����
                            Exit Do
                        End If
                    End If
                    'Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                End If
            Loop Until stringLineS.Item(stringLineS.Count - 1) Is Nothing
            'data = stringLines.ToArray()
            sr.Close()
            Return keywordIndex
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return -1
        Finally
            AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try
    End Function
    '�]�w�ɦW
    Public Sub setName(ByVal fileName As String)
        Me.name = fileName
    End Sub
    '�]�w���|�ɦW
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub
    '�^�ǧ�����| string 
    Function readPath() As String

        Return Path + "\" + name
    End Function
    '�^��data string()
    Function getData() As String()
        Return data
    End Function
    '�]�wdata
    Sub setData(ByVal Data As String())
        Me.data = Data
    End Sub
    '???
    Function readValue(ByVal FilePath As String, ByVal ElementName As String) As String
        Try
            Dim sr As StreamReader = New StreamReader(FilePath, System.Text.Encoding.Default)
            Dim line As String = Nothing
            Do
                line = sr.ReadLine()
                If Not (line Is Nothing) Then
                    If line.Length > 0 Then
                        If line.Substring(0, line.IndexOf(":")) = ElementName Then
                            sr.Close()
                            Return line.Substring(line.IndexOf(":") + 1, line.Length - line.IndexOf(":") - 1)
                        End If
                    End If
                End If
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    '�^���ɦW
    Function getName() As String
        Return name
    End Function
    '�]�w������|
    Sub setPathAndName(ByVal allpath As String)

        Dim newPath() As String = allpath.Split("\")
        Me.name = newPath(newPath.Length - 1)
        Me.Path = allpath.ToString.Substring(0, allpath.Length - name.Length - 1)
    End Sub
    '�Ѻc
    Sub Dispose() Implements IDisposable.Dispose
        GC.SuppressFinalize(Me)
    End Sub

End Class
'<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
'<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _

'<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _


'Ūcsv�ɪ��


'End Namespace
