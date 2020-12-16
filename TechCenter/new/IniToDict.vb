Imports System.IO
Imports System.Text
'20200520
'https://blog.csdn.net/gjban/article/details/48463319
Public Class IniToDict
    Public configSections As Dictionary(Of String, Dictionary(Of String, String))
    Private configData As Dictionary(Of String, String)
    Protected fullFileName As String
    Private m_Encoding As System.Text.Encoding
    Public Sub New(ByVal _filepath As String, t_Encoding As System.Text.Encoding)

        m_Encoding = t_Encoding
        configData = New Dictionary(Of String, String)()
        configSections = New Dictionary(Of String, Dictionary(Of String, String))()
        'fullFileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase & _filepath
        fullFileName = _filepath
        Dim hasCfgFile As Boolean = File.Exists(fullFileName)

        If Not hasCfgFile Then
            File.Create(fullFileName)
        End If
        Me.LoadConfigData()
    End Sub
    Public Sub New(ByVal _filepath As String)
        m_Encoding = Encoding.GetEncoding("utf-8")
        configData = New Dictionary(Of String, String)()
        configSections = New Dictionary(Of String, Dictionary(Of String, String))()
        'fullFileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase & _filepath
        fullFileName = _filepath
        Dim hasCfgFile As Boolean = File.Exists(fullFileName)

        If Not hasCfgFile Then
            File.Create(fullFileName)
        End If
        Me.LoadConfigData()
    End Sub

    'Private Class CSharpImpl
    '    <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
    '    Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
    '        target = value
    '        Return value
    '    End Function
    'End Class

    Public Function GetConfigValue(ByVal sectionName As String, ByVal key As String) As String
        If configSections Is Nothing Then
            Return String.Empty
        End If

        If configSections.Count <= 0 Then
            Return String.Empty
        End If

        If configSections.ContainsKey(sectionName) Then

            If configSections(sectionName).Count <= 0 Then
                Return String.Empty
            ElseIf configSections(sectionName).ContainsKey(key) Then
                Return configSections(sectionName)(key).ToString().Trim()
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function
    Public Sub SetConfigValue(ByVal sectionName As String, ByVal key As String, ByVal value As String)
        If configSections Is Nothing Then
            Return
        End If

        If Not configSections.ContainsKey(sectionName) Then
            configSections.Add(sectionName, New Dictionary(Of String, String)())
        End If

        If configSections(sectionName).ContainsKey(key) Then
            configSections(sectionName)(key) = value
        Else
            configSections(sectionName).Add(key, value)
        End If
    End Sub
    Function ContainsSectionName(ByVal sectionName As String) As Boolean
        Return Me.configSections.ContainsKey(sectionName)
    End Function
    Function ContainsKeyName(ByVal KeyName As String) As Boolean
        Return Me.configData.ContainsKey(KeyName)
    End Function
    '20200326
    Public Overridable Sub SaveConfigData()
        If configSections Is Nothing Then
            Return
        End If

        Using writer As StreamWriter = New StreamWriter(fullFileName, False, m_Encoding)
            Dim enuSection As IDictionaryEnumerator = configSections.GetEnumerator()

            While enuSection.MoveNext()
                writer.WriteLine("[" + enuSection.Key.ToString() + "]")
                Dim tmp As Dictionary(Of String, String) = TryCast(enuSection.Value, Dictionary(Of String, String))

                If tmp IsNot Nothing Then
                    Dim enu As IDictionaryEnumerator = tmp.GetEnumerator()

                    While enu.MoveNext()

                        If enu.Key.ToString().StartsWith(";") Then
                            writer.WriteLine(enu.Value)
                        Else
                            writer.WriteLine(enu.Key & "=" + enu.Value)
                        End If
                    End While
                End If
            End While
        End Using
    End Sub
    '20200326
    Public Overridable Sub LoadConfigData()

        Using reader As StreamReader = New StreamReader(fullFileName, m_Encoding)
            Dim line As String = String.Empty
            Dim indx As Integer = 0
            Dim bNewSection As Boolean = False
            line = reader.ReadLine
            While line IsNot Nothing
                Try
                    If line.StartsWith("[") Then
                        Dim line2 As String = line.Substring(1, line.Length - 2)
                        If Not configSections.ContainsKey(line2.Trim()) Then
                            configData = New Dictionary(Of String, String)()
                            configSections.Add(line2.Trim(), configData)
                            bNewSection = True
                        End If
                    Else

                        If bNewSection Then

                            If line.StartsWith(";") OrElse String.IsNullOrEmpty(line.Trim()) Then
                                configData.Add(";" & Math.Min(System.Threading.Interlocked.Increment(indx), indx - 1), line)
                            Else
                                Dim key_value As String() = line.Trim().Split("="c)

                                If key_value.Length >= 2 Then

                                    If Not configData.ContainsKey(key_value(0)) Then
                                        configData.Add(key_value(0), key_value(1))
                                    End If
                                Else
                                    configData.Add(";" & Math.Min(System.Threading.Interlocked.Increment(indx), indx - 1), line)
                                End If
                            End If
                        End If
                    End If

                Catch ex As Exception

                End Try

                line = reader.ReadLine
            End While
        End Using
    End Sub
    '20200401
    Public Function getConfigData(ByVal sectionName As String) As Dictionary(Of String, String)
        If Me.ContainsSectionName(sectionName) Then
            Return configSections(sectionName)
        Else
            Return Nothing
        End If

    End Function


