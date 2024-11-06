
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
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Realtime;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Layouts.Utilities;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Desktop.Mapping.Voxel;
using ArcGIS.Desktop.Mapping.Voxel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapAuthoring.VoxelLayers
{

	class ProSnippetRealtime
	{
		#region ProSnippet Group: Create Voxel Layer
		#endregion

		public async void Example1()
		{
			Map map = MapView.Active.Map;
			await QueuedTask.Run(() =>
			{

				// cref: ArcGIS.Desktop.Mapping.VoxelLayer
				// cref: ArcGIS.Desktop.Mapping.MapView.ViewingMode
				// cref: ArcGIS.Core.CIM.MapViewingMode
				#region Check if a Voxel Layer can be created

				//Map must be a local scene
				bool canCreateVoxel = (MapView.Active.ViewingMode == MapViewingMode.SceneLocal);

				if (canCreateVoxel)
				{
					//TODO - use the voxel api methods
				}
				#endregion

				// cref: ArcGIS.Desktop.Mapping.VoxelLayer
				// cref: ArcGIS.Core.CIM.CIMVoxelDataConnection
				// cref: ArcGIS.Core.CIM.CIMVoxelDataConnection.URI
				// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams
				// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.Create(ArcGIS.Core.CIM.CIMVoxelDataConnection)
				// cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
				// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.Variables
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.Variable
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.DataType
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.Description
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.IsDefault
				// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.IsSelected
				// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.SetDefaultVariable(ArcGIS.Desktop.Mapping.VoxelVariableCreationParams)
				// cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer
				// cref: ArcGIS.Desktop.Mapping.LayerFactory
				#region Create Voxel Layer
				//Must be on the QueuedTask.Run()

				//Must be a .NetCDF file for voxels
				var url = @"C:\MyData\AirQuality_Redlands.nc";
				var cim_connection = new CIMVoxelDataConnection()
				{
					URI = url
				};
				//Create a VoxelLayerCreationParams
				var createParams = VoxelLayerCreationParams.Create(cim_connection);
				createParams.IsVisible = true;

				//Can also just use the path directly...
				//var createParams = VoxelLayerCreationParams.Create(url);

				//Use VoxelLayerCreationParams to enumerate the variables within
				//the voxel
				var variables = createParams.Variables;
				foreach (var variable in variables)
				{
					var line = $"{variable.Variable}: {variable.DataType}, " +
					   $"{variable.Description}, {variable.IsDefault}, {variable.IsSelected}";
					System.Diagnostics.Debug.WriteLine(line);
				}
				//Optional: set the default variable
				createParams.SetDefaultVariable(variables.Last());

				//Create the layer - map must be a local scene
				VoxelLayer voxelLayer = LayerFactory.Instance.CreateLayer<VoxelLayer>(createParams, map);
				#endregion
			});
		}

		#region ProSnippet Group: Voxel Layer settings and properties
		#endregion

		public void Example2()
		{
			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer
			#region Get a Voxel Layer from the TOC
			//Get selected layer if a voxel layer is selected
			var voxelLayer = MapView.Active.GetSelectedLayers().OfType<VoxelLayer>().FirstOrDefault();
			if (voxelLayer == null)
			{
				//just get the first voxel layer in the TOC
				voxelLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<VoxelLayer>().FirstOrDefault();
				if (voxelLayer == null)
					return;
			}
			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer
			// cref: ArcGIS.Desktop.Mapping.Layer.SetExpanded
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetIsosurfaceContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetIsosurfaceContainerVisibility(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSectionContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSectionContainerVisibility(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetLockedSectionContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetLockedSectionContainerVisibility(System.Boolean)
			#region Manipulate the Voxel Layer TOC Group

			//var voxelLayer = ...
			//Must be on the QueuedTask.Run()

			//Toggle containers and visibility in the TOC
			voxelLayer.SetExpanded(!voxelLayer.IsExpanded);
			voxelLayer.SetVisibility(!voxelLayer.IsVisible);
			voxelLayer.SetIsosurfaceContainerExpanded(!voxelLayer.IsIsosurfaceContainerExpanded);
			voxelLayer.SetIsosurfaceContainerVisibility(!voxelLayer.IsIsosurfaceContainerVisible);
			voxelLayer.SetSliceContainerExpanded(voxelLayer.IsSliceContainerExpanded);
			voxelLayer.SetSliceContainerVisibility(!voxelLayer.IsSliceContainerVisible);
			voxelLayer.SetSectionContainerExpanded(!voxelLayer.IsSectionContainerExpanded);
			voxelLayer.SetSectionContainerVisibility(!voxelLayer.IsSectionContainerVisible);
			voxelLayer.SetLockedSectionContainerExpanded(!voxelLayer.IsLockedSectionContainerExpanded);
			voxelLayer.SetLockedSectionContainerVisibility(!voxelLayer.IsLockedSectionContainerVisible);

			#endregion

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedIsosurfaces()
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition
			// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
			// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
			// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelSection( ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
			// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelLockedSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
			#region Get/Set Selected Voxel Assets from the TOC

			var surfaces = MapView.Active.GetSelectedIsosurfaces();
			//set selected w/ MapView.Active.SelectVoxelIsosurface(isoSurface)
			var slices = MapView.Active.GetSelectedSlices();
			//set selected w/ MapView.Active.SelectVoxelSlice(slice)
			var sections = MapView.Active.GetSelectedSections();
			//set selected w/ MapView.Active.SelectVoxelSection(section)
			var locked_sections = MapView.Active.GetSelectedLockedSections();
			//set selected w/ MapView.Active.SelectVoxelLockedSection(locked_section)

			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
			// cref: ArcGIS.Core.CIM.VoxelVisualization
			#region Change the Voxel Visualization

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Change the visualization to Volume
			//e.g. for creating slices
			voxelLayer.SetVisualization(VoxelVisualization.Volume);

			//Change the visualization to Surface
			//e.g. to create isosurfaces and sections
			voxelLayer.SetVisualization(VoxelVisualization.Surface);
			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.CartographicOffset
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetCartographicOffset(System.Double)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.VerticalExaggeration
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVerticalExaggeration(System.Double)
			// cref: ArcGIS.Core.CIM.CIM3DLayerProperties.ExaggerationMode
			// cref: ArcGIS.Core.CIM.CIM3DLayerProperties.VerticalExaggeration
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.IsDiffuseLightingEnabled
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetDiffuseLightingEnabled(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.DiffuseLighting
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetDiffuseLighting(System.Double)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.IsSpecularLightingEnabled
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSpecularLightingEnabled(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SpecularLighting
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSpecularLighting(System.Double)
			#region Lighting Properties, Offset, Vertical Exaggeration

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Offset
			var offset = voxelLayer.CartographicOffset;
			//apply an offset
			voxelLayer.SetCartographicOffset(offset + 100.0);

			//VerticalExaggeration
			var exaggeration = voxelLayer.VerticalExaggeration;
			//apply an exaggeration
			voxelLayer.SetVerticalExaggeration(exaggeration + 100.0);
			
			//Change the exaggeration mode to "ScaleZ" - corresponds to 'Z-coordinates' 
			//on the Layer properties UI - must use the CIM
			var def = voxelLayer.GetDefinition() as CIMVoxelLayer;
			def.Layer3DProperties.ExaggerationMode = ExaggerationMode.ScaleZ;
			//can set vertical exaggeration via the CIM also
			//def.Layer3DProperties.VerticalExaggeration = exaggeration + 100.0;

			//apply the change
			voxelLayer.SetDefinition(def);

			//Diffuse Lighting
			if (!voxelLayer.IsDiffuseLightingEnabled)
				voxelLayer.SetDiffuseLightingEnabled(true);
			var diffuse = voxelLayer.DiffuseLighting;
			//set Diffuse lighting to a value between 0 and 1
			voxelLayer.SetDiffuseLighting(0.5); //50%

			//Specular Lighting
			if (!voxelLayer.IsSpecularLightingEnabled)
				voxelLayer.SetSpecularLightingEnabled(true);
			var specular = voxelLayer.SpecularLighting;
			//set Diffuse lighting to a value between 0 and 1
			voxelLayer.SetSpecularLighting(0.5); //50%

			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVolumes
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
			#region Get the Voxel Volume Dimensions

			//At 2.x - var volume = voxelLayer.GetVolumeSize();
			//var x_max = volume.Item1;
			//var y_max = volume.Item2;
			//var z_max = volume.Item3;

			var x_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().X);
			var y_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().Y);
			var z_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().Z);

			//Get the dimensions of just one volume
			var dimensions = voxelLayer.GetVolumes().FirstOrDefault();
			//Get the dimensions of the volume associated with the selected variable
			var dimensions2 = voxelLayer.SelectedVariableProfile.Volume.GetVolumeSize();

			#endregion
		}

		#region ProSnippet Group: Events
		#endregion

		public void ExampleEvents()
		{
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
			// cref: ArcGIS.Desktop.Mapping.Events.MapMemberEventHint
			// cref: ArcGIS.Desktop.Mapping.Events.MapMemberPropertiesChangedEventArgs.EventHints
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetChangedEvent.Subscribe
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.ChangeType
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.AssetType
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.VoxelAssetChangeType
			// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.VoxelAssetType
			#region Subscribe for Changes to a Voxel Layer

			ArcGIS.Desktop.Mapping.Events.MapMemberPropertiesChangedEvent.Subscribe((args) =>
			{
				var voxel = args.MapMembers.OfType<VoxelLayer>().FirstOrDefault();
				if (voxel == null)
					return;
				//Anything changed on a voxel layer?
				//At 2.x - if (args.EventHints.Any(hint => hint == MapMemberEventHint.VoxelSelectedVariableProfileIndex))
				if (args.EventHints.Any(hint => hint == MapMemberEventHint.VoxelSelectedVariable))
				{
					//Voxel variable profile selection changed
					var changed_variable_name = voxel.SelectedVariableProfile.Variable;
					//TODO respond to change, use QueuedTask if needed

				}
				else if (args.EventHints.Any(hint => hint == MapMemberEventHint.Renderer))
				{
					//This can fire when a renderer becomes ready on a new layer; the selected variable profile
					//is changed; visualization is changed, etc.
					var renderer = voxel.SelectedVariableProfile.Renderer;
					//TODO respond to change, use QueuedTask if needed

				}
			});

			ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetChangedEvent.Subscribe((args) =>
			{
				//An asset changed on a voxel layer
				System.Diagnostics.Debug.WriteLine("");
				System.Diagnostics.Debug.WriteLine("VoxelAssetChangedEvent");
				System.Diagnostics.Debug.WriteLine($" AssetType: {args.AssetType}, ChangeType: {args.ChangeType}");

				if (args.ChangeType == VoxelAssetEventArgs.VoxelAssetChangeType.Remove)
					return;
				//Get "what"changed - add or update
				//eg IsoSurface
				VoxelLayer voxelLayer = null;
				if (args.AssetType == VoxelAssetEventArgs.VoxelAssetType.Isosurface)
				{
					var surface = MapView.Active.GetSelectedIsosurfaces().FirstOrDefault();
					//there will only be one selected...
					if (surface != null)
					{
						voxelLayer = surface.Layer;
						//TODO respond to change, use QueuedTask if needed
					}
				}
				//Repeat for Slices, Sections, LockedSections...
				//GetSelectedSlices(), GetSelectedSections(), GetSelectedLockedSections();
			});

			#endregion
		}

		#region ProSnippet Group: Variable Profiles + Renderer
		#endregion

		public void Example4a()
		{
			//Get selected layer if a voxel layer is selected
			var voxelLayer = MapView.Active.GetSelectedLayers().OfType<VoxelLayer>().FirstOrDefault();
			if (voxelLayer == null)
			{
				//just get the first voxel layer in the TOC
				voxelLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<VoxelLayer>().FirstOrDefault();
				if (voxelLayer == null)
					return;
			}

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
			#region Get the Selected Variable Profile

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var sel_profile = voxelLayer.SelectedVariableProfile;

			//Get the variable profile name
			var profile_name = sel_profile.Variable;

			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSelectedVariableProfile(ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile)
			#region Change the Selected Variable Profile

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var profiles = voxelLayer.GetVariableProfiles();

			//Select any profile as long as it is not the current selected variable
			var not_selected = profiles.Where(p => p.Variable != sel_profile.Variable).ToList();
			if (not_selected.Count() > 0)
			{
				voxelLayer.SetSelectedVariableProfile(not_selected.First());
			}

			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile
			#region Get the Variable Profiles

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable_profiles = voxelLayer.GetVariableProfiles();

			#endregion
		}

		public void Example4()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
			// cref: ArcGIS.Core.CIM.CIMVoxelRenderer
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
			// cref: ArcGIS.Core.CIM.VoxelVariableDataType
			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer
			// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer
			#region Get the Variable Renderer

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable = voxelLayer.GetVariableProfiles().First();
			var renderer = variable.Renderer;
			if (variable.DataType == VoxelVariableDataType.Continuous)
			{
				//Renderer will be stretch
				var stretchRenderer = renderer as CIMVoxelStretchRenderer;
				//access the renderer

			}
			else //VoxelVariableDataType.Discrete
			{
				//Renderer will be unique value
				var uvr = renderer as CIMVoxelUniqueValueRenderer;
				//access the renderer
			}

			#endregion
		}

		public void Example5b()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Statistics
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MinimumValue
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MaximumValue
			// cref: ArcGIS.Core.CIM.VoxelVariableDataType
			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMax
			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMin
			#region Access Stats and Color Range for a Stretch Renderer

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Get the variable profile on which to access the data
			var variable = voxelLayer.SelectedVariableProfile;
			//or use ...voxelLayer.GetVariableProfiles()

			//Data range
			//At 2.x - 
			//var min = variable.GetVariableStatistics().MinimumValue;
			//var max = variable.GetVariableStatistics().MaximumValue;

			var min = variable.Statistics.MinimumValue;
			var max = variable.Statistics.MaximumValue;

			//Color range (Continuous only)
			double color_min, color_max;
			if (variable.DataType == VoxelVariableDataType.Continuous)
			{
				var renderer = variable.Renderer as CIMVoxelStretchRenderer;
				color_min = renderer.ColorRangeMin;
				color_max = renderer.ColorRangeMax;
			}

			#endregion
		}

		public void Example7()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMax
			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMin
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MinimumValue
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MAximumValue
			#region Change Stretch Renderer Color Range

			//Typically, the default color range covers the most
			//commonly occuring voxel values. Usually, the data
			//range is much broader than the color range

			//Get the variable profile whose renderer will be changed
			var variable = voxelLayer.SelectedVariableProfile;

			//Check DataType
			if (variable.DataType != VoxelVariableDataType.Continuous)
				return;//must be continuous to have a Stretch Renderer...

			var renderer = variable.Renderer as CIMVoxelStretchRenderer;
			var color_min = renderer.ColorRangeMin;
			var color_max = renderer.ColorRangeMax;
			//increase range by 10% of the current difference
			var dif = (color_max - color_min) * 0.05;
			color_min -= dif;
			color_max += dif;

			//make sure we do not exceed data range
			//At 2.x - 
			//if (color_min < variable.GetVariableStatistics().MinimumValue)
			//	color_min = variable.GetVariableStatistics().MinimumValue;
			//if (color_max > variable.GetVariableStatistics().MaximumValue)
			//	color_max = variable.GetVariableStatistics().MaximumValue;

			if (color_min < variable.Statistics.MinimumValue)
				color_min = variable.Statistics.MinimumValue;
			if (color_max > variable.Statistics.MaximumValue)
				color_max = variable.Statistics.MaximumValue;

			//variable.Statistics.MinimumValue
			renderer.ColorRangeMin = color_min;
			renderer.ColorRangeMax = color_max;

			//apply changes
			variable.SetRenderer(renderer);

			#endregion
		}

		public void Example7b()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer
			// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer.Classes
			// cref: ArcGIS.Core.CIM.CIMVoxelColorUniqueValue
			// cref: ArcGIS.Core.CIM.CIMVoxelColorUniqueValue.Visible
			#region Change The Visibility on a CIMVoxelColorUniqueValue class

			//Get the variable profile whose renderer will be changed
			var variable = voxelLayer.SelectedVariableProfile;

			//Check DataType
			if (variable.DataType != VoxelVariableDataType.Discrete)
				return;//must be Discrete to have a UV Renderer...

			var renderer = variable.Renderer as CIMVoxelUniqueValueRenderer;

			//A CIMVoxelUniqueValueRenderer consists of a collection of
			//CIMVoxelColorUniqueValue classes - one per discrete value
			//in the associated variable profile value array

			//Get the first class
			var classes = renderer.Classes.ToList();
			var unique_value_class = classes.First();
			//Set its visibility off
			unique_value_class.Visible = false;

			//Apply the change to the renderer
			renderer.Classes = classes.ToArray();
			//apply the changes
			variable.SetRenderer(renderer);

			#endregion
		}


		#region ProSnippet Group: IsoSurfaces
		#endregion

		public void Example3()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.MaxNumberOfIsosurfaces
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
			#region Check the MaxNumberofIsoSurfaces for a Variable

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable = voxelLayer.GetVariableProfiles().First();
			var max = variable.MaxNumberOfIsosurfaces;
			if (max >= variable.GetIsosurfaces().Count)
			{
				//no more surfaces can be created on this variable
			}

			#endregion
		}

		public void Example3b()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
			#region Check a Variable's Datatype

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable = voxelLayer.GetVariableProfiles().First();
			if (variable.DataType != VoxelVariableDataType.Continuous)
			{
				//No iso surfaces
				//Iso surface can only be created for VoxelVariableDataType.Continuous
			}

			#endregion
		}

		public void Example5()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.Visualization
			// cref: ArcGIS.Core.CIM.VoxelVisualization
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
			#region Check CanCreateIsoSurface

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Visualization must be surface or CanCreateIsosurface will return
			//false
			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);

			//Get the variable profile on which to create the iso surface
			var variable = voxelLayer.SelectedVariableProfile;
			//or use ...voxelLayer.GetVariableProfiles().First(....

			// o Visualization must be Surface
			// o Variable profile must be continuous
			// o Variable MaxNumberofIsoSurfaces must not have been reached...
			if (variable.CanCreateIsosurface)
			{
				//Do the create
			}

			#endregion
		}

		public void Example6()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.Visualization
			// cref: ArcGIS.Core.CIM.VoxelVisualization
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CreateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Value
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsVisible
			#region Create Isosurface

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Visualization must be surface
			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);

			//Get the variable profile on which to create the iso surface
			var variable = voxelLayer.SelectedVariableProfile;

			// o Visualization must be Surface
			// o Variable profile must be continuous
			// o Variable MaxNumberofIsoSurfaces must not have been reached...
			if (variable.CanCreateIsosurface)
			{
				//Note: calling create if variable.CanCreateIsosurface == false
				//will trigger an InvalidOperationException

				//Specify a voxel value for the iso surface
				//At 2.x - 
				//var min = variable.GetVariableStatistics().MinimumValue;
				//var max = variable.GetVariableStatistics().MaximumValue;

				var min = variable.Statistics.MinimumValue;
				var max = variable.Statistics.MaximumValue;
				var mid = (max + min) / 2;

				//color range (i.e. values that are being rendered)
				var renderer = variable.Renderer as CIMVoxelStretchRenderer;
				var color_min = renderer.ColorRangeMin;
				var color_max = renderer.ColorRangeMax;

				//keep the surface within the current color range (or it
				//won't render)
				if (mid < color_min)
				{
					mid = renderer.ColorRangeMin;
				}
				else if (mid > color_max)
				{
					mid = renderer.ColorRangeMax;
				}

				//Create the iso surface
				var suffix = Math.Truncate(mid * 100) / 100;
				variable.CreateIsosurface(new IsosurfaceDefinition()
				{
					Name = $"Surface {suffix}",
					Value = mid,
					IsVisible = true
				});
			}

			#endregion
		}

		public void Example7c()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Value
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Color
			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsCustomColor
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.UpdateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
			// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRamp
			// cref: ArcGIS.Desktop.Mapping.ColorFactory.GenerateColorsFromColorRamp
			#region How to Change Value and Color on an Isosurface

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable = voxelLayer.SelectedVariableProfile;

			//Change the color of the first surface for the given profile
			var surface = variable.GetIsosurfaces().FirstOrDefault();
			if (surface != null)
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);

				//Change the iso surface voxel value
				surface.Value = surface.Value * 0.9;

				//get a random color
				var count = new Random().Next(0, 100);
				var colors = ColorFactory.Instance.GenerateColorsFromColorRamp(
					((CIMVoxelStretchRenderer)variable.Renderer).ColorRamp, count);

				var idx = new Random().Next(0, count - 1);
				surface.Color = colors[idx];
				//set the custom color flag true to lock the color
				//locking the color prevents it from being changed if the
				//renderer color range or color theme is updated
				surface.IsCustomColor = true;

				//update the surface
				variable.UpdateIsosurface(surface);
			}

			#endregion

			// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsCustomColor
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaceColor(System.Double)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.UpdateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
			#region Change Isourface Color Back to Default

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()
			//var variable = ...;
			//var surface = ...;

			if (surface.IsCustomColor)
			{
				surface.Color = variable.GetIsosurfaceColor((double)surface.Value);
				surface.IsCustomColor = false;
				//update the surface
				variable.UpdateIsosurface(surface);
			}
			#endregion
		}

		public void Example7d()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DeleteIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
			#region Delete Isosurface- s

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var variable = voxelLayer.SelectedVariableProfile;

			//delete the last surface
			var last_surface = variable.GetIsosurfaces().LastOrDefault();

			if (last_surface != null)
			{
				variable.DeleteIsosurface(last_surface);
			}

			//delete all the surfaces
			foreach (var surface in variable.GetIsosurfaces())
				variable.DeleteIsosurface(surface);

			//Optional - set visualization back to Volume
			if (variable.GetIsosurfaces().Count() == 0)
			{
				voxelLayer.SetVisualization(VoxelVisualization.Volume);
			}

			#endregion
		}

		#region ProSnippet Group: Slices
		#endregion

		public void Example8()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Volume
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.IsVisible
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
			#region Get the Collection of Slices

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//At 2.x - var slices = voxelLayer.GetSlices();

			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var slices = volume.GetSlices();

			//Do something... e.g. make them visible
			foreach (var slice in slices)
			{
				slice.IsVisible = true;
				//at 2.x - voxelLayer.UpdateSlice(slice);
				volume.UpdateSlice(slice);
			}

			//expand the slice container and make sure container visibility is true
			voxelLayer.SetSliceContainerExpanded(true);
			voxelLayer.SetSliceContainerVisibility(true);

			#endregion
		}

		public void Example8g()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;
			var my_slice_id = "";

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Volume
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
			#region Get a Slice

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//At 2.x -
			//var slice = voxelLayer.GetSlices().FirstOrDefault();
			//var slice2 = voxelLayer.GetSlices().First(s => s.Id == my_slice_id);

			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var slice = volume.GetSlices().FirstOrDefault();
			var slice2 = volume.GetSlices().First(s => s.Id == my_slice_id);

			#endregion
		}


		public void Example8h()
		{

      // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
      // cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
      #region Get Selected Slice in TOC

      //Must be on the QueuedTask.Run()

      var slice = MapView.Active?.GetSelectedSlices()?.FirstOrDefault();
			if (slice != null)
			{

			}

			#endregion
		}

		public void Example8i()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Layer
			#region Get Voxel Layer for the Selected Slice in TOC

			//Must be on the QueuedTask.Run()

			VoxelLayer voxelLayer = null;
			var slice = MapView.Active?.GetSelectedSlices()?.FirstOrDefault();
			if (slice != null)
			{
				voxelLayer = slice.Layer;
				//TODO - use the layer
			}

			#endregion
		}


		public void Example8b()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.AutoShowExploreDockPane
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.#ctor
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.IsVisible
			#region Create a Slice

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Volume)
				voxelLayer.SetVisualization(VoxelVisualization.Volume);
			voxelLayer.SetSliceContainerExpanded(true);
			voxelLayer.SetSliceContainerVisibility(true);

			//To stop the Voxel Exploration Dockpane activating use:
			voxelLayer.AutoShowExploreDockPane = false;
			//This is useful if u have your own dockpane currently activated...

			//At 2.x - var volumeSize = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//Orientation 90 degrees (West), Tilt 0.0 (vertical)
			//Convert to a normal
			var normal = voxelLayer.GetNormal(90, 0.0);

			//Create the slice at the voxel mid-point. VoxelPosition
			//is specified in voxel-space coordinates

			//At 2.x - 
			//voxelLayer.CreateSlice(new SliceDefinition()
			//{
			//	Name = "Middle Slice",
			//	VoxelPosition = new Coordinate3D(volume.Item1 / 2, volume.Item2 / 2, volume.Item3 / 2),
			//	Normal = normal,
			//	IsVisible = true
			//});

			//Create the slice on the respective volume
			volume.CreateSlice(new SliceDefinition()
			{
				Name = "Middle Slice",
				VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
				Normal = normal,
				IsVisible = true
			});

			//reset if needed...
			voxelLayer.AutoShowExploreDockPane = true;

			#endregion
		}

		public void Example8c()
		{
			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetOrientationAndTilt(ArcGIS.Desktop.Mapping.Voxel.Coordinate3D)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
			#region Change Tilt on a Slice

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//To stop the Voxel Exploration Dockpane activating use:
			voxelLayer.AutoShowExploreDockPane = false;
			//This is useful if u have your own dockpane currently activated...
			//Normally, it would be set in your dockpane

			if (voxelLayer.Visualization != VoxelVisualization.Volume)
				voxelLayer.SetVisualization(VoxelVisualization.Volume);
			voxelLayer.SetSliceContainerVisibility(true);

			//At 2.x - var slice = voxelLayer.GetSlices().First(s => s.Name == "Change Tilt Slice");

			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var slice = volume.GetSlices().First(s => s.Name == "Change Tilt Slice");

			(double orientation, double tilt) = voxelLayer.GetOrientationAndTilt(slice.Normal);

			//Convert orientation and tilt to a normal
			slice.Normal = voxelLayer.GetNormal(orientation, 45.0);
			//At 2.x - voxelLayer.UpdateSlice(slice);
			volume.UpdateSlice(slice);

			//reset if needed...Normally this might be when your dockpane
			//was de-activated (ie "closed")
			voxelLayer.AutoShowExploreDockPane = true;

			#endregion
		}


		public void Example8d()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.DeleteSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
			#region Delete Slice

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//At 2.x
			//var last_slice = voxelLayer.GetSlices().LastOrDefault();
			//   if (last_slice != null)
			//     voxelLayer.DeleteSlice(last_slice);

			//   //Delete all slices
			//   var slices = voxelLayer.GetSlices();
			//   foreach (var slice in slices)
			//     voxelLayer.DeleteSlice(slice);

			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			var last_slice = volume.GetSlices().LastOrDefault();
			if (last_slice != null)
				volume.DeleteSlice(last_slice);

			//Delete all slices
			var slices = volume.GetSlices();
			foreach (var slice in slices)
				volume.DeleteSlice(slice);

			#endregion
		}

		#region ProSnippet Group: Sections
		#endregion


		public void Example9h()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;
			var my_section_id = "";

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
			#region Get a Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//At 2.x -
			//var section = voxelLayer.GetSections().FirstOrDefault();
			//var section2 = voxelLayer.GetSections().First(sec => sec.Id == my_section_id);

			//Use the SelectedVariableProfile to get the sections currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			var section = volume.GetSections().FirstOrDefault();
			var section2 = volume.GetSections().First(sec => sec.ID == my_section_id);

			#endregion
		}

		public void Example9g()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
			#region Get the Current Collection of Sections

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//At 2.x - var sections = voxelLayer.GetSections();

			//Use the SelectedVariableProfile to get the sections currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var sections = volume.GetSections();


			#endregion
		}

		public void Example9i()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
			#region Get the Selected Section in TOC

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var section = MapView.Active?.GetSelectedSections()?.FirstOrDefault();
			if (section != null)
			{

			}

			#endregion
		}

		public void Example9j()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Layer
			#region Get Voxel Layer for the Selected Section in TOC

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			VoxelLayer voxelLayer = null;
			var section = MapView.Active?.GetSelectedSections()?.FirstOrDefault();
			if (section != null)
			{
				voxelLayer = section.Layer;
				//TODO - use the layer
			}

			#endregion
		}

		public void Example9()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.AutoShowExploreDockPane
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
			#region Create a Section at the Voxel MidPoint

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//To stop the Voxel Exploration Dockpane activating use:
			voxelLayer.AutoShowExploreDockPane = false;
			//This is useful if u have your own dockpane currently activated...
			//Normally, it would be set in your dockpane

			//Create a section that cuts the volume in two on the vertical plane

			//At 2.x - var volume = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//Orientation 90 degrees (due West), Tilt 0 degrees
			var normal = voxelLayer.GetNormal(90, 0.0);

			//Position must be specified in voxel space

			//At 2.x -
			//voxelLayer.CreateSection(new SectionDefinition()
			//{
			//	Name = "Middle Section",
			//	VoxelPosition = new Coordinate3D(volume.Item1 / 2, volume.Item2 / 2, volume.Item3 / 2),
			//	Normal = normal,
			//	IsVisible = true
			//});
			volume.CreateSection(new SectionDefinition()
			{
				Name = "Middle Section",
				VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
				Normal = normal,
				IsVisible = true
			});

			//reset if needed...Normally this might be when your dockpane
			//was de-activated (ie "closed")
			voxelLayer.AutoShowExploreDockPane = true;

			#endregion
		}

		public void Example9a()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.CreateHorizontalSectionDefinition()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			#region Create a Horizontal Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//Create a section that cuts the volume in two on the horizontal plane
			
			//At 2.x - var volumeSize = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//Or use normal (0, 0, 1) or (0, 0, -1)...
			var horz_section = SectionDefinition.CreateHorizontalSectionDefinition();

			horz_section.Name = "Horizontal Section";
			horz_section.IsVisible = true;
			horz_section.VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2);

			//At 2.x - voxelLayer.CreateSection(horz_section);
			volume.CreateSection(horz_section);

			#endregion
		}

		public void Example9b()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
			#region Create Sections in a Circle Pattern

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//At 2.x - var volumeSize = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//180 degrees orientation is due South. 90 degrees orientation is due west.
			var south = 180.0;
			var num_sections = 12;
			var spacing = 1 / (double)num_sections;

			//Create a section every nth degree of orientation. Each section
			//bisects the middle of the voxel
			for (int s = 0; s < num_sections; s++)
			{
				var orientation = south * (s * spacing);
				//At 2.x -
				//voxelLayer.CreateSection(new SectionDefinition()
				//{
				//	Name = $"Circle {s + 1}",
				//	VoxelPosition = new Coordinate3D(volumeSize.Item1 / 2, volumeSize.Item2 / 2, volumeSize.Item3 / 2),
				//	Normal = voxelLayer.GetNormal(orientation, 0.0),
				//	IsVisible = true
				//});

				volume.CreateSection(new SectionDefinition()
				{
					Name = $"Circle {s + 1}",
					VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
					Normal = voxelLayer.GetNormal(orientation, 0.0),
					IsVisible = true
				});
			}

			#endregion
		}

		public void Example9c()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
			#region Create Sections that Bisect the Voxel

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//At 2.x - var volumeSize = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//Make three Normals - each is a Unit Vector (x, y, z)
			var north_south = new Coordinate3D(1, 0, 0);
			var east_west = new Coordinate3D(0, 1, 0);
			var horizontal = new Coordinate3D(0, 0, 1);

			int n = 0;
			//The two verticals bisect the x,y plane. The horizontal normal bisects
			//the Z plane.
			foreach (var normal in new List<Coordinate3D> { north_south, east_west, horizontal })
			{
				//At 2.x -
				//voxelLayer.CreateSection(new SectionDefinition()
				//{
				//	Name = $"Cross {++n}",
				//	VoxelPosition = new Coordinate3D(volumeSize.Item1 / 2, volumeSize.Item2 / 2, volumeSize.Item3 / 2),
				//	Normal = normal,
				//	IsVisible = true
				//});

				volume.CreateSection(new SectionDefinition()
				{
					Name = $"Cross {++n}",
					VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
					Normal = normal,
					IsVisible = true
				});
			}

			#endregion
		}

		public void Example9d()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
			#region Create Sections Diagonally across the Voxel

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//At 2.x - var volumeSize = voxelLayer.GetVolumeSize();

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var volumeSize = volume.GetVolumeSize();

			//make a diagonal across the voxel
			var voxel_pos = new Coordinate3D(0, 0, volumeSize.Z);
			var voxel_pos_ur = new Coordinate3D(volumeSize.X, volumeSize.Y, volumeSize.Z);

			var lineBuilder = new LineBuilderEx(voxel_pos, voxel_pos_ur, null);
			var diagonal = PolylineBuilderEx.CreatePolyline(lineBuilder.ToSegment());

			var num_sections = 12;
			var spacing = 1 / (double)num_sections;

			//change as needed
			var orientation = 20.0; //(approx NNW)
			var tilt = -15.0;

			var normal = voxelLayer.GetNormal(orientation, tilt);

			for (int s = 0; s < num_sections; s++)
			{
				Coordinate2D end_pt = new Coordinate2D(0, 0);
				if (s > 0)
				{
					//position each section evenly spaced along the diagonal
					var segments = new List<Segment>() as ICollection<Segment>;
					var part = GeometryEngine.Instance.GetSubCurve3D(
							diagonal, 0.0, s * spacing, AsRatioOrLength.AsRatio);
					part.GetAllSegments(ref segments);
					end_pt = segments.First().EndCoordinate;
				}

				//At 2.x -
				//voxelLayer.CreateSection(new SectionDefinition()
				//{
				//	Name = $"Diagonal {s + 1}",
				//	VoxelPosition = new Coordinate3D(end_pt.X, end_pt.Y, volumeSize.Item3),
				//	Normal = normal,
				//	IsVisible = true
				//});

				volume.CreateSection(new SectionDefinition()
				{
					Name = $"Diagonal {s + 1}",
					VoxelPosition = new Coordinate3D(end_pt.X, end_pt.Y, volumeSize.Z),
					Normal = normal,
					IsVisible = true
				});
			}

			#endregion
		}

		public void Example9e()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			#region Update Section Orientation and Tilt

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			//At 2.x - foreach (var section in voxelLayer.GetSections())
			foreach (var section in volume.GetSections())
			{
				//set each normal to 45.0 orientation and tilt
				section.Normal = voxelLayer.GetNormal(45.0, 45.0);
				//apply the change
				//At 2.x - voxelLayer.UpdateSection(section);
				volume.UpdateSection(section);
			}

			#endregion
		}

		public void Example9k()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			#region Update Section Visibility

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);

			//At 2.x - var sections = voxelLayer.GetSections().Where(s => !s.IsVisible);

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var sections = volume.GetSections().Where(s => !s.IsVisible);

			//Make them all visible
			foreach (var section in sections)
			{
				section.IsVisible = true;
				//apply the change
				//At 2.x - voxelLayer.UpdateSection(section);
				volume.UpdateSection(section);
			}

			#endregion
		}

		public void Example9f()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.DeleteSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			#region Delete Sections

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			//At 2.x - foreach (var section in voxelLayer.GetSections())
			//						voxelLayer.DeleteSection(section);
			foreach (var section in volume.GetSections())
				volume.DeleteSection(section);

			//optional...
			if (voxelLayer.Visualization != VoxelVisualization.Volume)
				voxelLayer.SetVisualization(VoxelVisualization.Volume);

			#endregion
		}

		#region ProSnippet Group: Locked Sections
		#endregion

		public void Example10()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
			#region Get the Current Collection of Locked Sections

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetLockedSectionContainerExpanded(true);
			voxelLayer.SetLockedSectionContainerVisibility(true);

			var locked_sections = voxelLayer.GetLockedSections();

			#endregion
		}

		public void Example10a()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;
			var my_locked_section_id = -1;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
			#region Get a Locked Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var locked_section = voxelLayer.GetLockedSections().FirstOrDefault();
			var locked_section2 = voxelLayer.GetLockedSections()
											.First(lsec => lsec.ID == my_locked_section_id);

			#endregion
		}

		public void Example10b()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
			#region Get Selected Locked Section in TOC

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
			if (locked_section != null)
			{

			}

			#endregion
		}

		public void Example10c()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.Layer
			#region Get Voxel Layer for the Selected Locked Section in TOC

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			VoxelLayer voxelLayer = null;
			var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
			if (locked_section != null)
			{
				voxelLayer = locked_section.Layer;
				//TODO - use the layer
			}

			#endregion
		}

		public void Example10d()
		{

			Map map = MapView.Active.Map;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.Layer
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.VariableName
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfile(System.String)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSelectedVariableProfile(ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile)
			#region Set the Variable Profile Active for Selected Locked Section

			//Must be on the QueuedTask.Run()

			var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
			if (locked_section != null)
			{
				var variable = locked_section.Layer.GetVariableProfile(locked_section.VariableName);
				locked_section.Layer.SetSelectedVariableProfile(variable);
			}

			#endregion
		}

		public void Example10e()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CanLockSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.LockSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
			#region Lock a Section/"Create" a Locked Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetLockedSectionContainerExpanded(true);
			voxelLayer.SetLockedSectionContainerVisibility(true);

			//Use the SelectedVariableProfile to get the sections
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			//get the selected section
			var section = MapView.Active.GetSelectedSections().FirstOrDefault();
			if (section == null)
      {
				//At 2.x - section = voxelLayer.GetSections().FirstOrDefault();
				section = volume.GetSections().FirstOrDefault();
			}

			if (section == null)
				return;

			//Lock the section (Creates a locked section, deletes
			//the section)
			//if (voxelLayer.CanLockSection(section))
			//	voxelLayer.LockSection(section);
			if (volume.CanLockSection(section))
				volume.LockSection(section);

				#endregion
		}

		public void Example10g()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
			// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.IsVisible
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
			#region Update Locked Section Visibility

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetLockedSectionContainerExpanded(true);
			voxelLayer.SetLockedSectionContainerVisibility(true);

			var locked_sections = voxelLayer.GetLockedSections().Where(ls => !ls.IsVisible);

			//Make them visible
			foreach (var locked_section in locked_sections)
			{
				locked_section.IsVisible = true;
				//apply change
				voxelLayer.UpdateSection(locked_section);
			}

			#endregion
		}


		public void Example10f()
		{

			Map map = MapView.Active.Map;
			VoxelLayer voxelLayer = null;

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.CanUnlockSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.UnlockSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
			#region Unlock a Locked Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			voxelLayer.SetSectionContainerExpanded(true);
			voxelLayer.SetSectionContainerVisibility(true);
			voxelLayer.SetLockedSectionContainerExpanded(true);

			//get the selected locked section
			var locked_section = MapView.Active.GetSelectedLockedSections().FirstOrDefault();
			if (locked_section == null)
				locked_section = voxelLayer.GetLockedSections().FirstOrDefault();
			if (locked_section == null)
				return;

			//Unlock the locked section (Deletes the locked section, creates
			//a section)
			if (voxelLayer.CanUnlockSection(locked_section))
				voxelLayer.UnlockSection(locked_section);

			#endregion

			// cref: ArcGIS.Desktop.Mapping.VoxelLayer.DeleteSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
			#region Delete a Locked Section

			//var voxelLayer = ... ;
			//Must be on the QueuedTask.Run()

			if (voxelLayer.GetLockedSections().Count() == 0)
				return;

			//Delete the last locked section from the collection of
			//locked sections
			voxelLayer.DeleteSection(voxelLayer.GetLockedSections().Last());

			#endregion
		}

	}
}
