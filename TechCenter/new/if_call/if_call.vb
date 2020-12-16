''將時間呼叫抽出 與功能分開  類似排程
Public Interface if_call
    Event Mcall()
    Sub strat()
    Sub close()

    Sub test_start()
    Event test_text(text As String)
End Interface
