using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    using System.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskCompletionSource<int> tskcomSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var worker = new Worker();
            await Task.Run(() => worker.Run());
            MessageBox.Show("Worker Done.");

            var worker2 = new WorkerAsync();
            await worker2.Run();
            MessageBox.Show("Worker2 Done.");
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            var worker2 = new WorkerAsync();
            worker2.RunWithoutAsync().ContinueWith(task => MessageBox.Show("Worker2 ContinueWith Done."));
        }

        private void ButtonBase_OnClickx(object sender, RoutedEventArgs e)
        {
            var worker = new Worker();
            var task1 = Task.Run(() => worker.Run());
            // var task1 = Task.Run(() => worker.Run()).ContinueWith(t => Console.Write(), TaskContinuationOptions.); // Kann nicht mehr auf UI zugreifen.
            // ...schreibe noch was nicht in UI thread

            MessageBox.Show("Worker Done.");

            var worker2 = new WorkerAsync();
            var taskSecond = worker2.Run();
            MessageBox.Show("Worker2 Done.");
            Task.WaitAll(task1, taskSecond);
            Task.WaitAny(task1, taskSecond); // wenn beliebiges Task fertig ist.
        }

        private Task<string> test() // mimmic a result return by task, similar is the Task.CompletedTask for mimmicing void,
        {
            return Task.FromResult<string>("a");
        }

        private async void ButtonNew(object sender, RoutedEventArgs e)
        {
            tskcomSource = new TaskCompletionSource<int>();
            var x = await tskcomSource.Task;
        }
        
        private void ButtonNew2(object sender, RoutedEventArgs e)
        {
            tskcomSource.SetCanceled();
        }
        
        
    }

    public class Worker
    {
        public void Run()
        {
            Thread.Sleep(2000);
        }
    }

    public class WorkerAsync
    {
        public async Task Run()
        {
            await Task.Delay(2000);
            await Task.Delay(2000);
        }

        public Task RunWithoutAsync()
        {
            return Task.Delay(2000);
        }
    }
}
