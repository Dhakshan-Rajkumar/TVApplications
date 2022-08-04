using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for TVApp.xaml
    /// </summary>
    public partial class TVApp : Window
    {
        int Startindex = 0;
        int Startindex1 = 0;
        Boolean playnext = false;
        Boolean playnext1 = false;
        string[] musicFileName, FilePath;
        private int selected = 0;
        private int begin = 0;
        private int end = 0;
        int picCount = 0;
        private string[] folderFile = null;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        bool setFullscreen = false;
        

        public TVApp()
        {
            InitializeComponent();
            ReadText();
            Loaded += MyWindow_Loaded;
           // Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
           // Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

           // Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            Application.Current.Shutdown();
            e.Handled = true;
        }


        
        //void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    e.Handled = true;
        //}

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // we cannot handle this, but not to worry, I have not encountered this exception yet.  
            // However, you can show/log the exception message and show a message that if the application is terminating or not.  
            var isTerminating = e.IsTerminating;
        }  

         private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }

      
       
        /// <summary>
         /// MyWindow_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadPicture();
                LoadMediaFile();
                LoadMusicFile();

                if (setFullscreen == false)
                {
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 20);
                    dispatcherTimer.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ReadText
        /// </summary>
        private void ReadText()
        {
            string textFile = ConfigurationManager.AppSettings["Text"];

            if (File.Exists(textFile))
            {
                var output = File.ReadAllText(textFile);
                if (output == null || output.Length < 1)
                {
                    txtMarquee.MarqueeContent = "Welcome to L.S SPINNING MILLS PRIVATE LIMITED"; 
                }
                else
                    txtMarquee.MarqueeContent = output; 
            }
            else
            {
                txtMarquee.MarqueeContent = "Welcome to L.S SPINNING MILLS PRIVATE LIMITED";
            }
        }

        /// <summary>
        /// dispatcherTimer_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            nextImage();
        }

        /// <summary>
        /// nextImage
        /// </summary>
        private void nextImage()
        {
            try
            {
                string[] loadPicCount = Directory.GetFiles(ConfigurationManager.AppSettings["Pic"]);

                if (picCount != loadPicCount.Length)
                {
                    folderFile = Directory.GetFiles(ConfigurationManager.AppSettings["Pic"]);
                    picCount = folderFile.Length;
                    selected = 0;
                }

                if (selected == folderFile.Length - 1)
                {
                    selected = 0;
                    showImage(folderFile[selected]);
                }
                else
                {
                    selected = selected + 1;
                    showImage(folderFile[selected]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// LoadPicture
        /// </summary>
        private void LoadPicture()
        {
            try
            {
                //NoticeFileName = Directory.GetFiles(ConfigurationManager.AppSettings["Notice"]);
                //if (NoticeFileName == null || NoticeFileName.Length <= 0)
                //{
                    folderFile = Directory.GetFiles(ConfigurationManager.AppSettings["Pic"]);
                //}
                //else
                //{
                    
                //}
                picCount = folderFile.Length;

                if (folderFile == null || folderFile.Length <= 0)
                {
                    dispatcherTimer.IsEnabled = false;
                    dispatcherTimer.Stop();
                    setFullscreen = true;
                }
                else
                {
                    selected = 0;
                    begin = 0;
                    end = folderFile.Length;
                    showImage(folderFile[selected]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// showImage
        /// </summary>
        /// <param name="path"></param>
        private void showImage(string path)
        {
            try
            {
                var uri = new Uri(path);


               // var kk = CreateIndexedImage(path);
                var bitmap = new BitmapImage(uri);
                lblDisplay.Text = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
                PictureBox1.Source = bitmap;

                ////var uri = new Uri(path);
                ////var bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.UriSource = uri;
                //bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //bitmap.EndInit();



                //BitmapImage bi = new BitmapImage();

                //bi.BeginInit();

                //MemoryStream ms = new MemoryStream();

                //// Save to a memory stream...

                //kk.Save(ms, ImageFormat.Bmp);

                //// Rewind the stream...

                //ms.Seek(0, SeekOrigin.Begin);

                //// Tell the WPF image to use this stream...

                //bi.StreamSource = ms;

                //bi.EndInit();





               // PictureBox1.Source = bi;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        /// <summary>
        /// CopyMemory
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        [DllImport("Kernel32.dll", EntryPoint = "CopyMemory")]
        private extern static void CopyMemory(IntPtr dest, IntPtr src, uint length);

        /// <summary>
        /// CreateIndexedImage
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Drawing.Image CreateIndexedImage(string path)
        {
            using (var sourceImage = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(path))
            {
                var targetImage = new System.Drawing.Bitmap(sourceImage.Width, sourceImage.Height,
                  sourceImage.PixelFormat);
                var sourceData = sourceImage.LockBits(
                  new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                  System.Drawing.Imaging.ImageLockMode.ReadOnly, sourceImage.PixelFormat);
                var targetData = targetImage.LockBits(
                  new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                  System.Drawing.Imaging.ImageLockMode.WriteOnly, targetImage.PixelFormat);
                CopyMemory(targetData.Scan0, sourceData.Scan0,
                  (uint)sourceData.Stride * (uint)sourceData.Height);
                sourceImage.UnlockBits(sourceData);
                targetImage.UnlockBits(targetData);
                return targetImage;
            }
        }

        /// <summary>
        /// LoadMediaFile
        /// </summary>
        private void LoadMediaFile()
        {
            try
            {
                listBox1.Items.Clear();
                Startindex = 0;
                playnext = false;

                FilePath = Directory.GetFiles(ConfigurationManager.AppSettings["Videos"],"*.*");
                if (FilePath == null || FilePath.Length <= 0)
                {

                }
                else
                {
                    for (int i = 0; i <= FilePath.Length - 1; i++)
                    {
                        listBox1.Items.Add(FilePath[i]);
                    }
                    Startindex = 0;
                    playfile(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// LoadMusicFile
        /// </summary>
        private void LoadMusicFile()
        {
            try
            {
                listBox2.Items.Clear();
                Startindex1 = 0;
                playnext1 = false;

                musicFileName = Directory.GetFiles(ConfigurationManager.AppSettings["Music"]);
                if (musicFileName == null || musicFileName.Length <= 0)
                {

                }
                else
                {
                    for (int i = 0; i <= musicFileName.Length - 1; i++)
                    {
                        listBox2.Items.Add(musicFileName[i]);
                    }
                    Startindex1 = 0;
                    playfile1(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// playfile
        /// </summary>
        /// <param name="playlistindex"></param>
        public void playfile(int playlistindex)
        {
            try
            {
                if (listBox1.Items.Count <= 0)
                { return; }

                if (playlistindex < 0)
                {
                    return;
                }
                if (FilePath.Length <= 0)
                {
                    return;
                }

                //if (setFullscreen == true)
                //{
                   // mePlayer.Visibility = System.Windows.Visibility.Hidden;
                  //  mePlayer1.LoadedBehavior = MediaState.Manual;
                   // mePlayer1.Source = new Uri(FilePath[playlistindex]);
                  //  mePlayer1.Play();
                //}
                //else
                //{
                    //mePlayer1.Visibility = System.Windows.Visibility.Hidden;
                    mePlayer.LoadedBehavior = MediaState.Manual;
                    mePlayer.Source = new Uri(FilePath[playlistindex]);
                    mePlayer.Play();
                //}
                lblDisplay1.Text = System.IO.Path.GetFileNameWithoutExtension(FilePath[playlistindex]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playlistindex"></param>
        public void playfile1(int playlistindex)
        {
            try
            {
                if (listBox2.Items.Count <= 0)
                { return; }

                if (playlistindex < 0)
                {
                    return;
                }
                if (musicFileName.Length <= 0)
                {
                    return;
                }

               
                mePlayer1.LoadedBehavior = MediaState.Manual;
                mePlayer1.Source = new Uri(musicFileName[playlistindex]);
                mePlayer1.Play();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /// <summary>
        /// mePlayer_MediaEnded_1
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void mePlayer_MediaEnded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                FilePath = Directory.GetFiles(ConfigurationManager.AppSettings["Videos"]);

                //Loading new files in to the list box
                if (listBox1.Items.Count != FilePath.Length)
                {
                    listBox1.Items.Clear();
                    for (int i = 0; i <= FilePath.Length - 1; i++)
                    {
                        listBox1.Items.Add(FilePath[i]);
                    }
                }

                if (Startindex == listBox1.Items.Count - 1)
                {
                    Startindex = 0;
                }
                else if (Startindex >= 0 && Startindex < listBox1.Items.Count - 1)
                {
                    Startindex = Startindex + 1;

                }
                playfile(Startindex);
                e.Handled = true;
                // ReadText();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// mePlayer1_MediaEnded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mePlayer1_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (Startindex1 == listBox2.Items.Count - 1)
            {
                Startindex1 = 0;
            }
            else if (Startindex1 >= 0 && Startindex1 < listBox2.Items.Count - 1)
            {
                Startindex1 = Startindex1 + 1;

            }
            playfile1(Startindex1);
            e.Handled = true;
        }
    
    }
}

