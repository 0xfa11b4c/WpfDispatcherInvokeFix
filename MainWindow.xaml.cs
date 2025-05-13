using System.Diagnostics;
using System.Windows;

namespace WpfInvokeFix
{
    public partial class MainWindow : Window
    {
        private Timer? timer;
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            timer = new Timer(TimerHandler, null, 0, 20);

            for (int i = 0; i < 100; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();

                await Parallel.ForAsync(0, 10, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (j, ct) =>
                {
                    await Task.Delay(1, ct);
                });

                sw.Stop();
                Debug.WriteLine($"elapsed time: {sw.ElapsedMilliseconds} ms");
            }

            base.OnContentRendered(e);
        }

        private void TimerHandler(object? state)
        {
            count++;

            Dispatcher.BeginInvoke(() =>
            {
                Label1.Content = count;
            });
        }
    }
}
