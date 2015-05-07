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
Class OnlyReadFile
    Implements IDisposable

    Private disposedValue As Boolean = False        ' �����h�l���I�s
    Private data As String()
    Private name As String
    Private Path As String
    Private output_mutex As Mutex = New Mutex(False)
#Region "New"
    '��Jstring���|,string�ɦW�̷ӿ�J�ȫإ�
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
    End Sub
    '��Jstring�ɦW�̷ӿ�J�ȫإ�
    Sub New(ByVal newName As String)
        Me.New(Directory.GetCurrentDirectory(), newName)
    End Sub
    Sub New()

    End Sub
#End Region

    'Ū���ɮפ��e��mdata(string)
    Public Function load() As String()
        If System.IO.File.Exists(Me.readPath) Then

            data = System.IO.File.ReadAllLines(Me.readPath)
            'data = read()
            Return data
        Else
            Throw New Exception(readPath)
            Return Nothing
        End If

    End Function
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

        output_mutex.WaitOne()
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
            output_mutex.ReleaseMutex()
        End Try

    End Function
    '    Function readArray() As Array

    '   End Function
#End Region
    '�]�w���|�ɦW
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub

    '�^�ǧ�����| string 
    Function readPath() As String

        Return Path + "\" + name
    End Function
    Sub close()

    End Sub
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: ���T�I�s������ Unmanaged �귽
            End If

            ' TODO: ����@�Ϊ� Unmanaged �귽
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' �� Visual Basic �s�W���{���X�H���T��@�i�B�m���Ҧ��C
    Public Sub Dispose() Implements IDisposable.Dispose
        ' �Ф��ܧ󦹵{���X�C�b�H�W�� Dispose �m�J�M���{���X (ByVal �������L�ȳB�m)�C
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
'<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Class OnlyWriteFile
    Implements IDisposable
    Private disposedValue As Boolean = False        ' �����h�l���I�s

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: ���T�I�s������ Unmanaged �귽
            End If

            ' TODO: ����@�Ϊ� Unmanaged �귽
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' �� Visual Basic �s�W���{���X�H���T��@�i�B�m���Ҧ��C
    Public Sub Dispose() Implements IDisposable.Dispose
        ' �Ф��ܧ󦹵{���X�C�b�H�W�� Dispose �m�J�M���{���X (ByVal �������L�ȳB�m)�C
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
'<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Public Class logFile_undone
    Implements IDisposable
    Private disposedValue As Boolean = False        ' �����h�l���I�s
    Private isHaveTime As Boolean
    Private FileName As String
    Private Encoding As System.Text.Encoding
    Private m_FileMaxCount As Integer
    Private directory As String

    Private FilePath As String
    Private fileDate As DateTime

    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub New(ByVal t_fileNmae As String, ByVal t_isHaveTime As Boolean, Optional ByVal t_directory As String = "")
        Me.FileName = t_fileNmae
        Me.isHaveTime = t_isHaveTime
        Me.Encoding = System.Text.Encoding.Default
        Me.m_FileMaxCount = 1


        If t_directory = "" Then
            Me.directory = System.IO.Directory.GetCurrentDirectory
        Else
            'If System.IO.Directory.Exists(t_directory) Then
            '    Me.directory = t_directory
            'Else
            '    Me.directory = System.IO.Directory.GetCurrentDirectory
            'End If
            Me.directory = t_directory
            If Not System.IO.Directory.Exists(Me.directory) Then
                System.IO.Directory.CreateDirectory(Me.directory)
            End If
        End If
        Me.fileDate = Now
        If Me.isHaveTime Then
            Me.FilePath = directory + "\" + FileName + "_" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"

        Else
            Me.FilePath = directory + "\" + FileName + ".txt"
        End If

        'Console.WriteLine("test")
    End Sub
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: ���T�I�s������ Unmanaged �귽
            End If

            ' TODO: ����@�Ϊ� Unmanaged �귽
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' �� Visual Basic �s�W���{���X�H���T��@�i�B�m���Ҧ��C
    Public Sub Dispose() Implements IDisposable.Dispose
        ' �Ф��ܧ󦹵{���X�C�b�H�W�� Dispose �m�J�M���{���X (ByVal �������L�ȳB�m)�C
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
    Public Function getDirectory() As String
        Return Me.directory
    End Function
    Public Function getPath() As String
        Return Me.FilePath
    End Function
    '�w�p�����ɮ׼�
    Property FileMaxCount() As Integer
        Set(ByVal value As Integer)
            AutoResetEvent.WaitOne()

            Me.m_FileMaxCount = value

            Try
                '��ƽƻs
                Dim sou_datastring As String() = System.IO.File.ReadAllLines(Me.FilePath)
                If sou_datastring.Length > Me.m_FileMaxCount Then
                    Dim newMaxCount As Integer = Me.m_FileMaxCount \ 2
                    Dim des_datastring(newMaxCount - 1) As String
                    Dim sou_index As Integer = sou_datastring.Length - newMaxCount
                    Array.Copy(sou_datastring, sou_index, des_datastring, 0, newMaxCount)
                    '��ƧR��
                    System.IO.File.Delete(Me.FilePath)
                    '��ƭ���
                    System.IO.File.WriteAllLines(Me.FilePath, des_datastring)
                End If


            Catch ex As Exception
            Finally
                AutoResetEvent.Set()

            End Try


        End Set
        Get
            Return Me.m_FileMaxCount
        End Get

    End Property
    Private Sub checktime()
        If Me.isHaveTime Then
            If Not fileDate.Day.Equals(Now.Day) Then
                fileDate = Now
                Me.FilePath = directory + "\" + FileName + "_" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"

            End If
        Else
            Me.FilePath = directory + "\" + FileName + ".txt"
        End If
    End Sub


    Sub Writte(ByVal text As String)

        AutoResetEvent.WaitOne()
        Try
            Me.checktime()
            Dim sw As New StreamWriter(FilePath, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()
        Catch ex As Exception

            AutoResetEvent.Set()
            M_catchException.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)
        End Try

        AutoResetEvent.Set()
    End Sub
    Sub time_Writte(ByVal t_time As DateTime, ByVal text As String)
        AutoResetEvent.WaitOne()
        Try
            Dim t_FilePath As String = directory + "\" + FileName + "_" + t_time.Year.ToString + "-" + t_time.Month.ToString("D2") + "-" + t_time.Day.ToString("D2") + ".txt"
            Dim sw As New StreamWriter(t_FilePath, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()
        Catch ex As Exception

            AutoResetEvent.Set()
            M_catchException.exWritte(Now.ToString + " logFile_undone.time_Writte " + ex.ToString)
        End Try

        AutoResetEvent.Set()
    End Sub
End Class
<ObsoleteAttribute("DLL�L��", False)> _
Public Class ParameterFile_undone
    'key:value,value
    Implements IDisposable
    Private Path As String

    Private m_mainKey As String
    Private m_subKey As String
    Private AutoResetEvent As AutoResetEvent
    Private waitSaveData As List(Of String)
    Sub New(ByVal m_Path As String)
        AutoResetEvent = New AutoResetEvent(True)
        waitSaveData = New List(Of String)
        m_mainKey = ":"
        m_subKey = ","
        Me.Path = m_Path
        If checkFilePath() Then

        Else
            '�ɮ׫إ�
            System.IO.File.WriteAllText(Me.Path, "")
        End If
    End Sub
    Private Function checkFilePath() As Boolean
        Return System.IO.File.Exists(Me.Path)
    End Function
    Private Function TryKeyIndex(ByVal TKey As String) As Integer

        If Not checkFilePath() Then
            Return False
        End If
        Dim data As String() = System.IO.File.ReadAllLines(Me.Path)
        For index As Integer = 0 To data.Length - 1
            '���LTKey
            If data(index).IndexOf(TKey) > -1 Then
                '���LmainKey
                If data(index).IndexOf(Me.m_mainKey) > -1 Then
                    Return index
                End If
            End If
        Next
        Return -1
    End Function

    Private Sub WriteLastLine(ByVal Text As String)
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(Me.Path, True, System.Text.Encoding.Default)
        sw.Write(Text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
    End Sub

    Private Sub WriteLastLine(ByVal Text As String())
        AutoResetEvent.WaitOne()
        System.IO.File.WriteAllLines(Me.Path, Text)
        AutoResetEvent.Set()
    End Sub
    '�]�w
    Function TrySetValue(ByVal TKey As String, ByVal TValue As String) As Boolean
        Try
            Dim KeyIndex As Integer = TryKeyIndex(TKey)
            If KeyIndex = -1 Then
                '�Lkey��
                Me.WriteLastLine(TKey + Me.m_mainKey + TValue)
                Return True
            Else
                Dim data As String() = System.IO.File.ReadAllLines(Me.Path)
                data(KeyIndex) = TKey + Me.m_mainKey + TValue
                Me.WriteLastLine(data)
                Return True
            End If
        Catch ex As Exception

        End Try

        Return False
    End Function
    Function TrySetValueS(ByVal TKey As String, ByVal TValues As String) As Boolean
        Return False
    End Function
    Function TrySetValue_waitSave(ByVal TKey As String, ByVal TValue As String) As Boolean
        Try

            Dim temp_data As String()
            Dim KeyIndex As Integer = -1
            '��list key
            If Me.waitSaveData.Count > 0 Then
                temp_data = Me.waitSaveData.ToArray
                For index As Integer = 0 To temp_data.Length - 1
                    '���LTKey
                    If temp_data(index).IndexOf(TKey) > -1 Then
                        '���LmainKey
                        If temp_data(index).IndexOf(Me.m_mainKey) > -1 Then
                            KeyIndex = index
                            Exit For
                        End If
                    End If
                Next
            End If

            If KeyIndex = -1 Then
                '�Lkey��
                Me.waitSaveData.Add(TKey + Me.m_mainKey + TValue)
                Return True
            Else


                Me.waitSaveData(KeyIndex) = TKey + Me.m_mainKey + TValue

                Return True
            End If
        Catch ex As Exception

        End Try

        Return False
    End Function
    Function TrySetValue_Save() As Boolean
        If Me.waitSaveData.Count > 0 Then
            Me.WriteLastLine(waitSaveData.ToArray)
            Me.waitSaveData.Clear()
        End If
        '�ݧ�
        Return True
    End Function
    'Ū��
    Function TryGetValue(ByVal TKey As String, ByRef TValue As String) As Boolean
        Try
            Dim KeyIndex As Integer = TryKeyIndex(TKey)
            If KeyIndex = -1 Then
                '�Lkey��
                Return False
            Else

                Dim data As String() = System.IO.File.ReadAllLines(Me.Path)
                Dim mainKeyIndex As Integer = data(KeyIndex).IndexOf(Me.m_mainKey)
                TValue = data(KeyIndex).Substring(mainKeyIndex + 1, data(KeyIndex).Length - mainKeyIndex - 1)
                Return True
            End If
        Catch ex As Exception

        End Try

        Return False
    End Function
    Function TryGetValueS(ByVal TKey As String, ByRef TValues As String()) As Boolean
        Return False
    End Function
    Public Property mainKey() As String
        Get
            Return Me.m_mainKey
        End Get
        Set(ByVal value As String)
            Me.m_mainKey = value
        End Set
    End Property
    Public Property subKey() As String
        Get
            Return Me.m_subKey
        End Get
        Set(ByVal value As String)
            Me.m_subKey = value
        End Set
    End Property
    Private disposedValue As Boolean = False        ' �����h�l���I�s
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: ���T�I�s������ Unmanaged �귽
            End If

            ' TODO: ����@�Ϊ� Unmanaged �귽
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' �� Visual Basic �s�W���{���X�H���T��@�i�B�m���Ҧ��C
    Public Sub Dispose() Implements IDisposable.Dispose
        ' �Ф��ܧ󦹵{���X�C�b�H�W�� Dispose �m�J�M���{���X (ByVal �������L�ȳB�m)�C
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Public Class ParameterArrayFile_undone
    Private TopNameList As List(Of String)
    Private ValueList As List(Of String)
    Private Path As String
    Private name As String
    Private data() As String
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
        TopNameList = New List(Of String)
    End Sub
    'Sub readFile()

    'End Sub
    'Sub saveFile()
    '    Dim tempList As List(Of String)

    'End Sub
    '�]�w���|�ɦW
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub
    'Ū���ɮפ��e��mdata(string)
    Public Function load() As String()
        If System.IO.File.Exists(readPath) Then
            data = System.IO.File.ReadAllLines(Me.readPath)
            'data = read()
            Return data
        Else
            Return Nothing
        End If
    End Function
    '�^�ǧ�����| string 
    Function readPath() As String
        Return Path + "\" + name
    End Function
    '�Ndata�x�s���ɮ�
    Public Sub save()
        isDataNull()
        System.IO.File.WriteAllLines(readPath, data)
    End Sub
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
#Region "TopName"
    Sub AddTopName(ByVal TopName As String)
        For index As Integer = 0 To TopNameList.Count - 1
            If TopNameList(index) = TopName Then
                Throw New Exception("TopName����")
            End If
        Next
        TopNameList.Add(TopName)
    End Sub
    Function getTopName() As String()
        Return TopNameList.ToArray
    End Function
    Private Function getTopNameArrayValue() As String
        Dim text As String = ""
        For index As Integer = 0 To TopNameList.Count - 1
            If index = 0 Then
                text = TopNameList(0)
            Else
                text = text + "," + TopNameList(index)
            End If
        Next
        Return text
    End Function

    Function getTopNameIndex(ByVal TopName As String) As Integer
        For index As Integer = 0 To TopNameList.Count - 1
            If TopNameList(index) = TopName Then
                Return index
            End If
        Next
        Return -1
    End Function
#End Region
#Region "Value"

#End Region
    Function getNewLineArray() As String()
        Dim return_string(TopNameList.Count - 1) As String
        Return return_string
    End Function
    Function SetNewValue(ByVal value As String()) As Boolean
        If Not value.Length = TopNameList.Count Then
            Return False
        End If
        Dim line As String = ""
        For index As Integer = 0 To value.Length - 1
            If index = 0 Then
                line = value(0)
            Else
                line = line + "," + value(index)
            End If
        Next
        ValueList.Add(line)
        '�ݧ�
        Return True
    End Function

End Class
'Ūcsv�ɪ��
<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Class csvFile_undone
    Inherits APFile
    Private Table(,) As String
    Private TableHead() As String
    Private isHead As Boolean
    Private max_X As Integer
    Private max_Y As Integer
    'x��V
    'y���V
    Sub New(ByVal newName As String, ByVal isHead As Boolean)
        MyBase.New(Directory.GetCurrentDirectory(), newName)

        Me.isHead = isHead
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), newName)
    End Sub

    Function readTable() As String(,)
        read()
        Dim tableIndex As Integer = 0
        If isHead Then
            TableHead = MyBase.getData(tableIndex).Split(",")
            tableIndex = tableIndex + 1
        End If
        Dim lines As String()
        max_Y = getData.Length - 1 - tableIndex
        max_X = getData(0).Split(",").Length - 1
        ReDim Table(max_X, max_Y)
        For index As Integer = tableIndex To getData.Length - 1
            'getData.CopyTo(TableHead, index, 0)
            lines = MyBase.getData(index).Split(",")

            For index2 As Integer = 0 To lines.Length - 1
                Table(index2, index - 1) = lines(index2)
            Next
        Next
        'max_Y = Table.GetLength(0)
        'max_X = Table.GetLength(1)
        Return getTable()
    End Function
    Function getTable() As String(,)
        Return Table
    End Function
    Function getTableHead() As String()
        Return TableHead
    End Function
End Class
<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Class csvWrite_undone
    Inherits APFile
    Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub
    Sub wrtite(ByVal text As String())
        Dim newText As String = String.Empty
        For index As Integer = 0 To text.Length - 1
            If index = 0 Then
                newText = text(index)
            Else
                newText = newText + "," + text(index)
            End If
        Next
        MyBase.write(newText)
    End Sub
End Class
'End Namespace
