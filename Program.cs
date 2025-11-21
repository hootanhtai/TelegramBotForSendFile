using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Telegram File Sender");
        Console.WriteLine("--------------------");

        string directoryPath = GetInput("Enter Directory Path: ", args, 0);
        string botToken = GetInput("Enter Bot Token: ", args, 1);
        string groupIdStr = GetInput("Enter Group ID: ", args, 2);

        if (!long.TryParse(groupIdStr, out long groupId))
        {
            Console.WriteLine("Invalid Group ID. Exiting.");
            return;
        }

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Directory does not exist. Exiting.");
            return;
        }

        var botClient = new TelegramBotClient(botToken);
        var rootDirInfo = new DirectoryInfo(directoryPath);
        string rootFolderName = rootDirInfo.Name;

        Console.WriteLine($"Scanning '{directoryPath}'...");
        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
        Console.WriteLine($"Found {files.Length} files.");

        int count = 0;
        foreach (var filePath in files)
        {
            count++;
            var fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;

            // Calculate Session Name (Subfolder)
            // Path.GetRelativePath returns "." if the paths are the same
            string relativePath = Path.GetRelativePath(directoryPath, fileInfo.DirectoryName ?? directoryPath);
            string sessionName = relativePath == "." ? "Root" : relativePath;

            string caption = $"Root: {rootFolderName} | Session: {sessionName} | File: {fileName}";

            Console.WriteLine($"[{count}/{files.Length}] Sending: {fileName} (Session: {sessionName})...");

            try
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    string extension = fileInfo.Extension.ToLowerInvariant();
                    var inputFile = InputFile.FromStream(stream, fileName);

                    if (IsImage(extension))
                    {
                        await botClient.SendPhoto(
                            chatId: groupId,
                            photo: inputFile,
                            caption: caption
                        );
                    }
                    else if (IsAnimation(extension))
                    {
                        await botClient.SendAnimation(
                            chatId: groupId,
                            animation: inputFile,
                            caption: caption
                        );
                    }
                    else if (IsVideo(extension))
                    {
                        await botClient.SendVideo(
                            chatId: groupId,
                            video: inputFile,
                            caption: caption
                        );
                    }
                    else if (IsAudio(extension))
                    {
                        await botClient.SendAudio(
                            chatId: groupId,
                            audio: inputFile,
                            caption: caption
                        );
                    }
                    else
                    {
                        // Using InputFile.FromStream for modern Telegram.Bot versions
                        await botClient.SendDocument(
                            chatId: groupId,
                            document: inputFile,
                            caption: caption
                        );
                    }
                }
                Console.WriteLine("Sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending file: {ex.Message}");
            }

            // Rate limiting: 1.5 seconds
            Thread.Sleep(1500);
        }

        Console.WriteLine("All files processed.");
    }

    static string GetInput(string prompt, string[] args, int index)
    {
        if (args.Length > index && !string.IsNullOrWhiteSpace(args[index]))
        {
            return args[index];
        }
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    static bool IsImage(string extension) =>
        new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp" }.Contains(extension);

    static bool IsAnimation(string extension) =>
        new[] { ".gif" }.Contains(extension);

    static bool IsVideo(string extension) =>
        new[] { ".mp4", ".avi", ".mov", ".mkv", ".webm" }.Contains(extension);

    static bool IsAudio(string extension) =>
        new[] { ".mp3", ".wav", ".ogg", ".m4a", ".flac" }.Contains(extension);
}
