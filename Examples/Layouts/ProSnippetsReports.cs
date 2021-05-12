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
            #region Get a specific report
            ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
            Report report =  reportProjItem?.GetReport();
            #endregion
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
          #region Reference the active report view
          //Confirm if the current, active view is a report view.  If it is, do something.
          ReportView activeReportView = ReportView.Active;
          if (activeReportView != null)
          {
            // do something
          }
          #endregion
          #region Refresh the report view
          if (reportView == null)
                return;
          QueuedTask.Run(() => reportView.Refresh());
          #endregion
          #region Zoom to whole page
          QueuedTask.Run(() => reportView.ZoomToWholePage());
      #endregion
      #region Zoom to specific location on Report view
        //On the QueuedTask
        var detailsSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportDetails>().FirstOrDefault();
        var bounds = detailsSection.GetBounds();
        ReportView.Active.ZoomTo(bounds);
      #endregion
      #region Zoom to page width
      //Process on worker thread
      QueuedTask.Run(() => reportView.ZoomToPageWidth());     
          #endregion
    }


    #region ProSnippet Group: Create Report
    #endregion
    public static async void GenerateReport(FeatureLayer featureLayer)
        {
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
            //pass true to use the selection set
            var reportDataSource = new ReportDataSource(featureLayer, defQuery, false, listFields);
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
            #region Import a report file
            //Note: Call within QueuedTask.Run()
            Item reportToImport = ItemFactory.Instance.Create(reportFile);
            Project.Current.AddItem(reportToImport as IProjectItem);
            #endregion

        }

        public static Task<bool> DeleteReport(string reportName)
        {
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
            
            #region Rename Report
            //Note: Call within QueuedTask.Run()
            ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
            reportProjItem.GetReport().SetName("RenamedReport");
            #endregion

            #region Modify the Report datasource
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
        #region ProSnippet Group: Report Design
        #endregion

        public static async Task<ReportTemplate> GetReportTemplates(string reportTemplateName)
        {
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

      #region Select elements
      //ReportDetailsSection contains the "Fields"
      var elements = reportDetailsSection.GetElementsAsFlattenedList();
      reportDetailsSection.SelectElements(elements);
      #endregion

      #region Select all elements
      //Select all elements in the Report Footer.
      ReportPageFooter pageFooterSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportPageFooter>().FirstOrDefault();
      pageFooterSection.SelectAllElements();
      #endregion

      #region Get selected elements
      IReadOnlyList<Element> selectedElements = report.GetSelectedElements();
      //Can also use the active ReportView
      IReadOnlyList<Element> selectedElementsFromView = ReportView.Active.GetSelectedElements();
      #endregion

      #region Zoom to selected elements
      QueuedTask.Run(() => reportView.ZoomToSelectedElements());
      #endregion Zoom to selected elements

      #region Clear element selection
      reportView.ClearElementSelection();
      #endregion

      #region Find specific elements in the report based on their Name.
      var reportElementsToFind = new List<string> { "ReportText1", "ReportText2" };
      var textReportElements = report.FindElements(reportElementsToFind);
      #endregion

      #region Delete Elements
      report.DeleteElements(textReportElements);
      #endregion
    }

    private static void CreateField(Report report)
    {
      #region Create a new field in the report
      //This is the gap between two fields.
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
      MapPoint newMinPoint = MapPointBuilder.CreateMapPoint(xMinOfFieldEnvelope + fieldIncrement, yMinOfFieldEnvelope);
      MapPoint newMaxPoint = MapPointBuilder.CreateMapPoint(xMaxOfFieldEnvelope + fieldIncrement, YMaxOfFieldEnvelope);
      Envelope newFieldEnvelope = EnvelopeBuilder.CreateEnvelope(newMinPoint, newMaxPoint);

      //Create field
      GraphicElement fieldGraphic = ReportElementFactory.Instance.CreateFieldValueTextElement(reportDetailsSection, newFieldEnvelope, newReportField);
      #endregion
    }
  }
}
