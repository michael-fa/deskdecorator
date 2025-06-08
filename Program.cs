using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace deskdecorator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        public static int gLanguage = 0;
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            var logger = Logger.Instance;
            Debug.WriteLine("Hello World, or nah?");



            if (args.Contains("--add-label"))
            {
                using (var client = new UdpClient())
                {
                    var message = Encoding.UTF8.GetBytes("DESKDEC|--add-label");
                    client.Send(message, message.Length, "127.0.0.1", 8534);
                }

                return; // Kein UI starten
            }
            Form1 form = new Form1(logger);
            Application.Run(form);



            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                // Perform cleanup tasks here
                Debug.WriteLine("============== FORM CLOSING ==============");
                form.Close();
                logger = null;
            };



        }
    }
}