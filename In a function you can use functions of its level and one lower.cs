private const int MinCountSymbols = 10;

private void OnButtonClick()
{
    X(sender);
}

private void X(object sender)
{
    string rawData = passportTextbox.Text.Trim().Replace(" ", string.Empty);
    
    if (IsLineFull() == false)
        return;

    if (IsCorrectFormat(rawData) == false)
        return;

    TryGrantAccess(rawData);
}

private bool IsLineFull()
{
    if (passportTextbox.Text.Trim() != string.Empty)
    {
        int num1 = (int)MessageBox.Show("Введите серию и номер паспорта");

        return false;
    }

    return true;
}

private bool IsCorrectFormat(string rawData)
{
    if (rawData.Length < MinCountSymbols)
    {
        textResult.Text = "Неверный формат серии или номера паспорта";

        return false;
    }

    return true;
}

private void TryGrantAccess(string rawData)
{
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
    string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");

    try
    {
        GrantAccess(commandText, connectionString);
    }
    catch (SQLiteException ex)
    {
        if (ex.ErrorCode != 1)
            return;

        int num2 = (int)MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
    }
}

private void GrantAccess(string commandText, string connectionString)
{
    SQLiteConnection connection = new SQLiteConnection(connectionString);
    connection.Open();
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));

    DataTable dataTable1 = new DataTable();
    DataTable dataTable2 = dataTable1;

    sqLiteDataAdapter.Fill(dataTable2);

    if (dataTable1.Rows.Count > 0)
    {
        if (Convert.ToBoolean(dataTable1.Rows[0].ItemArray[1]))
            textResult.Text = $"По паспорту «{passportTextbox.Text}» " + 
                $"доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
        else
            textResult.Text = $"По паспорту «{passportTextbox.Text}» " +
                $"доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
    }
    else
    {
        textResult.Text = $"Паспорт «{passportTextbox.Text}» " +
            $"в списке участников дистанционного голосования НЕ НАЙДЕН";
    }

    connection.Close();
}
