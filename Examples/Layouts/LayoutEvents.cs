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
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      #region ElementEvent_MapFrameActivated_Deactivated
      //Report the event args when a map frame is activated or deactivated.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ActivateMapFrameEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("ActiveMapFrameEvent:" +
      //  Environment.NewLine +
      //  "   arg IsActivated: " + args.IsActivated.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name +
      //  Environment.NewLine +
      //  "   arg MapFrame: " + args.MapFrame.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        var mapFrameName = args.Elements[0].Name;
        if (args.Hint == ElementEventHint.MapFrameActivated)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.MapFrameActivated: {mapFrameName}");
        }
        else if (args.Hint == ElementEventHint.MapFrameDeactivated)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.MapFrameDeactivated: {mapFrameName}");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventHint
      #region LayoutViewEvent_Activated_Deactivated
      //Report the event args when a the layout view has changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ActiveLayoutViewChangedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("ActiveLayoutViewChangedEvent:" +
      //    Environment.NewLine +
      //    "   arg Layoutview: " + args.LayoutView.Layout.Name +
      //    Environment.NewLine +
      //    "   arg Type: " + args.Type.ToString());
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        //this is the view being activated 
        var layout = args.LayoutView?.Layout?.Name ?? "null";

        if (args.Hint == LayoutViewEventHint.Activated)
        {
          System.Diagnostics.Debug.WriteLine($"LayoutViewEventHint.Activated: {layout}");
        }
        else if (args.Hint == LayoutViewEventHint.Deactivated)
        {
          System.Diagnostics.Debug.WriteLine($"LayoutViewEventHint.Deactivated: {layout}");
        }
      });
      #endregion


      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Elements
      #region ElementEvent_ElementAdded
      //Report the event args when an element is added to the layout.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ElementAddedEvent.Subscribe((args) => 
      //{
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("ElementAddedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        string elemNames = "null";
        if (args.Elements?.Count() > 0)
        {
          elemNames = string.Join(",", args.Elements.Select(e => e.Name).ToList());
        }
        if (args.Hint == ElementEventHint.ElementAdded)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.ElementAdded: {elemNames}");
        }
      });
      #endregion


      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Elements
      #region ElementEvent_ElementRemoved
      //Report the event args when an element is removed from the layout.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ElementRemovedEvent.Subscribe((args) => 
      //{
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("ElementRemovedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        string elemNames = "null";
        if (args.Elements?.Count() > 0)
        {
          elemNames = string.Join(",", args.Elements.Select(e => e.Name).ToList());
        }
        if (args.Hint == ElementEventHint.ElementRemoved)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.ElementRemoved: {elemNames}");
        }
      });
      #endregion


      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Elements
      #region ElementEvent_PlacementChanged
      //Report the event args when an element's placement properties are changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ElementsPlacementChangedEvent.Subscribe((args) => 
      //{
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("ElementsPlacementChangedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        string elemNames = "null";
        if (args.Elements?.Count() > 0)
        {
          elemNames = string.Join(",", args.Elements.Select(e => e.Name).ToList());
        }
        if (args.Hint == ElementEventHint.PlacementChanged)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.PlacementChanged: {elemNames}");
        }
      });
      #endregion


      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Elements
      #region ElementEvent_StyleChanged
      //Report the event args when an element's style is changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ElementStyleChangedEvent.Subscribe((args) => 
      //{
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("ElementStyleChangedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});
      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        string elemNames = "null";
        if (args.Elements?.Count() > 0)
        {
          elemNames = string.Join(",", args.Elements.Select(e => e.Name).ToList());
        }
        if (args.Hint == ElementEventHint.StyleChanged)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.StyleChanged: {elemNames}");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Container
      // cref: ArcGIS.Desktop.Mapping.IElementContainer.GetSelectedElements
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      #region ElementEvent_SelectionChanged
      //Report the event args when a layout's selection has changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutSelectionChangedEvent.Subscribe((args) => {
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("LayoutSelectionChangedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        if (args.Hint == ElementEventHint.SelectionChanged)
        {
          if (args.Container.GetSelectedElements()?.Count() > 0)
          {
            var elemNames = string.Join(",", args.Container.GetSelectedElements().Select(e => e.Name).ToList());
            System.Diagnostics.Debug.WriteLine($"ElementEventHint.SelectionChanged: {elemNames}");
          }
        }

      });
      #endregion LayoutSelectionChangedEvent

      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.ElementEventArgs.Container
      // cref: ArcGIS.Desktop.Mapping.IElementContainer.GetSelectedElements
      #region ElementEvent_PropertyChanged
      //Report the event args when an element has been updated.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.ElementsUpdatedEvent.Subscribe((args) => 
      //{
      //  var elements = args.Elements;
      //  string elemNames = "null";
      //  if (elements != null && elements.Count() > 0)
      //  {
      //    elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
      //  }
      //  System.Windows.MessageBox.Show("ElementsUpdatedEvent:" +
      //  Environment.NewLine +
      //  "   arg Elements: " + elemNames.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});
      ArcGIS.Desktop.Layouts.Events.ElementEvent.Subscribe((args) =>
      {
        string elemNames = "null";
        //can be null if the name was changed
        if (args.Elements?.Count() > 0)
        {
          elemNames = string.Join(",", args.Elements.Select(e => e.Name).ToList());
        }
        else if (args.Container.GetSelectedElements()?.Count() > 0)
        {
          elemNames = string.Join(",", args.Container.GetSelectedElements().Select(e => e.Name).ToList());
        }

        if (args.Hint == ElementEventHint.PropertyChanged)
        {
          System.Diagnostics.Debug.WriteLine($"ElementEventHint.PropertyChanged: {elemNames}");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs.Action
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem.GetLayout
      #region LayoutAdded_ProjectItemsChangedEvent_Add
      //Report the event args when a layout is added.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutAddedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutAddedEvent:" +
      //    Environment.NewLine +
      //    "   arg Layout: " + args.Layout.Name);
      //});

      //Use ProjectItemsChangedEvent at 3.x
      ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent.Subscribe((args) => {
        //Layout added. Layout removed would be NotifyCollectionChangedAction.Remove
        if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add &&
            args.ProjectItem is LayoutProjectItem layoutProjectItem)
        {
          var layout_name = layoutProjectItem.Name; 
          var layout = layoutProjectItem.GetLayout();
          System.Diagnostics.Debug.WriteLine($"Layout Added: {layout_name}");
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs.Action
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem.GetLayout
      #region LayoutRemoved_ProjectItemsChangedEvent_Remove
      //Report the event args when a layout is removed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutRemovedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutViewEvent:" +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      //Use ProjectItemsChangedEvent at 3.x
      ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent.Subscribe((args) => {
        //Layout added. Layout removed would be NotifyCollectionChangedAction.Remove
        if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove &&
            args.ProjectItem is LayoutProjectItem layoutProjectItem)
        {
          var layout_name = layoutProjectItem.Name;
          System.Diagnostics.Debug.WriteLine($"Layout Removed: {layout_name}");
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventHint
      #region LayoutEvent_PropertyChanged
      //Report the event args when a layout is changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutChangedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutChangedEvent:" +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name);
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe((args) =>
      {
        var layout = args.Layout;
        if (args.Hint == LayoutEventHint.PropertyChanged)
        {
          System.Diagnostics.Debug.WriteLine($"Layout PropertyChanged: {layout.Name}");
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.LayoutView
      #region LayoutViewEvent_LayoutClosed
      //Report the event args when a layout is closed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutClosedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutClosedEvent:" +
      //  Environment.NewLine +
      //  "   arg LayoutPane: " + args.LayoutPane.Caption);
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        var lv = args.LayoutView;
        var layoutName = lv?.Layout?.Name ?? "null";
        if (args.Hint == LayoutViewEventHint.Closed)
        {
          System.Diagnostics.Debug.WriteLine($"LayoutViewEventHint.Closed: {layoutName}");
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.LayoutView
      #region LayoutViewEvent_LayoutClosing
      //Report the event args when a layout is closing.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutClosingEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutClosingEvent:" +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name +
      //  Environment.NewLine +
      //  "   arg LayoutPane: " + args.LayoutPane.Caption);
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        var lv = args.LayoutView;
        var layoutName = lv?.Layout?.Name ?? "null";
        if (args.Hint == LayoutViewEventHint.Closing)
        {
          System.Diagnostics.Debug.WriteLine($"LayoutViewEventHint.Closing: {layoutName}");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.LayoutView
      #region LayoutViewEvent_PauseDrawingChanged
      //Report the event args when a layout's paused state is changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutPauseDrawingChangedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("LayoutPauseDrawingChangedEvent:" +
      //    Environment.NewLine +
      //    "   arg Layoutview: " + args.LayoutView.Layout.Name +
      //    Environment.NewLine +
      //    "   arg Type: " + args.Paused.ToString());
      //});

      //At 3.x use LayoutViewEvent
      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        var lv = args.LayoutView;
        var layout = args.LayoutView?.Layout;

        switch (args.Hint)
        {
          case LayoutViewEventHint.PauseDrawingChanged:
            System.Diagnostics.Debug.WriteLine($"LayoutViewEvent ElementEventHint: {args.Hint.ToString()}");
            break;
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ProjectItemsChangedEventArgs.Action
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem.GetLayout
      #region LayoutRemoved_ProjectItemsChangedEvent_Removing
      //Report the event args when a layout is about to be removed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.LayoutRemovingEvent.Subscribe((args) =>
      //{
      //  if (args.LayoutPath == "CIMPATH=layout.xml")
      //  {
      //    args.Cancel = true;
      //  }
      //  return Task.FromResult(0);
      //});

      //At 3.x use ProjectItemRemovingEvent
      ArcGIS.Desktop.Core.Events.ProjectItemRemovingEvent.Subscribe((args) =>
      {
        var layoutItems = args.ProjectItems.ToList().OfType<LayoutProjectItem>() ?? new List<LayoutProjectItem>();
        var layoutName = "DontDeleteThisOne";
        foreach(var layoutItem in layoutItems)
        {
          if (layoutItem.Name == layoutName)
          {
            args.Cancel = true;//Cancel the remove
            break;
          }
        }
        return Task.FromResult(0);
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventHint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutViewEventArgs.LayoutView
      #region LayoutViewEvent
      //Report the event args for the different types of layout view events.

      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        var lv = args.LayoutView;
        var layout = args.LayoutView?.Layout;

        switch(args.Hint)
        {
          case LayoutViewEventHint.PauseDrawingChanged:
          case LayoutViewEventHint.DrawingComplete:
          case LayoutViewEventHint.ExtentChanged:
          case LayoutViewEventHint.Activated:
          case LayoutViewEventHint.Deactivated:
          case LayoutViewEventHint.Closed:
          case LayoutViewEventHint.Closing:
          case LayoutViewEventHint.Opened:
            System.Diagnostics.Debug.WriteLine($"LayoutViewEvent ElementEventHint: {args.Hint.ToString()}");
            break;
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventHint
      // cref: ArcGIS.Desktop.Layouts.Layout.MapSeries
      #region LayoutEvent_MapSeries
      //Report the event args when the map series properties have changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.MapSeriesEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("MapSeriesEvent:" +
      //  Environment.NewLine +
      //  "   arg CurrentPageName: " + args.CurrentPageName.ToString() +
      //  Environment.NewLine +
      //  "   arg CurrentlPageNumber: " + args.CurrentPageNumber.ToString() +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name +
      //  Environment.NewLine +
      //  "   arg Type: " + args.Type.ToString());
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe((args) =>
      {
        var layout = args.Layout;
        if (args.Hint == LayoutEventHint.MapSeriesRefreshed ||
          args.Hint == LayoutEventHint.MapSeriesPageChanged)
        {
          var ms = layout.MapSeries;
          System.Diagnostics.Debug.WriteLine($"LayoutEvent {args.Hint.ToString()}: {layout.Name}");
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs.Hint
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventArgs.OldPage
      // cref: ArcGIS.Desktop.Layouts.Events.LayoutEventHint
      #region LayoutEvent_PageChanged
      //Report the event args when a layout page properties are changed.

      //At 2.x - ArcGIS.Desktop.Layouts.Events.PageChangedEvent.Subscribe((args) =>
      //{
      //  System.Windows.MessageBox.Show("PageChangedEvent:" +
      //  Environment.NewLine +
      //  "   arg Layout: " + args.Layout.Name +
      //  Environment.NewLine +
      //  "   arg New CIMPage (height): " + args.NewPage.Height.ToString() +
      //  Environment.NewLine +
      //  "   arg Old CIMPage (height): " + args.OldPage.Height.ToString());
      //});

      ArcGIS.Desktop.Layouts.Events.LayoutEvent.Subscribe((args) =>
      {
        var layout = args.Layout;
        if (args.Hint == LayoutEventHint.PageChanged)
        {
          var cimPage = args.OldPage;
          System.Diagnostics.Debug.WriteLine($"LayoutEvent {args.Hint.ToString()}: {layout.Name}");
        }
      });

      #endregion

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

    // Show the DockPane.
    internal static void Show()
    {
      DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
      if (pane == null)
        return;

      pane.Activate();
    }
  }

  // Button implementation to show the DockPane.
	internal class LayoutEventSpy_ShowButton : Button
  {
    protected override void OnClick()
    {
      LayoutEventSpyViewModel.Show();
    }
  }
}
