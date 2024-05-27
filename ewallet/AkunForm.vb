Imports System.Data.Odbc

Public Class AkunForm

    Dim Conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String

    Private userId As String
    Sub koneksi()
        MyDB = "DSN=DSDbGudang;Driver={MySQL ODBC 3.51 Driver};Database=ewallet;server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub

    Private Sub AkunForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
        ' Mengisi DataGridView1 dengan data dari tabel topups
        Dim DaTopups As New OdbcDataAdapter($"SELECT * FROM topups WHERE userId = '{userId}'", Conn)
        Dim DsTopups As New DataSet
        DaTopups.Fill(DsTopups, "topups")
        DataGridView1.DataSource = DsTopups.Tables("topups")

        ' Mengisi DataGridView2 dengan data dari tabel transfers
        Dim DaTransfers As New OdbcDataAdapter($"SELECT * FROM transfers WHERE fromUserId = '{userId}'", Conn)
        Dim DsTransfers As New DataSet
        DaTransfers.Fill(DsTransfers, "transfers")
        DataGridView2.DataSource = DsTransfers.Tables("transfers")

        Dim DaTarik As New OdbcDataAdapter($"SELECT * FROM tarik WHERE userId = '{userId}'", Conn)
        Dim DsTarik As New DataSet
        DaTransfers.Fill(DsTransfers, "tarik")
        DataGridView3.DataSource = DsTransfers.Tables("tarik")

    End Sub
    Public Sub New(receivedUserId As String)
        InitializeComponent()
        userId = receivedUserId
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtPwBaru.Text IsNot "" Or txtPwLama.Text IsNot "" Then

            'cek apakah password lama nya benar
            cmd = New OdbcCommand($"SELECT password FROM users WHERE userId = '{userId}'", Conn)
            Rd = cmd.ExecuteReader()

            If Rd.HasRows Then
                While Rd.Read
                    If txtPwLama.Text = Rd("password").ToString() Then
                        cmd = New OdbcCommand($"UPDATE users SET password = '{txtPwBaru.Text}' WHERE userId = '{userId}'", Conn)
                        cmd.ExecuteNonQuery()
                        MsgBox("Update Password berhasil!", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Informasi")
                    Else
                        MsgBox("Password lama salah!", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Informasi")
                    End If
                End While
            End If


        End If
    End Sub
End Class