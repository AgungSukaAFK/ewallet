Imports System.Data.Common
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar
Imports System.Data.Odbc

Public Class DashboardForm

    Private userId As String
    Private username As String

    Dim balance As String

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

        Dim sql As String = $"SELECT * FROM users WHERE userId LIKE '{userId}'"
        cmd = New OdbcCommand(sql, Conn)
        Rd = cmd.ExecuteReader()
        If Rd.HasRows Then
            While Rd.Read()
                Dim userId As String = Rd("userId").ToString()
                Dim dbuserName As String = Rd("userName").ToString()
                Dim userBalance As String = Rd("balance").ToString()

                username = dbuserName
                balance = userBalance
                labelUsername.Text = username
                Label2.Text = $"Rp. {String.Format("{0:#,###}", balance.ToString())}"
            End While
        Else
            Console.WriteLine("No rows found.")
        End If


    End Sub

    Public Sub New(receivedUserId As String)
        InitializeComponent()

        userId = receivedUserId
    End Sub
    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KondisiAwal()


    End Sub

    Private Sub DashboardForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim result As DialogResult = MessageBox.Show("Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Memeriksa hasil dari MessageBox
        If result = DialogResult.Yes Then
            ' Kode untuk keluar dari aplikasi
            Application.Exit()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Topup = New TopupForm(userId, balance)
        Topup.Show()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub DashboardForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        KondisiAwal()
    End Sub

    Private Sub DashboardForm_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        KondisiAwal()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Transfer = New TransferForm(userId)
        Transfer.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Tarik = New TarikForm(userId)
        Tarik.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Akun = New AkunForm(userId)
        Akun.Show()
    End Sub
End Class