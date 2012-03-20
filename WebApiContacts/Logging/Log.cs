using System;
using System.Web.Management;

namespace WebApiContacts.Logging
{
    public static class Log
    {
        public static void Notification(string message)
        {
            new LogEvent(message).Raise();
        }

        private class LogEvent : WebRequestErrorEvent
        {
            public LogEvent(string message)
                : base(null, null, 100001, new Exception(message))
            {

            }
        }
    }
}