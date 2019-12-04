Imports System.Xml
Imports System.IO
'20150320
'閱20180309 僅參考價值
Public Class xmlPFile

    'path new已驗證有無檔案
    Private path As String
    Private num As Integer

    '   Private oarray As Object()
    '讀存值獨立不影響
    Private GetArray As Object()
    Private willSetArray As Object()
    Private m_Mutex As System.Threading.Mutex
    Sub New(t_path As String, t_num As Integer)
        m_Mutex = New System.Threading.Mutex
        Me.path = t_path

        Me.num = t_num
        ReDim GetArray(t_num - 1)
        For index As Integer = 0 To GetArray.Length - 1
            GetArray(index) = New Dictionary(Of String, String)
        Next
        willSetArray = GetArray.Clone

        If Not System.IO.File.Exists(path) Then
            'Throw New Exception("路徑錯誤")
            Try
                '建立檔案
                Dim t_FileStream As System.IO.FileStream = System.IO.File.Create(path)
                '結束檔案使用 
                t_FileStream.Close()

                ' System.IO.File.Create(path)
                '存成xml格式
                Me.save()
            Catch ex As Exception
                Throw New Exception("路徑錯誤")
            End Try

        End If

        'Console.WriteLine(willSetArray.GetEnumerator.GetHashCode.ToString)
        'Console.WriteLine(GetArray.GetEnumerator.GetHashCode.ToString)
    End Sub
    Sub close()

        GetArray = Nothing
        willSetArray = Nothing
        m_Mutex.Close()
    End Sub

    ReadOnly Property FullName As String
        Get
            Dim t_fileInfo As New System.IO.FileInfo(Me.path)
            Return t_fileInfo.FullName
        End Get
    End Property

    Sub setValue(ByVal index As Integer, ByVal name As String, ByVal value As String)
        Dim tempDictionary As Dictionary(Of String, String) = willSetArray(index)
        If tempDictionary.ContainsKey(name) Then
            tempDictionary(name) = value
        Else
            tempDictionary.Add(name, value)
        End If

    End Sub
    Function getValue(ByVal index As Integer, ByVal name As String) As String

        Dim tempDictionary As Dictionary(Of String, String) = GetArray(index)
        Dim value As String = ""
        If tempDictionary.TryGetValue(name, value) Then
            Return value
        Else
            Return Nothing
        End If
    End Function
    Function getValue(ByVal index As Integer, ByVal name As String, Default_value As String) As String

        Dim tempDictionary As Dictionary(Of String, String) = GetArray(index)
        Dim value As String = ""
        If tempDictionary.TryGetValue(name, value) Then
            Return value
        Else
            Me.setValue(index, name, Default_value)
            Return Default_value
        End If
    End Function

    Sub save()
        m_Mutex.WaitOne()
        Dim XTW As New XmlTextWriter(Me.path, Nothing)
        XTW.WriteStartDocument()
        XTW.Formatting = Formatting.Indented
        XTW.WriteStartElement("main")
        Try
            For index1 As Integer = 0 To willSetArray.Length - 1

                XTW.WriteStartElement("Team" + index1.ToString("D2"))
                Dim tempDictionary As Dictionary(Of String, String) = willSetArray(index1)

                For Each kvp As KeyValuePair(Of String, String) In tempDictionary
                    XTW.WriteElementString(kvp.Key, kvp.Value)
                Next
                XTW.WriteEndElement()
            Next

        Catch ex As Exception

        Finally
            XTW.WriteEndElement()
            XTW.WriteEndDocument()
            XTW.Close()
        End Try
        m_Mutex.ReleaseMutex()


    End Sub
    Sub load()
        m_Mutex.WaitOne()
        Dim XmlDoc As New XmlDocument()

        XmlDoc.Load(Me.path)

        Dim root As XmlNode = XmlDoc.SelectSingleNode("main")
        '  root = root.SelectSingleNode("Team01")
        'Console.WriteLine(root.SelectSingleNode("rrr").InnerText)
        'Console.WriteLine(root.InnerText)
        'Exit Sub
        For index1 As Integer = 0 To GetArray.Length - 1
            Try
                Dim tempDictionary As Dictionary(Of String, String) = GetArray(index1)
                Dim teamXmlNode As XmlNode = root.SelectSingleNode("Team" + index1.ToString("D2"))
                Dim XNL As XmlNodeList = teamXmlNode.ChildNodes

                tempDictionary.Clear()
                Dim name As String = ""
                Dim value As String = ""
                For index2 As Integer = 0 To XNL.Count - 1
                    name = XNL(index2).Name
                    value = XNL(index2).InnerText
                    tempDictionary.Add(name, value)
                Next
            Catch ex As Exception
                m_Mutex.ReleaseMutex()
                Throw New Exception("load Fail")
            End Try

        Next
        m_Mutex.ReleaseMutex()
        GC.Collect()
    End Sub

    Private Sub testload()
        Dim XTR As XmlTextReader
        XTR = New Xml.XmlTextReader(Me.path)

        Try
            While XTR.Read()
                Select Case XTR.NodeType
                    Case XmlNodeType.Element
                        Console.Write("<{0}", XTR.Name)
                        If XTR.HasAttributes Then
                            While XTR.MoveToNextAttribute()
                                Console.Write(" {0}=" + Chr(34) + "{1}" + Chr(34), XTR.Name, XTR.Value)
                            End While
                            XTR.MoveToElement()
                        End If
                        Console.Write(">")
                    Case XmlNodeType.Text
                        Console.Write(XTR.Value)
                    Case XmlNodeType.CDATA
                        Console.Write("<![CDATA[{0}]]>", XTR.Value)
                    Case XmlNodeType.ProcessingInstruction
                        Console.Write("<?{0} {1}?>", XTR.Name, XTR.Value)
                    Case XmlNodeType.Comment
                        Console.Write("<!--{0}-->", XTR.Value)
                    Case XmlNodeType.XmlDeclaration
                        Console.WriteLine("<?xml version='1.0'?>")
                    Case XmlNodeType.Document

                    Case XmlNodeType.DocumentType
                        Console.Write("<!DOCTYPE {0} [{1}]", XTR.Name, XTR.Value)
                    Case XmlNodeType.EntityReference
                        Console.Write(XTR.Name)
                    Case XmlNodeType.EndElement
                        Console.WriteLine("</{0}>", XTR.Name)
                End Select
            End While



        Catch ex As Exception
        Finally
            XTR.Close()
        End Try
    End Sub
End Class
Public Class xmlPFileOne
    Inherits xmlPFile
    Sub New(t_path As String)
        MyBase.New(t_path, 1)
    End Sub
    Overloads Sub setValue(ByVal name As String, ByVal value As String)
        MyBase.setValue(1, name, value)
    End Sub
    Overloads Function getValue(ByVal name As String) As String
        Return MyBase.getValue(1, name)
    End Function
    Overloads Function getValue(ByVal index As Integer, ByVal name As String, Default_value As String) As String
        Return MyBase.getValue(1, name, Default_value)
    End Function
End Class

