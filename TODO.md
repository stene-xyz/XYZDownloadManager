# TODO

## 0.2a
#### Goal: All core functionality working well
- `Form1.cs/AddURL`: Check before adding URL to make sure file does not already exist
- `Form1.cs/Form1`: Update progress function to remove file from list if download finished
- `Form1.cs/deleteURLButon_click`: Remove URL from saved list when deleting URL
- `Form1.cs`: Change "Delete URL" to "Cancel Download"

## 0.3a
#### Goal: User customization
- `DownloadManager.cs`: Allow specifying a custom downloads folder
- `DownloadManager.cs`: Allow setting a time to wait in between retrying
- `DownloadManager.cs`: Better download error handling (400/500 errors)
- Create `SettingsForm.cs` for user settings 
- Move `Form1.cs/Error` to its own file

## 0.1b
#### Goal: Presentable project
- Code documentation
- Tests
