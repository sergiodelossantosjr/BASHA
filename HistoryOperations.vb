Imports System.Configuration
Imports System.Data.SqlClient

Public Class HistoryOperations

    Private connectionString As String = ConfigurationManager.ConnectionStrings("BASHAConnnection").ToString()

    Public Sub New()

    End Sub

    Public Sub InsertHistory(_history As History)
        Dim conn As New SqlConnection(connectionString)
        Dim command As New SqlCommand()

        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "INSERT INTO dbo.History VALUES(@sensor, @temperature, @magnitude, GETDATE())"
        command.Parameters.AddWithValue("sensor", _history.Sensor)
        command.Parameters.AddWithValue("temperature", _history.Temperature)
        command.Parameters.AddWithValue("magnitude", _history.Magnitude)

        conn.Open()

        Dim result As Integer = command.ExecuteNonQuery()

        conn.Close()
    End Sub

    Public Function GetHistory() As List(Of History)
        Dim conn As New SqlConnection(connectionString)
        Dim command As New SqlCommand()
        Dim _historyList As New List(Of History)

        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT CreatedOn, Magnitude FROM dbo.History"

        conn.Open()

        Dim sqlReader As SqlDataReader = command.ExecuteReader()
        While sqlReader.Read
            Dim _history As New History()

            _history.Magnitude = CType(sqlReader("Magnitude").ToString(), Decimal)
            _history.CreatedOn = CType(sqlReader("CreatedOn").ToString(), DateTime)

            _historyList.Add(_history)
        End While

        conn.Close()

        Return _historyList
    End Function

End Class
