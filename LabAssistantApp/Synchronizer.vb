Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.Threading

Public Class Synchronizer
    Implements ISynchronizeInvoke
    Private m_Thread As Thread
    Private m_Queue As New BlockingCollection(Of Message)()

    Public Sub New()
        m_Thread = New Thread(AddressOf Run)
        m_Thread.IsBackground = True
        m_Thread.Start()
    End Sub

    Private Sub Run()
        While True
            Dim message As Message = m_Queue.Take()
            message.[Return] = message.Method.DynamicInvoke(message.Args)
            message.Finished.[Set]()
        End While
    End Sub

    Public Function BeginInvoke(method As [Delegate], args As Object()) As IAsyncResult Implements ISynchronizeInvoke.BeginInvoke
        Dim message As New Message()
        message.Method = method
        message.Args = args
        m_Queue.Add(message)
        Return message
    End Function

    Public Function EndInvoke(result As IAsyncResult) As Object Implements ISynchronizeInvoke.EndInvoke
        Dim message As Message = TryCast(result, Message)
        If message IsNot Nothing Then
            message.Finished.WaitOne()
            Return message.[Return]
        End If
        Throw New ArgumentException("result")
    End Function

    Public Function Invoke(method As [Delegate], args As Object()) As Object Implements ISynchronizeInvoke.Invoke
        Dim message As New Message()
        message.Method = method
        message.Args = args
        m_Queue.Add(message)
        message.Finished.WaitOne()
        Return message.[Return]
    End Function

    Public ReadOnly Property InvokeRequired() As Boolean Implements ISynchronizeInvoke.InvokeRequired
        Get
            Return Not Thread.CurrentThread.Equals(m_Thread)
        End Get
    End Property

    Private Class Message
        Implements IAsyncResult
        Public Method As [Delegate] = Nothing
        Public Args As Object() = Nothing
        Public [Return] As Object = Nothing
        Public State As Object = Nothing
        Public Finished As New ManualResetEvent(False)

        Public ReadOnly Property AsyncState() As Object Implements IAsyncResult.AsyncState
            Get
                Return State
            End Get
        End Property

        Public ReadOnly Property AsyncWaitHandle() As WaitHandle Implements IAsyncResult.AsyncWaitHandle
            Get
                Return Finished
            End Get
        End Property

        Public ReadOnly Property CompletedSynchronously() As Boolean Implements IAsyncResult.CompletedSynchronously
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property IsCompleted() As Boolean Implements IAsyncResult.IsCompleted
            Get
                Return Finished.WaitOne(0)
            End Get
        End Property

    End Class
End Class