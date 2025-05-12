using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Presentations;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Presentations.Events;

namespace PresentationAPITesting
{
  internal static class PresentationHelper
  {
    #region ProSnippet Group: Presentations Project Items
    #endregion

    public static async void GetAllPresentations(string presentationName)
    {
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem
      #region Gets all the presentation items in the current project
      var projectPresentations = Project.Current.GetItems<PresentationProjectItem>();
      foreach (var projectItem in projectPresentations)
      {
        //Do Something with the presentation
      }
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem.GetPresentation
      // cref: ArcGIS.Desktop.Core.PresentationFrameworkExtender.CreatePresentationPaneAsync
      #region Reference an existing presentation
      //Reference a presentation associated with an active presentation view
      PresentationView activePresentationView = PresentationView.Active;
      if (activePresentationView != null)
      {
        Presentation presentation = activePresentationView.Presentation;
      }
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem.GetPresentation
      #region Get a specific presentation
      PresentationProjectItem presentationProjItem = Project.Current.GetItems<PresentationProjectItem>().FirstOrDefault(item => item.Name.Equals(presentationName));
      Presentation presentationFromItem = presentationProjItem?.GetPresentation();
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem.GetPresentation
      // cref: ArcGIS.Desktop.Core.PresentationFrameworkExtender.CreatePresentationPaneAsync
      #region Open a presentation project item in a new view
      //Open a presentation project item in a new view.
      //A presentation project item may exist but it may not be open in a view. 

      //Reference a presentation project item by name
      PresentationProjectItem presentationPrjItem = Project.Current.GetItems<PresentationProjectItem>().FirstOrDefault(item => item.Name.Equals(presentationName));

      //Get the presentation associated with the presentation project item
      Presentation presentationToOpen = await QueuedTask.Run(() => presentationPrjItem.GetPresentation());

      //Create the new pane
      IPresentationPane iNewPresentationPane = await ProApp.Panes.CreatePresentationPaneAsync(presentationToOpen); //GUI thread
      #endregion
    }

    #region ProSnippet Group: Create and export Presentation
    #endregion
    public static async void GeneratePresentation(FeatureLayer featureLayer)
    {
      // cref: ArcGIS.Desktop.Presentations.PresentationFactory
      // cref: ArcGIS.Desktop.Presentations.PresentationFactory.CreatePresentation
      #region Create presentation
      //Note: Call within QueuedTask.Run()
      await QueuedTask.Run(() =>
      {
        //Create a new presentation without parameters
        var presentation = PresentationFactory.Instance.CreatePresentation();
        // Use the new Presentation

        // Create a presentation specifying the name of the new presentation
        presentation = PresentationFactory.Instance.CreatePresentation("New Presentation");
        // Use the new Presentation
      });
      #endregion

      Presentation presentation = null;
      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.PresentationExportOptions
      // cref: ArcGIS.Desktop.Presentations.Presentation.Export
      // cref: ArcGIS.Desktop.Mapping.ExportPageOptions
      // cref: ArcGIS.Desktop.Mapping.MP4VideoFormat
      #region Export a presentation
      //Note: Call within QueuedTask.Run()
      await QueuedTask.Run(() =>
      {
        //Create mp4 format with appropriate settings
        MP4VideoFormat mp4Format = new MP4VideoFormat();
        mp4Format.Width = 800;
        mp4Format.Height = 600;
        mp4Format.OutputFileName = @"my folder\presentation.mp4";

        //Define Export Options
        PresentationExportOptions options = new PresentationExportOptions
        {
          PageRangeOption = ExportPageOptions.ExportByPageRange,
          CustomPages = "1,2,8"
        };

        //export as mp4
        presentation.Export(mp4Format, options);
      });
      #endregion
    }

