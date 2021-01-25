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
	public partial class LayerManager : Window
	{
		#region View Model
		public class ViewModel : INotifyPropertyChanged
		{
            #region Collections
            private IList<EditorLayer_v5> _Layers { get; set; }
			public IList<EditorLayer_v5> Layers
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
			public IList<HorizontalLayerScroll_v5> HorizontalLayerScroll
			{
				get
				{
					return GetHorizontalLayerScrolls();
				}
				set
				{
					SetHorizontalLayerScrolls(value);
					NotifyPropertyChanged("HorizontalLayerScroll");

				}
			}
			public IList<ScrollInfoLines_v5> ScrollInfoLines
			{
				get
				{
					return GetScrollInfoLines();
				}
				set
				{
					SetScrollInfoLines(value);
					NotifyPropertyChanged("ScrollInfoLines");
				}
			}
            #endregion

            #region Current Items
            public EditorLayer_v5 CurrentLayer
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
			public HorizontalLayerScroll_v5 CurrentHorizontalLayerScroll
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
			public ScrollInfoLines_v5 CurrentScrollInfoLines
			{
				get
				{
					return GetCurrentScrollInfoLine();
				}
				set
				{
					SetCurrentScrollInfoLine(value);
					NotifyPropertyChanged("CurrentScrollInfoLines");
				}
			}
			#endregion

			#region Indexes
			private int _SelectedLayerIndex { get; set; } = 0;
			private int _SelectedLayerHorizontalIndex { get; set; } = 0;
			private int _SelectedLayerScrollIndex { get; set; } = 0;

			public int SelectedLayerIndex
			{
				get
				{
					return _SelectedLayerIndex;
				}
				set
				{
					_SelectedLayerIndex = value;
					UpdateSection(true, true, true);
				}
			}
			public int SelectedLayerHorizontalIndex
			{
				get
				{
					return _SelectedLayerHorizontalIndex;
				}
				set
				{
					_SelectedLayerHorizontalIndex = value;
					UpdateSection(false, true, true);
				}
			}
			public int SelectedLayerScrollIndex
			{
				get
				{
					return _SelectedLayerScrollIndex;
				}
				set
				{
					_SelectedLayerScrollIndex = value;
					UpdateSection(false, false, true);
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
					if (Layers == null) return true;
					return (Layers[SelectedLayerIndex].HorizontalLayerRules.Count - 1 < SelectedLayerHorizontalIndex || SelectedLayerHorizontalIndex < 0);
				}
			}

			public bool OutsideOfScrollInfoBounds
			{
				get
				{
					if (Layers == null) return true;
					return (Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex].LinesMapList.Count - 1 < SelectedLayerScrollIndex || SelectedLayerScrollIndex < 0);
				}
			}

			#endregion

			#region Current Helper Methods
			private EditorLayer_v5 GetCurrentLayer()
			{
				if (OutsideOfLayerBounds) return null;
				return Layers[SelectedLayerIndex];
			}
			private void SetCurrentLayer(EditorLayer_v5 value)
			{
				if (OutsideOfLayerBounds) return;
				else Layers[SelectedLayerIndex] = value;

			}
			private HorizontalLayerScroll_v5 GetCurrentHorizontalLayerScroll()
			{
				if (OutsideOfLayerBounds) return null;
				else if (OutsideOfHorizontalRuleBounds) return null;
				return Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex];
			}
			private ScrollInfoLines_v5 GetCurrentScrollInfoLine()
			{
				if (OutsideOfLayerBounds) return null;
				else if (OutsideOfHorizontalRuleBounds) return null;
				else if (OutsideOfScrollInfoBounds) return null;
				else return Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex].LinesMapList[SelectedLayerScrollIndex];

			}
			private void SetCurrentHorizontalLayerScroll(HorizontalLayerScroll_v5 value)
			{
				if (OutsideOfLayerBounds) return;
				else if (OutsideOfHorizontalRuleBounds) return;
				else Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex] = value;
			}
			private void SetCurrentScrollInfoLine(ScrollInfoLines_v5 value)
			{
				if (OutsideOfLayerBounds) return;
				else if (OutsideOfHorizontalRuleBounds) return;
				else if (OutsideOfScrollInfoBounds) return;
				else Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex].LinesMapList[SelectedLayerScrollIndex] = value;
			}

            #endregion

            #region Collection Helper Methods

            private IList<HorizontalLayerScroll_v5> GetHorizontalLayerScrolls()
			{
				if (OutsideOfLayerBounds) return null;
				else return Layers[SelectedLayerIndex].HorizontalLayerRules;
			}
			private IList<ScrollInfoLines_v5> GetScrollInfoLines()
			{
				if (OutsideOfLayerBounds) return null;
				else if (OutsideOfHorizontalRuleBounds) return null;
				else return Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex].LinesMapList;

			}
			private void SetHorizontalLayerScrolls(IList<HorizontalLayerScroll_v5> value)
			{
				if (OutsideOfLayerBounds) return;
				Layers[SelectedLayerIndex].HorizontalLayerRules = value;
			}
			private void SetScrollInfoLines(IList<ScrollInfoLines_v5> value)
			{
				if (OutsideOfLayerBounds) return;
				else if (OutsideOfHorizontalRuleBounds) return;
				else Layers[SelectedLayerIndex].HorizontalLayerRules[SelectedLayerHorizontalIndex].LinesMapList = value;
			}

			#endregion

			#region Update Helper Methods

			private void UpdateSection(bool UpdateLayerProps, bool UpdateHorizontalProps, bool UpdateScrollMapProps)
			{
				if (UpdateLayerProps)
				{
					NotifyPropertyChanged("SelectedLayerIndex");
					NotifyPropertyChanged("CurrentLayer");
				}
				if (UpdateHorizontalProps)
				{
					NotifyPropertyChanged("SelectedLayerHorizontalIndex");
					NotifyPropertyChanged("CurrentHorizontalLayerScroll");
				}
				if (UpdateScrollMapProps)
				{
					NotifyPropertyChanged("SelectedLayerScrollIndex");
					NotifyPropertyChanged("CurrentScrollInfoLines");
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

		#region Index Definitions

		private int SelectedLayerIndex
		{
			get
			{
				return CurrentDataContext.SelectedLayerIndex;
			}
		}
		private int SelectedLayerScrollIndex
		{
			get
			{
				return CurrentDataContext.SelectedLayerScrollIndex;
			}
		}
		private int SelectedLayerHorizontalIndex
		{
			get
			{
				return CurrentDataContext.SelectedLayerHorizontalIndex;
			}
		}

		#endregion

		#region Loaded Controls

		public IEditor CurrentEditor { get; set; }

        #endregion

        #region Definitions
        private bool HasLayerArangementChanged { get; set; } = false;
		private bool isLoaded { get; set; } = true;


        #endregion

        #region Init
        public LayerManager()
		{
            InitializeComponent();
			Unload();
		}

		public void Load()
        {
			EditorHost.Children.Add(CurrentEditor as UserControl);
			OpenFile.IsEnabled = false;
			SaveFile.IsEnabled = true;
			SaveAsFile.IsEnabled = true;
			UnloadFile.IsEnabled = true;
		}

		public void Unload()
        {
			if (CurrentEditor != null) CurrentEditor.Unload();
			EditorHost.Children.Clear();
			CurrentEditor = null;
			OpenFile.IsEnabled = true;
			SaveFile.IsEnabled = false;
			SaveAsFile.IsEnabled = false;
			UnloadFile.IsEnabled = false;
		}

        #endregion

        #region File Tab Events

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
			System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
			open.Filter = "RSDKv5 Scene File|*.bin|RSDKv4 Backgrounds File|*.bin";
			if (open.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
			{
				try
                {
					switch (open.FilterIndex)
                    {
						case 1:
							EditorClass_v5 sceneFile_v5 = new EditorClass_v5(open.FileName);
							CurrentEditor = new Editor_v5();
							CurrentEditor.Load(sceneFile_v5);
							Load();
							break;
						case 2:
							EditorClass_v4 sceneFile_v4 = new EditorClass_v4(open.FileName);
							CurrentEditor = new Editor_v4();
							CurrentEditor.Load(sceneFile_v4);
							Load();
							break;
					}
                }
				catch (Exception ex)
                {
					MessageBox.Show(ex.Message);
					Unload();
                }
			}
		}

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
			if (CurrentEditor == null) return;
			try
			{
				CurrentEditor.Save();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
			if (CurrentEditor == null) return;

			string CurrentFilter = "RSDKv5 Scene File|*.bin";
			string CurrentFileType = "Scene.bin";
			string DefaultExtension = "bin";

			if (CurrentEditor is Editor_v4)
            {
				CurrentFilter = "RSDKv4 Backgrounds File|*.bin";
				CurrentFileType = "Backgrounds.bin";
				DefaultExtension = "bin";
			}

			System.Windows.Forms.SaveFileDialog save = new System.Windows.Forms.SaveFileDialog
			{
				Filter = CurrentFilter,
				DefaultExt = DefaultExtension,
				RestoreDirectory = true,
				FileName = CurrentFileType
			};
			if (save.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
				try
				{
					CurrentEditor.Save(save.FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void UnloadFile_Click(object sender, RoutedEventArgs e)
		{
			Unload();
		}

		private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
			Application.Current.Shutdown();
        }


        #endregion

        private void LayerManager_Load(object sender, RoutedEventArgs e)
        {

        }
    }
}