# Telegram File Sender

A C# Console Application to recursively scan a directory and send files to a Telegram Group.

## Prerequisites

- .NET 10 SDK (or latest available)
- A Telegram Bot Token
- A Target Chat/Group ID

## Usage

1. Open a terminal in the project directory.
2. Run the application:

   ```bash
   dotnet run
   ```

3. Follow the prompts to enter:
   - **Directory Path**: The local folder to scan.
   - **Bot Token**: Your Telegram Bot API token.
   - **Group ID**: The ID of the chat to send files to.

Alternatively, you can pass arguments directly:

```bash
dotnet run -- "C:\Path\To\Files" "YOUR_BOT_TOKEN" "YOUR_GROUP_ID"
```

## Features

- **Recursive Scan**: Finds files in all subfolders.
- **Smart Captions**: Generates captions with Root Folder, Session Name (Subfolder), and File Name.
- **Rate Limiting**: Pauses for 1.5 seconds between files to avoid API limits.
- **Memory Efficient**: Uses streams properly.

## Project Structure

- `Program.cs`: Main application logic.
- `TelegramBot.csproj`: Project configuration.
