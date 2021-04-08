using AudioToolbox;
using Foundation;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UIKit;

namespace XamariniOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            UIViewController controller = new UIViewController();
            controller.View.BackgroundColor = UIColor.DarkGray;
            controller.Title = "Xamarin iOS Debugger";

            UINavigationController navController = new UINavigationController(controller);

            Window.RootViewController = navController;

            // make the window visible
            Window.MakeKeyAndVisible();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "MyTestImage.jpg");
            var img = UIImage.FromBundle("TestImage.jpg");//the MyImage file is placed in the asset folder
            NSData image = img.AsJPEG();
            NSError err = null;
            image.Save(filename, false, out err);

            OutputAudioQueue audioQueue = new OutputAudioQueue(new AudioStreamBasicDescription()
                {
                    Format = AudioFormatType.MPEG4AAC_HE,
                    BytesPerPacket = 0,
                    BitsPerChannel = 0,
                    Reserved = 0,
                    FormatFlags = 0,
                    BytesPerFrame = 0, //Set this field to 0 for compressed formats. 
                    SampleRate = 16000,
                    ChannelsPerFrame = 1,
                    FramesPerPacket = 1024 //for AAC. 
                });
            const int BufferCountMax = 1000;
            const int AudioBufferSize = 1024 * 8;
            for (int i = 0; i < BufferCountMax; i++)
            {
                AudioQueueStatus aqs = audioQueue.AllocateBufferWithPacketDescriptors(
                    AudioBufferSize,
                    1,
                    out IntPtr ipBuffer
                    );

                if (aqs == AudioQueueStatus.Ok)
                {
                    //_queueAudioOutputBuffers.Enqueue(ipBuffer);
                    //_qFreeAudioOutputBuffers.Enqueue(ipBuffer);
                    byte[] abData = new byte[512];
                    Marshal.Copy(abData, 0, ipBuffer, abData.Length);

                }
                else
                {
                    Debug.WriteLine("AudioQueueStatus: " + aqs);
                }
            }

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background execution this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transition from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


