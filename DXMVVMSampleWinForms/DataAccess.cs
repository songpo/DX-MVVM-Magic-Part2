﻿using DXMVVMSampleWinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMVVMSampleWinForms
{
	public class LookupItem
	{
		public int Key { get; set; }
		public string Value { get; set; }
	}
	public enum NavigationKey
	{
		Tracks,
		Artists,
		Albums
	}
	public static class Regions
	{
		public static string Navigation { get { return "NavigationRegion"; } }
		public static string Content { get { return "ContentRegion"; } }
	}
	public class DataAccess
	{
		public static IEnumerable<TrackViewModel> GetTrackViewModelList()
		{
			using (var ctx = new ChinookContext())
			{
				var albumLookupData = (from album in ctx.Album
									   select new LookupItem { Key = album.AlbumId, Value = album.Title }).ToList();
				var mediaLookupData = (from media in ctx.MediaType
									   select new LookupItem() { Key = media.MediaTypeId, Value = media.Name }).ToList();
				var genreLookupData = (from genre in ctx.Genre
									   select new LookupItem { Key = genre.GenreId, Value = genre.Name }).ToList();

				foreach (var track in ctx.Track)
					yield return TrackViewModel.Create(track.TrackId,
										 track.Name,
										 track.AlbumId,
										 track.MediaTypeId,
										 track.GenreId,
										 track.Composer,
										 track.Milliseconds,
										 track.Bytes,
										 albumLookupData,
										 mediaLookupData,
										 genreLookupData);
			}
		}


		public static void PersistTrack(TrackViewModel track)
		{
			using (var ctx = new ChinookContext())
			{
				Track pTrack = track.TrackId.HasValue ?
					ctx.Track.First(t => t.TrackId == track.TrackId) :
					new Track();
				pTrack.Name = track.Name;
				pTrack.AlbumId = track.AlbumId;
				pTrack.MediaTypeId = track.MediaTypeId;
				pTrack.GenreId = track.GenreId;
				pTrack.Composer = track.Composer;
				pTrack.Milliseconds = track.Milliseconds;
				pTrack.Bytes = track.Bytes;
				ctx.SaveChanges();
			}
		}
		public static IEnumerable<ArtistViewModel> GetArtistViewModelList()
		{
			using (var ctx = new ChinookContext())
			{
				foreach (var artist in ctx.Artist)
					yield return ArtistViewModel.Create(artist.ArtistId, artist.Name);
			}
		}
		public static void PersistArtist(ArtistViewModel artist)
		{
			using (var ctx = new ChinookContext())
			{
				Artist pArtist = artist.ArtistId.HasValue ?
					ctx.Artist.First(t => t.ArtistId == artist.ArtistId) :
					new Artist();

				pArtist.Name = artist.Name;
				ctx.SaveChanges();
			}
		}
	}
}
