# TelegramBot Project Instructions

## Project Overview
This is a C# Console Application targeting .NET 10. Its primary purpose is to recursively scan a local directory and upload files to a specific Telegram Group using the Telegram Bot API.

## Tech Stack & Dependencies
- **Framework**: .NET 10.0
- **Key Library**: `Telegram.Bot` (NuGet)
- **Type**: Console Application

## Core Patterns & Conventions

### Telegram API Usage
- **Client**: Use `TelegramBotClient` initialized with a token.
- **File Uploads**:
  - Always use `SendDocumentAsync` (or `SendDocument` extension).
  - Wrap file streams in `using` statements to ensure disposal.
  - Use `InputFile.FromStream(stream, fileName)` for the document payload.
- **Rate Limiting**:
  - The application enforces a synchronous delay (e.g., `Thread.Sleep(1500)`) between uploads to prevent `429 Too Many Requests` errors from the Telegram API. Preserve this logic in loops.

### File System Operations
- **Scanning**: Use `Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)` for recursive scanning.
- **Session/Subfolder Logic**:
  - The "Session" name is derived using `Path.GetRelativePath(root, fileDir)`.
  - If the file is in the root, the session name defaults to "Root".

### Caption Formatting
- Captions are strictly formatted for user readability:
  ```text
  Root: {RootFolderName} | Session: {SubfolderName} | File: {FileName}
  ```

### Input Handling
- The application supports a hybrid input model:
  1.  **Command Line Arguments**: Checked first (Order: Path, Token, GroupID).
  2.  **Interactive Console**: Prompts user if args are missing.

## Developer Workflow
- **Run**: `dotnet run` (interactive) or `dotnet run -- "path" "token" "id"` (headless).
- **Build**: `dotnet build`.
- **Configuration**: No external config files; relies on runtime inputs.
