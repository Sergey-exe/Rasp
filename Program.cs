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
            Pathfinder pathfinder4 = new Pathfinder(new XConsoleLogger(new LoggerFriday(new ConsoleLogger())));
        }
    }

    //class ConsoleLogWriter
    //{
    //    public virtual void WriteError(string message)
    //    {
    //        Console.WriteLine(message);
    //    }
    //}

    //class FileLogWriter
    //{
    //    public virtual void WriteError(string message)
    //    {
    //        File.WriteAllText("log.txt", message);
    //    }
    //}

    //class SecureConsoleLogWriter : ConsoleLogWriter
    //{
    //    public override void WriteError(string message)
    //    {
    //        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
    //        {
    //            base.WriteError(message);
    //        }
    //    }
    //}

    public interface ILogger
    {
        void Find(string message);
    }

    public class Pathfinder : ILogger
    {
        private ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string log)
        {
            _logger.Find(log);
        }
    }

    public class LoggerFriday : ILogger
    {
        private ILogger _logger;

        public bool IsFriday => DateTime.Now.DayOfWeek == DayOfWeek.Friday;

        public LoggerFriday(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void Find(string log)
        {
            if(IsFriday)
                _logger.Find(log);
        }
    }

    public class XConsoleLogger : LogWriter
    {
        private ILogger _logger;

        public XConsoleLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override void Find(string log)
        {
            WriteConsoleLog(log);

            _logger.Find(log);
        }
    }

    public class ConsoleLogger : LogWriter
    {
        public override void Find(string log)
        {
            WriteConsoleLog(log);
        }
    }

    public class FileLogger : LogWriter
    {
        public override void Find(string log)
        {
            WriteFileLog(log);
        }
    }

    public abstract class LogWriter : ILogger
    {
        public abstract void Find(string log);

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