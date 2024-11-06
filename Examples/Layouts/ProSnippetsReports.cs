using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportAPITesting
{
  internal static class ReportHelper
  {
    #region ProSnippet Group: Report Project Items
    #endregion

    public static void GetAllReports()
    {
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      #region Gets all the reports in the current project
      var projectReports = Project.Current.GetItems<ReportProjectItem>();
      foreach (var reportItem in projectReports)
      {
        //Do Something with the report
      }
      #endregion

    }

    public static async void GetReport(string reportName)
    {
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport
      #region Get a specific report
      ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
      Report report = reportProjItem?.GetReport();
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport
      // cref: ArcGIS.Desktop.Core.ReportFrameworkExtender.CreateReportPaneAsync
      #region Open a Report project item in a new view
      //Open a report project item in a new view.
      //A report project item may exist but it may not be open in a view. 

      //Reference a report project item by name
      ReportProjectItem reportPrjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals("MyReport"));

      //Get the report associated with the report project item
      Report reportToOpen = await QueuedTask.Run(() => reportPrjItem.GetReport());

      //Create the new pane
      IReportPane iNewReporttPane = await ProApp.Panes.CreateReportPaneAsync(reportToOpen); //GUI thread
      #endregion

    }
    public static void ReportMethods()
    {
      // cref: ArcGIS.Desktop.Reports.IReportPane
      // cref: ArcGIS.Desktop.Reports.IReportPane.ReportView
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Core.ReportFrameworkExtender.FindReportPanes
      // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate
      #region Activate an already open report view
      Report report = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault().GetReport();
      var reportPane = FrameworkApplication.Panes.FindReportPanes(report).Last();
      if (reportPane == null)
        return;
      //Activate the pane
      (reportPane as ArcGIS.Desktop.Framework.Contracts.Pane).Activate();
      //Get the "ReportView" associated with the Report Pane.
      ReportView reportView = reportPane.ReportView;
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView
      // cref: ArcGIS.Desktop.Reports.ReportView.Active
      #region Reference the active report view
      //Confirm if the current, active view is a report view.  If it is, do something.
      ReportView activeReportView = ReportView.Active;
      if (activeReportView != null)
      {
        // do something
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.Refresh
      #region Refresh the report view
      if (reportView == null)
        return;
      QueuedTask.Run(() => reportView.Refresh());
      #endregion
      // cref: Zoom to whole page;ArcGIS.Desktop.Reports.ReportView.ZoomToWholePage
      #region Zoom to whole page
      QueuedTask.Run(() => reportView.ZoomToWholePage());
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Layouts.Element.GetBounds
      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomTo(ArcGIS.Core.Geometry.Geometry)
      #region Zoom to specific location on Report view
      //On the QueuedTask
      var detailsSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportDetails>().FirstOrDefault();
      var bounds = detailsSection.GetBounds();
      ReportView.Active.ZoomTo(bounds);
      #endregion
      // cref: Zoom to page width;ArcGIS.Desktop.Reports.ReportView.ZoomToPageWidth
      #region Zoom to page width
      //Process on worker thread
      QueuedTask.Run(() => reportView.ZoomToPageWidth());
      #endregion
    }


    #region ProSnippet Group: Create Report
    #endregion
    public static async void GenerateReport(FeatureLayer featureLayer)
    {
      // cref: ArcGIS.Core.CIM.CIMReportField
      // cref: ArcGIS.Core.CIM.CIMTableField
      // cref: ArcGIS.Core.CIM.CIMTableField.Name
      // cref: ArcGIS.Core.CIM.CIMTableField.FieldOrder
      // cref: ArcGIS.Core.CIM.CIMTableField.Group
      // cref: ArcGIS.Core.CIM.CIMTableField.SortInfo
      // cref: ArcGIS.Core.CIM.FieldSortInfo
      // cref: ArcGIS.Desktop.Reports.ReportDataSource
      // cref: ArcGIS.Core.CIM.CIMPage
      // cref: ArcGIS.Core.CIM.CIMPage.Height
      // cref: ArcGIS.Core.CIM.CIMPage.StretchElements
      // cref: ArcGIS.Core.CIM.CIMPage.Width
      // cref: ArcGIS.Core.CIM.CIMPage.ShowRulers
      // cref: ArcGIS.Core.CIM.CIMPage.ShowGuides
      // cref: ArcGIS.Core.CIM.CIMPage.Margin
      // cref: ArcGIS.Core.CIM.CIMPage.Units
      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMMargin.Bottom
      // cref: ArcGIS.Core.CIM.CIMMargin.Left
      // cref: ArcGIS.Core.CIM.CIMMargin.Right
      // cref: ArcGIS.Core.CIM.CIMMargin.Top
      // cref: ArcGIS.Core.Geometry.LinearUnit
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager.GetTemplatesAsync
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager.GetStylingsAsync
      // cref: ArcGIS.Desktop.Reports.ReportTemplate
      // cref: ArcGIS.Desktop.Reports.ReportTemplate.Name
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic.Field
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic.Statistic
      // cref: ArcGIS.Core.CIM.FieldStatisticsFlag
      // cref: ArcGIS.Desktop.Reports.ReportFactory
      // cref: ArcGIS.Desktop.Reports.ReportFactory.CreateReport
      #region Create report
      //Note: Call within QueuedTask.Run()
      //The fields in the datasource used for the report
      //This uses a US Cities dataset
      var listFields = new List<CIMReportField> {
                //Grouping should be the first field
                new CIMReportField{Name = "STATE_NAME", FieldOrder = 0, Group = true, SortInfo = FieldSortInfo.Desc}, //Group cities using STATES
                new CIMReportField{Name = "CITY_NAME", FieldOrder = 1},
                new CIMReportField{Name = "POP1990", FieldOrder = 2, },
            };
      //Definition query to use for the data source
      var defQuery = "STATE_NAME LIKE 'C%'";
      //Define the Datasource
      var reportDataSource = new ReportDataSource(featureLayer, defQuery, listFields);
      //The CIMPage defintion - page size, units, etc
      var cimReportPage = new CIMPage
      {
        Height = 11,
        StretchElements = false,
        Width = 6.5,
        ShowRulers = true,
        ShowGuides = true,
        Margin = new CIMMargin { Bottom = 1, Left = 1, Right = 1, Top = 1 },
        Units = LinearUnit.Inches
      };

      //Report template
      var reportTemplates = await ReportTemplateManager.GetTemplatesAsync();
      var reportTemplate = reportTemplates.Where(r => r.Name == "Attribute List with Grouping").First();

      //Report Styling
      var reportStyles = await ReportStylingManager.GetStylingsAsync();
      var reportStyle = reportStyles.Where(s => s == "Cool Tones").First();

      //Field Statistics
      var fieldStatisticsList = new List<ReportFieldStatistic> {
                new ReportFieldStatistic{ Field = "POP1990", Statistic = FieldStatisticsFlag.Sum}
                //Note: NoStatistics option for FieldStatisticsFlag is not supported.
            };
      var report = ReportFactory.Instance.CreateReport("USAReport", reportDataSource, cimReportPage, fieldStatisticsList, reportTemplate, reportStyle);
      #endregion

    }
    public static void ExportAReport(Report report, string path, bool useSelection = true)
    {
      if (report == null) return;

      // cref: ArcGIS.Desktop.Reports.ReportExportOptions
      // cref: ArcGIS.Desktop.Reports.ReportExportOptions.ExportPageOption
      // cref: ArcGIS.Desktop.Reports.ReportExportOptions.TotalPageNumberOverride
      // cref: ArcGIS.Desktop.Mapping.ExportPageOptions
      // cref: ArcGIS.Desktop.Mapping.PDFFormat
      // cref: ArcGIS.Desktop.Mapping.ExportFormat.Resolution
      // cref: ArcGIS.Desktop.Mapping.ExportFormat.OutputFileName
      // cref: ArcGIS.Desktop.Reports.Report.ExportToPDF
      #region Export report to pdf
      //Note: Call within QueuedTask.Run()
      //Define Export Options
      var exportOptions = new ReportExportOptions
      {
        ExportPageOption = ExportPageOptions.ExportAllPages,
        TotalPageNumberOverride = 0

      };
      //Create PDF format with appropriate settings
      PDFFormat pdfFormat = new PDFFormat();
      pdfFormat.Resolution = 300;
      pdfFormat.OutputFileName = path;
      report.ExportToPDF($"{report.Name}", pdfFormat, exportOptions, useSelection);
      #endregion
    }

    public static void ImportAReport(string reportFile)
    {
      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.IProjectItem
      #region Import a report file
      //Note: Call within QueuedTask.Run()
      Item reportToImport = ItemFactory.Instance.Create(reportFile);
      Project.Current.AddItem(reportToImport as IProjectItem);
      #endregion

    }

    public static Task<bool> DeleteReport(string reportName)
    {
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Delete a report
      //Note: Call within QueuedTask.Run()
      //Reference a reportitem in a project by name
      ReportProjectItem reportItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));

      //Check for report item
      if (reportItem == null)
        return Task.FromResult<bool>(false);

      //Delete the report from the project
      return Task.FromResult<bool>(Project.Current.RemoveItem(reportItem));
      #endregion
    }
    #region ProSnippet Group: Modify Reports 
    #endregion
    public static void ModifyReport(Report report, string reportName, FeatureLayer featureLayer)
    {
      // cref: ArcGIS.Desktop.Reports.Report.SetName
      #region Rename Report
      //Note: Call within QueuedTask.Run()
      ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
      reportProjItem.GetReport().SetName("RenamedReport");
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.RemoveGroups
      // cref: ArcGIS.Desktop.Reports.Report.AddGroup
      // cref: ArcGIS.Desktop.Reports.Report.SetDefinitionQuery
      // cref: ArcGIS.Desktop.Reports.Report.RemoveGroups
      #region Modify the Report DefinitionQuery
      //Note: Call within QueuedTask.Run()
      //Remove Groups
      // The fields in the datasource used for the report
      var listFields = new List<string> {
             "STATE_NAME"
            };
      report.RemoveGroups(listFields);

      //Add Group
      report.AddGroup("STATE_NAME", true, true, "");

      //Modify the Definition Query
      var defQuery = "STATE_NAME LIKE 'C%'";
      report.SetDefinitionQuery(defQuery);
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.SetPage
      // cref: ArcGIS.Desktop.Reports.Report.SetPageHeight
      // cref: ArcGIS.Core.CIM.CIMPage
      // cref: ArcGIS.Core.CIM.CIMPage.Height
      // cref: ArcGIS.Core.CIM.CIMPage.StretchElements
      // cref: ArcGIS.Core.CIM.CIMPage.Width
      // cref: ArcGIS.Core.CIM.CIMPage.ShowRulers
      // cref: ArcGIS.Core.CIM.CIMPage.ShowGuides
      // cref: ArcGIS.Core.CIM.CIMPage.Margin
      // cref: ArcGIS.Core.CIM.CIMPage.Units
      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMMargin.Bottom
      // cref: ArcGIS.Core.CIM.CIMMargin.Left
      // cref: ArcGIS.Core.CIM.CIMMargin.Right
      // cref: ArcGIS.Core.CIM.CIMMargin.Top
      #region Modify the report Page
      //Note: Call within QueuedTask.Run()
      var cimReportPage = new CIMPage
      {
        Height = 12,
        StretchElements = false,
        Width = 6.5,
        ShowRulers = true,
        ShowGuides = true,
        Margin = new CIMMargin { Bottom = 1, Left = 1, Right = 1, Top = 1 },
        Units = LinearUnit.Inches
      };
      report.SetPage(cimReportPage);
      //Change only the report's page height
      report.SetPageHeight(12);
      #endregion

    }

    public static void AddSubReport()
    {
      QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Reports.Report.AddSubReport
        #region Add SubReport
        //Note: Call within QueuedTask.Run()
        var mainReport = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(r => r.Name == "USAReports")?.GetReport();

        if (mainReport == null) return;
        //Add sub report
        var vermontReportItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(r => r.Name == "Vermont");
        if (vermontReportItem == null) return;
        Report vermontReport = vermontReportItem.GetReport();
        mainReport.AddSubReport(vermontReportItem, -1, true); //  If -1, the subreport is added to the end of the report.
        #endregion
      });
    }
    #region ProSnippet Group: Report Design
    #endregion

    public static async Task<ReportTemplate> GetReportTemplates(string reportTemplateName)
    {
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager.GetTemplatesAsync
      // cref: ArcGIS.Desktop.Reports.ReportTemplate.Name
      #region Get a report template
      //Report Template Styles:
      //Attribute List
      //Attribute List with Grouping
      //Basic Summary
      //Basic Summary with Grouping
      //Page Per Feature
      var reportTemplates = await ReportTemplateManager.GetTemplatesAsync();
      var reportTemplate = reportTemplates.Where(r => r.Name == reportTemplateName).First();
      #endregion

      System.Diagnostics.Debug.WriteLine(reportTemplate.Name);
      return reportTemplate;

    }

    public static async Task<string> GetReportStyling(string reportStyleName)
    {
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager.GetStylingsAsync
      #region Get a report styling
      //Report Styling:
      //Black and White
      //Cool Tones
      //Warm Tones
      var reportStyles = await ReportStylingManager.GetStylingsAsync();
      var reportStyle = reportStyles.Where(s => s == reportStyleName).First();
      #endregion
      System.Diagnostics.Debug.WriteLine(reportStyle.ToString());

      return reportStyle;
    }

    #region ProSnippet Group: Report Elements
    #endregion
    public static void ElementFactory(Report report, ReportView reportView)
    {
      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Reports.ReportHeader
      // cref: ArcGIS.Desktop.Reports.ReportPageHeader
      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Reports.ReportPageFooter
      // cref: ArcGIS.Desktop.Reports.ReportFooter
      #region Get various Report sections
      //Get the "ReportSection element"
      //ReportSectionElement contains the ReportHeader, ReportPageHeader, ReportDetails. ReportPageFooter, ReportFooter sections.
      var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();

      //Get the ReportHeader
      var reportHeader = mainReportSection?.Elements.OfType<ReportHeader>().FirstOrDefault();

      //Get the ReportHeader
      var reportPageHeader = mainReportSection?.Elements.OfType<ReportPageHeader>().FirstOrDefault();

      //Get the "ReportDetails" within the ReportSectionElement. ReportDetails is where "fields" are.
      var reportDetailsSection = mainReportSection?.Elements.OfType<ReportDetails>().FirstOrDefault();

      //Get the ReportPageFooter
      var reportPageFooter = mainReportSection?.Elements.OfType<ReportPageFooter>().FirstOrDefault();

      //Get the ReportFooter
      var reportFooter = mainReportSection?.Elements.OfType<ReportFooter>().FirstOrDefault();
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Desktop.Reports.ReportView.SelectElements(System.Collections.Generic.IReadOnlyList{ArcGIS.Desktop.Layouts.Element})
      #region Select elements
      //ReportDetailsSection contains the "Fields"

      var elements = reportDetailsSection.Elements;
      reportDetailsSection.SelectElements(elements);
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportPageFooter
      // cref: ArcGIS.Desktop.Reports.ReportSectionElement.SelectAllElements
      #region Select all elements
      //Select all elements in the Report Footer.
      ReportPageFooter pageFooterSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportPageFooter>().FirstOrDefault();
      pageFooterSection.SelectAllElements();
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.GetSelectedElements
      // cref: ArcGIS.Desktop.Reports.Report.GetSelectedElements
      #region Get selected elements
      IReadOnlyList<Element> selectedElements = report.GetSelectedElements();
      //Can also use the active ReportView
      IReadOnlyList<Element> selectedElementsFromView = ReportView.Active.GetSelectedElements();
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomToSelectedElements
      #region Zoom to selected elements
      QueuedTask.Run(() => reportView.ZoomToSelectedElements());
      #endregion Zoom to selected elements

      // cref: ArcGIS.Desktop.Reports.ReportView.ClearElementSelection
      #region Clear element selection
      reportView.ClearElementSelection();
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.FindElements
      #region Find specific elements in the report based on their Name.
      var reportElementsToFind = new List<string> { "ReportText1", "ReportText2" };
      var textReportElements = report.FindElements(reportElementsToFind);
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.DeleteElements
      #region Delete Elements

      QueuedTask.Run(() => report.DeleteElements(textReportElements));
      #endregion
    }

    private static void CreateField(Report report)
    {
      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Core.CIM.CIMReportField
      // cref: ArcGIS.Core.CIM.CIMTableField.Name
      // cref: ArcGIS.Core.CIM.CIMTableField.FieldOrder
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Desktop.Layouts.Element.GetBounds
      // cref: ArcGIS.Desktop.Reports.ReportElementFactory.CreateFieldValueTextElement
      #region Create a new field in the report
      //This is the gap between two fields
      double fieldIncrement = 0.9388875113593206276389;
      //On the QueuedTask
      //New field to add.
      var newReportField = new CIMReportField
      {
        Name = "POP1990",
        FieldOrder = 2,
      };
      //Get the "ReportSection element"				
      var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();
      if (mainReportSection == null) return;

      //Get the "ReportDetails" within the ReportSectionElement. ReportDetails is where "fields" are.
      var reportDetailsSection = mainReportSection?.Elements.OfType<ReportDetails>().FirstOrDefault();
      if (reportDetailsSection == null) return;

      //Within ReportDetails find the envelope that encloses a field.
      //We get the first CIMParagraphTextGraphic in the collection so that we can add the new field next to it.					
      var lastFieldGraphic = reportDetailsSection.Elements.FirstOrDefault((r) =>
      {
        var gr = r as GraphicElement;
        if (gr == null) return false;
        return (gr.GetGraphic() is CIMParagraphTextGraphic ? true : false);
      });
      //Get the Envelope of the last field
      var graphicBounds = lastFieldGraphic.GetBounds();

      //Min and Max values of the envelope
      var xMinOfFieldEnvelope = graphicBounds.XMin;
      var yMinOfFieldEnvelope = graphicBounds.YMin;

      var xMaxOfFieldEnvelope = graphicBounds.XMax;
      var YMaxOfFieldEnvelope = graphicBounds.YMax;
      //create the new Envelope to be offset from the existing field
      //At 2.x
      //MapPoint newMinPoint = MapPointBuilder.CreateMapPoint(xMinOfFieldEnvelope + fieldIncrement, yMinOfFieldEnvelope);
      //MapPoint newMaxPoint = MapPointBuilder.CreateMapPoint(xMaxOfFieldEnvelope + fieldIncrement, YMaxOfFieldEnvelope);
      //Envelope newFieldEnvelope = EnvelopeBuilder.CreateEnvelope(newMinPoint, newMaxPoint);

      MapPoint newMinPoint = MapPointBuilderEx.CreateMapPoint(xMinOfFieldEnvelope + fieldIncrement, yMinOfFieldEnvelope);
      MapPoint newMaxPoint = MapPointBuilderEx.CreateMapPoint(xMaxOfFieldEnvelope + fieldIncrement, YMaxOfFieldEnvelope);
      Envelope newFieldEnvelope = EnvelopeBuilderEx.CreateEnvelope(newMinPoint, newMaxPoint);

      //Create field
      GraphicElement fieldGraphic = ReportElementFactory.Instance.CreateFieldValueTextElement(reportDetailsSection, newFieldEnvelope, newReportField);
      #endregion
    }

    #region ProSnippet Group: Raster and Imagery Options
    #endregion

    internal static void Options()
    {
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.ReportOptions
      // cref: ArcGIS.Desktop.Core.ReportOptions
      #region Get/Set Report Options

      //toggle/switch option values
      var options = ApplicationOptions.ReportOptions;

      options.PreviewAllPages = false;
      options.NumberOfPreviewPages = 25;

      options.ReportCustomTemplatePath = @"c:\temp";

      #endregion
    }
  }
}
