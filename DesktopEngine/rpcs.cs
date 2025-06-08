using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using deskdecorator.DekstopEngine;

namespace deskdecorator
{
    public static class NetworkLabelReceiver
    {
        private static Thread? listenerThread;
        private static bool running = true;

        public static void StartListening(Form1 form)
        {
            listenerThread = new Thread(() =>
            {
                using (var udp = new UdpClient(8534))
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    while (running)
                    {
                        try
                        {
                            var data = udp.Receive(ref remoteEP);
                            var message = Encoding.UTF8.GetString(data);

                            if (message == "DESKDEC|--add-label")
                            {
                                form.Invoke(() =>
                                {
                                    var dialog = new AddLabel((Form1)Engine._form, true);
                                    dialog.Show();
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[ERROR] LabelReceiver Error: {ex.Message}");
                        }
                    }
                }
            });

            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        public static void Stop()
        {
            running = false;
            listenerThread?.Join();
        }
    }
}