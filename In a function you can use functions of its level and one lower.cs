using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

class Program
{
    public static void Main()
    {
        DataBaseConnector connector = new DataBaseConnector();
        PresenterFactory presenterFactory = new PresenterFactory();
        PassportView passportView = new PassportView(presenterFactory);

        Console.ReadKey();
    }
}

public interface IView
{
    void SetText(string text);
}

public class PresenterFactory
{
    public Presenter Create(IView view)
    {
        if (view == null)
            throw new ArgumentNullException();

        return new Presenter(view);
    }
}

public class PassportView : IView
{
    private TextBox _passportTextbox;
    private TextBox _textResult;
    private Presenter _presenter;

    public PassportView(PresenterFactory presenterFactory)
    {
        if (presenterFactory == null)
            throw new ArgumentNullException();

        _presenter = presenterFactory.Create(this);

        _passportTextbox = new TextBox();
        _textResult = new TextBox();
    }

    public void OnButtonClick()
    {
        _passportTextbox.Trim();
        _presenter.TryFindPassportInDatatable(_passportTextbox.Text);
    }

    public void SetText(string text)
    {
        _textResult.SetText(text);
    }
}

public class Presenter
{
    private IView _view;
    private DataBaseConnector _connector;
    private DataBase _dataBase;

    public Presenter (IView view)
    {
        _view = view ?? throw new ArgumentNullException();
        _connector = new DataBaseConnector();
        _dataBase = new DataBase();
    }

    public void TryFindPassportInDatatable(string rawData)
    {
        if (rawData == string.Empty)
            SetMessagePassportTextNotFound();

        Passport passport = new Passport(rawData);

        SqliteConnection connection = _connector.GetConnection();

        string commandText = SQLUtils.FormatToCommandText(rawData);

        if (_dataBase.TryProvideAccess(commandText, connection, out bool IsPassportFound))
        {
            connection.Close();

            if (IsPassportFound)
                SetMessagePassportNotFound(rawData);

            SetMessageAccessNotGranted(rawData);

            return;
        }

        SetMessageAccessGranted(rawData);
    }

    private void SetMessagePassportTextNotFound()
    {
        _view.SetText("Введите серию и номер паспорта");

        throw new ArgumentNullException();
    }

    private void SetMessageInvalidConnection()
    {
        _view.SetText("Ошибка соединения");

        throw new InvalidOperationException();
    }

    private void SetMessagePassportNotFound(string rawData)
    {
        _view.SetText($"Паспорт «{rawData}» в списке участников дистанционного голосования НЕ НАЙДЕН");
    }

    private void SetMessageAccessGranted(string rawData)
    {
        _view.SetText($"По паспорту «{rawData}» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН");
    }

    private void SetMessageAccessNotGranted(string rawData)
    {
        _view.SetText($"По паспорту «{rawData}» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ");
    }
}

public class DataBaseConnector
{
    private SqliteConnection _connection;
    private DataBase _dataBase;

    private bool _connected;

    public SqliteConnection GetConnection()
    {
        try
        {
            _connection = new SqliteConnection(SQLUtils.GetConnectionLine());
            _connection.Open();
            _connected = true;

            return _connection;
        }
        catch (SQLiteException sQLiteException)
        {
            if (sQLiteException.ErrorCode == 1)
                MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");

            throw new ArgumentException();
        }
    }

    public void Close()
    {
        if (_connected)
            _connection.Close();
    }
}

public class DataBase
{
    private DataTable _dataTable;

    public bool TryProvideAccess(string commandText, SqliteConnection connection, out bool IsPassportFound)
    {
        SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
        sqLiteDataAdapter.Fill(_dataTable);

        if (_dataTable.Rows.Count > 0)
        {
            IsPassportFound = true;
        }
        else
        {
            IsPassportFound = false;

            return false;
        }

        if (Convert.ToBoolean(_dataTable.Rows[0].ItemArray[1]))
            return true;
        else
            return false;
    }
}

public class SQLiteDataAdapter
{
    private SQLiteCommand _command;

    public SQLiteDataAdapter(SQLiteCommand command)
    {
        _command = command ?? throw new ArgumentNullException();
    }

    public void Fill(DataTable dataTable)
    {
        throw new NotImplementedException();
    }
}

public class SQLiteCommand
{
    string _commandText;

    SqliteConnection _connection;

    public SQLiteCommand(string commandText, SqliteConnection connection)
    {
        _commandText = commandText;
        _connection = connection ?? throw new ArgumentNullException();
    }
}

public class Passport
{
    private const int MinCountSymbols = 10;

    public Passport(string series)
    {
        if (series.Length < MinCountSymbols)
        {
            MessageBox.Show("Неверный формат серии или номера паспорта!");

            throw new ArgumentException(nameof(series));
        }

        if (int.TryParse(series, out int convertSeries) == false)
        {
            MessageBox.Show("Серия и номер могут содержать только арабские цифры!");

            throw new InvalidOperationException(nameof(series));
        }

        Series = convertSeries;
    }

    public int Series { get; }
}

public static class MessageBox
{
    public static void Show(string text)
    {
        Console.WriteLine(text);
    }
}

public class TextBox : IView
{
    public string Text { get; private set; }

    public string Trim()
    {
        char trimSymbol = ' ';
        string line = string.Empty;

        for (int i = 0; i < Text.Length; i++)
        {
            if (Text[i] != trimSymbol)
                line += Text[i];
        }

        return line;
    }

    public void SetText(string text)
    {
        Text = text;
    }
}

public class SQLUtils
{
    public static string FormatToCommandText(string rawData)
    {
        SHAHasher hasher = new SHAHasher();

        return string.Format("select * from passports where num='{0}' limit 1;", hasher.GetStringHash(rawData));
    }

    public static string GetConnectionLine()
    {
        return string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
    }
}

public class SHAHasher
{
    public string GetStringHash(string line)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(line);

        SHA256 sha256 = SHA256.Create();

        return Encoding.UTF8.GetString(sha256.ComputeHash(bytes));
    }
}

public class SQLiteException : Exception
{
    public int ErrorCode { get; private set; }
}
