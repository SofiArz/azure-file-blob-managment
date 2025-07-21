# Azure Blob Storage Management API

This is a minimal ASP.NET Core Web API that provides endpoints to **upload**, **rename**, and **delete** files in an Azure Blob Storage container. It's built using the official `Azure.Storage.Blobs` SDK and demonstrates safe file operations using leases and blob copy mechanisms.

## Features

- Upload a file to Azure Blob Storage
- Rename a file using lease and blob copy
- Rename via copy-and-delete fallback
- Swagger UI for testing endpoints

## Technologies Used

- ASP.NET Core Web API
- Azure.Storage.Blobs SDK
- Dependency Injection (DI)
- Swagger (Swashbuckle)

## Endpoints

### `POST /upload`

Uploads a file to the blob container named `files`.

**Form Data**:

- `file` (IFormFile): File to upload

---

### `POST /rename`

Renames a blob by leasing, copying to a new name, and deleting the original.

**Form Data**:

- `fileName`: Existing blob name
- `newName`: Desired new name

---

### `POST /createthendelete`

Alternative rename: Copies blob to new name and deletes the old one.

**Form Data**:

- `fileName`: Existing blob name
- `newName`: Desired new name

## Project Structure

```
AzureBlobStorageManagement/
├── Controllers/
│   └── FileController.cs         # API endpoints
├── Models/
│   └── FileModel.cs              # File upload model
├── Services/
│   ├── FileManagerLogic.cs       # Blob operation logic
│   └── IFileManagerLogic.cs      # Interface abstraction
├── Startup.cs                    # Service and middleware config
├── Program.cs                    # App entry point
└── appsettings.json              # Connection strings
```

## Configuration

In `appsettings.json`, define your Azure Blob connection string:

```json
"ConnectionStrings": {
  "AzureBlobStorage": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
}
```

## How to Run

1. Clone the repo:

```bash
git clone https://github.com/SofiArz/azure-file-blob-managment.git
cd azure-file-blob-managment
```

2. Add your Azure connection string to `appsettings.json`.

3. Run the application:

```bash
dotnet run
```

4. Open Swagger UI at `https://localhost:<port>/` to test the API.

## Notes

- Container name is hardcoded as `files` in the service.
- Lease mechanism ensures that renaming is safe and atomic.
- For simple renaming, use `/createthendelete`.
- This project has no demo frontend and no license.
