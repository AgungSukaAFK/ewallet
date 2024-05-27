Imports System.Data.Odbc

Public Class TopupForm

    Dim Conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim Ds As DataSet
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String
    Sub koneksi()
        MyDB = "DSN=DSDbGudang;Driver={MySQL ODBC 3.51 Driver};Database=ewallet;server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub

    Private userId As String
    Private balance As Integer
    Private Sub TopupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        labelUserId.Text = userId
        koneksi()
    End Sub

    Public Sub New(receivedUserId As String, receivedBalance As Integer)
        InitializeComponent()
        userId = receivedUserId
        balance = receivedBalance
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Jika bukan angka dan bukan karakter kontrol (seperti backspace), maka tolak input
            e.Handled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 1000).ToString()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 5000).ToString()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 10000).ToString()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 20000).ToString()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 50000).ToString()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim input = Integer.Parse(TextBox1.Text)
        TextBox1.Text = (input + 100000).ToString()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sql As String = $"UPDATE users SET balance = {balance + Integer.Parse(TextBox1.Text)} WHERE userId LIKE '{userId}'"
        cmd = New OdbcCommand(sql, Conn)
        cmd.ExecuteNonQuery()
        Dim currentDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Debug.WriteLine(currentDate)
        sql = $"INSERT INTO topups (userId, amount, date) VALUES ('{userId}', {Integer.Parse(TextBox1.Text)}, '{currentDate}')"
        cmd = New OdbcCommand(sql, Conn)
        cmd.ExecuteNonQuery()

        MsgBox("Topup Berhasil", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Pemberitahuan")
        Me.Hide()
    End Sub
End Class