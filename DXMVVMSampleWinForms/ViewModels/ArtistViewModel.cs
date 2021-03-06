﻿using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;

namespace DXMVVMSampleWinForms.ViewModels
{
	[POCOViewModel]
	public class ArtistViewModel
	{
		protected ArtistViewModel()
		{
		}
		protected ArtistViewModel(int? artistId, string name)
		{
			this.ArtistId = artistId;
			this.Name = name;

			//calculated fields here

			//lookup data here
		}

		public static ArtistViewModel Create()
		{
			return ViewModelSource.Create(() => new ArtistViewModel(0, "No Name"));
		}

		public static ArtistViewModel Create(int? artistId, string name)
		{
			return ViewModelSource.Create(() => new ArtistViewModel(artistId, name));
		}
		public bool CanResetName()
		{
			return !String.IsNullOrEmpty(Name);
		}
		public void ResetName()
		{
			if (MessageBoxService.ShowMessage("Are you sure you want to reset the Name value?",
								"Question",
								MessageButton.YesNo,
								MessageIcon.Question,
								MessageResult.No) == MessageResult.Yes)
				Name = "";
		}

		[ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
		protected virtual IMessageBoxService MessageBoxService { get { return null; } }
		protected virtual ICurrentWindowService CurrentWindowService { get { return null; } }

		[Editable(false)]
		public int? ArtistId { get; set; }
		public virtual string Name { get; set; }

		public ArtistViewModel Clone()
		{
			return ArtistViewModel.Create(ArtistId, Name);
		}

		public void Assign(ArtistViewModel source)
		{
			this.ArtistId = source.ArtistId;
			this.Name = source.Name;
		}

	}
}