    #region ProSnippet Group: Presentation View 
    #endregion
    public static async void ActivateView(string presentationName)
    {
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem
      // cref: ArcGIS.Desktop.Presentations.PresentationProjectItem.GetPresentation
      // cref: ArcGIS.Desktop.Core.PresentationFrameworkExtender.CreatePresentationPaneAsync
      // cref: ArcGIS.Desktop.Presentations.PresentationView
      #region Activate a presentation view
      //Assume we want to open a view for a particular presentation or activate a view if one is already open

      //A presentation project item is an item that appears in the Presentation folder in the Catalog pane.
      PresentationProjectItem presentationItem = Project.Current.GetItems<PresentationProjectItem>()
                                                   .FirstOrDefault(item => item.Name.Equals(presentationName));

      //Reference a presentation associated with a presentation project item
      if (presentationItem != null)
      {
        //Get the presentation associated with the presentationItem
        Presentation presentationToOpen = await QueuedTask.Run(() => presentationItem.GetPresentation());

        //Next check to see if a presentation view is already open that references the presentation
        foreach (var pane in ProApp.Panes)
        {
          var prePane = pane as IPresentationPane;
          if (prePane == null)  // Not a presentation view, continue to the next pane
            continue;

          //if there is a match, activate the view
          if (prePane.PresentationView.Presentation == presentationToOpen)
          {
            (prePane as Pane).Activate();
            return;
          }
        }

        //No pane found, activate a new one - must be called on UI
        IPresentationPane iNewPresentationPane = await ProApp.Panes.CreatePresentationPaneAsync(presentationToOpen); //GUI thread
      }
      #endregion


      #region ProSnippet Group: Presentation Page
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationView
      // cref: ArcGIS.Desktop.Presentations.Presentation.AddBlankPage
      #region Blank page
      //Note: Call within QueuedTask.Run()
      Presentation presentation = PresentationView.Active.Presentation;
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        // add a blank page with with title and paragraph body text element
        presentation.AddBlankPage(BlankPageTemplateType.TitleAndParagraph, -1);
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.Presentation.AddImagePage
      #region Image page
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        // add a new image page in current active presentation
        var imagePage = presentation.AddImagePage("my image source", -1);

        // change the image source
        imagePage.SetImageSource("new image source");
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.Presentation.AddVideoPage
      #region Video page
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        // add a new video page in current active presentation
        var videoPage = presentation.AddVideoPage("my video file", -1);

        // change the image source
        videoPage.SetVideoSource("new video source");

        // change the start time of video to 3s
        videoPage.SetStartTime(3.0);
        // change the end time of video to 10s
        videoPage.SetEndTime(10.0);
      });

      #endregion

      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.Presentation.AddMapPage
      #region Map page
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        // retrieve a map from the project based on the map name
        MapProjectItem mpi = Project.Current.GetItems<MapProjectItem>()
                                   .FirstOrDefault(m => m.Name.Equals("Your Map Name", StringComparison.CurrentCultureIgnoreCase));
        Map map = mpi.GetMap();
        //create a map page using map's default extent
        presentation.AddMapPage(map, -1);

