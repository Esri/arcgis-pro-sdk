/*

   Copyright 2018 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Collections.Generic;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Core.Events;
using ArcGIS.Desktop.Mapping.Events;
using System.Linq;

namespace Framework.Snippets {

  internal class Dockpane1ViewModel : ArcGIS.Desktop.Framework.Contracts.DockPane
  {
        #region  How to subscribe and unsubscribe to events when the dockpane is visible or hidden
        private SubscriptionToken _eventToken = null;
        // Called when the visibility of the DockPane changes.
        protected override void OnShow(bool isVisible)
        {
            if (isVisible && _eventToken == null) //Subscribe to event when dockpane is visible
            {
                _eventToken = MapSelectionChangedEvent.Subscribe(OnMapSelectionChangedEvent);
            }

            if (!isVisible && _eventToken != null) //Unsubscribe as the dockpane closes.
            {
                MapSelectionChangedEvent.Unsubscribe(_eventToken);
                _eventToken = null;
            }
        }
        //Event handler when the MapSelection event is triggered.
        private void OnMapSelectionChangedEvent(MapSelectionChangedEventArgs obj)
        {
            MessageBox.Show("Selection has changed");
        }
        #endregion
    }



    internal class ProSnippets2
  {
    public void Snippets()
    {
          #region Execute a command
          IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("esri_editing_ShowAttributes");
          var command = wrapper as ICommand; // tool and command(Button) supports this

          if ((command != null) && command.CanExecute(null))
            command.Execute(null);

          #endregion

          #region Set the current tool

          // use SetCurrentToolAsync
          FrameworkApplication.SetCurrentToolAsync("esri_mapping_selectByRectangleTool");

          // or use ICommand.Execute
          ICommand cmd = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectByRectangleTool") as ICommand;
          if ((cmd != null) && cmd.CanExecute(null))
            cmd.Execute(null);
          #endregion

          #region Activate a tab
          FrameworkApplication.ActivateTab("esri_mapping_insertTab");
          #endregion

          bool activate = true;
          #region Activate/Deactivate a state - to modify a condition

          // Define the condition in the DAML file based on the state 
          if (activate)
            FrameworkApplication.State.Activate("someState");
          else
            FrameworkApplication.State.Deactivate("someState");
          #endregion

          #region Determine if the application is busy

          // The application is considered busy if a task is currently running on the main worker thread or any 
          // pane or dock pane reports that it is busy or intiializing.   

          // Many Pro styles (such as Esri_SimpleButton) ensure that a button is disabled when FrameworkApplication.IsBusy is true
          // You would use this property to bind to the IsEnabled property of a control (such as a listbox) on a dockpane or pane in order
          // to disable it from user interaction while the application is busy. 
          bool isbusy = FrameworkApplication.IsBusy;
          #endregion

          #region Get the Application main window
          System.Windows.Window window = FrameworkApplication.Current.MainWindow;

          // center it
          Rect rect = System.Windows.SystemParameters.WorkArea;
          FrameworkApplication.Current.MainWindow.Left = rect.Left + (rect.Width - FrameworkApplication.Current.MainWindow.ActualWidth) / 2;
          FrameworkApplication.Current.MainWindow.Top = rect.Top + (rect.Height - FrameworkApplication.Current.MainWindow.ActualHeight) / 2;

          #endregion

          #region Close ArcGIS Pro
          FrameworkApplication.Close();
            #endregion

            #region Get ArcGIS Pro version
            //"GetEntryAssembly" should be ArcGISPro.exe 
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            #endregion
            #region Close a specific pane

            string _viewPaneID = "my pane"; //DAML ID of your pane
            //You could have multiple instances (InstanceIDs) of your pane. 
            //So you can iterate through the Panes to get "your" panes only
            IList<uint> myPaneInstanceIDs = new List<uint>();
            foreach (Pane pane in FrameworkApplication.Panes)
            {
                if (pane.ContentID == _viewPaneID)   
                {
                    myPaneInstanceIDs.Add(pane.InstanceID); //InstanceID of your pane, could be multiple, so build the collection                    
                }
            }
            foreach (var instanceID in myPaneInstanceIDs) //close each of "your" panes.
            {
                FrameworkApplication.Panes.ClosePane(instanceID);
            }
            #endregion

            #region Activate a pane
            var mapPanes = ProApp.Panes.OfType<IMapPane>();
            foreach (Pane pane in mapPanes)
            {
                if (pane.Caption == "MyMap")
                {
                    pane.Activate();
                    break;
                }
            }
            #endregion

        }

        public void Dockpane1()
    {
      #region Find a dockpane
      // in order to find a dockpane you need to know it's DAML id
      var pane = FrameworkApplication.DockPaneManager.Find("esri_core_ProjectDockPane");

      #endregion
    }

    public void Dockpane2()
    {
      #region Dockpane properties and methods
      // in order to find a dockpane you need to know it's DAML id
      var pane = FrameworkApplication.DockPaneManager.Find("esri_core_ProjectDockPane");

      // determine visibility
      bool visible = pane.IsVisible;

      // activate it
      pane.Activate();

      // determine dockpane state
      DockPaneState state = pane.DockState;

      // pin it
      pane.Pin();

      // hide it
      pane.Hide();

      #endregion
    }

    public async void Dockpane3()
    {
      #region Dockpane undo / redo

      // in order to find a dockpane you need to know it's DAML id
      var pane = FrameworkApplication.DockPaneManager.Find("esri_core_contentsDockPane");

      // get the undo stack
      OperationManager manager = pane.OperationManager;
      if (manager != null)
      {
        // undo an operation
        if (manager.CanUndo)
          await manager.UndoAsync();

        // redo an operation
        if (manager.CanRedo)
          await manager.RedoAsync();

        // clear the undo and redo stack of operations of a particular category
        manager.ClearUndoCategory("Some category");
        manager.ClearRedoCategory("Some category");
      }
      #endregion
    }

    public void Dockpane4()
    {
      #region Find a dockpane and obtain its ViewModel
      // in order to find a dockpane you need to know it's DAML id.  
      
      // Here is a DAML example with a dockpane defined. Once you have found the dockpane you can cast it
      // to the dockpane viewModel which is defined by the className attribute. 
      // 
      //<dockPanes>
      //  <dockPane id="MySample_Dockpane" caption="Dockpane 1" className="Dockpane1ViewModel" dock="bottom" height="5">
      //    <content className="Dockpane1View" />
      //  </dockPane>
      //</dockPanes>

      Dockpane1ViewModel vm = FrameworkApplication.DockPaneManager.Find("MySample_Dockpane") as Dockpane1ViewModel;
            #endregion

        #region Open the Backstage tab
        //Opens the Backstage to the "About ArcGIS Pro" tab.
        FrameworkApplication.OpenBackstage("esri_core_aboutTab");
            #endregion
        #region Access the current theme
            //Gets the application's theme
            var theme = FrameworkApplication.ApplicationTheme;
            //ApplicationTheme enumeration
            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Dark) {
                //Dark theme
            }

            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.HighContrast){
                //High Contrast
            }
            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Default) {
                //Light/Default theme
            }

        #endregion

        }

        public void SnippetsAdvanced()
    {
      #region Display a Pro MessageBox

      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Some Message", "Some title", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
      #endregion

      #region Add a toast notification

      Notification notification = new Notification();
      notification.Title = FrameworkApplication.Title;
      notification.Message = "Notification 1";
      notification.ImageUrl = @"pack://application:,,,/ArcGIS.Desktop.Resources;component/Images/ToastLicensing32.png";

      ArcGIS.Desktop.Framework.FrameworkApplication.AddNotification(notification);
      #endregion
    }

    #region Change a buttons caption or image

    private void ChangeCaptionImage()
    {
      IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("MyAddin_MyCustomButton");
      if (wrapper != null)
      {
        wrapper.Caption = "new caption";

        // ensure that T-Rex16 and T-Rex32 are included in your add-in under the images folder and have 
        // BuildAction = Resource and Copy to OutputDirectory = Do not copy
        wrapper.SmallImage = BuildImage("T-Rex16.png");
        wrapper.LargeImage = BuildImage("T-Rex32.png");
      }
    }

    private ImageSource BuildImage(string imageName)
    {
      return new BitmapImage(PackUriForResource(imageName));
    }

    private Uri PackUriForResource(string resourceName)
    {
      string asm = System.IO.Path.GetFileNameWithoutExtension(
          System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
      return new Uri(string.Format("pack://application:,,,/{0};component/Images/{1}", asm, resourceName), UriKind.Absolute);
    }

        #endregion

        private void GetButtonTooltipHeading()
        {
            #region Get a button's tooltip heading
            //Pass in the daml id of your button. Or pass in any Pro button ID.
            IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("button_id_from daml");
            var buttonTooltipHeading = wrapper.TooltipHeading;
            #endregion
        }

        #region Subscribe to Active Tool Changed Event
        private void SubscribeEvent()
    {
      ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Subscribe(OnActiveToolChanged);
    }
    private void UnSubscribeEvent()
    {
      ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Unsubscribe(OnActiveToolChanged);
    }
    private void OnActiveToolChanged(ArcGIS.Desktop.Framework.Events.ToolEventArgs args)
    {
      string prevTool = args.PreviousID;
      string newTool = args.CurrentID;
    }
    #endregion

    #region Progressor - Simple and non-cancelable

    public async Task Progressor_NonCancelable()
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource ps = new ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource("Doing my thing...", false);

      int numSecondsDelay = 5;
      //If you run this in the DEBUGGER you will NOT see the dialog
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => Task.Delay(numSecondsDelay * 1000).Wait(), ps.Progressor);
    }

    #endregion

    #region Progressor - Cancelable

    public async Task Progressor_Cancelable()
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource cps = 
        new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("Doing my thing - cancelable", "Canceled");

      int numSecondsDelay = 5;
      //If you run this in the DEBUGGER you will NOT see the dialog

      //simulate doing some work which can be canceled
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => 
      {
          cps.Progressor.Max = (uint)numSecondsDelay;
          //check every second
          while (!cps.Progressor.CancellationToken.IsCancellationRequested) {
              cps.Progressor.Value += 1;
              cps.Progressor.Status = "Status " + cps.Progressor.Value;
              cps.Progressor.Message = "Message " + cps.Progressor.Value;

              if (System.Diagnostics.Debugger.IsAttached) {
                  System.Diagnostics.Debug.WriteLine(string.Format("RunCancelableProgress Loop{0}", cps.Progressor.Value));
              }
              //are we done?
              if (cps.Progressor.Value == cps.Progressor.Max) break;
              //block the CIM for a second
              Task.Delay(1000).Wait();

          }
          System.Diagnostics.Debug.WriteLine(string.Format("RunCancelableProgress: Canceled {0}",
                                              cps.Progressor.CancellationToken.IsCancellationRequested));

      }, cps.Progressor);
    }
    #endregion

  }
    #region customize the disabedText property of a button or tool
    //Set the tool's loadOnClick attribute to "false" in the config.daml. 
    //This will allow the tool to be created when Pro launches, so that the disabledText property can display customized text at startup.
    //Remove the "condition" attribute from the tool. Use the OnUpdate method(below) to set the enable\disable state of the tool.
    //Add the OnUpdate method to the tool.
    //Note: since OnUpdate is called very frequently, you should avoid lengthy operations in this method 
    //as this would reduce the responsiveness of the application user interface.
    internal class SnippetButton : ArcGIS.Desktop.Framework.Contracts.Button
    {
        protected override void OnUpdate()
        {
            bool enableSate = true; //TODO: Code your enabled state  
            bool criteria = true;  //TODO: Evaluate criteria for disabledText  

            if (enableSate)
            {
                this.Enabled = true;  //tool is enabled  
            }
            else
            {
                this.Enabled = false;  //tool is disabled  
                                       //customize your disabledText here  
                if (criteria)
                    this.DisabledTooltip = "Missing criteria 1";
            }
        }
    }
    #endregion

    internal class ProSnippets1 
  {
		#region Get an Image Resource from the Current Assembly
		
		public static void ExampleUsage() {
			//Image 'Dino32.png' is added as Build Action: Resource, 'Do not copy'
			var img = ForImage("Dino32.png");
			//Use the image...
		}
		
		public static BitmapImage ForImage(string imageName) {
            return new BitmapImage(PackUriForResource(imageName));
        }
        public static Uri PackUriForResource(string resourceName, string folderName = "Images") {
            string asm = System.IO.Path.GetFileNameWithoutExtension(
                System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string uriString = folderName.Length > 0
                ? string.Format("pack://application:,,,/{0};component/{1}/{2}", asm, folderName, resourceName)
                : string.Format("pack://application:,,,/{0};component/{1}", asm, resourceName);
            return new Uri(uriString, UriKind.Absolute);
        }
		
		#endregion
  }

  #region Prevent ArcGIS Pro from Closing

  // There are two ways to prevent ArcGIS Pro from closing
  // 1. Override the CanUnload method on your add-in's module and return false.
  // 2. Subscribe to the ApplicationClosing event and cancel the event when you receive it

  internal class Module1 : Module
  {

    // Called by Framework when ArcGIS Pro is closing
    protected override bool CanUnload()
    {
      //return false to ~cancel~ Application close
      return false;
    }

    internal class Module2 : Module
    {
      public Module2()
      {
        ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Subscribe(OnApplicationClosing);
      }
      ~Module2()
      {
        ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Unsubscribe(OnApplicationClosing);
      }

      private Task OnApplicationClosing(System.ComponentModel.CancelEventArgs args)
      {
        args.Cancel = true;
        return Task.FromResult(0);
      }
        #region How to determine when a project is opened
        protected override bool Initialize() //Called when the Module is initialized.
        {
            ProjectOpenedEvent.Subscribe(OnProjectOpened); //subscribe to Project opened event
            return base.Initialize();
        }

        private void OnProjectOpened(ProjectEventArgs obj) //Project Opened event handler
        {
            MessageBox.Show($"{Project.Current} has opened"); //show your message box
        }

        protected override void Uninitialize() //unsubscribe to the project opened event
        {
            ProjectOpenedEvent.Unsubscribe(OnProjectOpened); //unsubscribe
            return;
        }
        #endregion
      }
    }
  #endregion

    internal class ProSnippetMapTool : MapTool
    {

        #region How to position an embeddable control inside a MapView

        public ProSnippetMapTool()
        {
            //Set the MapTool base class' OverlayControlID to the DAML id of your embeddable control in the constructor
            this.OverlayControlID = "ProAppModule1_EmbeddableControl1";
        }

        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                e.Handled = true;
        }

        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                //assign the screen coordinate clicked point to the MapTool base class' OverlayControlLocation property.
                this.OverlayControlPositionRatio = e.ClientPoint;

            });
        }

        #endregion




    }

}


