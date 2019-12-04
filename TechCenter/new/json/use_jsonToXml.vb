Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Public Class use_jsonToXml
    '依據變數名稱改變
    Sub New()
        Dim vd As New vd
        'Dim Response As New Response
        vd.vd.address = "sunsky"
        vd.vd.time = Now.ToString("yyyy-MM-dd hh:mm:ss")
        Dim lane1 As New lanedata
        lane1.lane = "1"
        lane1.direction = "南"
        lane1.smallcar = "1"
        lane1.bigcar = "4"
        lane1.totalcar = (CInt(lane1.smallcar) + CInt(lane1.bigcar)).ToString
        Dim lane2 As New lanedata
        lane2.lane = "2"
        lane2.direction = "北"
        lane2.smallcar = "3"
        lane2.bigcar = "1"
        lane2.totalcar = (CInt(lane1.smallcar) + CInt(lane1.bigcar)).ToString
        vd.vd.lanedata.Add(lane1)
        vd.vd.lanedata.Add(lane2)
        Dim jsonText As String = JsonConvert.SerializeObject(vd)
        Dim resultXml As XmlDocument = JsonConvert.DeserializeXmlNode(jsonText)
        resultXml.PrependChild(resultXml.CreateXmlDeclaration("1.0", "utf-8", Nothing))
        resultXml.Save("test.xml")
    End Sub
End Class
