using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using Uno.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppDebugUno
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            ConfigureFilters(Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				// this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Windows.UI.Xaml.Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Windows.UI.Xaml.Window.Current.Activate();
            }

#if NETFX_CORE
            RegisterBackgroundTask();
            //RegisterToastNotificcationTrigger();
#endif
        }

        AsyncLock alBackgroundRegistration = new AsyncLock();

        private async void RegisterBackgroundTask()
        {
            using (await alBackgroundRegistration.LockAsync(new System.Threading.CancellationToken()))
            {
                try
                {

                    var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
                    if (backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed ||
                        backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
                    {
                        foreach (var task in BackgroundTaskRegistration.AllTasks)
                        {
                            if (task.Value.Name == "foo")
                            {
                                task.Value.Unregister(true);
                            }
                        }

                        BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                        taskBuilder.Name = "foo";
                        taskBuilder.TaskEntryPoint = "background.Foo";
                        //     If FreshnessTime is set to less than 15 minutes,
                        //     an exception is thrown when attempting to register the background task.
                        taskBuilder.SetTrigger(new TimeTrigger(15, false));
                        BackgroundTaskRegistration registration = taskBuilder.Register();
                        registration.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// Handle background task completion.
        /// </summary>
        /// <param name="task">The task that is reporting completion.</param>
        /// <param name="e">Arguments of the completion report.</param>
        private async void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {

            try
            {
                await Window.Current.Content.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            Debug.WriteLine(task.Name);
                        });
            }
            catch (Exception ex)
            {
                Debug.Write("Exception from OnCompleted():" + ex.Message);
            }
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        /// <summary>
        /// Configures global logging
        /// </summary>
        /// <param name="factory"></param>
        static void ConfigureFilters(ILoggerFactory factory)
        {

        }

        async void RegisterToastNotificcationTrigger()
        {
            try
            {
                //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/send-local-toast
                // If background task is already registered, do nothing

                if (BackgroundTaskRegistration.AllTasks.Count > 0)
                {
                    foreach (var task in BackgroundTaskRegistration.AllTasks)
                    {
                        if (task.Value.Name == "foo")
                        {
                            task.Value.Unregister(true);
                        }
                        else
                        {
                            //do nothing
                        }

                    }
                }
                else
                {
                    //do nothing
                }


                if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals("foo")))
                    return;

                // Otherwise request access
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

                // Create the background task
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
                {
                    Name = "fooToaster",
                };

                // Assign the toast action trigger
                builder.SetTrigger(new ToastNotificationActionTrigger());

                // And register the task
                BackgroundTaskRegistration registration = builder.Register();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
