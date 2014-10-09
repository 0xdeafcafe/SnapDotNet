using SnapDotNet.Data.ApiResponses;
using SnapDotNet.Utilities;
using System;
using System.Diagnostics.Contracts;

namespace SnapDotNet.Data
{
	/// <summary>
	/// Represents a note attached to a personal story.
	/// </summary>
	public class StoryNote
		: ObservableObject
	{
		internal StoryNote() { }

		/// <summary>
		/// Gets the username of the person who viewed this story.
		/// </summary>
		public string Viewer
		{
			get { return _viewer; }
			private set { SetValue(ref _viewer, value); }
		}
		private string _viewer;

		/// <summary>
		/// Gets the date and time the viewer viewed the story.
		/// </summary>
		public DateTime ViewedAt
		{
			get { return _viewedAt; }
			private set { SetValue(ref _viewedAt, value); }
		}
		private DateTime _viewedAt;

		/// <summary>
		/// Gets a boolean value indicating whether this viewer captured a screenshot of the story.
		/// </summary>
		public bool IsScreenshotted
		{
			get { return _screenShotted; }
			private set { SetValue(ref _screenShotted, value); }
		}
		private bool _screenShotted;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		[Pure]
		internal static StoryNote CreateFromResponse(StoryNoteResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			var storyNote = new StoryNote();
			storyNote.UpdateFromResponse(response);
			return storyNote;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		internal void UpdateFromResponse(StoryNoteResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			Viewer = response.Viewer;
			ViewedAt = response.Timestamp;
			IsScreenshotted = response.Screenshotted;
		}
	}
}
