Imports System.Threading
Imports ServiceStack.Redis
Imports System.IO.Ports
Imports GMap.NET.MapProviders
Imports GMap.NET
Imports GMap.NET.WindowsForms
Imports GMap.NET.WindowsForms.Markers
Imports Nucleio.BASHA.DataAccess

Public Class FrmMain

    Private redisConsumer, redisClient As New RedisClient("XXX.XXX.X.XXX", "6379")
    Private _historyOperation As New HistoryOperations()
    Private mySerialPort As SerialPort
    Private baudRate As Integer = 9600
    Private port As String = "COM4"
    Private alertCount As Integer = 0
    Dim arr As String() = New String(2) {}
    Dim itm As ListViewItem
    Dim threadObj As Thread

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadMap()
        LoadSerial()


        threadObj = New Thread(Sub()
                                   Dim subscription = redisConsumer.CreateSubscription()
                                   Dim ChannelName As String = "SENSORONECHANNEL"
                                   subscription.OnMessage = Function(channel, msg)

                                                                DisplayInfo(msg)

                                                                Return True
                                                            End Function

                                   subscription.SubscribeToChannels(ChannelName)

                               End Sub)
        threadObj.IsBackground = True
        threadObj.Start()
    End Sub

#Region "external functions"
    Private Sub DisplayInfo(value As String)
        Dim threshold = redisClient.Custom("GET", "AlarmLimit")

        'Parse value here
        'temperature
        'magnitude
        'sensorname
        'Redis command PUBLISH SENSORONECHANNEL "31,33000,SENSOR1"

        Dim values As String() = value.Split(",")

        If Not String.IsNullOrEmpty(threshold.Text) Then

            If value <> "nan" Then

                If Convert.ToDouble(values(1)) > Convert.ToDouble(threshold.Text) Then
                    lblMagnitude.Invoke(Sub() lblMagnitude.ForeColor = Color.Red)
                    lblMagnitude.Invoke(Sub() lblMagnitude.Text = "ALERT!")

                    lblTemperature.Invoke(Sub() lblTemperature.Text = values(0))
                    lblMagnitude.Invoke(Sub() lblMagnitude.Text = values(1))
                    lblAlertThreshold.Invoke(Sub() lblAlertThreshold.Text = threshold.Text)


                    'add items to ListView
                    arr(0) = values(2)
                    arr(1) = values(1)

                    itm = New ListViewItem(arr)
                    lstvSensorAlerts.Invoke(Sub() lstvSensorAlerts.Items.Add(itm))

                    'save to database history
                    Dim _history As New History()

                    _history.Sensor = values(2)
                    _history.Temperature = values(0)
                    _history.Magnitude = values(1)
                    _historyOperation.InsertHistory(_history)

                    GMapControl1.Invoke(Sub() GMapControl1.Refresh())

                    alertCount += 1
                    If alertCount = 5 Then

                        'mySerialPort.Write("s")
                        alertCount = 0
                    End If
                Else
                    lblMagnitude.Invoke(Sub() lblMagnitude.ForeColor = Color.LightGreen)
                    lblTemperature.Invoke(Sub() lblTemperature.Text = values(0))
                    lblMagnitude.Invoke(Sub() lblMagnitude.Text = values(1))
                    lblAlertThreshold.Invoke(Sub() lblAlertThreshold.Text = threshold.Text)

                    GMapControl1.Invoke(Sub() GMapControl1.Refresh())
                End If

            End If

        End If

        Chart1.Invoke(Sub() Chart1.Series("MAGNITUDE").Points.AddXY(DateTime.Now.ToString("ss"), value))
    End Sub
    Private Sub LoadMap()
        GMapControl1.Manager.Mode = AccessMode.ServerAndCache
        GMapControl1.MapProvider = GoogleMapProvider.Instance

        With Me.GMapControl1
            .Position = New PointLatLng(14.5357772, 120.9875052)
            .ZoomAndCenterMarkers(CType(vbNull, String))
            .Zoom = 17
            .ShowCenter = True
        End With

        Dim markersOverlay As GMapOverlay = New GMapOverlay("markers")
        Dim marker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(14.5357772, 120.9875052), GMarkerGoogleType.green)

        markersOverlay.Markers.Add(marker)
        GMapControl1.Invoke(Sub() GMapControl1.Overlays.Add(markersOverlay))

        GMapControl1.Refresh()
    End Sub

    Private Sub FrmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not mySerialPort Is Nothing Then
            mySerialPort.Close()
        End If
        threadObj.Abort()
    End Sub

    Private Sub SetThresholdToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetThresholdToolStripMenuItem.Click
        Dim _frmThreshold As New FrmThreshold()

        _frmThreshold.ShowDialog()
    End Sub

    Private Sub AlertHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlertHistoryToolStripMenuItem.Click
        Dim _frmHistory As New FrmHistory()

        _frmHistory.ShowDialog()
    End Sub

    Private Sub LoadSerial()

        Try
            mySerialPort = New SerialPort(port)
            mySerialPort.BaudRate = baudRate
            mySerialPort.Parity = Parity.None
            mySerialPort.StopBits = StopBits.One
            mySerialPort.DataBits = 8
            mySerialPort.Handshake = Handshake.None
            If mySerialPort.IsOpen Then
                mySerialPort.Close()
            Else
                mySerialPort.Open()
            End If
        Catch ex As Exception
            mySerialPort = Nothing
            MessageBox.Show(ex.Message + " Please connect your device properly.", "Serial Device", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub
#End Region
End Class
