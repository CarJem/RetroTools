using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RetroLayers.Classes;
using RetroLayers.Converters;
using RetroLayers.Classes.v4;
using RetroLayers.Classes.v5;

namespace RetroLayers.Controls
{
	/// <summary>
	/// Interaction logic for Editor_v4.xaml
	/// </summary>
	public partial class Editor_v4 : UserControl, IEditor
	{
		#region View Model
		public class ViewModel : INotifyPropertyChanged
		{
			#region Collections
			private IList<EditorLayer_v4> _Layers { get; set; }
			private IList<ScrollInfo_v4> _HLines { get; set; }
			private IList<ScrollInfo_v4> _VLines { get; set; }
			public IList<EditorLayer_v4> Layers
			{
				get
				{
					return _Layers;
				}
				set
				{
					_Layers = value;
					NotifyPropertyChanged("Layers");
				}
			}
			public IList<ScrollInfo_v4> HLines
			{
				get
				{
					return _HLines;
				}
				set
				{
					_HLines = value;
					NotifyPropertyChanged("HLines");
				}
			}
			public IList<ScrollInfo_v4> VLines
			{
				get
				{
					return _VLines;
				}
				set
				{
					_VLines = value;
					NotifyPropertyChanged("VLines");
				}
			}
			#endregion

			#region Current Items
			public EditorLayer_v4 CurrentLayer
			{
				get
				{
					return GetCurrentLayer();
				}
				set
				{
					SetCurrentLayer(value);
					NotifyPropertyChanged("CurrentLayer");
				}
			}
			public ScrollInfo_v4 CurrentHorizontalLayerScroll
			{
				get
				{
					return GetCurrentHorizontalLayerScroll();
				}
				set
				{
					SetCurrentHorizontalLayerScroll(value);
					NotifyPropertyChanged("CurrentHorizontalLayerScroll");
				}
			}
			public ScrollInfo_v4 CurrentVerticalLayerScroll
			{
				get
				{
					return GetCurrentVerticalLayerScroll();
				}
				set
				{
					SetCurrentVerticalLayerScroll(value);
					NotifyPropertyChanged("CurrentVerticalLayerScroll");
				}
			}
			#endregion

			#region Indexes
			private int _SelectedLayerIndex { get; set; } = 0;
			private int _SelectedHorizontalIndex { get; set; } = 0;
			private int _SelectedVerticalIndex { get; set; } = 0;
			private int _SelectedLayerLineIndex { get; set; } = 0;

			public int SelectedLayerIndex
			{
				get
				{
					return _SelectedLayerIndex;
				}
				set
				{
					_SelectedLayerIndex = value;
					UpdateSection(true, false, false, true);
				}
			}
			public int SelectedHorizontalIndex
			{
				get
				{
					return _SelectedHorizontalIndex;
				}
				set
				{
					_SelectedHorizontalIndex = value;
					UpdateSection(false, true, false, false);
				}
			}
			public int SelectedVerticalIndex
			{
				get
				{
					return _SelectedVerticalIndex;
				}
				set
				{
					_SelectedVerticalIndex = value;
					UpdateSection(false, false, true, false);
				}
			}
			public int SelectedLayerLineIndex
			{
				get
				{
					return _SelectedLayerLineIndex;
				}
				set
				{
					_SelectedLayerLineIndex = value;
					UpdateSection(false, false, false, true);
				}
			}


			#endregion

			#region Index Helpers

			public bool OutsideOfLayerBounds
			{
				get
				{
					if (Layers == null) return true;
					return (Layers.Count - 1 < SelectedLayerIndex || SelectedLayerIndex < 0);
				}
			}
			public bool OutsideOfHorizontalRuleBounds
			{
				get
				{
					if (HLines == null) return true;
					return (HLines.Count - 1 < SelectedHorizontalIndex || SelectedHorizontalIndex < 0);
				}
			}
			public bool OutsideOfVerticalRuleBounds
			{
				get
				{
					if (VLines == null) return true;
					return (VLines.Count - 1 < SelectedVerticalIndex || SelectedVerticalIndex < 0);
				}
			}

