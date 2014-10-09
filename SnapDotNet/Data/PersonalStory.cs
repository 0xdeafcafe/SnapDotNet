using SnapDotNet.Data.ApiResponses;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SnapDotNet.Data
{
	/// <summary>
	/// Represents a story posted by the user.
	/// </summary>
	public class PersonalStory
		: Story
	{
		internal PersonalStory() { }

		/// <summary>
		/// Gets a collection of notes attached to this story.
		/// </summary>
		public ObservableCollection<StoryNote> StoryNotes
		{
			get { return _storyNotes; }
			private set { SetValue(ref _storyNotes, value); }
		}
		private ObservableCollection<StoryNote> _storyNotes = new ObservableCollection<StoryNote>();

		/// <summary>
		/// Gets the total number of screenshots for this story.
		/// </summary>
		public int ScreenshotCount
		{
			get { return _screenshot; }
			private set { SetValue(ref _screenshot, value); }
		}
		private int _screenshot;

		/// <summary>
		/// Gets the total number of views for this story.
		/// </summary>
		public int ViewCount
		{
			get { return _viewCount; }
			private set { SetValue(ref _viewCount, value); }
		}
		private int _viewCount;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		[Pure]
		internal static PersonalStory CreateFromResponse(MyStoryResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			var personalStory = new PersonalStory();
			personalStory.UpdateFromResponse(response);
			return personalStory;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		internal void UpdateFromResponse(MyStoryResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			// Update story.
			base.UpdateFromResponse(response.Story);

			// Update properties.
			ViewCount = response.StoryExtras.ViewCount;
			ScreenshotCount = response.StoryExtras.ScreenshotCount;

			// Add new notes.
			foreach (var note in response.StoryNotes.Where(n1 => StoryNotes.FirstOrDefault(n => n.Viewer == n1.Viewer) == null))
				StoryNotes.Add(StoryNote.CreateFromResponse(note));

			// Update existing notes.
			foreach (var note in StoryNotes)
			{
				var updatedNote = response.StoryNotes.FirstOrDefault(n => n.Viewer == note.Viewer);
				if (updatedNote == null) continue;
				note.UpdateFromResponse(updatedNote);
			}

			// Remove nonexistent notes.
			foreach (var redundantStory in StoryNotes.Where(n => response.StoryNotes.FirstOrDefault(n1 => n1.Viewer == n.Viewer) == null).ToList())
				StoryNotes.Remove(redundantStory);
		}
	}
}