        //create a page using map's bookmark
        Bookmark bookmark = map.GetBookmarks().FirstOrDefault(
                     b => b.Name == "Your bookmark"); // get the bookmark based on the bookmark's name
        presentation.AddMapPage(bookmark, -1);
      });
      #endregion

      #region ProSnippet Group: Map Page
      #endregion

      var activePresentationView = PresentationView.Active;

      // cref: ArcGIS.Desktop.Presentations.MapPresentationPage
      // cref: ArcGIS.Desktop.Presentations.Presentation.GetPage
      // cref: ArcGIS.Desktop.Presentations.PresentationPage
      // cref: ArcGIS.Desktop.Presentations.MapPresentationPage.SetCamera
      #region Change map page camera settings
      //Must be on the QueuedTask
      await QueuedTask.Run(() =>
      {
        //Reference a map page
        var mpage = activePresentationView.Presentation.GetPage(4) as MapPresentationPage;

        //Set the map frame extent based on the new camera's X,Y, Scale and heading values
        Camera cam = new Camera(329997.6648, 6248553.1457, 2403605.8968, 24);
        mpage.SetCamera(cam);
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.MapPresentationPage
      // cref: ArcGIS.Desktop.Presentations.Presentation.GetPage
      // cref: ArcGIS.Desktop.Presentations.PresentationPage
      // cref: ArcGIS.Desktop.Presentations.MapPresentationPage.SetCamera
      #region Zoom map page to extent of a single layer
      //Must be on the QueuedTask
      await QueuedTask.Run(() =>
      {
        //Reference map page
        var mpage = activePresentationView.Presentation.GetPage(4) as MapPresentationPage;

        //Reference map and layer
        MapProjectItem mp = Project.Current.FindItem("Page name") as MapProjectItem;
        Map map = mp.GetMap();
        FeatureLayer lyr = map.FindLayers("GreatLakes").First() as FeatureLayer;

        //Set the map frame extent to all features in the layer
        mpage.SetCamera(lyr);
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationView.ActivateMapPageAsync
      // cref: ArcGIS.Desktop.Presentations.PresentationView.ActivePage
      #region Activate a map page
      // Note: we are on the UI thread!
      // A presentation view must be active
      if (PresentationView.Active == null)
        return;
      PresentationPage activePage = activePresentationView.ActivePage;

      //check if the current page is a map page
      if (activePage is MapPresentationPage)
      {
        await activePresentationView.ActivateMapPageAsync();
      }

      //move to the QueuedTask to do something
      await QueuedTask.Run(() =>
      {
        // TODO
      });
      #endregion

      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMMargin.Bottom
      // cref: ArcGIS.Core.CIM.CIMMargin.Left
      // cref: ArcGIS.Core.CIM.CIMMargin.Right
      // cref: ArcGIS.Core.CIM.CIMMargin.Top
      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMRGBColor
      // cref: ArcGIS.Core.CIM.CIMPresentationTransition
      // cref: ArcGIS.Desktop.Presentations.Presentation.GetPage
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.SetTransition
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.SetMargin
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.SetBackgroundColor
      // cref: ArcGIS.Desktop.Presentations.PresentationTransitionType
      #region Presentation page design
      // create customized margin and color 
      CIMMargin pMargin = new CIMMargin() { Left = 0.2, Right = 0.3, Top = 0.15, Bottom = 0.25 };
      CIMRGBColor pColor = new CIMRGBColor() { R = 255, G = 255, B = 0, Alpha = 50 };

      //Reference a page and its transition 
      var page = activePresentationView.Presentation.GetPage(0);
      CIMPresentationTransition transition = page.Transition;

      // update the transition style
      transition.TransitionType = PresentationTransitionType.Swipe;
      transition.Duration = 2.0;
      transition.SwipeDirection = SwipeDirection.Top;

      //Must be on the QueuedTask
      await QueuedTask.Run(() =>
      {
        //Set the new margin, new background color and new transition effect
        page.SetMargin(pMargin);
        page.SetBackgroundColor(pColor);
        page.SetTransition(transition);
      });
      #endregion

      #region ProSnippet Group: Elements in presentations
      #endregion

      // cref: ArcGIS.Desktop.Presentations.IPresentationElementFactory
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.Instance
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.CreateTextElement
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.CreatePictureGraphicElement
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Presentations.PresentationElementFactory.CreateGroupElement
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Desktop.Layouts.ElementInfo
      #region Create elements on a presentation page
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        //create a picture element
        var imgPath = @"https://www.esri.com/content/dam/esrisites/en-us/home/" +
         "homepage-tile-podcast-business-resilience-climate-change.jpg";

        //Build a geometry to place the picture
        Coordinate2D ll = new Coordinate2D(3.5, 1);
        Coordinate2D ur = new Coordinate2D(6, 5);
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);
        //create a picture element on the page
        var gElement = PresentationElementFactory.Instance.CreatePictureGraphicElement(page, env, imgPath);

        //create a text element
        //Set symbology, create and add element to a presentation page
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                      ColorFactory.Instance.RedRGB, 15, "Arial", "Regular");
        //use ElementInfo to set placement properties
        var elemInfo = new ElementInfo()
        {
          Anchor = Anchor.CenterPoint,
          Rotation = 45
        };
        string textString = "My text";
        var textPos = new Coordinate2D(5, 3).ToMapPoint();
        var tElement = PresentationElementFactory.Instance.CreateTextGraphicElement(page,
          TextType.PointText, textPos, sym, textString, "telement", false, elemInfo);

        //create a group element with elements created above
        var elmList = new List<Element> { gElement, tElement };
        GroupElement grp1 = PresentationElementFactory.Instance.CreateGroupElement(page, elmList, "My Group");
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationPage
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.FindElement
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.FindElements
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.GetElements
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.GetFlattenedElements
      #region Element selection and navigation
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        // Find specific elements by name
        var ge_rect = page.FindElement("Rectangle") as GraphicElement;
        var elements = new List<string>();
        elements.Add("Text");
        elements.Add("Polygon");
        var elems = page.FindElements(elements);

        //Get elements retaining hierarchy
        var top_level_elems = page.GetElements();

        //Flatten hierarchy
        var all_elems = page.GetFlattenedElements();

        //Use LINQ with any of the collections
        //Retrieve just those elements that are Visible
        var some_elems = all_elems.Where(ge => ge.IsVisible).ToList();
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.PresentationPage
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.FindElement
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.FindElements
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.GetElements
      // cref: ArcGIS.Desktop.Presentations.PresentationPage.GetFlattenedElements
      #region Element selection manipulation
      //Must be on QueuedTask
      await QueuedTask.Run(() =>
      {
        //Select/unselect some elements...
        var elems = activePage.GetFlattenedElements();
        //select any element not a group element
        activePage.SelectElements(elems.Where(e => !e.Name.StartsWith("Group")));
        activePage.UnSelectElements(elems.Where(e => !e.Name.StartsWith("Group")));

        //Select/unselect all visible, graphic elements
        var ge_elems = elems.Where(ge => ge.IsVisible).ToList();
        activePage.SelectElements(ge_elems);
        activePage.UnSelectElements(ge_elems);

        //Select/unselect a specific element
        var na = activePage.FindElement("My Text Element");
        activePage.SelectElement(na);
        activePage.UnSelectElement(na);

        //Select everything
        activePage.SelectElements(elems);

        //enumerate the selected elements
        foreach (var sel_elem in activePage.GetSelectedElements())
        {
          //TODO
        }
      });
      #endregion

      #region ProSnippet Group: Presentation events
      #endregion

      // cref: ArcGIS.Desktop.Presentations.Events.PresentationEvent
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationEvent.Subscribe
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationEventHint
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationEventArgs
      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.PresentationEvent
      // cref: ArcGIS.Desktop.Presentations.PresentationEventHint
      // cref: ArcGIS.Desktop.Presentations.PresentationEventArgs
      // cref: ArcGIS.Desktop.Presentations.PresentationEvent.Subscribe
      // cref: ArcGIS.Desktop.Presentations.PresentationEventHint
      // cref: ArcGIS.Desktop.Presentations.PresentationEventArgs
      #region Detect changes to the presentation
      ArcGIS.Desktop.Presentations.Events.PresentationEvent.Subscribe((args) =>
      {
        var presentation = args.Presentation; //The presentation that was changed

        //Check what triggered the event and take appropriate action
        switch (args.Hint)
        {
          case PresentationEventHint.PropertyChanged:
            //TODO handle presentation property changed
            break;
          case PresentationEventHint.PageAdded:
            //TODO handle a new page added
            break;
          case PresentationEventHint.PageRemoved:
            //TODO handle a page removed from the presentation
            break;
          case PresentationEventHint.PageSettingChanged:
            //TODO handle page settings changed
            break;
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Presentations.Events.PresentationViewEvent
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationViewEvent.Subscribe
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationViewEventHint
      // cref: ArcGIS.Desktop.Presentations.Events.PresentationViewEventArgs
      // cref: ArcGIS.Desktop.Presentations.PresentationView
      // cref: ArcGIS.Desktop.Presentations.Presentation
      // cref: ArcGIS.Desktop.Presentations.PresentationEvent
      #region Detect changes to the presentation view
      //For UI context changes associated with a presentation, subscribe to the PresentationView
      //event - views activated/deactivated, views opened/closed
      ArcGIS.Desktop.Presentations.Events.PresentationViewEvent.Subscribe((args) =>
      {
        //get the affected view and presentation
        var view = args.PresentationView;
        var presentation = args.PresentationView?.Presentation;
        if (presentation == null)
        {
          //FYI presentationview and/or presentation can be null...
          //eg closed, deactivation
        }
        //Check what triggered the event and take appropriate action
        switch (args.Hint)
        {
          case PresentationViewEventHint.Activated:
            // Presentation view activated
            break;
          case PresentationViewEventHint.Opened:
            //A PresentationView has been initialized and opened
            break;
          case PresentationViewEventHint.Deactivated:
            // Presentation view deactivated
            break;
          case PresentationViewEventHint.Closing:
            //Set args.Cancel = true to prevent closing
            break;
          case PresentationViewEventHint.ExtentChanged:
            //presentation view extent has changed
            break;
          case PresentationViewEventHint.DrawingComplete:
            break;
          case PresentationViewEventHint.PauseDrawingChanged:
            break;
        }
      });
      #endregion
    }

    }
  }
