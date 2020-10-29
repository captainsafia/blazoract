using System;
using System.Collections.Generic;
using blazoract.Shared;
using Blazored.LocalStorage;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;

namespace blazoract.Client.Data
{
    public class NotebookService
    {
        public event Action OnChange;

        private ILocalStorageService _storage;
        private HttpClient _http;

        private Dictionary<string, Notebook> _inMemoryNotebooks = new Dictionary<string, Notebook>();

        public NotebookService(ILocalStorageService storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        public async Task<Notebook> GetById(string id)
        {
            if (!_inMemoryNotebooks.TryGetValue(id, out var result))
            {
                result = await _storage.GetItemAsync<Notebook>(id);
                _inMemoryNotebooks[id] = result;
            }

            return result;
        }

        public async Task<string> CreateNewNotebook()
        {
            var title = "New notebook";
            var notebook = new Notebook(title);
            notebook.Cells = new List<Cell>() { new Cell(notebook.NotebookId, "// Type your code here", 0) };
            await _storage.SetItemAsync(notebook.NotebookId, notebook);

            var notebooks = await _storage.GetItemAsync<List<string>>("blazoract-notebooks") ?? new List<string>();
            notebooks.Add(notebook.NotebookId);
            await _storage.SetItemAsync("blazoract-notebooks", notebooks);

            _inMemoryNotebooks[notebook.NotebookId] = notebook;
            return notebook.NotebookId;
        }

        public async Task<Notebook> AddCell(string id, string content, CellType type, int position)
        {
            var notebook = await GetById(id);
            notebook.Cells.Insert(position, new Cell(id, content, type));
            await _storage.SetItemAsync(id, notebook);
            OnChange.Invoke();
            return notebook;
        }

        public async Task Save(Notebook notebook)
        {
            notebook.Updated = DateTime.Now;
            await _storage.SetItemAsync(notebook.NotebookId, notebook);
        }

        public async Task<IEnumerable<Notebook>> GetNotebooks()
        {
            var notebooks = await _storage.GetItemAsync<List<string>>("blazoract-notebooks");
            if (notebooks != null)
            {
                return await Task.WhenAll(notebooks.Select(async notebookId => await GetById(notebookId)));
            }
            return new List<Notebook>();
        }
    }
}