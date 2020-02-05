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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Editing.Attributes;

namespace AnnotationSnippets
{
	internal class Annotation
	{

		internal class AnnoConstructionTool : MapTool
		{
			public AnnoConstructionTool()
			{
				IsSketchTool = true;
				UseSnapping = true;
				SketchType = SketchGeometryType.Point;
			}

			#region Create Annotation Construction Tool

			//In your config.daml...set the categoryRefID
			//<tool id="..." categoryRefID="esri_editing_construction_annotation" caption="Create Anno" ...>

			//Sketch type Point or Line or BezierLine in the constructor...
			//internal class AnnoConstructionTool : MapTool  {
			//  public AnnoConstructionTool()  {
			//    IsSketchTool = true;
			//    UseSnapping = true;
			//    SketchType = SketchGeometryType.Point;
			//

			protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
			{
				if (CurrentTemplate == null || geometry == null)
					return false;

				// Create an edit operation
				var createOperation = new EditOperation();
				createOperation.Name = string.Format("Create {0}", CurrentTemplate.Layer.Name);
				createOperation.SelectNewFeatures = true;

				// update the geometry point into a 2 point line
				//annotation needs at minimum a 2 point line for the text to be placed
				double tol = 0.01;
				var polyline = await CreatePolylineFromPointAsync((MapPoint)geometry, tol);

				// Queue feature creation
				createOperation.Create(CurrentTemplate, polyline);

				// Execute the operation
				return await createOperation.ExecuteAsync();
			}

			internal Task<Polyline> CreatePolylineFromPointAsync(MapPoint pt, double tolerance)
			{
				return QueuedTask.Run(() =>
				{
				// create a polyline from a starting point
				//use a tolerance to construct the second point
				MapPoint pt2 = MapPointBuilder.CreateMapPoint(pt.X + tolerance, pt.Y, pt.SpatialReference);
					return PolylineBuilder.CreatePolyline(new List<MapPoint>() { pt, pt2 });
				});
			}

			#endregion
		}

		//Using Inspector...
		internal async void UpdateTextString()
		{

			BasicFeatureLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().First() as BasicFeatureLayer;
			var oid = 1;

			#region Update Annotation Text via attribute. Caveat: The TEXTSTRING Anno attribute must exist

			//See "Change Annotation Text Graphic" for an alternative if TEXTSTRING is missing from the schema
			await QueuedTask.Run(() =>
			{
				//annoLayer is ~your~ Annotation layer...

				// use the inspector methodology
				var insp = new Inspector();
				insp.Load(annoLayer, oid);

				// make sure TextString attribute exists.
				//It is not guaranteed to be in the schema
				ArcGIS.Desktop.Editing.Attributes.Attribute att = insp.FirstOrDefault(a => a.FieldName == "TEXTSTRING");
				if (att != null)
				{
					insp["TEXTSTRING"] = "Hello World";

					//create and execute the edit operation
					EditOperation op = new EditOperation();
					op.Name = "Update annotation";
					op.Modify(insp);

					//OR using a Dictionary - again TEXTSTRING has to exist in the schema
					//Dictionary<string, object> newAtts = new Dictionary<string, object>();
					//newAtts.Add("TEXTSTRING", "hello world");
					//op.Modify(annoLayer, oid, newAtts);

					op.Execute();
				}
			});
			#endregion

			#region Rotate or Move the Annotation

			await QueuedTask.Run(() =>
			{
				//Don't use 'Shape'....Shape is the bounding box of the annotation text. This is NOT what you want...
				//
				//var insp = new Inspector();
				//insp.Load(annoLayer, oid);
				//var shape = insp["SHAPE"] as Polygon;
				//...wrong shape...

				//Instead, we must get the TextGraphic from the anno feature.
				//The TextGraphic shape will be the anno baseline...
				//At 2.1 the only way to retrieve this textLine is to obtain the TextGraphic from the AnnotationFeature
				QueryFilter qf = new QueryFilter()
				{
					WhereClause = "OBJECTID = 1"
				};

        //annoLayer is ~your~ Annotation layer

        using (var rowCursor = annoLayer.Search(qf))
        {
          if (rowCursor.MoveNext())
          {
            using (var annoFeature = rowCursor.Current as ArcGIS.Core.Data.Mapping.AnnotationFeature)
            {
              var graphic = annoFeature.GetGraphic();
              var textGraphic = graphic as CIMTextGraphic;
              var textLine = textGraphic.Shape as Polyline;
              // rotate the shape 90 degrees
              var origin = GeometryEngine.Instance.Centroid(textLine);
              Geometry rotatedPolyline = GeometryEngine.Instance.Rotate(textLine, origin, System.Math.PI / 2);
              //Move the line 5 "units" in the x and y direction
              //GeometryEngine.Instance.Move(textLine, 5, 5);

              EditOperation op = new EditOperation();
              op.Name = "Change annotation angle";
              op.Modify(annoLayer, oid, rotatedPolyline);
              op.Execute();
            }
          }
        }
      });

			#endregion

			#region Change Annotation Text Graphic

			await QueuedTask.Run(() =>
			{

				EditOperation op = new EditOperation();
				op.Name = "Change annotation graphic";

				//At 2.1 we must use an edit operation Callback...
				op.Callback(context =>
				{
					QueryFilter qf = new QueryFilter()
					{
						WhereClause = "OBJECTID = 1"
					};
          //Cursor must be non-recycling. Use the table ~not~ the layer..i.e. "GetTable().Search()"
          //annoLayer is ~your~ Annotation layer
          using (var rowCursor = annoLayer.GetTable().Search(qf, false))
          {
            if (rowCursor.MoveNext())
            {
              using (var annoFeature = rowCursor.Current as ArcGIS.Core.Data.Mapping.AnnotationFeature)
              {
                //Get the graphic from the anno feature
                var graphic = annoFeature.GetGraphic();
                var textGraphic = graphic as CIMTextGraphic;

                // change the text and the color
                textGraphic.Text = "hello world";
                var symbol = textGraphic.Symbol.Symbol;
                symbol.SetColor(ColorFactory.Instance.RedRGB);
                textGraphic.Symbol = symbol.MakeSymbolReference();
                // update the graphic
                annoFeature.SetGraphic(textGraphic);
                // store is required
                annoFeature.Store();
                //refresh layer cache
                context.Invalidate(annoFeature);
              }
            }
          }
        }, annoLayer.GetTable());

				op.Execute();
			});

			#endregion
		}
	}
}
