using System;
using System.Collections.Generic;
using blazoract.Shared;
using Blazored.LocalStorage;
using System.Threading;
using System.Threading.Tasks;

namespace blazoract.Client.Data
{
    public class ContentService
    {
        public event Action OnChange;

        private ILocalStorageService _storage;

        public ContentService(ILocalStorageService storage)
        {
            _storage = storage;
            var initialContent = new List<Cell>();
            for (var i = 0; i < 100; i++)
            {
                initialContent.Add(new Cell($"{i} * {i}", i));
            }
            _storage.SetItemAsync("_default_notebook", initialContent);
        }
        public async Task<List<Cell>> GetInitialContent()
        {
            return await _storage.GetItemAsync<List<Cell>>("_default_notebook");
        }

        public async Task<List<Cell>> GetById(string id)
        {
            return await _storage.GetItemAsync<List<Cell>>(id);
        }

        public async Task<string> CreateNewNotebook()
        {
            var id = Guid.NewGuid().ToString("N");
            await _storage.SetItemAsync(id, new List<Cell>() { new Cell("", 0) });
            return id;
        }

        public async Task<List<Cell>> AddCell(string id, string content, CellType type, int position)
        {
            var notebook = await _storage.GetItemAsync<List<Cell>>(id);
            notebook.Insert(position, new Cell(content, position, type));
            await _storage.SetItemAsync(id, notebook);
            OnChange.Invoke();
            return notebook;
        }

    }
}