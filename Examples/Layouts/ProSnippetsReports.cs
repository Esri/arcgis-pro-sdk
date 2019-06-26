using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Reports;
using System;
using System.Collections.Generic;
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

        public static Report GetReport(string reportName)
        {
            #region Get a specific report
            ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
            return reportProjItem?.GetReport();
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
        public static void ModifyReport(string reportName, FeatureLayer featureLayer)
        {
            var report = GetReport(reportName);
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

      



      

    }
}
