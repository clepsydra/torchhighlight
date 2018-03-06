using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorchlightWindow
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using QRCoder;

    public class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public ViewModel()
        {
            this.Initialize();
        }
        private void Initialize()
        {
            for (int i = 0; i < avgLength; i++)
            {
                this.spotXValues.Add(0);
                this.spotYValues.Add(0);
            }


            string machineName = Environment.MachineName;
            int port = Utils.FindAvailablePort();
            string url = $"http://{machineName}:{port}/";
            this.Url = url + "index.html";

            Thread thread = new Thread(() => this.RunListener(url));
            thread.Start();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            var hBitmap = qrCodeImage.GetHbitmap();


            var bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                  hBitmap,
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  BitmapSizeOptions.FromEmptyOptions());

            this.QRCode = bitSrc;

            this.evaluateShowQRCodeTimer = new Timer(this.EvaluateShowQRCode, null, 0, 1000);
        }

        private string _Url;

        public string Url
        {
            get => this._Url;
            set
            {
                this._Url = value;
                this.OnPropertyChanged();
            }
        }

        Timer evaluateShowQRCodeTimer = null;

        private void EvaluateShowQRCode(object state)
        {
            this.IsShowQR = DateTime.Now.Subtract(this.lastReceived).TotalSeconds > 2;
        }

        private BitmapSource _QRCode;

        public BitmapSource QRCode
        {
            get => this._QRCode;
            set
            {
                this._QRCode = value;
                this.OnPropertyChanged();
            }
        }



        private string _Rotate = string.Empty;
        public string Rotate
        {
            get => this._Rotate;
            set
            {
                this._Rotate = value;
                this.OnPropertyChanged();
                
                if (double.TryParse(this._Rotate, NumberStyles.Any, CultureInfo.InvariantCulture, out double rotateValue))
                {
                    this.SpotX = -MaxSpotX / 2.0 * rotateValue / 5.0 + MaxSpotX / 2.0;
                }
            }
        }

        private bool _IsShowQR = true;

        public bool IsShowQR
        {
            get => this._IsShowQR;
            set
            {
                this._IsShowQR = value;
                this.OnPropertyChanged();
            }
        }

        private bool _IsDimm = true;

        public bool IsDimm
        {
            get => this._IsDimm;
            set
            {
                this._IsDimm = value;
                this.OnPropertyChanged();
            }
        }

        private string _Y = string.Empty;
        public string Y
        {
            get => this._Y;
            set
            {
                this._Y = value;
                this.OnPropertyChanged();

                if (double.TryParse(this._Y, NumberStyles.Any, CultureInfo.InvariantCulture, out double yValue))
                {
                    this.SpotY = -MaxSpotY / 2.0 * yValue / 1.5 + MaxSpotY / 2.0;
                }
            }
        }

        private const double MaxSpotX = 1000;
        private const double MaxSpotY = 800;

        private const int avgLength = 10;

        private List<double> spotXValues = new List<double>();
        private List<double> spotYValues = new List<double>();

        private double _SpotX = 0;
        public double SpotX
        {
            get
            {
                lock (this.spotXValues)
                {
                    double avg = this.spotXValues.Average();
                    return avg;
                }
            }
            set
            {
                lock (this.spotXValues)
                {
                    this.spotXValues.RemoveAt(0);
                    this.spotXValues.Add(value);
                }
                //this._SpotX = value;
                this.OnPropertyChanged();

                this.OnPropertyChanged(nameof(this.Center));
            }
        }

        private double _SpotY = 0;
        public double SpotY
        {
            get
            {
                lock (this.spotYValues)
                {
                    double avg = this.spotYValues.Average();
                    return avg;
                }
            }
            set
            {
                lock (this.spotYValues)
                {
                    this.spotYValues.RemoveAt(0);
                    this.spotYValues.Add(value);
                }
                //this._SpotY = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Center));
            }
        }

        public System.Windows.Point Center => new System.Windows.Point(this.SpotX, this.SpotY);

        private DateTime lastReceived = DateTime.MinValue;

        private void RunListener(string url)
        {
            this.listener = new HttpListener();
            this.listener.Prefixes.Add(url);

            this.listener.Start();

          while (this.listener.IsListening)
            {
                try
                {
                    var context = this.listener.GetContext();
                    var query = context.Request.Url.Query;
                    if (context.Request.HttpMethod == "POST")
                    {
                        this.lastReceived = DateTime.Now;
                        if (query.Contains("y="))
                        {
                            this.Y = query.Replace("?y=", "");
                        }
                        else if (query.Contains("rotate="))
                        {
                            this.Rotate = query.Replace("?rotate=", "");
                        }
                        else if (query.Contains("dimm="))
                        {
                            if (bool.TryParse(query.Replace("?dimm=", ""), out bool isDimm))
                            {
                                this.IsDimm = isDimm;
                            }
                        }

                        context.Response.ContentType = "text/plain";
                        using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                        {
                            writer.Write("OK");
                        }
                    }
                    else if (context.Request.HttpMethod == "GET")
                    {
                        string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dist");
                        //string basePath = @"C:\Source\Projects\TorchHighlight\Web\TorchHighlightWeb\dist";
                        string fileName = Path.Combine(basePath, context.Request.Url.AbsolutePath.Substring(1));
                        if (File.Exists(fileName))
                        {
                            //context.Response.ContentType = "text/plain";
                            using (FileStream fileStream = File.OpenRead(fileName))
                            {
                                fileStream.CopyTo(context.Response.OutputStream);
                                context.Response.OutputStream.Close();
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private HttpListener listener;

        private void OnPropertyChanged([CallerMemberName] string callerMember = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerMember));
        }

        public void Dispose()
        {
            this.listener.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
