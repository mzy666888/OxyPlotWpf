using GalaSoft.MvvmLight;
using System;
using System.Threading;
using System.Threading.Tasks;

using OxyPlot;
using OxyPlot.Axes;

using LineSeries = OxyPlot.Series.LineSeries;
using System.Collections.ObjectModel;

namespace OxyPlotDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            TempDataPoints = new ObservableCollection<DataPoint>();
            HumiDataPoints = new ObservableCollection<DataPoint>();
           
            Model = new PlotModel(){Title = "Simple Example",Subtitle = "using OxyPlot"};
            var series1 = new LineSeries { Title = "梁業", MarkerType = MarkerType.Circle,Smooth = true};
            var series2 = new LineSeries { Title = "物業", MarkerType = MarkerType.Star, Smooth = true ,MarkerStroke = OxyColors.Red};
            var dateTimeAxis1 = new DateTimeAxis();
            dateTimeAxis1.Title = "Time";
            Model.Axes.Add(dateTimeAxis1);
            Model.Series.Add(series1);
            Model.Series.Add(series2);

            Random rd = new Random();
            Task.Run(
                () =>
                    {
                        while (true)
                        {
                            series1.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, rd.Next(10, 30)));
                            series2.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, rd.Next(10, 30)));
                            if (series1.Points.Count > 100)
                            {
                                series1.Points.RemoveAt(0);
                                series2.Points.RemoveAt(0);
                            }
                            Model.InvalidatePlot(true);
                            Thread.Sleep(1000);
                        }
                    });
            Task.Run(
                () =>
                    {
                        while (true)
                        {
                            var date = DateTime.Now;
                            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    TempDataPoints.Add(DateTimeAxis.CreateDataPoint(date, (double)(rd.Next(100, 500) / 10.0)));
                                    HumiDataPoints.Add(DateTimeAxis.CreateDataPoint(date, (double)(rd.Next(500, 800) / 10.0)));
                                    if (TempDataPoints.Count > 300)
                                    {
                                        TempDataPoints.RemoveAt(0);
                                        HumiDataPoints.RemoveAt(0);
                                    }
                                }));
                            Thread.Sleep(1000);
                        }
                    });
        }

        private PlotModel _model;
        /// <summary>
        /// PlotModel
        /// </summary>
        public PlotModel Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }

        private ObservableCollection<DataPoint> _tempDataPoints;
        /// <summary>
        /// 梁業
        /// </summary>
        public ObservableCollection<DataPoint> TempDataPoints
        {
            get { return _tempDataPoints; }
            set { Set(ref _tempDataPoints, value); }
        }

        private ObservableCollection<DataPoint> _humiDataPoints;
        /// <summary>
        /// 物業
        /// </summary>
        public ObservableCollection<DataPoint> HumiDataPoints
        {
            get { return _humiDataPoints; }
            set { Set(ref _humiDataPoints, value); }
        }
    }
}