			#endregion

			#region Current Helper Methods
			private EditorLayer_v4 GetCurrentLayer()
			{
				if (OutsideOfLayerBounds) return null;
				return Layers[SelectedLayerIndex];
			}
			private ScrollInfo_v4 GetCurrentHorizontalLayerScroll()
			{
				if (OutsideOfHorizontalRuleBounds) return null;
				return HLines[SelectedHorizontalIndex];
			}
			private ScrollInfo_v4 GetCurrentVerticalLayerScroll()
			{
				if (OutsideOfVerticalRuleBounds) return null;
				else return VLines[SelectedVerticalIndex];

			}
			private void SetCurrentLayer(EditorLayer_v4 value)
			{
				if (OutsideOfLayerBounds) return;
				else Layers[SelectedLayerIndex] = value;

			}
			private void SetCurrentHorizontalLayerScroll(ScrollInfo_v4 value)
			{
				if (OutsideOfHorizontalRuleBounds) return;
				else HLines[SelectedHorizontalIndex] = value;
			}
			private void SetCurrentVerticalLayerScroll(ScrollInfo_v4 value)
			{
				if (OutsideOfVerticalRuleBounds) return;
				else VLines[SelectedVerticalIndex] = value;
			}

			#endregion

			#region Update Helper Methods

			private void UpdateSection(bool UpdateLayerProps, bool UpdateHorizontalProps, bool UpdateVerticalProps, bool UpdateLayerIndexesProps)
			{
				if (UpdateLayerProps)
				{
					NotifyPropertyChanged("SelectedLayerIndex");
					NotifyPropertyChanged("CurrentLayer");
				}
				if (UpdateHorizontalProps)
				{
					NotifyPropertyChanged("SelectedHorizontalIndex");
					NotifyPropertyChanged("CurrentHorizontalLayerScroll");
				}
				if (UpdateVerticalProps)
				{
					NotifyPropertyChanged("SelectedVerticalIndex");
					NotifyPropertyChanged("CurrentVerticalLayerScroll");
				}
				if (UpdateLayerIndexesProps)
				{
					NotifyPropertyChanged("SelectedLayerLineIndex");
				}
			}

			#endregion

			#region INotifyPropertyChanged Properties

			public event PropertyChangedEventHandler PropertyChanged;

