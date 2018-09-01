using System;
using System.IO.Ports;
using System.Threading;
using Growl.Connector;
using System.Linq; // for Except function

public class PortChat
{
    static bool _continue;

    public static void Main()
    {
        Thread detectThread = new Thread(Detect);

        _continue = true;
        detectThread.Start();
        detectThread.Join();
    }

    public static void Detect()
    {
        string[] lastPorts = {};
        string[] curPorts = {};
        GrowlConnector growl = new GrowlConnector();
        Application application = new Application("serial port");
        NotificationType plugInOut = new NotificationType("serial port plug in or plug out", "");
        growl.Register(application, new NotificationType[] { plugInOut });

        while (_continue)
        {
            curPorts = SerialPort.GetPortNames();
            /* Console.WriteLine("Available Ports:"); */
            foreach (string s in curPorts.Except(lastPorts))
            {
                /* Console.WriteLine("plug in {0}", s); */
                string info = string.Format("plug in: {0}", s);
                Notification notification = new Notification("serial port", "serial port plug in or plug out", "", "serial port", info);
                growl.Notify(notification);
            }

            foreach (string s in lastPorts.Except(curPorts))
            {
                /* Console.WriteLine("plug out {0}", s); */
                string info = string.Format("plug out: {0}", s);
                Notification notification = new Notification("serial port", "serial port plug in or plug out", "", "serial port", info);
                growl.Notify(notification);
            }

            lastPorts = curPorts;

            Thread.Sleep(1000);
        }
    }
}
