Imports System.Data.Odbc
Imports Microsoft.VisualBasic.ApplicationServices

Public Class TransferForm

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
    Private Sub TransferForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
    End Sub

    Public Sub New(receivedUserId As String)
        InitializeComponent()
        userId = receivedUserId
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

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Jika bukan angka dan bukan karakter kontrol (seperti backspace), maka tolak input
            e.Handled = True
        End If
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        Dim sql As String = $"SELECT * FROM users WHERE userId = '{txtTujuan.Text}'"
        cmd = New OdbcCommand(sql, Conn)
        Rd = cmd.ExecuteReader()

        If (Rd.HasRows) Then
            MsgBox("UserId ditemukan!", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Pemberitahuan")
        Else
            'userId tidak ditemukan
            MsgBox("UserId tidak ditemukan!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")
        End If


    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        '1. cek apakah saldo saya mencukupi
        '2. cukup -> transfer / tidak cukup -> Msgbox
        '3. Query untuk menambah uang userId tujuan & query untuk menyimpan riwayat transfer.

        Dim sql = $"SELECT balance FROM users WHERE userId = '{userId}'"
        cmd = New OdbcCommand(sql, Conn)
        Rd = cmd.ExecuteReader()
        If Rd.HasRows Then
            While Rd.Read()

                Dim amountTo = Integer.Parse(TextBox1.Text)
                Dim userIdTo = txtTujuan.Text
                Dim balance = Rd("balance")
                If balance >= amountTo Then
                    'uang mencukupi
                    Dim result As DialogResult = MessageBox.Show($"Anda yakin ingin transfer Rp. {amountTo} kepada userId: {userIdTo}?", "Konfirmasi Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        'Transfer
                        Dim sql2 As String = $"SELECT balance FROM users WHERE userId LIKE '{userIdTo}'" 'Ambil saldo tujuan
                        cmd = New OdbcCommand(sql2, Conn)
                        Rd = cmd.ExecuteReader()

                        If (Rd.HasRows) Then
                            While Rd.Read
                                'userId valid
                                Dim saldoTujuan = Rd("balance")
                                Dim sql3 As String = $"UPDATE users SET balance = {saldoTujuan + amountTo} WHERE userId LIKE '{userIdTo}'" 'tambah uang tujuan
                                cmd = New OdbcCommand(sql3, Conn)
                                cmd.ExecuteNonQuery()

                                sql3 = $"UPDATE users SET balance = {balance - amountTo} WHERE userId LIKE '{userId}'" 'mengurangi saldo pengirim (saya)
                                cmd = New OdbcCommand(sql3, Conn)
                                cmd.ExecuteNonQuery()

                                Dim currentDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

                                sql3 = $"INSERT INTO transfers (fromUserId, toUserId, amount, date) VALUES ('{userId}', '{userIdTo}', {amountTo}, '{currentDate}')" 'Menyimpan riwayat transaksi
                                cmd = New OdbcCommand(sql3, Conn)
                                cmd.ExecuteNonQuery()

                                MsgBox("Transfer Berhasil", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Informasi")
                                Me.Hide()
                                Return
                            End While

                        Else
                            'userId tidak ditemukan
                            MsgBox("UserId tujuan tidak ditemukan!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")
                        End If
                    Else
                        'Batal transfer
                    End If
                Else
                    'uang tidak cukup
                    MsgBox("Saldo anda tidak mencukupi!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")

                End If
            End While
        End If
    End Sub
End Class