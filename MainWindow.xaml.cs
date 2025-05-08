using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfInvokeFix
{
    public partial class MainWindow : Window
    {
        private Timer timer;
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            timer = new Timer(TimerHandler, null, 0, 20);

            for (int i = 0; i < 100; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();

                Parallel.For(0, 10, new ParallelOptions { MaxDegreeOfParallelism = 4 }, _ =>
                {
                    Thread.Sleep(1);
                });

                sw.Stop();
                Debug.WriteLine($"elapsed time: {sw.ElapsedMilliseconds} ms");
            }

            base.OnContentRendered(e);
        }

        private void TimerHandler(object state)
        {
            count++;

            // Invoke to BeginInvoke
            Dispatcher.BeginInvoke(() =>
            {
                Label1.Content = count;
            });
        }
    }
}
