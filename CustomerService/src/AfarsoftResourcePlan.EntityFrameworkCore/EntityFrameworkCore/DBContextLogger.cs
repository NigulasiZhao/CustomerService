using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    public class DBContextLogger : ILogger
    {
        public bool IsDebugEnabled => true;

        public bool IsErrorEnabled => false;

        public bool IsFatalEnabled => false;

        public bool IsInfoEnabled => false;

        public bool IsWarnEnabled => false;

        public ILogger CreateChildLogger(string loggerName)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(Func<string> messageFactory)
        {
            messageFactory();
        }

        public void Debug(string message, Exception exception)
        {
            Console.WriteLine(message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void DebugFormat(Exception exception, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(Func<string> messageFactory)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Func<string> messageFactory)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(Func<string> messageFactory)
        {
            messageFactory();
        }

        public void Info(string message, Exception exception)
        {
            Console.WriteLine(message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void InfoFormat(Exception exception, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(Func<string> messageFactory)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
