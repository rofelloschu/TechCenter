Imports System.IO
Imports System.Threading
<ObsoleteAttribute("DLL過時", False)> _
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
            '檔案建立
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
            '有無TKey
            If data(index).IndexOf(TKey) > -1 Then
                '有無mainKey
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
    '設定
    Function TrySetValue(ByVal TKey As String, ByVal TValue As String) As Boolean
        Try
            Dim KeyIndex As Integer = TryKeyIndex(TKey)
            If KeyIndex = -1 Then
                '無key值
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
            '找list key
            If Me.waitSaveData.Count > 0 Then
                temp_data = Me.waitSaveData.ToArray
                For index As Integer = 0 To temp_data.Length - 1
                    '有無TKey
                    If temp_data(index).IndexOf(TKey) > -1 Then
                        '有無mainKey
                        If temp_data(index).IndexOf(Me.m_mainKey) > -1 Then
                            KeyIndex = index
                            Exit For
                        End If
                    End If
                Next
            End If

            If KeyIndex = -1 Then
                '無key值
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
        '待改
        Return True
    End Function
    '讀取
    Function TryGetValue(ByVal TKey As String, ByRef TValue As String) As Boolean
        Try
            Dim KeyIndex As Integer = TryKeyIndex(TKey)
            If KeyIndex = -1 Then
                '無key值
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
    Private disposedValue As Boolean = False        ' 偵測多餘的呼叫
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 明確呼叫時釋放 Unmanaged 資源
            End If

            ' TODO: 釋放共用的 Unmanaged 資源
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' 由 Visual Basic 新增此程式碼以正確實作可處置的模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 請勿變更此程式碼。在以上的 Dispose 置入清除程式碼 (ByVal 視為布林值處置)。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class