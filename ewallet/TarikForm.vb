Imports System.Data.Odbc

Public Class TarikForm

    Dim Conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim Ds As DataSet
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String

    Private userId As String
    Sub koneksi()
        MyDB = "DSN=DSDbGudang;Driver={MySQL ODBC 3.51 Driver};Database=ewallet;server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub
    Private Sub TarikForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
    End Sub

    Public Sub New(receivedUserId As String)
        InitializeComponent()
        userId = receivedUserId
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
        '1. Cek saldo cukup atau tidak
        Dim amountTo = Integer.Parse(TextBox1.Text)
        Dim sql = $"SELECT balance FROM users WHERE userId = '{userId}'"
        cmd = New OdbcCommand(sql, Conn)
        Rd = cmd.ExecuteReader()

        If Rd.HasRows Then
            While Rd.Read()
                Dim balance As Integer = Rd("balance")
                If (balance >= amountTo) Then
                    'uang cukup
                    Dim result As DialogResult = MessageBox.Show($"Anda yakin ingin tarik uang sejumlah Rp. {amountTo} dari saldo anda?", "Konfirmasi Penarikan", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        'kurangi saldo
                        cmd = New OdbcCommand($"UPDATE users SET balance = {balance - amountTo} WHERE userId = '{userId}'", Conn)
                        cmd.ExecuteNonQuery()
                        'Buat riwayat penarikan
                        Dim currentDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        cmd = New OdbcCommand($"INSERT INTO tarik (userId, amount, date) VALUES ('{userId}', {amountTo}, '{currentDate}')", Conn)
                        cmd.ExecuteNonQuery()
                        MsgBox("Penarikan Berhasil", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Pemberitahuan")
                        Me.Hide()
                    End If
                Else
                    'uang tidak cukup
                    MsgBox("Saldo kurang", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")
                End If

            End While
        End If

        '2. Kurangi saldo
        '3. Buat riwayat tarik
        'Dim currentDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")


    End Sub
End Class