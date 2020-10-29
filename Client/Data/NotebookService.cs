using System;
using System.Collections.Generic;
using blazoract.Shared;
using Blazored.LocalStorage;
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

        private Dictionary<string, Task<Notebook>> _inMemoryNotebooks = new Dictionary<string, Task<Notebook>>();

        public NotebookService(ILocalStorageService storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        public async Task<Notebook> GetInitialContent()
        {
            return await _http.GetFromJsonAsync<Notebook>("/data/default-notebook.json");
        }

        public Task<Notebook> GetById(string id)
        {

            if (!_inMemoryNotebooks.TryGetValue(id, out var result))
            {
                result = _storage.GetItemAsync<Notebook>(id).AsTask();
                _inMemoryNotebooks[id] = result;
            }
            else
            {
            }

            return result;
        }

        public async Task<Notebook> CreateNewNotebook(bool addSample = false)
        {
            var id = Guid.NewGuid().ToString("N");
            var notebook = new Notebook("New notebook", id);
            notebook.Cells = new List<Cell>();

            if (addSample)
            {
                notebook.Cells.Add(new Cell(id, @"int Fibonacci(int n)
{
    return n < 2 ? 1 : Fibonacci(n-1) + Fibonacci(n-2);
}

Fibonacci(5)", 0));
                notebook.Cells.Add(new Cell(id, @"Enumerable.Range(1, 10).Select(Fibonacci).ToArray()", 0));
            }
            else
            {
                notebook.Cells.Add(new Cell(id, "// Type your code here", 0));
            }

            await _storage.SetItemAsync(id, notebook);
            _inMemoryNotebooks[id] = Task.FromResult(notebook);

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
