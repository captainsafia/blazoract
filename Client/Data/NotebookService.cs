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
        public const string DefaultNotebookId = "_default_notebook";

        public event Action OnChange;

        private ILocalStorageService _storage;
        private HttpClient _http;

        private Dictionary<string, Notebook> _inMemoryNotebooks = new Dictionary<string, Notebook>();

        public NotebookService(ILocalStorageService storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
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

                if (result == null && id == DefaultNotebookId)
                {
                    result = await CreateNewNotebook(defaultNotebook: true);
                }

                _inMemoryNotebooks[id] = result;
            }

            return result;
        }

        public async Task<Notebook> CreateNewNotebook(bool defaultNotebook = false)
        {
            var id = defaultNotebook ? DefaultNotebookId : Guid.NewGuid().ToString("N");
            var notebook = new Notebook("New notebook", id);

            if (!defaultNotebook)
            {
                notebook.Cells = new List<Cell>() { new Cell(id, "// Type your code here", 0) };
            }
            else
            {
                notebook.Cells = new List<Cell>();
                for (var i = 0; i < 100; i++)
                {
                    notebook.Cells.Add(new Cell(id, $"{i} * {i}"));
                }
            }

            await _storage.SetItemAsync(id, notebook);

            var notebooks = await _storage.GetItemAsync<List<string>>("blazoract-notebooks") ?? new List<string>();
            notebooks.Add(id);
            await _storage.SetItemAsync("blazoract-notebooks", notebooks);

            return notebook;
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
