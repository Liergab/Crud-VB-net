Imports MySql.Data.MySqlClient
Public Class Form1
    Dim conn As New MySqlConnection("server=localhost; port=3306;username=root;password=;database=crud")
    Dim i As Integer
    Dim dr As MySqlDataReader
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DGV_load()
    End Sub

    Public Sub DGV_load()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT * FROM tbl_crud", conn)
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(dr.Item("PRODUCTNO"), dr.Item("PRODUCTNAME"), dr.Item("PRICE"), dr.Item("GROUP"), dr.Item("EXPDATE"), Format(CBool(dr.Item("STATUS").ToString)))
            End While


        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            conn.Close()

        End Try
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        save()
        DGV_load()
    End Sub

    Public Sub save()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("INSERT INTO `tbl_crud`(`PRODUCTNO`,`PRODUCTNAME`,`PRICE`,`GROUP`, `EXPDATE`, `STATUS`) VALUES (@PRODUCTNO,@PRODUCTNAME,@PRICE,@GROUP,@EXPDATE,@STATUS)", conn)

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PRODUCTNO", txt_prono.Text)
            cmd.Parameters.AddWithValue("@PRODUCTNAME", txt_Proname.Text)
            cmd.Parameters.AddWithValue("@PRICE", CDec(txt_price.Text))
            cmd.Parameters.AddWithValue("@GROUP", compo_proGroup.Text)
            cmd.Parameters.AddWithValue("@EXPDATE", CDate(exp_datepicker.Value))
            cmd.Parameters.AddWithValue("@STATUS", CBool(status_checkbox.Checked.ToString))

            i = cmd.ExecuteNonQuery

            If i > 0 Then
                MessageBox.Show("Record Save Success!", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Record Save Failed!", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
        Clear()
    End Sub

    Public Sub Clear()
        txt_prono.Clear()
        txt_Proname.Clear()
        txt_price.Clear()
        compo_proGroup.Text = ""
        exp_datepicker.Value = Now
        status_checkbox.CheckState = False
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        txt_prono.Text = DataGridView1.CurrentRow.Cells(0).Value
        txt_Proname.Text = DataGridView1.CurrentRow.Cells(1).Value
        txt_price.Text = DataGridView1.CurrentRow.Cells(2).Value
        compo_proGroup.Text = DataGridView1.CurrentRow.Cells(3).Value
        exp_datepicker.Text = DataGridView1.CurrentRow.Cells(4).Value
        status_checkbox.Checked = DataGridView1.CurrentRow.Cells(5).Value

        txt_prono.ReadOnly = True
        btn_save.Enabled = False
    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        Clear()
    End Sub

    Public Sub Edit()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("UPDATE `tbl_crud` SET `PRODUCTNAME`=@PRODUCTNAME, `PRICE`=@PRICE, `GROUP`=@GROUP, `EXPDATE`=@EXPDATE, `STATUS`=@STATUS WHERE `PRODUCTNO`=@PRODUCTNO", conn)

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PRODUCTNO", txt_prono.Text)
            cmd.Parameters.AddWithValue("@PRODUCTNAME", txt_Proname.Text)
            cmd.Parameters.AddWithValue("@PRICE", CDec(txt_price.Text))
            cmd.Parameters.AddWithValue("@GROUP", compo_proGroup.Text)
            cmd.Parameters.AddWithValue("@EXPDATE", CDate(exp_datepicker.Value))
            cmd.Parameters.AddWithValue("@STATUS", CBool(status_checkbox.Checked.ToString))

            i = cmd.ExecuteNonQuery()

            If i > 0 Then
                MessageBox.Show("Record Update Success!", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Record Update Failed!", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
        Clear()
        DGV_load()
    End Sub
    Private Sub btn_edit_Click(sender As Object, e As EventArgs) Handles btn_edit.Click
        Edit()
        DGV_load()
    End Sub

    Public Sub delete()
        If MsgBox("Are You sure delete this Record", MsgBoxStyle.Question + vbYesNo) = vbYes Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM `tbl_crud` WHERE `PRODUCTNO`=@PRODUCTNO", conn)

                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@PRODUCTNO", txt_prono.Text)

                i = cmd.ExecuteNonQuery()
                If i > 0 Then
                    MessageBox.Show("Record Deleted Successfully!", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No record found with the specified PRODUCTNO.", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception

                MsgBox("Error: " & ex.Message)
            Finally
                conn.Close()
            End Try
            Clear()
            DGV_load()
        Else
            Return
        End If
    End Sub


    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        delete()
    End Sub

    Private Sub txt_search_TextChanged(sender As Object, e As EventArgs) Handles txt_search.TextChanged

        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT * FROM tbl_crud WHERE PRODUCTNO like '%" & txt_search.Text & "%' OR  PRODUCTNAME like '%" & txt_search.Text & "%' ", conn)
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(dr.Item("PRODUCTNO"), dr.Item("PRODUCTNAME"), dr.Item("PRICE"), dr.Item("GROUP"), dr.Item("EXPDATE"), Format(CBool(dr.Item("STATUS").ToString)))
            End While


        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            conn.Close()

        End Try

    End Sub
End Class
