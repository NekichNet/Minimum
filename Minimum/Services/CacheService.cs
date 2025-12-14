using Minimum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class CacheService
    {
        private readonly string _root;
        private readonly string _chatsFile;
        private readonly string _messagesDir;
        private readonly string _filesDir;
        private readonly string _avatarsDir;
        private readonly string _settingsFile;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public CacheService(string? root = null) 
        {
            _root = root ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MinimumCache");
            _chatsFile = Path.Combine(_root, "chats.json");
            _messagesDir = Path.Combine(_root, "messages");
            _filesDir = Path.Combine(_root, "files");
            _avatarsDir = Path.Combine(_root, "avatars");
            _settingsFile = Path.Combine(_root, "settings.json");

            Directory.CreateDirectory(_root);
            Directory.CreateDirectory(_messagesDir);
            Directory.CreateDirectory(_filesDir);
            Directory.CreateDirectory(_avatarsDir);
        }



        // Chats
        public async Task SaveChatsAsync(IEnumerable<Chat> chats)
        {
            var arr = chats ?? Array.Empty<Chat>();
            await File.WriteAllTextAsync(_chatsFile, JsonSerializer.Serialize(arr, _jsonOptions));
        }

        public async Task<List<Chat>> LoadChatsAsync()
        {
            if (!File.Exists(_chatsFile)) return new List<Chat>();
            try
            {
                var json = await File.ReadAllTextAsync(_chatsFile);
                return JsonSerializer.Deserialize<List<Chat>>(json) ?? new List<Chat>();
            }
            catch
            {
                return new List<Chat>();
            }
        }



        // Messages
        private string MessagesPath(int chatId) => Path.Combine(_messagesDir, $"messages_{chatId}.json");

        public async Task SaveMessagesAsync(int chatId, IEnumerable<Message> messages)
        {
            var path = MessagesPath(chatId);
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(messages ?? Array.Empty<Message>(), _jsonOptions));
        }

        public async Task<List<Message>> LoadMessagesAsync(int chatId)
        {
            var path = MessagesPath(chatId);
            if (!File.Exists(path)) return new List<Message>();
            try
            {
                var json = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>();
            }
            catch
            {
                return new List<Message>();
            }
        }



        // Files (uploaded/downlaoded)
        private string FilePath(string fileId) => Path.Combine(_filesDir, fileId);

        public async Task SaveFileAsync(string fileId, byte[] data)
        {
            var path = FilePath(fileId);
            await File.WriteAllBytesAsync(path, data);
        }

        public bool FileExists(string fileId) => File.Exists(FilePath(fileId));

        public string? GetFilePath(string fileId)
        {
            var path = FilePath(fileId);
            return File.Exists(path) ? path : null;
        }



        // Avatars (user or chat)
        private string AvatarPath(string id) => Path.Combine(_avatarsDir, id);

        public async Task SaveAvatarAsync(string id, byte[] data)
        {
            var path = AvatarPath(id);
            await File.WriteAllBytesAsync(path, data);
        }

        public string? GetAvatarPath(string id)
        {
            var path = AvatarPath(id);
            return File.Exists(path) ? path : null;
        }



        // User Settings
        public async Task SaveSettingsAsync(UserSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, _jsonOptions);
            await File.WriteAllTextAsync(_settingsFile, json);
        }

        public async Task<UserSettings> LoadSettingsAsync()
        {
            if (!File.Exists(_settingsFile))
                return new UserSettings();

            var json = await File.ReadAllTextAsync(_settingsFile);
            return JsonSerializer.Deserialize<UserSettings>(json, _jsonOptions) ?? new UserSettings();
        }
    }
}
