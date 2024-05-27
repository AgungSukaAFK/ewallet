Imports System.Data.Odbc
Imports System.Runtime.Intrinsics.Arm

Public Class RegistForm

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
    Sub KondisiAwal()

        Call koneksi()

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim valid As Boolean = isFilled()
        If (valid) Then
            Dim sql As String = $"SELECT * FROM users WHERE userId LIKE '{txtUserId.Text}'"
            cmd = New OdbcCommand(sql, Conn)
            Rd = cmd.ExecuteReader()

            If (Rd.HasRows) Then
                MsgBox("UserID sudah terdaftar", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")
                txtUserId.Clear()
            Else
                register()
            End If

        Else
            MsgBox("Form tidak boleh kosong!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Pemberitahuan")
        End If


    End Sub

    Private Sub RegistForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KondisiAwal()
    End Sub

    Private Sub register()
        Dim inputUserId As String = txtUserId.Text
        Dim inputPassword As String = txtPassword.Text
        Dim inputUsername As String = txtUsername.Text

        Dim sql As String = $"INSERT INTO users (userId, username, password, balance) VALUES ('{inputUserId}', '{inputUsername}', '{inputPassword}', 0)"
        cmd = New OdbcCommand(sql, Conn)
        Rd = cmd.ExecuteReader()

        MsgBox("Akun berhasil terdaftar, selanjutnya silahkan Login dengan akun anda.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Pemberitahuan")
        LoginForm.Show()
        Me.Hide()
        Form1.Hide()

    End Sub

    Private Function isFilled() As Boolean
        If (txtUserId.Text = "" Or txtUsername.Text = "" Or txtPassword.Text = "") Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        LoginForm.Show()
        Me.Hide()
    End Sub
End Class