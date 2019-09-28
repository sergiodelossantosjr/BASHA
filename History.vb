Public Class History

    Private _Id As Integer
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Private _sensor As String
    Public Property Sensor() As String
        Get
            Return _sensor
        End Get
        Set(ByVal value As String)
            _sensor = value
        End Set
    End Property

    Private _Temperature As Decimal
    Public Property Temperature() As Decimal
        Get
            Return _Temperature
        End Get
        Set(ByVal value As Decimal)
            _Temperature = value
        End Set
    End Property

    Private _Magnitude As Decimal
    Public Property Magnitude() As Decimal
        Get
            Return _Magnitude
        End Get
        Set(ByVal value As Decimal)
            _Magnitude = value
        End Set
    End Property

    Private _createdOn As DateTime
    Public Property CreatedOn() As DateTime
        Get
            Return _createdOn
        End Get
        Set(ByVal value As DateTime)
            _createdOn = value
        End Set
    End Property
End Class
