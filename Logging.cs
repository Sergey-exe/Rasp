using System;
using System.Diagnostics;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder pathfinder = new Pathfinder(new LoggerFriday(new ConsoleLogger()));
            Pathfinder pathfinder1 = new Pathfinder(new LoggerFriday(new FileLogger()));
            Pathfinder pathfinder2 = new Pathfinder(new ConsoleLogger());
            Pathfinder pathfinder3 = new Pathfinder(new FileLogger());
            Pathfinder pathfinder4 = new Pathfinder(new MultiLogger(new ConsoleLogger(), new LoggerFriday(new FileLogger())));

            pathfinder4.WriteLog("Лог");
            Console.ReadKey();
        }
    }

    public interface ILogger
    {
        void WriteLog(string message);
    }

    public class Pathfinder : ILogger
    {
        private ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException();

            _logger = logger;
        }

        public void WriteLog(string log)
        {
            _logger.WriteLog(log);
        }

        public void Find(float name)
        {
            _logger.WriteLog($"Выполнен поиск объекта {name}");
        }
    }

    public class LoggerFriday : ILogger
    {
        private ILogger _logger;

        public bool IsFriday => DateTime.Now.DayOfWeek == DayOfWeek.Friday;

        public LoggerFriday(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException();

            _logger = logger;
        }

        public virtual void WriteLog(string log)
        {
            if(IsFriday)
                _logger.WriteLog(log);
        }
    }

    public class MultiLogger : LogWriter
    {
        private ILogger[] _loggers;

        public MultiLogger(params ILogger[] logger)
        {
            if (logger == null)
                throw new ArgumentNullException();

            _loggers = logger;
        }

        public override void WriteLog(string log)
        {
            foreach(var logger in _loggers)
                logger.WriteLog(log);
        }
    }

    public class ConsoleLogger : LogWriter
    {
        public override void WriteLog(string log)
        {
            WriteConsoleLog(log);
        }
    }

    public class FileLogger : LogWriter
    {
        public override void WriteLog(string log)
        {
            WriteFileLog(log);
        }
    }

    public abstract class LogWriter : ILogger
    {
        public abstract void WriteLog(string log);

        protected void WriteConsoleLog(string log)
        {
            Console.WriteLine(log);
        }

        protected void WriteFileLog(string log)
        {
            File.WriteAllText("log.txt", log);
        }
    }
}
