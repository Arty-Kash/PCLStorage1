using System;
using Xamarin.Forms;
using PCLStorage;

namespace PCLStrg
{
	public partial class EditFile : ContentPage
	{
		public EditFile(string filename)
		{
			InitializeComponent();

			// Get File Name tapped in the List
			TappedFileName.Text = filename.Remove(0, 7);

			// Read File Contents and Bind Editor control
			ImportFile();
		}

		// Get Root Folder Path
		IFolder rootFolder = FileSystem.Current.LocalStorage;

		public string content0;

		// Read File Contents and Bind Editor Control
		async void ImportFile()
		{
			IFile file = await rootFolder.GetFileAsync(TappedFileName.Text);
			string content = await file.ReadAllTextAsync();
			content0 = content;
			FileContent.Text = content;
		}


		// Save File
		async void SaveFile(object sender, EventArgs e)
		{
			IFile file = 
			  await rootFolder.CreateFileAsync(TappedFileName.Text, CreationCollisionOption.OpenIfExists);
			await file.WriteAllTextAsync(FileContent.Text);
			await DisplayAlert("File Content", TappedFileName.Text + " has been saved successfully!", "OK");
		}

		// not Save File
		async void NotSaveFile(object sender, EventArgs e)
		{
			IFile file = 
			  await rootFolder.CreateFileAsync(TappedFileName.Text, CreationCollisionOption.OpenIfExists);
			await file.WriteAllTextAsync(content0);
			await DisplayAlert("File Content", TappedFileName.Text + " has not been saved!", "OK");
			await Navigation.PushAsync(new FileIO());
		}

		// Close File (Back to File I/O page)
		async void CloseFile(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new FileIO());
		}
	}
}
