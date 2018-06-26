-- -- SDKExamples.lua
-- solution "SDKExamples"
   -- configurations { "Debug", "Release" }

-- project "GeodatabaseCS"
   -- kind "SharedLib"
   -- language "C#"
   -- targetdir "bin/%{cfg.buildcfg}"

   -- files { "GeodatabaseSDK/**.cs" }
   
   links {
   --Pro Assemblies
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Core/ArcGIS.Desktop.Core.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Mapping/ArcGIS.Desktop.Mapping.dll",
   "C:/Program Files/ArcGIS/Pro/bin/ArcGIS.Desktop.Framework.dll",
   "C:/Program Files/ArcGIS/Pro/bin/ArcGIS.Core.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Editing/ArcGIS.Desktop.Editing.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Catalog/ArcGIS.Desktop.Catalog.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/DesktopExtensions/ArcGIS.Desktop.Extensions.dll",
   "C:/Program Files/ArcGIS/Pro/bin/ArcGIS.Desktop.Shared.Wpf.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/ArcGISSearch/ArcGIS.Desktop.Search.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/DataReviewer/ArcGIS.Desktop.DataReviewer.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/DataSourcesRaster/ArcGIS.Desktop.DataSourcesRaster.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/GeoProcessing/ArcGIS.Desktop.GeoProcessing.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Layout/ArcGIS.Desktop.Layouts.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Sharing/ArcGIS.Desktop.Sharing.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/TaskAssistant/ArcGIS.Desktop.TaskAssistant.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/Workflow/ArcGIS.Desktop.Workflow.dll",
   "C:/Program Files/ArcGIS/Pro/bin/ArcGIS.CoreHost.dll",
   "C:/Program Files/ArcGIS/Pro/bin/Extensions/DataReviewer/ArcGIS.Desktop.DataReviewer.dll"

}


   -- filter "configurations:Debug"
      -- defines { "DEBUG" }
      -- --flags { "Symbols" }

   -- filter "configurations:Release"
      -- defines { "NDEBUG" }
      -- optimize "On"