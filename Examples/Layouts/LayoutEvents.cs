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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Layouts.Events;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Core;

namespace LayoutEvents
{
  internal class LayoutEventSpyViewModel : DockPane
  {
    private const string _dockPaneID = "LayoutEvents_LayoutEventSpy";
    private static readonly object _lock = new object();
    private List<string> _entries = new List<string>();
    private ICommand _clearCmd = null;

    protected LayoutEventSpyViewModel() { }

    async protected override Task InitializeAsync()
    {
      LayoutView lytView = LayoutView.Active;
      await ProApp.Panes.CreateLayoutPaneAsync(lytView.Layout);
      #region ActiveLayoutViewChangedEvent
      ArcGIS.Desktop.Layouts.Events.ActiveLayoutViewChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("ActiveLayoutViewChangedEvent:" +
          Environment.NewLine +
          "   arg Layoutview: " + args.LayoutView.Layout.Name +
          Environment.NewLine +
          "   arg Type: " + args.Type.ToString());
      });
      #endregion ActiveLayoutViewChangedEvent

      #region ActivateMapFrameEvent
      ArcGIS.Desktop.Layouts.Events.ActivateMapFrameEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("ActiveMapFrameEvent:" +
        Environment.NewLine +
        "   arg IsActivated: " + args.IsActivated.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg MapFrame: " + args.MapFrame.Name);
      });
      #endregion ActivateMapFrameEvent

      #region ElementsPlacementChangedEvent
      ArcGIS.Desktop.Layouts.Events.ElementsPlacementChangedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementsPlacementChangedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementsPlacementChangedEvent

      #region ElementsUpdatedEvent
      ArcGIS.Desktop.Layouts.Events.ElementsUpdatedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementsUpdatedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementsUpdatedEvent

      #region LayoutChangedEvent
      ArcGIS.Desktop.Layouts.Events.LayoutChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutChangedEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutChangedEvent

      #region LayoutClosedEvent
      ArcGIS.Desktop.Layouts.Events.LayoutClosedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutClosedEvent:" +
        Environment.NewLine +
        "   arg LayoutPane: " + args.LayoutPane.Caption);
      });
      #endregion LayoutClosedEvent

      #region LayoutClosingEvent
      ArcGIS.Desktop.Layouts.Events.LayoutClosingEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutClosingEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg LayoutPane: " + args.LayoutPane.Caption);
      });
      #endregion LayoutClosingEvent

      #region LayoutSelectionChangedEvent
      ArcGIS.Desktop.Layouts.Events.LayoutSelectionChangedEvent.Subscribe((args) => {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("LayoutSelectionChangedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutSelectionChangedEvent

      #region LayoutRemovedEvent
      ArcGIS.Desktop.Layouts.Events.LayoutRemovedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutViewEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutRemovedEvent

      #region LayoutViewEvent
      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutViewEvent:" +
        Environment.NewLine +
        "   arg Layoutview: " + args.LayoutView.Layout.Name +
        Environment.NewLine +
        "   arg Type: " + args.Type.ToString());
      });
      #endregion LayoutViewEvent

      #region MapSeriesEvent
      ArcGIS.Desktop.Layouts.Events.MapSeriesEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("MapSeriesEvent:" +
        Environment.NewLine +
        "   arg CurrentPageName: " + args.CurrentPageName.ToString() +
        Environment.NewLine +
        "   arg CurrentlPageNumber: " + args.CurrentPageNumber.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg Type: " + args.Type.ToString());
      });
      #endregion MapSeriesEvent

      #region PageChangedEvent   
      ArcGIS.Desktop.Layouts.Events.PageChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("PageChangedEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg New CIMPage (height): " + args.NewPage.Height.ToString() +
        Environment.NewLine +
        "   arg Old CIMPage (height): " + args.OldPage.Height.ToString());
      });
      #endregion PageChangedEvent

      
      //return Task.FromResult(0);
    }

    private void AddEntry(string eventName, string entry) {
      lock (_lock)
      {
        _entries.Add($"{eventName} {entry}");
      }
      NotifyPropertyChanged(nameof(EventLog));
    }

    public ICommand ClearTextCmd
    {
      get
      {
        if (_clearCmd == null)
          _clearCmd = new RelayCommand(() => {
            lock (_lock) {
              _entries.Clear();
            }
            NotifyPropertyChanged(nameof(EventLog));
          });
        return _clearCmd;
      }
    }

    public string EventLog
    {
      get
      {
        string contents = "";
        lock (_lock) {
          contents = string.Join("\r\n", _entries.ToArray());
        }
        return contents;
      }
    }

    /// <summary>
    /// Show the DockPane.
    /// </summary>
    internal static void Show()
    {
      DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
      if (pane == null)
        return;

      pane.Activate();
    }
  }

  /// <summary>
  /// Button implementation to show the DockPane.
  /// </summary>
	internal class LayoutEventSpy_ShowButton : Button
  {
    protected override void OnClick()
    {
      LayoutEventSpyViewModel.Show();
    }
  }
}
