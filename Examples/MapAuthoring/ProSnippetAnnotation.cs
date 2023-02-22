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
using ArcGIS.Core.Data.Mapping;

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
				MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(pt.X + tolerance, pt.Y, pt.SpatialReference);
					return PolylineBuilderEx.CreatePolyline(new List<MapPoint>() { pt, pt2 });
				});
			}

			#endregion
		}

		//Using Inspector...
		internal async void UpdateTextString()
		{

			BasicFeatureLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().First() as BasicFeatureLayer;
			var oid = 1;

			// cref: ArcGIS.Desktop.Editing.Attributes.Attribute
			// cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
			// cref: ArcGIS.Desktop.Editing.EditOperation.Modify
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

			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphic()
			// cref: ArcGIS.Core.CIM.CIMTextGraphic
			// cref: ArcGIS.Core.CIM.CIMTextGraphicBase.Shape
			// cref: ArcGIS.Core.Geometry.GeometryEngine.Centroid
			// cref: ArcGIS.Core.Geometry.GeometryEngine.Rotate
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
            using (var annoFeature = rowCursor.Current as 
						ArcGIS.Core.Data.Mapping.AnnotationFeature)
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

			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphic()
			// cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetTable
			#region Get the Annotation Text Graphic

			await QueuedTask.Run(() =>
			{
				using (var table = annoLayer.GetTable())
				{
					using (var rc = table.Search())
					{
						rc.MoveNext();
						using (var af = rc.Current as AnnotationFeature)
						{
							var graphic = af.GetGraphic();
							var textGraphic = graphic as CIMTextGraphic;

							//Note: 
							//var outline_geom = af.GetGraphicOutline(); 
							//gets the anno text outline geometry...
						}
					}
				}
			});

			#endregion
		}

		public void Masking1()
		{
			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature
			// cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.GetGraphicOutline()
			#region Get the Outline Geometry for an Annotation

			var annoLayer = MapView.Active.Map.GetLayersAsFlattenedList()
				                           .OfType<AnnotationLayer>().FirstOrDefault();
			if (annoLayer == null)
				return;

			QueuedTask.Run(() =>
			{
				//get the first annotation feature...
				//...assuming at least one feature gets selected
				using (var fc = annoLayer.GetFeatureClass())
				{
					using (var rc = fc.Search())
					{
						rc.MoveNext();
						using (var af = rc.Current as AnnotationFeature)
						{
							var outline_geom = af.GetGraphicOutline();
							//TODO - use the outline...

							//Note: 
							//var graphic = annoFeature.GetGraphic(); 
							//gets the CIMTextGraphic...
						}
					}
				}
			});
			#endregion

		}

		public void Masking2()
		{
			// cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetDrawingOutline(System.Int64, ArcGIS.Desktop.Mapping.MapView, ArcGIS.Desktop.Mapping.DrawingOutlineType)
			// cref: ArcGIS.Desktop.Mapping.AnnotationLayer.GetFeatureClass
			#region Get the Mask Geometry for an Annotation

			var annoLayer = MapView.Active.Map.GetLayersAsFlattenedList()
				                     .OfType<AnnotationLayer>().FirstOrDefault();
			if (annoLayer == null)
				return;
			var mv = MapView.Active;

			QueuedTask.Run(() =>
			{
				//get the first annotation feature...
				//...assuming at least one feature gets selected
				using (var fc = annoLayer.GetFeatureClass())
				{
					using (var rc = fc.Search())
					{
						rc.MoveNext();
						using (var row = rc.Current)
						{
							var oid = row.GetObjectID();

							//Use DrawingOutlineType.BoundingEnvelope to retrieve a generalized
							//mask geometry or "Box". The mask will be in the same SpatRef as the map.
							//The mask will be constructed using the anno class reference scale
							//At 2.x - var mask_geom = annoLayer.QueryDrawingOutline(oid, mv, DrawingOutlineType.Exact);

							var mask_geom = annoLayer.GetDrawingOutline(oid, mv, DrawingOutlineType.Exact);
						}
					}
				}
			});
			#endregion

		}
	}
}
