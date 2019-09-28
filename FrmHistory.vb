Imports Nucleio.BASHA.DataAccess

Public Class FrmHistory

    Private _historyOperation As New HistoryOperations()

    Private Sub FrmHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim _historyList = _historyOperation.GetHistory()

        For Each _history As History In _historyList
            Chart1.Series("HISTORY").Color = Color.Red
            Chart1.Series("HISTORY").Points.AddXY(DateTime.Now.ToString(_history.CreatedOn.ToString("HH:mm:ss")), _history.Magnitude)
        Next
    End Sub
End Class