End Class
'20200520
Public Class IniToDict_2
    Inherits IniToDict
    Public Sub New(ByVal _fileName As String)
        MyBase.New(_fileName)
    End Sub
    Public Sub New(ByVal _fileName As String, t_Encoding As System.Text.Encoding)
        MyBase.New(_fileName, t_Encoding)
    End Sub
    Function get_key(Sections_name As String) As String()
        If configSections.ContainsKey(Sections_name) Then
            Dim key_data As Dictionary(Of String, String) = configSections(Sections_name)
            Dim keyColl As Dictionary(Of String, String).KeyCollection = key_data.Keys
            'keyColl.ToArray() 'test

            Dim keyList As New List(Of String)
            For Each s As String In keyColl
                ''Console.WriteLine("Key = {0}", s)
                keyList.Add(s)
            Next s
            Return keyList.ToArray
        Else
            Return Nothing
        End If

    End Function
    Function get_Sections() As String()
        Dim keyColl As Dictionary(Of String, Dictionary(Of String, String)).KeyCollection = configSections.Keys
        'keyColl.ToArray() 'test

        Dim keyList As New List(Of String)
        For Each s As String In keyColl
            ''Console.WriteLine("Key = {0}", s)
            keyList.Add(s)
        Next s
        Return keyList.ToArray
    End Function
#Region "dict"

    Sub init_default(ByVal sectionName As String, ByVal key As String, ByVal value As String)
        If configSections Is Nothing Then
            Return
        End If

        If Not configSections.ContainsKey(sectionName) Then
            configSections.Add(sectionName, New Dictionary(Of String, String)())
        End If

        If configSections(sectionName).ContainsKey(key) Then
            'configSections(sectionName)(key) = value
        Else
            configSections(sectionName).Add(key, value)
        End If
    End Sub
    Sub updata(ByVal sectionName As String, ByVal key As String, ByVal value As String)
        MyBase.SetConfigValue(sectionName, key, value)
    End Sub

    Sub delete(ByVal sectionName As String, ByVal key As String)
        'MyBase.SetConfigValue(sectionName, key, Nothing)
        If configSections Is Nothing Then
            Exit Sub
        End If

        If Not configSections.ContainsKey(sectionName) Then
            configSections.Add(sectionName, New Dictionary(Of String, String)())
        End If
        configSections(sectionName).Remove(key)
        'If configSections(sectionName).ContainsKey(key) Then
        '    configSections(sectionName)(key) = value
        'Else
        '    configSections(sectionName).Add(key, value)
        'End If
    End Sub
    Sub delete(ByVal sectionName As String)
        'MyBase.SetConfigValue(sectionName, Nothing, Nothing)
        If configSections Is Nothing Then
            Exit Sub
        End If
        configSections.Remove(sectionName)
        'If Not configSections.ContainsKey(sectionName) Then
        '    configSections.Add(sectionName, New Dictionary(Of String, String)())
        'End If
        'configSections.Remove(key)
    End Sub

    Function search(ByVal sectionName As String, ByVal key As String) As String
        Return MyBase.GetConfigValue(sectionName, key)
    End Function
    Sub insert(ByVal sectionName As String, ByVal key As String, ByVal value As String)
        MyBase.SetConfigValue(sectionName, key, value)
    End Sub

    'Public Overrides Sub SaveConfigData()

    'End Sub
    'Public Overrides Sub LoadConfigData()

    'End Sub
#End Region
End Class