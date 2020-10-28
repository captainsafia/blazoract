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

            var id = Guid.NewGuid().ToString("N");
            var title = "Default Notebook";
            var notebook = new Notebook(id, title);
            var initialContent = new List<Cell>();
            for (var i = 0; i < 100; i++)
            {
                initialContent.Add(new Cell(id, $"{i} * {i}"));
            }
            notebook.Cells = initialContent;
            _storage.SetItemAsync("_default_notebook", notebook);
        }

        public async Task<Notebook> GetInitialContent()
        {
            return await _http.GetFromJsonAsync<Notebook>("/data/default-notebook.json");
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
            var id = Guid.NewGuid().ToString("N");
            var title = "New notebook";
            var notebook = new Notebook(title, id);
            notebook.Cells = new List<Cell>() { new Cell(id, "// Type your code here", 0) };
            await _storage.SetItemAsync(id, notebook);

            var notebooks = await _storage.GetItemAsync<List<string>>("blazoract-notebooks") ?? new List<string>();
            notebooks.Add(id);
            await _storage.SetItemAsync("blazoract-notebooks", notebooks);

            _inMemoryNotebooks[id] = notebook;
            return id;
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