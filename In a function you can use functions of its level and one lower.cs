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
        PassportView passportView = new PassportView(new PresenterFactory());
        passportView.OnButtonClick();

        Console.ReadKey();
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
        _passportTextbox.SetText(Console.ReadLine());
        _presenter.TryFindPassportInDataTable(_passportTextbox.Text);
    }

    public void SetText(string text)
    {
        _textResult.SetText(text);
        Console.WriteLine(text);
    }
}

public class Presenter
{
    private CitizenService _citizenService;
    private IView _view;

    public Presenter(IView view, CitizenService citizenService)
    {
        _view = view ?? throw new ArgumentNullException();
        _citizenService = citizenService ?? throw new ArgumentNullException();
    }

    public void TryFindPassportInDataTable(string rawData)
    {
        try
        {
            Passport passport = new Passport(rawData);

            switch (_citizenService.TryShowPassport(passport.Series.ToString()))
            {
                case null:
                    SetMessagePassportNotFound(rawData);
                    return;

                case true:
                    SetMessageAccessGranted(rawData);
                    return;

                case false:
                    SetMessageAccessNotGranted(rawData);
                    return;
            }
        }
        catch (Exception exception)
        {
            _view.SetText(exception.Message);
        }
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

public class PresenterFactory
{
    public Presenter Create(IView view)
    {
        if (view == null)
            throw new ArgumentNullException();

        var citizenService = new CitizenService();

        return new Presenter(view, citizenService);
    }
}

public class Passport
{
    private const int MinCountSymbols = 10;

    public Passport(string series)
    {
        if (series.Length < MinCountSymbols)
            throw new ArgumentException("Неверный формат серии или номера паспорта!");

        Series = series;
    }

    public string Series { get; }
}

public class TextBox : IView
{
    public string Text { get; private set; }

    public void SetText(string text)
    {
        Text = text.Trim();
    }
}

public class CitizenService
{
    private DataBase _dataBase;

    public bool? TryShowPassport(string rawData)
    {
        DataTable dataTable = _dataBase.OpenDataTable(SHAHasher.GetStringHash(rawData));

        if (dataTable.Rows.Count < 0)
            return null;

        if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
            return true;

        return false;
    }
}

public class DataBase
{
    public DataTable OpenDataTable(string hash)
    {
        string command = SQLUtils.FormatToCommandText(hash);
        string connectionString = SQLUtils.GetConnectionLine();

        try
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(new SQLiteCommand(command, connection));
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            connection.Close();

            return dataTable;
        }
        catch (SQLiteException exception)
        {
            if (exception.ErrorCode == 1)
                throw new InvalidOperationException("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");

            throw;
        }
    }
}

public interface IView
{
    void SetText(string message);
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
    private string _commandText;

    private SqliteConnection _connection;

    public SQLiteCommand(string commandText, SqliteConnection connection)
    {
        _commandText = commandText;
        _connection = connection ?? throw new ArgumentNullException();
    }
}

public static class SQLUtils
{
    public static string FormatToCommandText(string hash)
    {
        return string.Format("select * from passports where num='{0}' limit 1;", hash);
    }

    public static string GetConnectionLine()
    {
        return string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
    }
}

public static class SHAHasher
{
    public static string GetStringHash(string line)
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
