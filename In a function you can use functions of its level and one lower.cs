using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

public partial class Program
{
    public static void Main()
    {
        MessageBox messageBox = new MessageBox();
        TextBox textBox = new TextBox(Console.ReadLine());
        View view = new View(messageBox, textBox);
        DataBase dataBase = new DataBase();
        Presenter presenter = new Presenter(view, dataBase);
        view.ButtinCilcked();

        Console.ReadKey();
    }
}

public class View
{
    private MessageBox _messageBox;
    private TextBox _passportTextBox;

    public Action ButtinCilcked;

    public View(MessageBox messageBox, TextBox textBox)
    {
        _messageBox = messageBox ?? throw new ArgumentNullException();
        _passportTextBox = textBox ?? throw new ArgumentNullException();
    }

    public void OnClick()
    {
        ButtinCilcked?.Invoke();
    }

    public string GetTextBoxText()
    {
        return _passportTextBox.Trim();
    }

    public void Show(string text)
    {
        _messageBox.Show(text);
    }
}

public class Presenter
{
    private const int MinCountSymbols = 10;

    private View _view;
    private DataBase _database;

    public Action<string> TextChanged;

    public Presenter(View view, DataBase database)
    {
        _view = view ?? throw new ArgumentNullException();
        _database = database ?? throw new ArgumentNullException();

        _view.ButtinCilcked += GrantAccess;
        TextChanged += _view.Show;
    }

    public void GrantAccess()
    {
        string rawData = _view.GetTextBoxText();

        if (ThereSeries(rawData) == false)
            return;

        if (IsCorrectSeries(rawData) == false)
            return;

        if (int.TryParse(rawData, out int series) == false)
        {
            TextChanged?.Invoke("Серия и номер должны содержать только арабские цифры!");

            return;
        }

        Passport passport = new Passport(series);

        _database.GrantAccess(rawData, passport);
    }

    private bool ThereSeries(string series)
    {
        if (series == string.Empty)
        {
            TextChanged?.Invoke("Введите серию и номер паспорта!");

            return false;
        }

        return true;
    }

    private bool IsCorrectSeries(string series)
    {
        if (series.Length < MinCountSymbols)
        {
            TextChanged?.Invoke("Неверный формат серии или номера паспорта");

            return false;
        }

        return true;
    }

    private void OnDisable()
    {
        _view.ButtinCilcked -= GrantAccess;
        TextChanged -= _view.Show;
    }
}

public class DataBase
{
    private SqliteConnector _connector;

    public Action<string> TextChanged;

    public DataBase()
    {
        _connector = new SqliteConnector();
    }

    public void GrantAccess(string rawData, Passport passport)
    {
        int row = 0;
        int index = 1;

        try
        {
            SQLiteDataAdapter sQLiteDataAdapter = _connector.Connect(rawData);

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = dataTable1;

            sQLiteDataAdapter.Fill(dataTable2);

            if (dataTable1.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dataTable1.Rows[row].ItemArray[index]))
                    TextChanged.Invoke($"По паспорту «{passport.Series}» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН");
                else
                    TextChanged.Invoke($"По паспорту «{passport.Series}» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ");

                _connector.Close();
            }
            else
            {
                TextChanged.Invoke($"Паспорт «{passport.Series}» в списке участников дистанционного голосования НЕ НАЙДЕН");
            }
        }
        catch (SQLiteException exception)
        {
            if (exception.ErrorCode == 1)
                TextChanged.Invoke("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
        }
    }
}

public class SqliteConnector
{
    SqliteConnection _connection;

    public SqliteConnector()
    {
    }

    public SQLiteDataAdapter Connect(string rawData)
    {
        string connectionLine = SQLUtils.GetConnectionLine();
        string commandText = SQLUtils.FormatToCommandText(rawData);

        _connection = new SqliteConnection(connectionLine);

        _connection.Open();

        return new SQLiteDataAdapter(new SQLiteCommand(commandText, _connection));
    }

    public void Close()
    {
        if (_connection == null)
            throw new ArgumentNullException();

        _connection.Close();
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
    public Passport(int series)
    {
        Series = series;
    }

    public int Series { get; }
}

public class TextBox
{
    public TextBox(string text)
    {
        Text = text;
    }

    public string Text { get; }

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
}

public class MessageBox
{
    public void Show(string text)
    {
        Console.WriteLine(text);
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

public class SQLiteException : Exception
{
    public int ErrorCode { get; private set; }
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
