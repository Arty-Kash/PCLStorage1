using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using PCLStorage;

namespace PCLStrg
{
	public partial class FileIO : ContentPage
	{
		public FileIO()
		{
			InitializeComponent();
		}

		//Get Root Folder Path
		IFolder rootFolder = FileSystem.Current.LocalStorage;

		//
		//  File I/O
		//
		//Create a new File
		async void CreateFile(object sender, EventArgs e)
		{
			var Entry = EntCreateFile;

			// Alert, if no File Name
			if (Entry.Text == null)
			{
				await DisplayAlert("Create File", "Input File Name!", "OK");
				return;
			}

			// Create a new File named in EntCreateFile
			IFile newFile =
				await rootFolder.CreateFileAsync(Entry.Text, CreationCollisionOption.OpenIfExists);
			await DisplayAlert("Create File", Entry.Text + " has been created successfully!", "OK");
		}


		//Check File Existence
		async void CheckFile(object sender, EventArgs e)
		{
			var Entry = EntCheckFile;

			// Alert, if no File name
			if (Entry.Text == null)
			{
				await DisplayAlert("Check File", "Input File Name!", "OK");
				return;
			}

			// Check File Existence entered in EntCheckFile
			var fileExists = await rootFolder.CheckExistsAsync(Entry.Text);
			if (fileExists != ExistenceCheckResult.FileExists)
			{
				await DisplayAlert("Check File", Entry.Text + " doesn't exist", "OK");
				return;
			}
			await DisplayAlert("Check File", Entry.Text + " exists!", "OK");
		}


		//Delete File
		async void DeleteFile(object sender, EventArgs e)
		{
			var Entry = EntDeleteFile;

			// Alert, if no File Name
			if (Entry.Text == null)
			{
				await DisplayAlert("Delete File", "Input File Name!", "OK");
				return;
			}

			// Delete File entered in EntDeleteSubFolder
			var fileExists = await rootFolder.CheckExistsAsync(Entry.Text);
			if (fileExists != ExistenceCheckResult.FileExists)
			{
				await DisplayAlert("Delete File", Entry.Text + " doesn't exist", "OK");
				return;
			}
			IFile file = await rootFolder.GetFileAsync(Entry.Text);
			await file.DeleteAsync();
			await DisplayAlert("Delete File", Entry.Text + " has been deleted!", "OK");
		}


		// Get & Display File list
		async void GetFiles(object sender, EventArgs e)
		{
			//Get File list
			IList<IFile> files = await rootFolder.GetFilesAsync();

			//Get all File Name as string data
			IEnumerator filesList = files.GetEnumerator();
			int i = 0;
			string[] fileName = new string[10];
			fileName[i] = "Files in the Root Folder:"; i++;
			while (filesList.MoveNext())
			{
				IFile val = (IFile)filesList.Current;
				fileName[i] = "     - " + val.Name;
				i++;
			}

			//The number of Files
			fileName[i] = "//  " + files.Count.ToString() + " files";

			//Display File names and the number of them
			DisplayResult(FileIOResult, fileName);
		}


		// Additionally Write text to File
		async void WriteFile(object sender, EventArgs e)
		{
			var Entry_FileName = EntWriteFile_FileName;
			var Entry_Content = EntWriteFile_Content;
			string content = "";

			// Alert, if no File Name
			if (Entry_FileName.Text == null)
			{
				await DisplayAlert("Check File", "Input File Name!", "OK");
				return;
			}

			// Check File Existence entered in EntWriteFile
			var fileExists = await rootFolder.CheckExistsAsync(Entry_FileName.Text);
			if (fileExists != ExistenceCheckResult.FileExists)
			{
				// Create&Write File or Quit App, if File doesn't exist 
				if (await DisplayAlert("Write File", "File doesn' exist", "Create?", "No"))
				{
					IFile file1 = await rootFolder.CreateFileAsync(Entry_FileName.Text, CreationCollisionOption.OpenIfExists);
					await file1.WriteAllTextAsync(Entry_Content.Text);
				}
				return;
			}

			// Add new content to existing content
			IFile file = await rootFolder.CreateFileAsync(Entry_FileName.Text, CreationCollisionOption.OpenIfExists);
			content = await file.ReadAllTextAsync();
			await file.WriteAllTextAsync(content + Environment.NewLine + Entry_Content.Text);
		}


		//Read File
		async void ReadFile(object sender, EventArgs e)
		{
			var Entry = EntReadFile;
			string content = "";

			// Alert, if no File Name
			if (Entry.Text == null)
			{
				await DisplayAlert("Read File", "Input File Name!", "OK");
				return;
			}

			// Read File entered in EntReadFile
			var fileExists = await rootFolder.CheckExistsAsync(Entry.Text);
			if (fileExists != ExistenceCheckResult.FileExists)
			{
				await DisplayAlert("Delete File", Entry.Text + " doesn't exist", "OK");
				return;
			}
			IFile file = await rootFolder.GetFileAsync(Entry.Text);
			content = await file.ReadAllTextAsync();
			await DisplayAlert("Read File", content, "OK");
		}


		//
		//  Edit & Delete File from Menu Context Actions
		//
		async void MenuEdit(object sender, EventArgs e)
		{
			var item = (MenuItem)sender;

			// Move EditFile page
			await Navigation.PushAsync(new EditFile(item.CommandParameter.ToString()));
		}
		async void MenuDelete(object sender, EventArgs e)
		{
			var item = (MenuItem)sender;
			IFile file = await rootFolder.GetFileAsync(item.CommandParameter.ToString().Remove(0, 7));

			// Delete File
			await file.DeleteAsync();

			// Re-Display List
			GetFiles(null, null);
			await DisplayAlert("Delete File", item.CommandParameter + " has been deleted!", "OK");
		}


		//
		//  Display and Clear File I/O Result
		//
		//Display the results in ListView
		void DisplayResult(ListView ListViewName, string[] result)
		{
			ListViewName.ItemsSource = result;
		}
		//Clear all the File I/O result
		void ClearFileIOResult(object sender, EventArgs e)
		{
			FileIOResult.ItemsSource = null;
		}
		//Display the tapped File in EditFile page
		async void TappedResultItem(object sender, ItemTappedEventArgs e)
		{
			await Navigation.PushAsync(new EditFile(e.Item.ToString()));
		}
	}
}
