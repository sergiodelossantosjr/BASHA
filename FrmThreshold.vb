Imports ServiceStack.Redis
Public Class FrmThreshold

    Private redisClient As New RedisClient("192.168.1.103", "6379")

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim result = redisClient.Set("AlarmLimit", Convert.ToDouble(txtThreshold.Text))

        If result Then
            MessageBox.Show("Success!", "Set Threshold", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If

    End Sub

    Private Sub FrmThreshold_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim threshold = redisClient.Custom("GET", "AlarmLimit")

        Me.txtThreshold.Text = threshold.Text
    End Sub
End Class