			protected void NotifyPropertyChanged(String info)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(info));
				}
			}

			#endregion
		}

		public ViewModel CurrentDataContext
		{
			get
			{
				return (this.DataContext as ViewModel);
			}
			set
			{
				this.DataContext = value;
			}
		}

		#endregion

		#region Base Definitions

		private EditorClass_v4 EditorScene;

		#endregion

		#region Index Definitions

		public int SelectedLayerLineIndex
        {
			get
            {
				return CurrentDataContext.SelectedLayerLineIndex;

			}
        }

		private int SelectedLayerIndex
		{
			get
			{
				return CurrentDataContext.SelectedLayerIndex;
			}
		}
		private int SelectedLayerVerticalIndex
		{
			get
			{
				return CurrentDataContext.SelectedVerticalIndex;
			}
		}
		private int SelectedLayerHorizontalIndex
		{
			get
			{
				return CurrentDataContext.SelectedHorizontalIndex;
			}
		}

		#endregion

		#region Definitions
		private bool HasLayerArangementChanged { get; set; } = false;
		private bool isLoaded { get; set; } = true;


		#endregion

		#region Init
		public Editor_v4()
		{
			InitializeComponent();

			lbLayers.SelectionChanged += OversizeTextUpdater;
			lbHorizontalRules.SelectionChanged += OversizeTextUpdater;
			lbVerticalRules.SelectionChanged += OversizeTextUpdater;

			Unload();
		}

		public void Load(IEditorClass scene)
		{
			EditorScene = (EditorClass_v4)scene;
			CurrentDataContext = new ViewModel();
			InitDataBindings(EditorScene);
		}

		public void Unload()
		{
			EditorScene = null;
			CurrentDataContext = null;
		}

		public void Save(string filePath = null)
		{
			EditorScene.Save(filePath);
		}



		private void InitDataBindings(EditorClass_v4 Scene)
		{

			BindControl(lbLayers, ListBox.SelectedIndexProperty, "SelectedLayerIndex");
			BindControl(lbHorizontalRules, ListBox.SelectedIndexProperty, "SelectedLayerHorizontalIndex");
			BindControl(lbVerticalRules, ListBox.SelectedIndexProperty, "SelectedLayerVerticalIndex");
			BindControl(LayerLineIndexes, ListBox.SelectedIndexProperty, "SelectedLayerLineIndex");

			BindControl(lbLayers, ListBox.ItemsSourceProperty, "Layers");
			BindControl(lbHorizontalRules, ListBox.ItemsSourceProperty, "HLines");
			BindControl(lbVerticalRules, ListBox.ItemsSourceProperty, "VLines");

			lbLayers.Items.Refresh();
			lbHorizontalRules.Items.Refresh();
			lbVerticalRules.Items.Refresh();
			LayerLineIndexes.Items.Refresh();


			/// Layers

			CurrentDataContext.Layers = Scene.Editor_Layers;
			CurrentDataContext.HLines = Scene.Editor_HLines;
			CurrentDataContext.VLines = Scene.Editor_VLines;

			BindControl(LayerLineIndexes, ListBox.ItemsSourceProperty, "CurrentLayer.LineIndexes");

			BindControlOneWay(lblRawWidthValue, Label.ContentProperty, "CurrentLayer.Width");
			BindControlOneWay(lblRawHeightValue, Label.ContentProperty, "CurrentLayer.Height");

			BindControl(nudWidth, Xceed.Wpf.Toolkit.UShortUpDown.ValueProperty, "CurrentLayer.WorkingWidth");
			BindControl(nudHeight, Xceed.Wpf.Toolkit.UShortUpDown.ValueProperty, "CurrentLayer.WorkingHeight");

			BindControlEffictiveSize(lblEffSizeWidth, Label.ContentProperty, "CurrentLayer.Width");
			BindControlEffictiveSize(lblEffSizeHeight, Label.ContentProperty, "CurrentLayer.Height");

			BindControl(LayerBehaviorNUD, Xceed.Wpf.Toolkit.ByteUpDown.ValueProperty, "CurrentLayer.Behaviour");
			BindControl(LayerRelativeSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentLayer.RelativeSpeed");
			BindControl(LayerConstantSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentLayer.ConstantSpeed");


			/// Horizontal Rules
			BindControl(HorizontalBehaviorNUD, Xceed.Wpf.Toolkit.ByteUpDown.ValueProperty, "CurrentHorizontalLayerScroll.Behaviour");
			BindControl(HorizontalRelativeSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentHorizontalLayerScroll.RelativeSpeed");
			BindControl(HorizontalConstantSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentHorizontalLayerScroll.ConstantSpeed");

			/// Vertical Rules
			BindControl(VerticalBehaviorNUD, Xceed.Wpf.Toolkit.ByteUpDown.ValueProperty, "CurrentVerticalLayerScroll.Behaviour");
			BindControl(VerticalRelativeSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentVerticalLayerScroll.RelativeSpeed");
			BindControl(VerticalConstantSpeedNUD, Xceed.Wpf.Toolkit.ShortUpDown.ValueProperty, "CurrentVerticalLayerScroll.ConstantSpeed");
		}
		#endregion

		#region Layer Manipulation
		private void ResizeLayer()
		{
			/*
			if (MessageBox.Show(@"Resizing a layer can not be undone. You really should save what you have and take a backup first. Proceed with the resize?", "Caution!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				var layer = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
				layer.Resize(Convert.ToUInt16(nudWidth.Value), Convert.ToUInt16(nudHeight.Value));
				UpdateListDetails();
			}
			*/
		}

		private void MoveLayerUp()
		{
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			if (current == null) return;

			int index = lbLayers.SelectedIndex;
			if (index == 0) return;
			CurrentDataContext.Layers.Remove(current);
			CurrentDataContext.Layers.Insert(--index, current);
			lbLayers.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();

		}
		private void MoveLayerDown()
		{
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			if (current == null) return;

			int index = lbLayers.SelectedIndex;
			if (index == CurrentDataContext.Layers.Count - 1) return;
			CurrentDataContext.Layers.Remove(current);
			CurrentDataContext.Layers.Insert(++index, current);
			lbLayers.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();
		}
		private void AddLayer()
		{
			/*
			EditorLayer_v5 newEditorLayer = EditorScene.ProduceLayer();
			CurrentDataContext.Layers.Add(newEditorLayer);
			int newIndex = CurrentDataContext.Layers.IndexOf(newEditorLayer);
			lbLayers.SelectedIndex = newIndex;
			HasLayerArangementChanged = true;
			UpdateListDetails();
			*/

		}
		private void DeleteLayer()
		{
			/*
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			if (null == current) return;

			if (MessageBox.Show($@"Deleting a layer can not be undone! Are you sure you want to delete the [{current.Name}] layer?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				EditorScene.DeleteLayer(current);
				HasLayerArangementChanged = true;
				UpdateListDetails();
			}
			*/
		}
		private void CutLayer()
		{
			/*
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			if (null == current) return;
			CopyLayerToClipboard(current);
			CurrentDataContext.Layers.Remove(current);
			HasLayerArangementChanged = true;
			UpdateListDetails();
			*/

		}
		private void CopyLayer()
		{
						/*
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			if (null == current) return;
			CopyLayerToClipboard(current);
			UpdateListDetails();
						*/
		}
		private void DuplicateLayer()
		{
			/*
			var current = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex) as EditorLayer_v5;
			if (null == current) return;
			CurrentDataContext.Layers.Insert(SelectedLayerIndex, current);
			HasLayerArangementChanged = true;
			UpdateListDetails();
			*/

		}
		private void PasteLayer()
		{
			PasteLayerFromClipboard();
			UpdateListDetails();
		}

		#endregion

		#region Horizontal Mapping Manipulation
		private void MoveVerticalMappingUp()
		{
			var current = CurrentDataContext.VLines.ElementAt(SelectedLayerVerticalIndex);
			if (current == null) return;

			int index = lbVerticalRules.SelectedIndex;
			if (index == 0) return;
			CurrentDataContext.VLines.Remove(current);
			CurrentDataContext.VLines.Insert(--index, current);
			lbVerticalRules.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();

		}
		private void MoveVerticalMappingDown()
		{
			var current = CurrentDataContext.VLines.ElementAt(SelectedLayerVerticalIndex);
			if (current == null) return;

			int index = lbVerticalRules.SelectedIndex;
			if (index == CurrentDataContext.VLines.Count - 1) return;
			CurrentDataContext.VLines.Remove(current);
			CurrentDataContext.VLines.Insert(++index, current);
			lbVerticalRules.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();
		}
		private void AddHorizontalMapping()
		{
			/*
			var hls = CurrentDataContext.HorizontalLayerScroll.ElementAt(SelectedLayerHorizontalIndex);
			if (null == hls) return;

			hls.AddMapping();
			*/
		}
		private void RemoveVerticalRule()
		{
			/*
			var itemsToRemove = lbVerticalRules.SelectedItems;
			if (null == itemsToRemove) return;

			if (MessageBox.Show($@"Deleting a set of horizontal scrolling rule mappings can not be undone!
			Are you sure you want to delete?",
			"Confirm Deletion",
			MessageBoxButton.YesNo,
			MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (var entry in itemsToRemove)
				{
					CurrentDataContext.Layers.ElementAt(SelectedLayerIndex).HorizontalLayerRules.ElementAt(SelectedLayerHorizontalIndex).LinesMapList.Remove(entry as ScrollInfoLines_v5);
				}

				lbVerticalRules.Items.Refresh();
			}
			*/
		}


		#endregion

		#region Horizontal Rule Manipulation
		private void MoveHorizontalRuleUp()
		{
			var current = CurrentDataContext.HLines.ElementAt(SelectedLayerHorizontalIndex);
			if (current == null) return;

			int index = lbHorizontalRules.SelectedIndex;
			if (index == 0) return;
			CurrentDataContext.HLines.Remove(current);
			CurrentDataContext.HLines.Insert(--index, current);
			lbHorizontalRules.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();

		}
		private void MoveHorizontalRuleDown()
		{
			var current = CurrentDataContext.HLines.ElementAt(SelectedLayerHorizontalIndex);
			if (current == null) return;

			int index = lbHorizontalRules.SelectedIndex;
			if (index == CurrentDataContext.HLines.Count - 1) return;
			CurrentDataContext.HLines.Remove(current);
			CurrentDataContext.HLines.Insert(++index, current);
			lbHorizontalRules.SelectedIndex = index;
			HasLayerArangementChanged = true;
			UpdateListDetails();
		}
		private void AddHorizontalRule()
		{
			/*
			// create the horizontal rule set
			var layer = CurrentDataContext.Layers.ElementAt(SelectedLayerIndex);
			layer.ProduceHorizontalLayerScroll();

			// make sure our view of the underlying set of rules is refreshed
			lbHorizontalRules.Items.Refresh();

			// and select the one we just made
			lbHorizontalRules.SelectedIndex = lbHorizontalRules.Items.Count - 1;
			*/
		}
		private void RemoveHorizontalRule()
		{
			/*
			int amountSelected = lbHorizontalRules.SelectedItems.Count;

			if (CurrentDataContext.Layers.ElementAt(SelectedLayerIndex).HorizontalLayerRules.Count - amountSelected < 1)
			{
				MessageBox.Show("There must be at least one set of horizontal scrolling rules.",
				"Delete not allowed.",
				MessageBoxButton.OK,
				MessageBoxImage.Error);
				return;
			}

			var itemsToRemove = lbHorizontalRules.SelectedItems;

			if (null == itemsToRemove) return;

			if (MessageBox.Show($@"Deleting a set of horizontal scrolling rules can not be undone!
			Are you sure you want to delete these set of rules?
			All mappings for this rule will be deleted too!",
			"Confirm Deletion",
			MessageBoxButton.YesNo,
			MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (var entry in itemsToRemove)
				{
					CurrentDataContext.Layers.ElementAt(SelectedLayerIndex).HorizontalLayerRules.Remove(entry as HorizontalLayerScroll_v5);
				}

				lbHorizontalRules.Items.Refresh();
			}
			*/
		}

		#endregion

		#region Clipboard

		private void CopyLayerToClipboard(EditorLayer_v5 layerToCopy)
		{
			EditorLayer_v5 copyData = layerToCopy.Clone();

			//TODO: Reimplement
			//Methods.Solution.SolutionClipboard.SetLayerClipboard(new Classes.Clipboard.LayerClipboardEntry(copyData));

		}
		public void PasteLayerFromClipboard()
		{
			//TODO: Reimplement
			/*
			if (Clipboard.ContainsData("ManiacLayer") && false)
			{

				var layerToPaste = (Classes.Scene.EditorLayer)Clipboard.GetDataObject().GetData("ManiacLayer");
				CurrentDataContext.Layers.Insert(CurrentDataContext.Layers.Count - 1, layerToPaste);
				HasLayerArangementChanged = true;
			}
			else if (Methods.Solution.SolutionClipboard.LayerClipboard != null)
			{
				var layerToPaste = Methods.Solution.SolutionClipboard.LayerClipboard;
				CurrentDataContext.Layers.Insert(CurrentDataContext.Layers.Count - 1, layerToPaste.GetData());
				HasLayerArangementChanged = true;		
			}
			*/
		}

		#endregion

		#region Events
		private void VerticalRulesChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			lbVerticalRules.Items.Refresh();
		}
		private void DesiredSizeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			UpdateDesiredSizeLabel();
		}
		private void btnAddHorizontalRule_Click(object sender, RoutedEventArgs e)
		{
			AddHorizontalRule();
		}
		private void btnRemoveHorizontalRule_Click(object sender, RoutedEventArgs e)
		{
			RemoveHorizontalRule();
		}
		private void btnAddHorizontalMapping_Click(object sender, RoutedEventArgs e)
		{
			AddHorizontalMapping();
		}
		private void btnRemoveHorizontalMapping_Click(object sender, RoutedEventArgs e)
		{
			RemoveVerticalRule();
		}
		private void OversizeTextUpdater(object sender, RoutedEventArgs e)
		{

		}

		private void btnResize_Click(object sender, RoutedEventArgs e)
		{
			ResizeLayer();
		}

		private void btnUp_Click(object sender, RoutedEventArgs e)
		{
			MoveLayerUp();
		}

		private void btnDown_Click(object sender, RoutedEventArgs e)
		{
			MoveLayerDown();
		}
		private void btnUpMappings_Click(object sender, RoutedEventArgs e)
		{
			MoveVerticalMappingUp();
		}

		private void btnDownMappings_Click(object sender, RoutedEventArgs e)
		{
			MoveVerticalMappingDown();
		}

		private void btnUpRules_Click(object sender, RoutedEventArgs e)
		{
			MoveHorizontalRuleUp();
		}

		private void btnDownRules_Click(object sender, RoutedEventArgs e)
		{
			MoveHorizontalRuleDown();
		}
		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			AddLayer();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			DeleteLayer();
		}

		private void btnCut_Click(object sender, RoutedEventArgs e)
		{
			CutLayer();
		}

		private void btnCopy_Click(object sender, RoutedEventArgs e)
		{
			CopyLayer();
		}

		private void btnDuplicate_Click(object sender, RoutedEventArgs e)
		{
			DuplicateLayer();
		}

		private void btnPaste_Click(object sender, RoutedEventArgs e)
		{
			PasteLayer();
		}

		#endregion

		#region UI Updating

		public void UpdateListDetails()
		{
			lbLayers.Items.Refresh();
			lbHorizontalRules.Items.Refresh();
			lbVerticalRules.Items.Refresh();
			LayerLineIndexes.Items.Refresh();
		}
		public void UpdateDesiredSizeLabel()
		{
			lblResizedEffective.Text = $"Effective Width {(nudWidth.Value * 16):N0}, " + $"Effective Height {(nudHeight.Value * 16):N0}";
		}
		public void BindControl(DependencyObject control, DependencyProperty prop, string path)
		{
			var binding = new Binding(path);
			binding.Source = DataContext;
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(control, prop, binding);
		}
		public void BindControlOneWay(DependencyObject control, DependencyProperty prop, string path)
		{
			var binding = new Binding(path);
			binding.Source = DataContext;
			binding.Mode = BindingMode.OneWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(control, prop, binding);
		}
		public void BindControlEffictiveSize(DependencyObject control, DependencyProperty prop, string path)
		{
			var binding = new Binding(path);
			binding.Source = DataContext;
			binding.Mode = BindingMode.OneWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			binding.Converter = new TilePixelConverter();
			BindingOperations.SetBinding(control, prop, binding);
		}

		#endregion

		#region Selection Changed

		private void LbHorizontalRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
		private void LbMappings_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
		private void LbLayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
		#endregion

		#region Open/Close Events

		private void LayerManager_FormClosed(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!HasLayerArangementChanged) return;

			MessageBox.Show(@"If you have changed the number, or order of the layers, 
			you may need to update any layer switching entities/objects in the scene too.

			If you don't, strange things may well happen.
			They may well happen anway, this is all experimental!",
			"Don't forget!",
			MessageBoxButton.OK,
			MessageBoxImage.Information);
		}
		private void LayerManager_Load(object sender, RoutedEventArgs e)
		{
			isLoaded = false;
		}

		#endregion

		#region Context Menu

		private void lbLayers_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed)
			{
				if (lbLayers.SelectedItem != null)
				{
					lbLayers.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
					lbLayers.ContextMenu.IsOpen = true;
				}

			}
		}



        #endregion

        private void lbVerticalRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddVerticalRule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveVerticalRule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpVertRules_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDownVertRules_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddLineIndexes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveLineIndexes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoveUpLineIndexes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoveDownLineIndexes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LayerLineIndexes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
