﻿using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DXMVVMSampleWPF.ViewModels
{
	[POCOViewModel]
	public class AlbumTreeViewModel
	{
		public virtual ObservableCollection<AlbumViewModel> Albums
		{
			get;
			/* We only want to set this through the ViewModel code */
			protected set;
		}
		//CurrentTrack is only needed for Winforms app since the WinForms Grid doesn't have a RowDblClick event
		public virtual ArtistViewModel CurrentTrack { get; set; }
		public virtual bool IsLoading
		{
			get;
			protected set;
		}

		protected AlbumTreeViewModel()
		{
			ViewInjectionManager.Default.RegisterNavigatedEventHandler(this, () => {
				ViewInjectionManager.Default.Navigate(Regions.Navigation, NavigationKey.Artists);
			});
		}

		public static AlbumTreeViewModel Create()
		{
			return ViewModelSource.Create(() => new AlbumTreeViewModel());
		}

		[ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
		public virtual IDialogService DialogService { get { return null; } }
		[ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
		protected virtual IDispatcherService DispatcherService { get { return null; } }


		public void EditTrack(AlbumViewModel album)
		{
			var clone = album.Clone();
			if (DialogService.ShowDialog(
				MessageButton.OKCancel, "Edit Artist", "ArtistView", clone) == MessageResult.OK)
			{
				album.Assign(clone);
				DataAccess.PersistAlbum(album);
			}
		}

		public Task LoadTracks()
		{
			IsLoading = true;

			return Task.Factory.StartNew((state) =>
			{
				var results = new ObservableCollection<AlbumViewModel>(DataAccess.GetAlbumViewModelList());
				// Update on UI Thread
				((IDispatcherService)state).BeginInvoke(() => {
					Albums = results;
					IsLoading = false;
				});

			}, DispatcherService);
		}
	}
}