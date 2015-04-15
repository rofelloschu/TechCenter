'參數
'<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
'Public Class ParameterFile
'    Private ParameterFile As tools_nf2.ParameterFile_undone
'    'Private ParameterPath As String = M_TCParameter.CurrentPath + "\AVIMgr\AVIMgr_Parameter.cfg"
'    '  Private ParameterName As Dictionary(Of ParameterValue(Of Object), Integer)

'    Sub New()
'        'Dim testint As New ParameterValue(Of Integer)("testint", 1)
'        'testint = New ParameterValue(Of Integer)("testint", 1)
'        'Dim s As List(Of ParameterValue(Of Object))
'        '  s.Add(testint)
'        Dim a As xx(Of xxxx)
'        a = New xx(Of xxxx)

'    End Sub
'    Sub close()

'    End Sub
'End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class xx(Of s)
    Sub New()

    End Sub
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Enum xxxx
    a
    b

End Enum
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class ParameterValue2
    Protected m_value As Object
    Protected m_Name As String
    Protected m_RecordValue As String
    Sub New(ByVal t_name As String, ByVal t_value As Object)
        Me.m_Name = t_name
        Me.m_value = t_value
    End Sub
    Public Property Name() As String
        Get
            Return Me.m_Name
        End Get
        Set(ByVal value As String)
            Me.m_Name = value
        End Set
    End Property
    Function getValue() As Object
        Return m_value
    End Function


    Public Property RecordValue() As String
        Get
            Me.ValueToRecordValue()
            Return Me.m_RecordValue
        End Get
        Set(ByVal value As String)
            Me.m_RecordValue = value
            Me.RecordValueToValue()
        End Set
    End Property
    Protected Overridable Sub RecordValueToValue()
        Me.m_value = Me.m_RecordValue
    End Sub
    Protected Overridable Sub ValueToRecordValue()
        Me.m_RecordValue = Me.m_value
    End Sub
End Class
 
